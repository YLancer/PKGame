using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;
using LitJson;

public class TenSS : MonoBehaviour
{

    public List<PlayerItemScript> playerItems;   // 玩家人数 
    public List<AvatarVO> avatarList;
    public Text gameNumber;    // 游戏局数
    public Text gameRules;
    public Button invite_Btn;  // 邀请按钮
    public Button ready_Btn;   // 准备按钮
    int bankerId;              //庄家ID 
    int curDirIndex;           //当前位置的序号
    public List<List<GameObject>> handerCardList; // handCardList[i] 是人    handlist[i][j]是牌
    public List<Transform> handTransformList;    //手牌的位置： 第7位是桌面发牌即为handTransformList[6]   L-改至11位是桌面发牌即为handTransformList[10]
    private bool isGameStart = false;    //游戏是否开始的标志位
    private bool isFinshing = false;     //是否可以整理牌组了
    /**更否申请退出房间申请**/
    private bool canClickButtonFlag = false;
    private bool isFirstOpen = true;
    private float timer = 0;
    private List<List<int>> mineList;
    private GameObject biaoqingObj;
    private biaoqingScript bqScript;
    void Start()
    {
        addListener();  //初始化监听

        if (GlobalDataScript.reEnterRoomData != null)
        { //断线重连进入房间
            GlobalDataScript.loginResponseData.roomId = GlobalDataScript.reEnterRoomData.roomId;
            reEnterRoom();
        }
        else if (GlobalDataScript.roomJoinResponseData != null)
        {//进入他人房间
            joinToRoom(GlobalDataScript.roomJoinResponseData.playerList);
        }
        else
        {//创建房间
            createRoomAddAvatarVO(GlobalDataScript.loginResponseData);
        }

        loadBiaoQing();
        showFangzhu();
    }
    void addListener()
    {
        SocketEventHandle.getInstance().StartGameNotice += startGame;

    }
    void removeListener()
    {
        SocketEventHandle.getInstance().StartGameNotice -= startGame;
    }

    /*  玩家掉线后重新连接游戏*/
    void reEnterRoom()
    {
        if (GlobalDataScript.reEnterRoomData != null)
        {
            //显示房间基本信息
            GlobalDataScript.roomVo.roundNumber = GlobalDataScript.reEnterRoomData.roundNumber;
            GlobalDataScript.roomVo.roomId = GlobalDataScript.reEnterRoomData.roomId;
            GlobalDataScript.roomVo.gameType = GlobalDataScript.reEnterRoomData.gameType;
            GlobalDataScript.roomVo.initFen_dzpk = GlobalDataScript.reEnterRoomData.initFen_dzpk;
            setRoomRemark();

            //确定自己和其它玩家
            avatarList = GlobalDataScript.reEnterRoomData.playerList;
            GlobalDataScript.loginResponseData = avatarList[getMyIndexFromList()];
            GlobalDataScript.count_Players_DN = avatarList.Count;
            GlobalDataScript.roomAvatarVoList = GlobalDataScript.reEnterRoomData.playerList;
            for (int i = 0; i < avatarList.Count; i++)
            {
                AvatarVO ava = avatarList[i];
                setSeat(ava);

                if (ava.main)
                {
                    bankerId = i;
                    GlobalDataScript.mainUuid = ava.account.uuid;
                    playerItems[getIndexByDir(getDirection(bankerId))].setbankImgEnable(true);//新一局游戏还没开始庄家是上一局的庄家不显示，因已经准备好，只是别人还没准备好
                }
                if (avatarList[i].account.uuid == GlobalDataScript.loginResponseData.account.uuid
                    && ava.isReady)
                {
                    ready_Btn.gameObject.SetActive(false);
                }
            }
            GlobalDataScript.loginResponseData.account.roomcard = GlobalDataScript.reEnterRoomData.playerList[getMyIndexFromList()].account.roomcard;
            int[][] selfPaiArray = GlobalDataScript.loginResponseData.paiArray;
            if (selfPaiArray == null || selfPaiArray.Length == 0)
            {//游戏还没有开始
            }
            else
            {//牌局已开始
                ready_Btn.gameObject.SetActive(false);

                UpdateTimeReStart();
                isGameStart = true;

                setAllPlayerReadImgVisbleToFalse();
                cleanGameplayUI();

                //initJiaZhuText();//加注按钮赋值，与创建房间的初始分有关
                //initSliderZhuArray();//设置滑动条初始数组，与创建房间的初始分有关

                initArrayList();

                //显示打牌数据
                mineList = ToList(selfPaiArray);
                DisplayHandCard();

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < avatarList.Count; j++)
                    {
                        if (avatarList[j].scores > GlobalDataScript.roomVo.initFen_dzpk / 100)
                        {
                            int seat = getIndexByDir(getDirection(j));
                            if (seat == 0)
                            {
                                continue;
                            }
                            GameObject obj = Instantiate(Resources.Load("prefab/dn/HandCard_s_DN")) as GameObject;
                            if (obj != null)
                            {
                                obj.transform.SetParent(handTransformList[seat]);
                                obj.transform.localScale = new Vector3(1, 1, 1);
                                handerCardList[seat].Add(obj);
                            }
                        }
                    }
                }
            }
        }
    }

    void UpdateTimeReStart()   // 每个玩家操作可考虑时间段
    {
        timer = 24;
    }
    void initArrayList()   // 玩家手牌位置及手牌数
    {
        mineList = new List<List<int>>();
        handerCardList = new List<List<GameObject>>();
        for (int i = 0; i < 11; i++)
        {                    // 7->11  L-
            handerCardList.Add(new List<GameObject>());
        }
    }
    private void setAllPlayerReadImgVisbleToFalse()  // 所有玩家准备图片
    {
        for (int i = 0; i < 10; i++)
        {                     //  6-10
            playerItems[i].readyImg.enabled = false;
        }
    }
    void setRoomRemark()    //显示创建房间信息
    {
        RoomCreateVo roomvo = GlobalDataScript.roomVo;
        GlobalDataScript.totalTimes = roomvo.roundNumber;
        GlobalDataScript.surplusTimes = roomvo.roundNumber;//剩余多少局

        string roomInfo = "";
        if (GlobalDataScript.goldType)
        {
            roomInfo += "房间号<训练场>";
        }
        else
        {
            roomInfo += "房间号<" + roomvo.roomId + ">";
            roomInfo += "、局数<" + GlobalDataScript.roomVo.roundNumber + "局>";
        }

        roomInfo += "、初始分:" + GlobalDataScript.roomVo.initFen_dzpk;

        gameRules.text = roomInfo;
    }
    int getMyIndexFromList()   //根据openid和uuid 判断座位编号
    {
        if (avatarList != null)
        {
            for (int i = 0; i < avatarList.Count; i++)
            {
                if (avatarList[i].account.uuid == GlobalDataScript.loginResponseData.account.uuid
                    || avatarList[i].account.openid == GlobalDataScript.loginResponseData.account.openid)
                {
                    GlobalDataScript.loginResponseData.account.uuid = avatarList[i].account.uuid;
                    print("++getMyIndexFromList+++数据正常返回" + i);
                    return i;
                }
            }
        }
        MyDebug.Log("---getMyIndexFromList----数据异常返回0");
        return 0;
    }
    private void setSeat(AvatarVO avatar)
    {
        //游戏结束后用的数据，勿删！！！
        if (avatar.account.uuid == GlobalDataScript.loginResponseData.account.uuid)
        {
            playerItems[0].gameObject.SetActive(true);
            playerItems[0].setAvatarVo(avatar);
        }
        else
        {
            int myIndex = getMyIndexFromList();
            int curAvaIndex = avatarList.IndexOf(avatar);
            int seatIndex = curAvaIndex - myIndex;
            if (seatIndex < 0)
            {
                if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.Tensanshui
                    && GlobalDataScript.roomVo.typeOfTensanshui == (int)GameType.GameType_Tensanshui)  //经典十三水   4人
                    seatIndex = 4 + seatIndex;
                else if (GlobalDataScript.roomVo.gameType == (int)GameType.GameType_Tensanshui
                    && GlobalDataScript.roomVo.typeOfTensanshui == (int)GameType.GameType_Purecolor)  //疯狂纯一色
                    seatIndex = 4 + seatIndex;
                else if (GlobalDataScript.roomVo.gameType == (int)GameType.GameType_Tensanshui
                    && GlobalDataScript.roomVo.typeOfTensanshui == (int)GameType.GameType_Sevenandthree) //七人加三色
                    seatIndex = 5 + seatIndex;
                else if (GlobalDataScript.roomVo.gameType == (int)GameType.GameType_Tensanshui
                    && GlobalDataScript.roomVo.typeOfTensanshui == (int)GameType.GameType_SevenmenOverlord) //七人霸王庄  
                    seatIndex = 10 + seatIndex;

            }
            playerItems[seatIndex].gameObject.SetActive(true);
            playerItems[seatIndex].setAvatarVo(avatar);
        }
    }
    /// 根据一个人在数组里的索引，得到这个人所在的方位，L-左，T-上,R-右，B-下（自己的方位永远都是在下方）
	private string getDirection(int avatarIndex)
    {
        string result = DirectionEnum.Bottom;
        int myselfIndex = getMyIndexFromList();
        if (myselfIndex == avatarIndex)
        {
            MyDebug.Log("getDirection == B");
            curDirIndex = 0;
            return result;
        }
        //从自己开始计算，下一位的索引                   玩家位置的索引 0-9  供10个人
        for (int i = 0; i < 10; i++)
        {                   // 6->10    位置   L-
            myselfIndex++;
            if (myselfIndex >= 10)
            {
                myselfIndex = 0;
            }
            if (myselfIndex == avatarIndex)
            {
                if (i == 0)
                {
                    MyDebug.Log("getDirection == Right");
                    curDirIndex = 1;
                    return DirectionEnum.Right;
                }
                else if (i == 1)
                {
                    MyDebug.Log("getDirection == TopRight");
                    curDirIndex = 2;
                    return DirectionEnum.TopRight;
                }
                else if (i == 2)
                {
                    MyDebug.Log("getDirection == TopLeft");
                    curDirIndex = 3;
                    return DirectionEnum.TopLeft;
                }
                else if (i == 3)
                {
                    MyDebug.Log("getDirection == Left");
                    curDirIndex = 4;
                    return DirectionEnum.Left;
                }
                else if (i == 4)
                {
                    MyDebug.Log("getDirection == LeftBottom");
                    curDirIndex = 5;
                    return DirectionEnum.LeftBottom;
                }
                else if (i == 5)
                {
                    MyDebug.Log("getDirection == Player_7");
                    curDirIndex = 6;
                    return DirectionEnum.Player_7;
                }
                else if (i == 6)
                {
                    MyDebug.Log("getDirection == Player_8");
                    curDirIndex = 7;
                    return DirectionEnum.Player_8;
                }
                else if (i == 7)
                {
                    MyDebug.Log("getDirection == Player_9");
                    curDirIndex = 8;
                    return DirectionEnum.Player_9;
                }
                else if (i == 8)
                {
                    MyDebug.Log("getDirection == Player_10");
                    curDirIndex = 9;
                    return DirectionEnum.Player_10;
                }
            }
        }
        MyDebug.Log("getDirection == B");
        curDirIndex = 0;
        return DirectionEnum.Bottom;
    }
    private int getIndexByDir(string dir)
    {
        int result = 0;
        switch (dir)
        {
            case DirectionEnum.Bottom: //下
                result = 0;
                break;
            case DirectionEnum.Right: //右
                result = 1;
                break;
            case DirectionEnum.TopRight: //上右
                result = 2;
                break;
            case DirectionEnum.TopLeft://上左
                result = 3;
                break;
            case DirectionEnum.Left://左
                result = 4;
                break;
            case DirectionEnum.LeftBottom:
                result = 5;
                break;
            case DirectionEnum.Player_7:            //L-  
                result = 6;
                break;
            case DirectionEnum.Player_8:
                result = 7;
                break;
            case DirectionEnum.Player_9:
                result = 8;
                break;
            case DirectionEnum.Player_10:
                result = 9;
                break;
        }
        return result;
    }
    private void cleanGameplayUI()
    {
        canClickButtonFlag = true;
        invite_Btn.transform.gameObject.SetActive(false);
    }
    private List<List<int>> ToList(int[][] param)
    {
        List<List<int>> returnData = new List<List<int>>();
        for (int i = 0; i < param.Length; i++)
        {
            List<int> temp = new List<int>();
            for (int j = 0; j < param[i].Length; j++)
            {
                temp.Add(param[i][j]);
            }
            returnData.Add(temp);
        }
        return returnData;
    }
    private void DisplayHandCard(bool hasShowed = false)   //显示手里的牌  L-
    {
        for (int i = 0; i < 52; i++)
        {
            if (mineList[0][i] == 1)
            {
                GameObject gob = Instantiate(Resources.Load("prefab/dn/HandCard_s_DN")) as GameObject;
                gob.GetComponent<pdkCardScript>().isSmall = true;
                if (gob != null)
                {
                    gob.transform.SetParent(handTransformList[0]);
                    gob.transform.localScale = new Vector3(1, 1, 1);
                    gob.GetComponent<pdkCardScript>().setPoint(i, 4);
                    handerCardList[0].Add(gob);
                }
            }
        }
        float time = 0;
        for (int i = 0; i < handerCardList[0].Count; i++)
        {
            GameObject go = handerCardList[0][i];
            //StartCoroutine(FanPai(time, go));
            go.GetComponent<pdkCardScript>().hasShow = true;
            time += 0.3f;
        }
    }
    void joinToRoom(List<AvatarVO> avatars)
    {
        avatarList = avatars;
        for (int i = 0; i < avatars.Count; i++)
        {
            setSeat(avatars[i]);
        }
        setRoomRemark();
    }    //加入房间
    void createRoomAddAvatarVO(AvatarVO avatar)   //创建房间
    {
        avatar.scores = GlobalDataScript.roomVo.initFen_dzpk;
        addAvatarVOToList(avatar);
        setRoomRemark();
    }
    void addAvatarVOToList(AvatarVO avatar)
    {
        if (avatarList == null)
        {
            avatarList = new List<AvatarVO>();
        }
        avatarList.Add(avatar);
        setSeat(avatar);
    }
    void loadBiaoQing()
    {
        biaoqingObj = Instantiate(Resources.Load("Prefab/Panel_biaoqing")) as GameObject;
        biaoqingObj.transform.parent = this.gameObject.transform;
        biaoqingObj.transform.localPosition = new Vector3(0, 1600, 1);
        bqScript = biaoqingObj.GetComponent<biaoqingScript>();
    }
    public biaoqingScript getBqScript()
    {
        return bqScript;
    }
    void showFangzhu()//考虑放到显示房主   L-
    {
        int myIndex = getMyIndexFromList();
        if (myIndex == 0)
        {
            playerItems[0].showFangZhu();
        }
        else if (myIndex >= 1 && myIndex <= 9)
        {
            playerItems[playerItems.Count - myIndex].showFangZhu();     
        }
    }

    void startGame(ClientResponse response)
    {
        GlobalDataScript.roomAvatarVoList = avatarList;
        print(" startGame " + avatarList.Count);
        StartGameVO sgvo = JsonMapper.ToObject<StartGameVO>(response.message);
        cleanGameplayUI();
        //cleanDesk_dzpk();  清理十三水扑克桌面    TODO
        GlobalDataScript.surplusTimes--;
        if (GlobalDataScript.goldType) { 
            gameNumber.text = GlobalDataScript.playCountForGoldType + "";
        }
        else
            gameNumber.text = GlobalDataScript.surplusTimes + "/" + GlobalDataScript.roomVo.roundNumber;

        bankerId = sgvo.bankerId;
        avatarList[bankerId].main = true;
        PlayerItemScript bankerItem = getPlayItem(bankerId);
        bankerItem.setbankImgEnable(true);

        GlobalDataScript.finalGameEndVo = null;
        initArrayList();

        isFirstOpen = false;
        GlobalDataScript.isOverByPlayer = false;
        int selfIndex = getMyIndexFromList();
        mineList = sgvo.paiArray;
        setAllPlayerReadImgVisbleToFalse();

        //所有玩家发牌移动动画          动画可不要
        float time = 0;
        if (GlobalDataScript.roomVo.AA == false)   //德扑发两张牌  
        {
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < avatarList.Count; j++)
                {
                    if (avatarList[j].scores > GlobalDataScript.roomVo.initFen_dzpk / 100)
                    {
                        int seat = getIndexByDir(getDirection(j));
                        StartCoroutine(DisplayFaPaiMove(seat, time, i));
                        time += 0.4f;
                    }
                }
            }

        }
    }
    private PlayerItemScript getPlayItem(int index)
    {
        string dirstr = getDirection(index);
        PlayerItemScript res = null;
        switch (dirstr)
        {
            case DirectionEnum.Bottom:
                res = playerItems[0];
                break;
            case DirectionEnum.Right:
                res = playerItems[1];
                break;
            case DirectionEnum.TopRight:
                res = playerItems[2];
                break;
            case DirectionEnum.TopLeft:
                res = playerItems[3];
                break;
            case DirectionEnum.Left:
                res = playerItems[4];
                break;
            case DirectionEnum.LeftBottom:
                res = playerItems[5];
                break;
            case DirectionEnum.Player_7:          //  L- 
                res = playerItems[6];
                break;
            case DirectionEnum.Player_8:
                res = playerItems[7];
                break;
            case DirectionEnum.Player_9:
                res = playerItems[8];
                break;
            case DirectionEnum.Player_10:
                res = playerItems[9];
                break;
        }
        return res;
    }   //根据玩家位置   返回位置index
    IEnumerator DisplayFaPaiMove(int seat, float time, int cardIndex)
    {
        yield return new WaitForSeconds(time);
        GameObject obj = Instantiate(Resources.Load("prefab/dn/HandCard_s_DN")) as GameObject;
        if (obj != null)
        {
            SoundCtrl.getInstance().playSoundByDZPK("fapai");
            obj.transform.SetParent(gameObject.transform);
            obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            obj.transform.localPosition = new Vector3(6, 180, 0);
            obj.transform.SetSiblingIndex(5);

            Vector3 pos = getHandCardPosition(seat, cardIndex);

            StartCoroutine(MoveToPosition(obj, pos, seat, cardIndex));
        }
    }
    Vector3 getHandCardPosition(int seat,int cardIndex)
    {
        Vector3 vec = new Vector3(0, 0, 0);
        return vec;
    }    //TODO
    IEnumerator MoveToPosition(GameObject obj, Vector3 dest, int seat, int cardIndex)
    {
        while (obj.transform.localPosition != dest)
        {
            obj.transform.localPosition = Vector3.MoveTowards(obj.transform.localPosition, dest, 1700 * Time.deltaTime);
            yield return 0;
        }
        Destroy(obj);
        if (seat == 0 && cardIndex == 1)
        {
            DisplayHandCard();
        }
        if (seat != 0)
        {
            GameObject gob = Instantiate(Resources.Load("prefab/dn/HandCard_s_DN")) as GameObject;    //发牌  牌只能看到背面
            if (gob != null)
            {
                gob.transform.SetParent(handTransformList[seat]);
                gob.transform.localScale = new Vector3(1, 1, 1);
                handerCardList[seat].Add(gob);
            }
        }
    }
}
