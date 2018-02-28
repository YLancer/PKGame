using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssemblyCSharp;
using DG.Tweening;
using UnityEngine.UI;
using LitJson;
using UnityEngine.SceneManagement;

public enum CARDTYPE
{
	c0,
	// 不能出牌
	c1,
	// 单牌。
	c2,
	// 对子。
	c3,
	// 3不带。
	c4,
	// 炸弹。
	c31,
	// 3带1。
	c311,
	//3带2
	c32,
	// 3带对。
	c411,
	// 4带2个单，或者一对
	c422,
	// 4带2对
	c12345,
	// 连子。
	c1122,
	// 连队。
	c112233,
	// 三连及以上
	c111222,
	// 飞机。
	c11122234,
	// 飞机带单排
	c111222N,
	//飞机随机带
	c1112223456,
	// 飞机带4子
	c1112223344
	// 飞机带对子.
}

public class MyPDKScript : MonoBehaviour
{

	public List<PlayerItemScript> playerItems;
	//所有玩家
	public List<AvatarVO> avatarList;

	private biaoqingScript bqScript;
	private GameObject bqObj;

	/**更否申请退出房间申请**/
	private bool canClickButtonFlag = false;

	//解散房间
	public Image dialog_fanhui;
	public Text dialog_fanhui_text;

	public Text versionText;
	public Text Number;
	public Text roomRemark;

	public Button readyBtn;
	public Button inviteFriendButton;

	private int bankerId;
	private int curDirIndex;          //当前玩家位置索引？
	private int currentIndex = -1;    //当前玩家索引？
	//当前出牌人
	private string curDirString = "B";
	//当前的方向字符串

	private bool isFirstOpen = true;

	public pdkButtonActionScript btnActionScript;

	private List<List<int>> mineList;
    private List<List<int>> deskList;    // 桌面上该出现的四张牌牌组
    public List<Transform> handTransformList;
	public List<Transform> ThrowTransformList;

	public List<List<GameObject>> handerCardList;
    private List<int> handCards;
    public Transform landlord_TransformList;
    private List<GameObject> landlord_deskCardList;       // 斗地主桌面上方的

    private float timer = 0;
	private bool isGameStart = false;

	private int[] lastCard;
    //上家、或上上家最后出的牌

    public GameObject panel_landlordChoose;
    public GameObject panel_Ti;
    public GameObject panel_GenTi;
    public GameObject panel_HuiPai;
    
    void Start ()
	{
		init ();
        
		if (GlobalDataScript.reEnterRoomData != null) { //断线重连进入房间
			GlobalDataScript.loginResponseData.roomId = GlobalDataScript.reEnterRoomData.roomId;
			reEnterRoom ();
		} else if (GlobalDataScript.roomJoinResponseData != null) {//进入他人房间
			joinToRoom (GlobalDataScript.roomJoinResponseData.playerList);
            print(" GlobalDataScript.roomJoinResponseData.playerList" + GlobalDataScript.roomJoinResponseData.playerList);
		} else {//创建房间
			createRoomAddAvatarVO (GlobalDataScript.loginResponseData);
		}

		showFangzhu ();
		loadBiaoQing ();
	}

	public void init ()
	{
		addListener ();

		versionText.text = "V" + Application.version;
	}

	public void showFangzhu ()
	{
		//显示房主
		int myIndex = getMyIndexFromList ();
		if (myIndex == 0) {
			playerItems [0].showFangZhu ();
		} else if (myIndex == 1) {
			playerItems [2].showFangZhu ();
		} else if (myIndex == 2) {
			playerItems [1].showFangZhu ();
		}
	}

	// Update is called once per frame
	void Update ()
	{
		timer -= Time.deltaTime;
		if (timer < 0) {
			timer = 0;
		}

		if (isGameStart && currentIndex >= 0) {
			playerItems [currentIndex].showClockText (Math.Floor (timer) + "");
		}
	}

	void OnGUI ()
	{
		Event e = Event.current;
		if (e.isMouse && (e.clickCount == 2)) {
			MyDebug.Log ("用户双击了鼠标");
            if (handerCardList != null && handerCardList.Count > 0)              // -lan  根据玩家确定手牌
            {
                cardReset ();
				isCanChu ();
			}
		}		        
	}

	public void addListener ()
	{
		SocketEventHandle.getInstance ().outRoomCallback += outRoomCallbak;  
		SocketEventHandle.getInstance ().otherUserJointRoomCallBack += otherUserJointRoom;
		SocketEventHandle.getInstance ().messageBoxNotice += messageBoxNotice;
		SocketEventHandle.getInstance ().gameReadyNotice += gameReadyNotice;
		SocketEventHandle.getInstance ().StartGameNotice += startGame;
		SocketEventHandle.getInstance ().PDK_putOutCardCallBack += putOutCardCallback;
		SocketEventHandle.getInstance ().serviceErrorNotice += serviceErrorNotice;
		SocketEventHandle.getInstance ().PDK_ChiBuQiCallBack += ChiBuQiCallBack;
		SocketEventHandle.getInstance ().returnGameResponse += returnGameResponse;
		SocketEventHandle.getInstance ().dissoliveRoomResponse += dissoliveRoomResponse;
		SocketEventHandle.getInstance ().offlineNotice += offlineNotice;
		SocketEventHandle.getInstance ().onlineNotice += onlineNotice;
		SocketEventHandle.getInstance ().HupaiCallBack += hupaiCallBack;
		CommonEvent.getInstance ().closeGamePanel += exitOrDissoliveRoom;

        SocketEventHandle.getInstance().DDZ_qiangResponse += DDZ_qiangResponse;
        SocketEventHandle.getInstance().DDZ_zhuangResponse += DDZ_zhuangResponse;
        SocketEventHandle.getInstance().DDZ_TIResponse += DDZ_TIResponse;
        SocketEventHandle.getInstance().DDZ_ALL_TI_Response += DDZ_ALL_TI_Response;
    }

	private void removeListener ()
	{
		SocketEventHandle.getInstance ().outRoomCallback -= outRoomCallbak;
		SocketEventHandle.getInstance ().otherUserJointRoomCallBack -= otherUserJointRoom;
		SocketEventHandle.getInstance ().messageBoxNotice -= messageBoxNotice;
		SocketEventHandle.getInstance ().gameReadyNotice -= gameReadyNotice;
		SocketEventHandle.getInstance ().StartGameNotice -= startGame;
		SocketEventHandle.getInstance ().PDK_putOutCardCallBack -= putOutCardCallback;
		SocketEventHandle.getInstance ().serviceErrorNotice -= serviceErrorNotice;
		SocketEventHandle.getInstance ().PDK_ChiBuQiCallBack -= ChiBuQiCallBack;
		SocketEventHandle.getInstance ().returnGameResponse -= returnGameResponse;
		SocketEventHandle.getInstance ().dissoliveRoomResponse -= dissoliveRoomResponse;
		SocketEventHandle.getInstance ().offlineNotice -= offlineNotice;
		SocketEventHandle.getInstance ().onlineNotice -= onlineNotice;
		SocketEventHandle.getInstance ().HupaiCallBack -= hupaiCallBack;
		CommonEvent.getInstance ().closeGamePanel -= exitOrDissoliveRoom;

        SocketEventHandle.getInstance().DDZ_qiangResponse -= DDZ_qiangResponse;
        SocketEventHandle.getInstance().DDZ_zhuangResponse -= DDZ_zhuangResponse;
        SocketEventHandle.getInstance().DDZ_TIResponse -= DDZ_TIResponse;
        SocketEventHandle.getInstance().DDZ_ALL_TI_Response -= DDZ_ALL_TI_Response;
    }

	/**
	 * 赢牌请求回调
	 */ 
	private void hupaiCallBack (ClientResponse response)
	{
		currentIndex = -1;
		if(GlobalDataScript.goldType)
			GlobalDataScript.playCountForGoldType++;
		//删除这句，未区分胡家是谁
		GlobalDataScript.hupaiResponseVo = new HupaiResponseVo ();
		GlobalDataScript.hupaiResponseVo = JsonMapper.ToObject<HupaiResponseVo> (response.message);

		string scores = GlobalDataScript.hupaiResponseVo.currentScore;
		//hupaiCoinChange (scores);

		if (GlobalDataScript.hupaiResponseVo.type == "0") {
			/*
			for (int i = 0; i < GlobalDataScript.hupaiResponseVo.avatarList.Count; i++) {
				int huType = checkAvarHupai (GlobalDataScript.hupaiResponseVo.avatarList [i]);

				effectType = "fei";
				pengGangHuEffectCtrl ();

				playerItems [getIndexByDir (getDirection (i))].setHuFlagHidde ();
			}*/


			Invoke ("openGameOverPanelSignal", 0.5f);
		
		}else {
			Invoke ("openGameOverPanelSignal", 0.5f);
		}
	}

	private void openGameOverPanelSignal ()
	{//单局结算
        //setAllPlayerHuImgVisbleToFalse ();
        
        GameObject obj = PrefabManage.loadPerfab ("Prefab/Panel_Game_Over");

		getDirection (bankerId);
		playerItems [curDirIndex].setbankImgEnable (false);
        playerItems[curDirIndex].setTiImage(false);
        if (handerCardList != null && handerCardList.Count > 0 && handerCardList[0].Count > 0)
        {
            for (int i = 0; i < handerCardList[0].Count; i++)
            {
                handerCardList[0][i].GetComponent<pdkCardScript>().onDragSelect -= dragSelect;
                handerCardList[0][i].GetComponent<pdkCardScript>().onCardSelect -= cardSelect;
            }
        }

        initPanel ();
		obj.GetComponent<GameOverScript> ().setDisplayContent_pdk (0, avatarList, null, null);
		GlobalDataScript.singalGameOverList.Add (obj);
		avatarList [bankerId].main = false;
	}
    
    public void qiangzhuang_DDZ(bool isJiao)    
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        timer = -1;
        CustomSocket sok = CustomSocket.getInstance();
        sok.sendMsg(new QiangZhuangDDZRequest(isJiao?1:0));
    }
    
    void DDZ_qiangResponse(ClientResponse response)                                 
    {
        JsonData json = JsonMapper.ToObject(response.message);
        int uuid = (int)json["dzUuid"];                            
        int multiple = (int)json["multiple"];
        int index =  getIndex(uuid);
        if (uuid == GlobalDataScript.loginResponseData.account.uuid && index ==0)                    
        {
            panel_landlordChoose.SetActive(false);
        }
    }

    // 庄家确定回调{"avatarIndex":0,"curAvatarIndex":0,"curTCardAvatarIndex":100372}
    void DDZ_zhuangResponse(ClientResponse response)   
    {
        JsonData json = JsonMapper.ToObject(response.message);
        bankerId = (int)json["avatarIndex"];

        curDirString = getDirection(bankerId);
        if (curDirString == DirectionEnum.Bottom)
        {
            btnActionScript.showBtn();
            GlobalDataScript.isDrag = true;
        }
        else
        {
            GlobalDataScript.isDrag = false;
        }
        avatarList[bankerId].main = true;
        int bankerIdInedx = getIndexByDir(getDirection(bankerId));
        playerItems[bankerIdInedx].setbankImgEnable(true);
        bankerAddCard(bankerIdInedx);

        int TCarduuid = getIndex((int)json["curTCardAvatarIndex"]);
        if(TCarduuid == getMyIndexFromList())
        {
            panel_Ti.SetActive(true);
        }
    }

    public void ti_DDZ(bool isTi)
    {  
        SoundCtrl.getInstance().playSoundByActionButton(1);
        timer = -1;
        CustomSocket sok = CustomSocket.getInstance();
        sok.sendMsg(new TiPaiRequest(isTi ? 1 : 0));
    }

    // 踢牌的回调 {"multiple":2,"uuid":100311}
    void DDZ_TIResponse(ClientResponse response)
    {
        JsonData json = JsonMapper.ToObject(response.message);
        int multiple = (int)json["multiple"];
        int uuid= (int)json["uuid"];
        print(" + ddz_TIRespone " + response.message);
        int index =getIndex(uuid);
        if(index == getMyIndexFromList())   
        {
            panel_Ti.SetActive(false);
            panel_GenTi.SetActive(false);
            panel_HuiPai.SetActive(false);
        }
    }

    // 踢牌通知所有人  {curTCardAvatarIndex":100371,"multiple":2}
    void DDZ_ALL_TI_Response(ClientResponse response)
    {
        JsonData json = JsonMapper.ToObject(response.message);
        int GCarduuid = getIndex((int)json["curTCardAvatarIndex"]);
        int multiple = (int)json["multiple"];
        if (GCarduuid== getMyIndexFromList() && multiple==2){
            panel_GenTi.SetActive(true);
        }
        else if (GCarduuid == getMyIndexFromList() && multiple == 1)
        {
            panel_Ti.SetActive(true);
        }
        else if((int)json["curTCardAvatarIndex"] == GlobalDataScript.loginResponseData.account.uuid)
        {
            panel_HuiPai.SetActive(true);
        }
    }

    void initPanel ()
	{
		clean ();
		readyBtn.gameObject.SetActive (true);
		isGameStart = false;
		for (int i=0; i<playerItems.Count; i++) {
			PlayerItemScript pi = playerItems[i];
			pi.showClock (false);
			if(i>0)
				pi.showPaiCountImage (false);
		}	
	}

	/// <summary>
	/// 清理桌面
	/// </summary>
	public void clean ()
	{
        // -lan  change
        cleanArrayList (handerCardList);

		//去除打出的牌
		for (int i = 0; i < 3; i++) {
			cleanChuPai (i);
		}
	
		if (mineList != null) {
			mineList.Clear ();
		}
	}

	private void cleanArrayList (List<List<GameObject>> list)
	{
		if (list != null) {
			while (list.Count > 0) {
				List<GameObject> tempList = list [0];
				list.RemoveAt (0);
				cleanList (tempList);
			}
		}
	}

	private void cleanList (List<GameObject> tempList)
	{
		if (tempList != null) {
			while (tempList.Count > 0) {
				GameObject temp = tempList [0];
				tempList.RemoveAt (0);
				GameObject.Destroy (temp);
			}
		}
	}
	/**用户离线回调**/
	public void offlineNotice (ClientResponse response)
	{
		int uuid = int.Parse (response.message);
		int index = getIndex (uuid);
		string dirstr = getDirection (index);
		switch (dirstr) {
		case DirectionEnum.Bottom:
			playerItems [0].GetComponent<PlayerItemScript> ().setPlayerOffline ();
			break;
		case DirectionEnum.Right:
			playerItems [1].GetComponent<PlayerItemScript> ().setPlayerOffline ();
			break;
		case DirectionEnum.Left:
			playerItems [2].GetComponent<PlayerItemScript> ().setPlayerOffline ();
			break;
		}

		//申请解散房间过程中，有人掉线，直接不能解散房间
		if (GlobalDataScript.isonApplayExitRoomstatus) {
			if (dissoDialog != null) {
				dissoDialog.GetComponent<VoteScript> ().removeListener ();
				Destroy (dissoDialog.GetComponent<VoteScript> ());
				Destroy (dissoDialog);
			}
			TipsManagerScript.getInstance ().setTips ("由于" + avatarList [index].account.nickname + "离线，系统不能解散房间。");
		}
	}

	/**用户上线提醒**/
	public void onlineNotice (ClientResponse  response)
	{
		int uuid = int.Parse (response.message);
		int index = getIndex (uuid);
		string dirstr = getDirection (index);
		switch (dirstr) {
		case DirectionEnum.Bottom:
			playerItems [0].GetComponent<PlayerItemScript> ().setPlayerOnline ();
			break;
		case DirectionEnum.Right:
			playerItems [1].GetComponent<PlayerItemScript> ().setPlayerOnline ();
			break;
		case DirectionEnum.Left:
			playerItems [2].GetComponent<PlayerItemScript> ().setPlayerOnline ();
			break;
		}
	}
	/***
	 * 申请解散房间回调
	 */
	GameObject dissoDialog;
	private string dissoliveRoomType = "0";

	public void dissoliveRoomResponse (ClientResponse response)
	{
		MyDebug.Log ("dissoliveRoomResponse" + response.message);
		DissoliveRoomResponseVo dissoliveRoomResponseVo = JsonMapper.ToObject<DissoliveRoomResponseVo> (response.message);
		string plyerName = dissoliveRoomResponseVo.accountName;
		if (dissoliveRoomResponseVo.type == "0") {
			GlobalDataScript.isonApplayExitRoomstatus = true;
			dissoliveRoomType = "1";
			dissoDialog = PrefabManage.loadPerfab ("Prefab/Panel_Apply_Exit");
			dissoDialog.GetComponent<VoteScript> ().iniUI (plyerName, avatarList);
		} else if (dissoliveRoomResponseVo.type == "3") {
			
			GlobalDataScript.isonApplayExitRoomstatus = false;
			if (dissoDialog != null) {
				GlobalDataScript.isOverByPlayer = true;
				dissoDialog.GetComponent<VoteScript> ().removeListener ();
				Destroy (dissoDialog.GetComponent<VoteScript> ());
				Destroy (dissoDialog);
			}
		}  
	}

	/// <summary>
	/// 重新开始计时
	/// </summary>
	void UpdateTimeReStart ()
	{
		timer = 16;
	}

	public void returnGameResponse (ClientResponse response)
	{
		JsonData json = JsonMapper.ToObject (response.message);
		int gameRound = int.Parse (json ["gameRound"].ToString ());
		GlobalDataScript.surplusTimes = gameRound;

		int curAvatarIndex = (int)json ["curAvatarIndex"];
		bool isNewTurn = (bool)json ["isNewTurn"];
		string chuPai = (string)json ["chuPai"];
		int outIndex = getIndexByDir (getDirection (curAvatarIndex));

		currentIndex = outIndex;
		if (outIndex == 0) {//自己
			btnActionScript.showBtn ();
		} else {//别人
			
		}

		if (!isNewTurn) {//不是新的一轮必须显示打牌数据
			displayChupai (chuPai);
		} else {
			lastCard = null;
		}
	}

	public void displayChupai (string chupai)
	{
		try {
			string[] chupaiArray = chupai.Split (';');
			for (int k = 0; k < chupaiArray.Length; k++) {
				string str = chupaiArray [k];
				if (str == null || str.Length == 0)
					continue;
				int avatarIndex = int.Parse (str.Split (':') [0]);
				string chuStr = str.Split (':') [1];

				int outIndex = getIndexByDir (getDirection (avatarIndex));

				//去除上轮打出的牌
				cleanChuPai(outIndex);

				//显示打出的牌
				string[] card = chuStr.Split (',');
				if (card.Length == 1 && int.Parse (card [0]) == -1) {
					playerItems [outIndex].showYaoBuQi (true);
				} else {
					List<int> pai = new List<int> ();
					int count = 0;
					for (int i = 0; i < card.Length; i++) {
						GameObject gob = Instantiate (Resources.Load ("prefab/pdk/HandCard_s")) as GameObject;
						if (gob != null) {
							gob.transform.SetParent (ThrowTransformList [outIndex]);
							gob.GetComponent<pdkCardScript> ().setPoint (int.Parse (card [i]));
							gob.transform.localScale = new Vector3 (1f, 1f, 1);
							gob.transform.localPosition = new Vector3 (-200 + count * 25, 0, 0);
							count++;
							pai.Add (int.Parse (card [i]));
						}
					}

					lastCard = ToArray (pai);
				}
			}
		} catch (Exception ex) {
			MyDebug.Log (ex.ToString ());
		}
	}

	public void ChiBuQiCallBack (ClientResponse response)
	{
		cleanClock ();
		UpdateTimeReStart ();
		JsonData json = JsonMapper.ToObject (response.message);
		int type = (int)json ["type"];
		if (type == 0) {//吃不起
			int AvatarIndex = (int)json ["AvatarIndex"];
			int outIndex = getIndexByDir (getDirection (AvatarIndex));

			//去除上轮打出的牌
			cleanChuPai(outIndex, true);

			if (outIndex == 0) {//自己
				currentIndex = 1;
			} else {//别人
				if (outIndex == 2) {//上家
					currentIndex = 0;
				} else if (outIndex == 1) {//下家
					currentIndex = 2;
				}
			}

			cleanChuPai (currentIndex);
		} else if (type == 1) {//接上家 继续出牌的提示
			btnActionScript.showBtn ();

			//去除上轮打出的牌
			cleanChuPai(0);

			TiShi ();//智能自动弹出能出的牌
		} else if (type == 2) {//都吃不起，开始新一轮出牌的提示
			lastCard = null;

			int AvatarIndex = (int)json ["AvatarIndex"];
			int outIndex = getIndexByDir (getDirection (AvatarIndex));

			if (outIndex == 0) {
				btnActionScript.showBtn ();
			}

			//去除上轮打出的牌
			for (int i = 0; i < 3; i++) {
				cleanChuPai (i);
			}
		}
	}

	public void serviceErrorNotice (ClientResponse response)
	{
		TipsManagerScript.getInstance ().setTips (response.message);
	}

	public void TiShi ()
	{ 
		List<int> pai = new List<int> ();
		for (int i =handerCardList [0].Count - 1; i >= 0; i--) {
			GameObject gob = handerCardList [0] [i];
			if (gob != null) {
				pai.Add (gob.GetComponent<pdkCardScript> ().getPoint ());
			}
		}

		if (lastCard == null || lastCard.Length == 0) {//庄家第一手出牌
			
		} else {//吃上家或上上家的牌
			showTiShi (lastCard, ToArray (pai));
		}
	}

	private bool isClickTiShi = false;
	private int tishiIndex = 0;
	List<int[]> result = new List<int[]> ();

	public void showTiShi (int[] chuCard_old, int[] myCardArray_old)
	{
		cardReset ();//牌全部先复位
		int[] chuCard = new int[chuCard_old.Length];
		for (int i = 0; i < chuCard_old.Length; i++) {
            if(chuCard_old[i] <52){
                chuCard[i] = chuCard_old[i] % 13;
            }                                 //新增大小王  需要判断lan
            else {
                chuCard[i] = chuCard_old[i];
            }
		}
		// 从小到大排序
		Array.Sort (chuCard);

		int[] myCardArray = new int[myCardArray_old.Length];
		for (int i = 0; i < myCardArray_old.Length; i++) {
            if(myCardArray_old[i]<52){
                myCardArray[i] = myCardArray_old[i] % 13;
            }                                 //新增大小王  需要判断lan
            else {
                myCardArray[i] = myCardArray_old[i];
            }
		}

		// 从小到大排序
		Array.Sort (myCardArray);

		if (!isClickTiShi) {
			result = returnResult (chuCard, myCardArray);//玩家手牌返回所有大于当前出牌的链表   todo
            print(" showTiShi ++++++++" + result);
			tishiIndex = 0;
			isClickTiShi = true;
		}

		if (result == null || result.Count == 0)
			return;

		pdkCardType pct = new pdkCardType ();
		pdkCardType.CARDTYPE type1 = pct.getType (chuCard_old);

        if (type1 == pdkCardType.CARDTYPE.c1
            || type1 == pdkCardType.CARDTYPE.c2
            || type1 == pdkCardType.CARDTYPE.c4) {//单牌、双牌、炸弹

            int[] re = result[tishiIndex];
            print("showTiShi ++++++++ //单牌、双牌、炸弹" + re);
            int count = 0;
            for (int i = handerCardList[0].Count - 1; i >= 0; i--) {
                GameObject gob = handerCardList[0][i];
                if (gob != null) {
                    pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                    if (pcs.getPoint() % 13 == re[1]) {
                        gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                        pcs.selected = true;
                        count++;
                        if (count >= re[0])
                            break;
                    }
                }
            }
        } else if (type1 == pdkCardType.CARDTYPE.c311) {//3带11
            int[] re = result[tishiIndex];
            print("showTiShi ++++++++ //3带11 " + re);
            int count = 0;
            for (int i = handerCardList[0].Count - 1; i >= 0; i--) {
                GameObject gob = handerCardList[0][i];
                if (gob != null) {
                    pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                    if (pcs.getPoint() % 13 == re[1]) {
                        gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                        pcs.selected = true;
                        count++;
                        if (count >= re[0])
                            break;
                    }
                }
            }
            if (re[0] == 3) {//要2个杂牌起立
                count = 0;
                for (int i = handerCardList[0].Count - 1; i >= 0; i--) {
                    GameObject gob = handerCardList[0][i];
                    if (gob != null) {
                        pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                        if (!pcs.selected) {
                            gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                            pcs.selected = true;
                            count++;
                            if (count >= 2)
                                break;
                        }
                    }
                }
            }
        }
        else if(type1 == pdkCardType.CARDTYPE.c111122)    // 四带二的
        {
            int[] re = result[tishiIndex];
            int count = 0;
            for (int i = handerCardList[0].Count - 1; i >= 0; i--)
            {
                GameObject gob = handerCardList[0][i];
                if (gob != null)
                {
                    pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                    if (pcs.getPoint() % 13 == re[1])
                    {
                        gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                        pcs.selected = true;
                        count++;
                        if (count >= re[0])
                            break;
                    }
                }
            }
            if (re[0] == 6){//要2个杂牌起立  lan
                count = 0;
                for (int i = handerCardList[0].Count - 1; i >= 0; i--)
                {
                    GameObject gob = handerCardList[0][i];
                    if (gob != null)
                    {
                        pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                        if (!pcs.selected)
                        {
                            gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                            pcs.selected = true;
                            count++;
                            if (count >= 2)
                                break;
                        }
                    }
                }
            }
        }
        else if (type1 == pdkCardType.CARDTYPE.c12345) {//顺子
            int[] re = result[tishiIndex];
            print("showTiShi ++++++++ //顺子 " + re);
            if (re[0] == 4) {//炸弹
                int count = 0;
                for (int i = handerCardList[0].Count - 1; i >= 0; i--) {
                    GameObject gob = handerCardList[0][i];
                    if (gob != null) {
                        pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                        if (pcs.getPoint() % 13 == re[0]) {
                            gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                            pcs.selected = true;
                            count++;
                            if (count >= re[0])
                                break;
                        }
                    }
                }
            } else {//顺子
                for (int j = 1; j < re.Length; j++) {
                    for (int i = handerCardList[0].Count - 1; i >= 0; i--) {
                        GameObject gob = handerCardList[0][i];
                        if (gob != null) {
                            pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                            if (pcs.getPoint() % 13 == re[j]) {
                                gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                                pcs.selected = true;
                                break;
                            }
                        }
                    }
                }
            }
        } else if (type1 == pdkCardType.CARDTYPE.c112233) {//连对
            int[] re = result[tishiIndex];

            if (re[0] == 4 && re.Length == 2) {//炸弹
                int count = 0;
                for (int i = handerCardList[0].Count - 1; i >= 0; i--) {
                    GameObject gob = handerCardList[0][i];
                    if (gob != null) {
                        pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                        if (pcs.getPoint() % 13 == re[0]) {
                            gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                            pcs.selected = true;
                            count++;
                            if (count >= re[0])
                                break;
                        }
                    }
                }
            } else {//连对
                for (int j = 1; j < re.Length; j++) {
                    int count = 0;
                    for (int i = handerCardList[0].Count - 1; i >= 0; i--) {
                        GameObject gob = handerCardList[0][i];
                        if (gob != null) {
                            pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                            if (pcs.getPoint() % 13 == re[j]) {
                                gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                                pcs.selected = true;
                                if (++count >= 2)
                                    break;
                            }
                        }
                    }
                }
            }
        } else if (type1 == pdkCardType.CARDTYPE.c1112223456) {//飞机带翅膀
            int[] re = result[tishiIndex];
            if (re[0] == 4 && re.Length == 2) {//炸弹
                int count = 0;
                for (int i = handerCardList[0].Count - 1; i >= 0; i--) {
                    GameObject gob = handerCardList[0][i];
                    if (gob != null) {
                        pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                        if (pcs.getPoint() % 13 == re[0]) {
                            gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                            pcs.selected = true;
                            count++;
                            if (count >= re[0])
                                break;
                        }
                    }
                }
            } else {//飞机
                for (int j = 1; j < re.Length; j++) {
                    int count = 0;
                    for (int i = handerCardList[0].Count - 1; i >= 0; i--) {
                        GameObject gob = handerCardList[0][i];
                        if (gob != null) {
                            pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                            if (pcs.getPoint() % 13 == re[j]) {
                                gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                                pcs.selected = true;
                                if (++count >= 3)
                                    break;
                            }
                        }
                    }
                }

                //起立几个杂牌
                int counts = 0;
                for (int i = handerCardList[0].Count - 1; i >= 0; i--) {
                    GameObject gob = handerCardList[0][i];
                    if (gob != null) {
                        pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                        if (!pcs.selected) {
                            gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                            pcs.selected = true;
                            if (++counts >= (re.Length - 1) * 2)
                                break;
                        }
                    }
                }
            }
        }

		isCanChu ();

		tishiIndex++;
		if (tishiIndex >= result.Count)
			tishiIndex = 0;
	}

	//返回所有可以大过 当前所出牌的链表
	public List<int[]> returnResult (int[] chuCard, int[] myCardArray)
	{
		List<int[]> result = new List<int[]> ();

		pdkCardType pct = new pdkCardType ();
		pdkCardType.CARDTYPE type1 = pct.getType (chuCard);

		List<int>[] a = pct.getCount (chuCard);
		List<int>[] b = pct.getCount (myCardArray);
        print("returnResultreturnResultreturnResultreturnResult " + a + b);

		int length = myCardArray.Length;

        if (type1 == pdkCardType.CARDTYPE.c1)
        {//单牌

            //单张
            for (int k = 0; k < b[0].Count; k++)
            {
                int[] re = new int[2];
                if (b[0][k] > chuCard[0])
                {
                    re[1] = b[0][k];
                    re[0] = 1;
                    result.Add(re);
                }
            }

            //双张
            for (int k = 0; k < b[1].Count; k++)
            {
                int[] re = new int[2];
                if (b[1][k] > chuCard[0])
                {
                    re[1] = b[1][k];
                    re[0] = 1;
                    result.Add(re);
                }
            }

            //三张
            for (int k = 0; k < b[2].Count; k++)
            {
                int[] re = new int[2];
                if (b[2][k] > chuCard[0])
                {
                    re[1] = b[2][k];
                    re[0] = 1;
                    result.Add(re);
                }
            }

            //4张        //临汾斗地主 特殊的炸弹两张3，两张2    是否可以放在这里？？？
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                if (b[3][k] > chuCard[0])
                {
                    re[1] = b[3][k];
                    re[0] = 1;
                    result.Add(re);
                }
            }

            //炸弹
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                re[1] = b[3][k];
                re[0] = 4;
                result.Add(re);
            }
        }
        else if (type1 == pdkCardType.CARDTYPE.c2)
        {// 对子
         //双张
            for (int k = 0; k < b[1].Count; k++)
            {
                int[] re = new int[2];
                if (b[1][k] > chuCard[0])
                {
                    re[1] = b[1][k];
                    re[0] = 2;
                    result.Add(re);
                }
            }

            //三张
            for (int k = 0; k < b[2].Count; k++)
            {
                int[] re = new int[2];
                if (b[2][k] > chuCard[0])
                {
                    re[1] = b[2][k];
                    re[0] = 2;
                    result.Add(re);
                }
            }

            //4张
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                if (b[3][k] > chuCard[0])
                {
                    re[1] = b[3][k];
                    re[0] = 2;
                    result.Add(re);
                }
            }
            //炸弹
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                re[1] = b[3][k];
                re[0] = 4;
                result.Add(re);
            }
        }
        else if (type1 == pdkCardType.CARDTYPE.c112233)
        {//连对

            //循环遍历玩家手牌 是否有大于出牌的
            for (int i = (int)a[1][0] + 1; i < 13; i++)
            {
                int findCount = 0;
                int[] re = new int[a[1].Count + 1];
                re[0] = a[1].Count;
                for (int j = 0; j < chuCard.Length / 2; j++)
                {
                    //遍历2张的。
                    for (int k = 0; k < b[1].Count; k++)
                    {
                        if ((int)b[1][k] != 12 && (int)b[1][k] == i + j)
                        {
                            findCount++;
                            re[findCount] = b[1][k];
                            break;
                        }
                    }
                    //遍历3张的。
                    for (int k = 0; k < b[2].Count; k++)
                    {
                        if ((int)b[2][k] != 12 && (int)b[2][k] == i + j)
                        {
                            findCount++;
                            re[findCount] = b[2][k];
                            break;
                        }
                    }
                    //遍历4张的。
                    for (int k = 0; k < b[3].Count; k++)
                    {
                        if ((int)b[3][k] != 12 && (int)b[3][k] == i + j)
                        {
                            findCount++;
                            re[findCount] = b[3][k];
                            break;
                        }
                    }
                }

                if (findCount >= chuCard.Length / 2)
                {
                    result.Add(re);
                }
            }

            //炸弹
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                re[1] = b[3][k];
                re[0] = 4;
                result.Add(re);
            }

        }
        else if (type1 == pdkCardType.CARDTYPE.c311)
        {//三带2

            //三张
            for (int k = 0; k < b[2].Count; k++)
            {
                int[] re = new int[2];
                if (b[2][k] > a[2][0])
                {
                    re[1] = b[2][k];
                    re[0] = 3;
                    result.Add(re);
                }
            }

            //4张
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                if (b[3][k] > a[2][0])
                {
                    re[1] = b[3][k];
                    re[0] = 3;
                    result.Add(re);
                }
            }

            //炸弹
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                re[1] = b[3][k];
                re[0] = 4;
                result.Add(re);
            }

        }
        else if (type1 == pdkCardType.CARDTYPE.c1112223456)
        {//飞机带翅膀

            //循环遍历玩家手牌 是否有大于出牌的
            for (int i = (int)a[2][0] + 1; i < 13; i++)
            {
                int findCount = 0;
                int[] re = new int[a[2].Count + 1];
                re[0] = a[2].Count;
                for (int j = 0; j < chuCard.Length / 5; j++)
                {
                    //遍历3张的。
                    for (int k = 0; k < b[2].Count; k++)
                    {
                        if ((int)b[2][k] != 12 && (int)b[2][k] == i + j)
                        {
                            findCount++;
                            re[findCount] = b[2][k];
                            break;
                        }
                    }
                    //遍历4张的。
                    for (int k = 0; k < b[3].Count; k++)
                    {
                        if ((int)b[3][k] != 12 && (int)b[3][k] == i + j)
                        {
                            findCount++;
                            re[findCount] = b[3][k];
                            break;
                        }
                    }
                }

                if (findCount >= chuCard.Length / 5)
                {
                    result.Add(re);
                }

            }

            //炸弹
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                re[1] = b[3][k];
                re[0] = 4;
                result.Add(re);
            }
        }
        else if (type1 == pdkCardType.CARDTYPE.c12345)
        {//顺子

            //循环遍历玩家手牌 是否有大于出牌的
            for (int i = chuCard[0] + 1; i < 13; i++)
            {
                int[] re = new int[chuCard.Length + 1];
                re[0] = chuCard.Length;
                int findCount = 0;
                for (int j = 0; j < chuCard.Length; j++)
                {
                    for (int k = 0; k < length; k++)
                    {
                        if (myCardArray[k] != 12 && myCardArray[k] == i + j)
                        {
                            findCount++;
                            re[findCount] = myCardArray[k];
                            break;
                        }
                    }
                }

                if (findCount >= chuCard.Length)
                {
                    result.Add(re);
                }
            }

            //炸弹
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                re[1] = b[3][k];
                re[0] = 4;
                result.Add(re);
            }
        }
        else if (type1 == pdkCardType.CARDTYPE.c4){//炸弹
            //炸弹
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                if (b[3][k] > chuCard[0])
                {
                    re[1] = b[3][k];
                    re[0] = 4;
                    result.Add(re);
                }
            }
        }
        else if (type1 == pdkCardType.CARDTYPE.c111122) {//四带二lan

            //4张
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                if (b[3][k] > a[3][0])
                {
                    re[1] = b[3][k];
                    re[0] =6;            //  四带二的指针
                    result.Add(re);
                }
            }
            //炸弹
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                re[1] = b[3][k];
                re[0] = 4;
                result.Add(re);
            }
        }
        return result;
	}

	//自己出牌
	public void putOutCard ()
	{
		pdkCardVO cardvo = new pdkCardVO ();
		List<int> pai = new List<int> ();
		for (int i = 0; i < handerCardList [0].Count; i++) {
			GameObject gob = handerCardList [0] [i];
			if (gob != null) {
                print("+ _____putOutCard "+ gob.GetComponent<pdkCardScript>().selected);
				if (gob.GetComponent<pdkCardScript> ().selected) {
					pai.Add (gob.GetComponent<pdkCardScript> ().getPoint ());
                    playerItems[getMyIndexFromList()].hanCards.Remove(gob.GetComponent<pdkCardScript>().getPoint());
                    handerCardList [0].Remove (gob);                              
					Destroy (gob);
					i--;
				}
			}
		}

		int outIndex = 0;
		//去除上轮打出的牌
		cleanChuPai(0);
		//显示打出的牌
		List<int> card = pai;
		int count = 0;
		for (int i = 0; i < card.Count; i++) {
			GameObject gob = Instantiate (Resources.Load ("prefab/pdk/HandCard_s")) as GameObject;
			if (gob != null) {
				gob.transform.SetParent (ThrowTransformList [outIndex]);
				gob.GetComponent<pdkCardScript> ().setPoint (card [i]);
				gob.transform.localScale = new Vector3 (1f, 1f, 1);
				gob.transform.localPosition = new Vector3 (-200 + count * 25, 0, 0);
				count++;
			}
		}

		if (pai.Count == 0)
			return;
		
		SetPosition ();

		cardvo.card = ToArray (pai);
		CustomSocket.getInstance ().sendMsg (new PDK_PutOutCardRequest (cardvo));
		btnActionScript.cleanBtnShow ();

		isClickTiShi = false;
	}

	public void cleanClock ()
	{
		foreach (PlayerItemScript pis in playerItems) {
			pis.showClock (false);
		}
	}

	/// <summary>
	/// 接收到其它人的出牌通知
	/// </summary>
	/// <param name="response">Response.</param>
	public void putOutCardCallback (ClientResponse response)
	{	
		cleanClock ();
		UpdateTimeReStart ();
		putOutCardVO cardvo = JsonMapper.ToObject<putOutCardVO> (response.message);
		int outIndex = getIndexByDir (getDirection (cardvo.curAvatarIndex));

		//如果是自己出牌，去掉自己的手牌
		if (outIndex == 0) {
			currentIndex = 1;
		} else {//别人出牌
			//去除上轮打出的牌
			cleanChuPai(outIndex);
			//显示打出的牌
			List<int> card = cardvo.card;
			lastCard = ToArray (card);
			int count = 0;
			for (int i = 0; i < card.Count; i++) {
				GameObject gob = Instantiate (Resources.Load ("prefab/pdk/HandCard_s")) as GameObject;
				if (gob != null) {
					gob.transform.SetParent (ThrowTransformList [outIndex]);
					gob.GetComponent<pdkCardScript> ().setPoint (card [i]);
					gob.transform.localScale = new Vector3 (1f, 1f, 1);
					gob.transform.localPosition = new Vector3 (-200 + count * 25, 0, 0);
					count++;
				}
			}

			int oldCount = int.Parse (playerItems [outIndex].paiCountText.text);
			playerItems [outIndex].showPaiCountText (oldCount - card.Count);

			if (outIndex == 2) {//上家出牌
				currentIndex = 0;
			} else if (outIndex == 1) {//下家出牌
				currentIndex = 2;
			}
		}

		//去除下家上轮打出的牌
		cleanChuPai(currentIndex);
	}

	//去除打出的牌
	public void cleanChuPai(int outIndex, bool Yaobuqi = false){
		for (int i = 0; i < ThrowTransformList [outIndex].childCount; i++) {
			GameObject go = ThrowTransformList [outIndex].GetChild (i).gameObject;
			Destroy (go);
		}
		playerItems [outIndex].showYaoBuQi (Yaobuqi);
	}

	public void SetPosition ()//设置位置
    {
        cleanList(handerCardList[0]);
        int index = getMyIndexFromList();
        int count = 0;
        for (int i=0;i< playerItems[index].hanCards.Count;i++)
        {
            GameObject gob = Instantiate(Resources.Load("prefab/pdk/HandCard")) as GameObject;
            if (gob != null)
            {
                gob.GetComponent<pdkCardScript>().onDragSelect += dragSelect;
                gob.GetComponent<pdkCardScript>().onCardSelect += cardSelect;
                //gob.transform.SetParent (handTransformList [0]);                     
                gob.transform.SetParent(playerItems[index].handCard);
                gob.GetComponent<pdkCardScript>().setPoint(playerItems[index].hanCards[i]);
                gob.transform.localScale = Vector3.one;
                gob.transform.localPosition = new Vector3(-370 + count * 50f, 0, 0);
                handerCardList[0].Add(gob);// 手牌的增加
                count++;
            }
        }  
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="response">Response.</param>
    public void startGame (ClientResponse response)
	{
		GlobalDataScript.roomAvatarVoList = avatarList;
		StartGameVO sgvo = JsonMapper.ToObject<StartGameVO> (response.message);
		//bankerId = sgvo.bankerId;           原版是开始游戏时候就确定谁是庄家   -lan
		GlobalDataScript.roomVo.guiPai = sgvo.gui;
		GlobalDataScript.roomVo.guiPai2 = sgvo.gui2;

		cleanGameplayUI ();

		//开始游戏后不显示
		MyDebug.Log ("startGame");
		GlobalDataScript.surplusTimes--;
		//curDirString = getDirection (bankerId);            -lan

		//if (!isFirstOpen) {                                -lan
		//	avatarList [bankerId].main = true;
		//}

		GlobalDataScript.finalGameEndVo = null;
		//GlobalDataScript.mainUuid = avatarList [0].account.uuid;
		initArrayList ();
		//playerItems [curDirIndex].setbankImgEnable (true);     -lan
		//SetDirGameObjectAction (bankerId);
		isFirstOpen = false;
		GlobalDataScript.isOverByPlayer = false;

		mineList = sgvo.paiArray;
        deskList = sgvo.lastPaiArray;                                 // -桌面上四张牌的数值在游戏开始时赋值
		//UpateTimeReStart ();

		setAllPlayerReadImgVisbleToFalse ();
		//if (GlobalDataScript.roomVo.zhang16) {
			initMyCardListAndOtherCard (16, 16, 16);   // 每个人发16张牌
		//} else {
			//initMyCardListAndOtherCard (15, 15, 15);
		//}
        // 只有庄家才能有提示出牌
		//if (curDirString == DirectionEnum.Bottom) {
		//	btnActionScript.showBtn ();
		//	GlobalDataScript.isDrag = true;
		//} else {
		//	GlobalDataScript.isDrag = false;
		//}

		UpdateTimeReStart ();
		isGameStart = true;
		currentIndex = curDirIndex;
        
        //在第一局开始的时候是房主作为第一个抢地主的玩家，其次开始轮转
        //在开始阶段需要服务器返回一个位置  作为可抢地主的玩家索引  
        if (sgvo.GrabAvatarIndex == getMyIndexFromList())
        {
            panel_landlordChoose.SetActive(true);
        }
    }

    private void initArrayList ()      //  -change -lan
    {

		mineList = new List<List<int>> ();
        deskList = new List<List<int>>();                //桌面牌值的初始化
        handCards = new List<int>();
        landlord_deskCardList = new List<GameObject>();  //桌面牌的初始化
        handerCardList = new List<List<GameObject>> ();
		//tableCardList = new List<List<GameObject>> ();
		for (int i = 0; i < 3; i++) {
            handerCardList.Add (new List<GameObject> ());
			//tableCardList.Add (new List<GameObject> ());
		}
	}

	private void initMyCardListAndOtherCard (int topCount, int leftCount, int rightCount)
	{
      
        displaySelfhanderCard ();
        AddDeskCard();

        initOtherCardList (DirectionEnum.Left, leftCount);
		initOtherCardList (DirectionEnum.Right, rightCount);
	}

	private void displaySelfhanderCard ()            
    {
        int index = getMyIndexFromList();
       
        List<int> sort = new List<int>();
        //for (int i = 12; i >= 0; i--) {
        //for (int j = 0; j < 4; j++) {
        for (int point = 0; point < 54; point++) {          
              //int point = i + 13 * j;
            if (mineList[0][point] == 1) {
                playerItems[index].hanCards.Add(point);
           	    }
            playerItems[index].hanCards.Sort(delegate (int a, int b)
            {
                if(a <52 && b<52)
                {
                    return (a % 13).CompareTo(b % 13);
                }
                else
                {
                    return (a.CompareTo(b));
                }
            });
        }       
        SetPosition ();
	}

    private void AddDeskCard()      // 桌面牌的增加
    {
        for (int point = 0; point < 54; point++)
        {
            if (deskList[0][point] == 1)
            {
                GameObject gob_back = Instantiate(Resources.Load("prefab/pdk/HandCard_Back")) as GameObject;
                if (gob_back != null)
                {
                    gob_back.transform.SetParent(landlord_TransformList);
                    gob_back.GetComponent<pdkCardScript>().setPoint(point);
                    landlord_deskCardList.Add(gob_back);
                }
            }
        }
    }


    /**显示其他人的手牌**/
    private void displayOtherHandercard ()
	{
		for (int i = 0; i < GlobalDataScript.reEnterRoomData.playerList.Count; i++) {
			string dir = getDirection (getIndex (GlobalDataScript.reEnterRoomData.playerList [i].account.uuid));
			int count = GlobalDataScript.reEnterRoomData.playerList [i].commonCards;
			if (dir != DirectionEnum.Bottom) {
				initOtherCardList (dir, count);
			}
		}
	}

    /* 庄家增加四张牌*/
    void bankerAddCard(int bankerIndex)
    {
        // 首先要消失所有的手牌   
        cleanList(handerCardList[0]);
        for (int t = 0; t < landlord_deskCardList.Count; t++)
        {
            int point = landlord_deskCardList[t].GetComponent<pdkCardScript>().getPoint();
            playerItems[bankerIndex].hanCards.Add(point);
            print(" bankerAddCard ---------------" + t);                               
            //landlord_deskCardList.Remove(landlord_deskCardList[t]);
            Destroy(landlord_deskCardList[t]);

            playerItems[bankerIndex].hanCards.Sort(delegate (int a, int b)
            {
                if (a < 52 && b < 52)
                {
                    return (a % 13).CompareTo(b % 13);
                }
                else
                {
                    return (a.CompareTo(b));
                }
            });
        }
        SetPosition();
        playerItems[bankerIndex].showPaiCountText(int.Parse(playerItems[bankerIndex].paiCountText.text) + 4);
    }

    IEnumerator FanPai(float time, GameObject go)
    {
        yield return new WaitForSeconds(time);
        if (go.GetComponent<pdkCardScript>().cardPoint != -1)
        {
            go.GetComponent<pdkCardScript>().startPlay();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="initDirection"></param>
    private void initOtherCardList (string initDiretion, int count) //初始化
	{
		switch (initDiretion) {
		case DirectionEnum.Left: //左
			playerItems [2].showPaiCountText (count);
			break;
		case DirectionEnum.Right: //右
			playerItems [1].showPaiCountText (count);
			break;
		}
	}


	/// <summary>
	/// Cards the select.
	/// </summary>
	/// <param name="obj">Object.</param>
	public void dragSelect (GameObject gameobject)
	{
		for (int i = 0; i <handerCardList [0].Count; i++) {
			GameObject obj =handerCardList [0] [i];
			if (obj != null) {
				if (obj.GetComponent<pdkCardScript> ().dragFlag) {
					pdkCardScript script = obj.transform.GetComponent<pdkCardScript> ();
					if (script.selected == false) {
						obj.transform.localPosition = new Vector3 (obj.transform.localPosition.x, 40);
						script.selected = true;
					} else {
						obj.transform.localPosition = new Vector3 (obj.transform.localPosition.x, 0);
						script.selected = false;
					}
					obj.GetComponent<pdkCardScript> ().dragFlag = false;
				}
			}
		}

		isCanChu ();
	}

	/// <summary>
	/// Cards the select.
	/// </summary>
	/// <param name="obj">Object.</param>
	public void cardSelect (GameObject obj)
	{
		if (obj != null) {
			pdkCardScript script = obj.transform.GetComponent<pdkCardScript> ();
			if (script.selected == false) {
				obj.transform.localPosition = new Vector3 (obj.transform.localPosition.x, 40);
				script.selected = true;
			} else {
				obj.transform.localPosition = new Vector3 (obj.transform.localPosition.x, 0);
				script.selected = false;
			}
		}

		isCanChu ();
	}

	public void isCanChu ()
	{
		List<int> pai = new List<int> ();
        for (int i = 0; i < handerCardList [0].Count; i++) {
			GameObject gob =handerCardList [0] [i];
			if (gob != null) {
				if (gob.GetComponent<pdkCardScript> ().selected) {
					pai.Add (gob.GetComponent<pdkCardScript> ().getPoint ());
				}
			}
		}

		int[] card = ToArray (pai);

		pdkCardType pct = new pdkCardType ();
		if (lastCard == null || lastCard.Length == 0) {	//庄家第一手出牌
			pdkCardType.CARDTYPE type = pct.getType (card);
			if (type == pdkCardType.CARDTYPE.c0) {
				if (btnActionScript.tishiBtn.active == true) {
					btnActionScript.showBtn (2);
				}
			} else if (type == pdkCardType.CARDTYPE.c3
			           || type == pdkCardType.CARDTYPE.c31
			           || type == pdkCardType.CARDTYPE.c111222N) {

				if (btnActionScript.tishiBtn.active == true) {
					if (handerCardList [0].Count == pai.Count)//最后一手牌
							btnActionScript.showBtn (3);
					else
						btnActionScript.showBtn (2);
				}
				
			} else {
				if (btnActionScript.tishiBtn.active == true) {
					if (GlobalDataScript.roomVo.xian3) {
						bool isFind3 = false;
						foreach (int p in card) {
							if (p == 0)
								isFind3 = true;
						}

						if (GlobalDataScript.roomVo.zhang16) {
							if (handerCardList [0].Count == 16) {
								if (!isFind3 && GlobalDataScript.totalTimes-GlobalDataScript.surplusTimes==1) {
									btnActionScript.showBtn (2);
									return;
								}
							}
						} else {
							if (handerCardList [0].Count == 15) {
								if (!isFind3 && GlobalDataScript.totalTimes-GlobalDataScript.surplusTimes==1) {
									btnActionScript.showBtn (2);
									return;
								}
							}
						}
						btnActionScript.showBtn (3);
					} else {
						btnActionScript.showBtn (3);
					}
				}
			}
		} else if (pct.compareType (lastCard, card)) {
			if (btnActionScript.tishiBtn.active == true) {
				btnActionScript.showBtn (3);
			}
		} else {
			if (btnActionScript.tishiBtn.active == true) {
				btnActionScript.showBtn ();
			}
		}
	}

	//*
	//所有牌复位
	//*
	public void cardReset ()
	{
		for (int i = 0; i < handerCardList [0].Count; i++) {
			if (handerCardList [0] [i] == null) {
                handerCardList [0].RemoveAt (i);
				i--;
			} else {
                handerCardList [0] [i].transform.localPosition = new Vector3 (handerCardList [0] [i].transform.localPosition.x, 0);
                handerCardList [0] [i].transform.GetComponent<pdkCardScript> ().selected = false;
                handerCardList [0] [i].transform.GetComponent<pdkCardScript> ().dragFlag = false;
			}
		}
	}

	private void cleanGameplayUI ()
	{
		canClickButtonFlag = true;
		//Number.gameObject.SetActive (true);
		inviteFriendButton.transform.gameObject.SetActive (false);
	}

	private void setAllPlayerReadImgVisbleToFalse ()
	{
		for (int i = 0; i < playerItems.Count; i++) {
			playerItems [i].readyImg.enabled = false;
		}
	}

	public void gameReadyNotice (ClientResponse response)
	{
		JsonData json = JsonMapper.ToObject (response.message);
        int avatarIndex = Int32.Parse (json ["avatarIndex"].ToString ());
		int myIndex = getMyIndexFromList ();
		int seatIndex = avatarIndex - myIndex;
		if (seatIndex < 0) {
			seatIndex = 3 + seatIndex;
		}

		//自己
		if (avatarIndex == myIndex) {
			markselfReadyGame ();
		}

		this.playerItems [seatIndex].readyImg.enabled = true;
		avatarList [avatarIndex].isReady = true;

		string readyUid = json ["readyUid"].ToString ();
		//20170310 增加一句结算时 某玩家断线后重连 看不到他人准备 
		for (int i = 0; i < avatarList.Count; i++) {
			if (readyUid.IndexOf (avatarList [i].account.uuid.ToString ()) != -1) {
				avatarList [i].isReady = true;
				setSeat (avatarList [i]);
			}
		}

		SoundCtrl.getInstance ().playSoundByActionButton (3);
	}

	/*显示自己准备*/
	public void markselfReadyGame ()
	{
		playerItems [0].readyImg.enabled = true;
		readyBtn.gameObject.SetActive (false);
	}

	/**
    *准备游戏
	*/
	public void  readyGame ()
	{
		CustomSocket.getInstance ().sendMsg (new GameReadyRequest ());
	}

	public void messageBoxNotice (ClientResponse response)
	{
		string[] arr = response.message.Split (new char[1]{ '|' });
		if (int.Parse (arr [0]) == 1) {
			int uuid = int.Parse (arr [2]);
			int myIndex = getMyIndexFromList ();
			int curAvaIndex = getIndex (uuid);
			int seatIndex = curAvaIndex - myIndex;
			if (seatIndex < 0) {
				seatIndex = 3 + seatIndex;
			}            

			int code = int.Parse (arr [1]);
			//传输性别  大于3000为女
			if (code > 3000) {
				code = code - 3000;
				playerItems [seatIndex].showChatMessage (code);
				SoundCtrl.getInstance ().playMessageBoxSound (code, 0);
			} else {
				code = code - 1000;
				playerItems [seatIndex].showChatMessage (code);
				SoundCtrl.getInstance ().playMessageBoxSound (code, 1);
			}
		} else if (int.Parse (arr [0]) == 2) {
			int uuid = int.Parse (arr [2]);
			int myIndex = getMyIndexFromList ();
			int curAvaIndex = getIndex (uuid);
			int seatIndex = curAvaIndex - myIndex;
			if (seatIndex < 0) {
				seatIndex = 3 + seatIndex;
			}
			playerItems [seatIndex].showChatMessage (arr [1]);
		} else if (int.Parse (arr [0]) == 3) {
			int uuid = int.Parse (arr [2]);
			int myIndex = getMyIndexFromList ();
			int curAvaIndex = getIndex (uuid);
			int seatIndex = curAvaIndex - myIndex;
			if (seatIndex < 0) {
				seatIndex = 3 + seatIndex;
			}
			playerItems [seatIndex].showBiaoQing (bqScript.getBiaoqing (int.Parse (arr [1]) - 1));
		} else if(int.Parse (arr [0]) == 4){
			int srcuuid = int.Parse (arr [2]);
			int destuuid = int.Parse (arr [3]);
			int myIndex = getMyIndexFromList ();
			int srcAvaIndex = getIndex (srcuuid);
			int destAvaIndex = getIndex (destuuid);

			int destSeatIndex = destAvaIndex - myIndex;
			if (destSeatIndex < 0) {
				destSeatIndex = 3 + destSeatIndex;
			}
			int srcSeatIndex = srcAvaIndex - myIndex;
			if (srcSeatIndex < 0) {
				srcSeatIndex = 3 + srcSeatIndex;
			}
			//XXX
			playerItems [destSeatIndex].showMfbq (srcSeatIndex, destSeatIndex, arr[1]);
		}


	}

	public void setRoomRemark ()
	{
		RoomCreateVo roomvo = GlobalDataScript.roomVo;
		GlobalDataScript.totalTimes = roomvo.roundNumber;
		GlobalDataScript.surplusTimes = roomvo.roundNumber;//剩余多少局

		string roomInfo = "";
		if (GlobalDataScript.goldType) {
			roomInfo += "房间号<训练场>";
		} else {
			roomInfo += "房间号<" + roomvo.roomId + ">";
			roomInfo += "、局数<" + GlobalDataScript.roomVo.roundNumber + "局>";
		}

        roomInfo += "、炸弹数目：" + roomvo.bombMultiple;

        if(GlobalDataScript.roomVo.isKick ){
            roomInfo += "、踢牌";
        }
        else{
            roomInfo += "、不踢牌";
        }

        if(GlobalDataScript.roomVo.AA){
            roomInfo += "、房主付费";
        }
        else{
            roomInfo += "、AA付费";
        }


  //      if (GlobalDataScript.roomVo.zhang16)
		//	roomInfo += "、每人16张";
		//else
		//	roomInfo += "、每人15张";

		//if(GlobalDataScript.roomVo.showPai)
		//	roomInfo += "、显示牌";
		//else
		//	roomInfo += "、不显示牌";

		//if(GlobalDataScript.roomVo.xian3)
		//	roomInfo += "、首轮先出黑桃3";

		roomRemark.text = roomInfo;
	}

	public void otherUserJointRoom (ClientResponse response)
	{
		AvatarVO avatar = JsonMapper.ToObject<AvatarVO> (response.message);
		addAvatarVOToList (avatar);
	}

	public void outRoomCallbak (ClientResponse response)
	{
		OutRoomResponseVo responseMsg = JsonMapper.ToObject<OutRoomResponseVo> (response.message);
		if (responseMsg.status_code == "0") {
			if (responseMsg.type == "0") {

				int uuid = responseMsg.uuid;
				if (uuid != GlobalDataScript.loginResponseData.account.uuid) {
					int index = getIndex (uuid);
					avatarList.RemoveAt (index);

					for (int i = 0; i < playerItems.Count; i++) {
						playerItems [i].setAvatarVo (null);
					}

					if (avatarList != null) {
						for (int i = 0; i < avatarList.Count; i++) {
							setSeat (avatarList [i]);
						}
					}
				} else {
					exitOrDissoliveRoom ();
				}

			} else {
				exitOrDissoliveRoom ();
			}

		} else {            
			TipsManagerScript.getInstance ().setTips ("退出房间失败：" + responseMsg.error);
		}
	}

	public void exitOrDissoliveRoom ()
	{
		GlobalDataScript.loginResponseData.resetData ();//复位房间数据
		GlobalDataScript.loginResponseData.roomId = 0;//复位房间数据
		GlobalDataScript.roomJoinResponseData = null;//复位房间数据
		GlobalDataScript.roomVo.roomId = 0;
		GlobalDataScript.soundToggle = true;
		clean ();
		removeListener ();


		if (GlobalDataScript.homePanel != null) {
			GlobalDataScript.homePanel.SetActive (true);
			GlobalDataScript.homePanel.transform.SetSiblingIndex (1);
		} else {
			GlobalDataScript.homePanel = PrefabManage.loadPerfab ("Prefab/Panel_Home");
			GlobalDataScript.homePanel.transform.SetSiblingIndex (1);
		}

		while (playerItems.Count > 0) {
			PlayerItemScript item = playerItems [0];
			playerItems.RemoveAt (0);
			item.clean ();
			Destroy (item.gameObject);
			Destroy (item);
		}
		Destroy (this);
		Destroy (gameObject);
	}
		
	public void joinToRoom (List<AvatarVO> avatars)
	{
		avatarList = avatars;
		for (int i = 0; i < avatars.Count; i++) {
			setSeat (avatars [i]);
		}
		setRoomRemark ();
		//readyGame ();
		//markselfReadyGame ();
	}

	public void createRoomAddAvatarVO (AvatarVO avatar)
	{
		avatar.scores = 0;
		addAvatarVOToList (avatar);
		setRoomRemark ();
		//readyGame ();

		//markselfReadyGame ();
	}

	private void addAvatarVOToList (AvatarVO avatar)
	{
		if (avatarList == null) {
			avatarList = new List<AvatarVO> ();
		}
		avatarList.Add (avatar);
		setSeat (avatar);
	}

	/*************************断线重连*********************************/
	public void reEnterRoom ()
	{
		if (GlobalDataScript.reEnterRoomData != null) {
			//显示房间基本信息
			GlobalDataScript.roomVo.roundNumber = GlobalDataScript.reEnterRoomData.roundNumber;
			GlobalDataScript.roomVo.roomId = GlobalDataScript.reEnterRoomData.roomId;
			GlobalDataScript.roomVo.gameType = GlobalDataScript.reEnterRoomData.gameType;
			GlobalDataScript.roomVo.zhang16 = GlobalDataScript.reEnterRoomData.zhang16;
			GlobalDataScript.roomVo.showPai = GlobalDataScript.reEnterRoomData.showPai;
			GlobalDataScript.roomVo.xian3 = GlobalDataScript.reEnterRoomData.xian3;

			avatarList = GlobalDataScript.reEnterRoomData.playerList;
			GlobalDataScript.roomAvatarVoList = GlobalDataScript.reEnterRoomData.playerList;
			for (int i = 0; i < avatarList.Count; i++) {
                AvatarVO ava = avatarList[i];
                setSeat (avatarList [i]);
                //GlobalDataScript.mainUuid = avatarList [0].account.uuid;
                GlobalDataScript.mainUuid = ava.account.uuid;
                if (avatarList [i].main) {
					bankerId = i;
                    GlobalDataScript.mainUuid = ava.account.uuid;
                }

				if (avatarList [i].account.uuid == GlobalDataScript.loginResponseData.account.uuid
				    && avatarList [i].isReady) {
					readyBtn.gameObject.SetActive (false);
				}
			}

			//恢复房卡数据，此时主界面还没有load所以无需操作界面显示
			GlobalDataScript.loginResponseData.account.roomcard = GlobalDataScript.reEnterRoomData.playerList [getMyIndexFromList ()].account.roomcard;

			setRoomRemark ();

			int[][] selfPaiArray = GlobalDataScript.reEnterRoomData.playerList [getMyIndexFromList ()].paiArray;
			if (selfPaiArray == null || selfPaiArray.Length == 0) {//游戏还没有开始


			} else {//牌局已开始
				UpdateTimeReStart ();
				isGameStart = true;

				setAllPlayerReadImgVisbleToFalse ();
				cleanGameplayUI ();
				initArrayList ();

				//显示打牌数据
				//displayTableCards ();

				mineList = ToList (GlobalDataScript.reEnterRoomData.playerList [getMyIndexFromList ()].paiArray);
				displaySelfhanderCard ();//显示自己的手牌
				displayOtherHandercard ();//显示其他玩家手牌数

				//CustomSocket.getInstance ().sendMsg (new CurrentStatusRequest ());
			}
		}
	}

	/// <summary>
	/// 设置当前角色的座位
	/// </summary>
	/// <param name="avatar">Avatar.</param>
	private void setSeat (AvatarVO avatar)
	{
        //游戏结束后用的数据，勿删！！！
		//GlobalDataScript.palyerBaseInfo.Add (avatar.account.uuid, avatar.account);

		if (avatar.account.uuid == GlobalDataScript.loginResponseData.account.uuid) {
			playerItems [0].setAvatarVo (avatar);
		} else {
			int myIndex = getMyIndexFromList ();
			int curAvaIndex = avatarList.IndexOf (avatar);
			int seatIndex = curAvaIndex - myIndex;
			if (seatIndex < 0) {
				if (GlobalDataScript.roomVo.gameType == 0)
					seatIndex = 4 + seatIndex;
				else if (GlobalDataScript.roomVo.gameType == 1)
					seatIndex = 3 + seatIndex;
                else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.DDZ)            // 斗地主  7 
                    seatIndex = 3 + seatIndex;

            } 
			playerItems [seatIndex].setAvatarVo (avatar);
		}

	}

	/// <summary>
	/// Gets the index by dir.
	/// </summary>
	/// <returns>The index by dir.</returns>
	/// <param name="dir">Dir.</param>
	private int getIndexByDir (string dir)
	{
		int result = 0;
		switch (dir) {
		case DirectionEnum.Left: //左
			result = 2;
			break;
		case DirectionEnum .Right: //右
			result = 1;
			break;
		case DirectionEnum.Bottom: //下
			result = 0;
			break;
		}
		return result;
	}

	public int getIndex (int uuid)
	{
		if (avatarList != null) {
			for (int i = 0; i < avatarList.Count; i++) {
				if (avatarList [i].account != null) {
					if (avatarList [i].account.uuid == uuid) {
						return i;
					}
				}
			}
		}
		return 0;
	}

	/// <summary>
	/// Gets my index from list.
	/// </summary>
	/// <returns>The my index from list.</returns>
	public int getMyIndexFromList ()
	{
		if (avatarList != null) {
			for (int i = 0; i < avatarList.Count; i++) {
				if (avatarList [i].account.uuid == GlobalDataScript.loginResponseData.account.uuid
				    || avatarList [i].account.openid == GlobalDataScript.loginResponseData.account.openid) {
					GlobalDataScript.loginResponseData.account.uuid = avatarList [i].account.uuid;
					MyDebug.Log ("数据正常返回" + i);
					return i;
				}

			}
		}

		MyDebug.Log ("数据异常返回0");
		return 0;
	}

	/// <summary>
	/// 根据一个人在数组里的索引，得到这个人所在的方位，L-左，T-上,R-右，B-下（自己的方位永远都是在下方）
	/// </summary>
	/// <returns>The direction.</returns>
	/// <param name="avatarIndex">Avatar index.</param>
	private String getDirection (int avatarIndex)
	{
		String result = DirectionEnum.Bottom;
		int myselfIndex = getMyIndexFromList ();
		if (myselfIndex == avatarIndex) {
			MyDebug.Log ("getDirection == B");
			curDirIndex = 0;
			return result;
		}
		//从自己开始计算，下一位的索引
		for (int i = 0; i < 3; i++) {
			myselfIndex++;
			if (myselfIndex >= 3) {
				myselfIndex = 0;
			}
			if (myselfIndex == avatarIndex) {
				if (i == 0) {
					MyDebug.Log ("getDirection == R");
					curDirIndex = 1;
					return DirectionEnum.Right;
				} else if (i == 1) {
					MyDebug.Log ("getDirection == T");
					curDirIndex = 2;
					return DirectionEnum.Left;
				}
			}
		}
		MyDebug.Log ("getDirection == B");
		curDirIndex = 0;
		return DirectionEnum.Bottom;
	}


	void loadBiaoQing ()
	{
		bqObj = Instantiate (Resources.Load ("Prefab/Panel_biaoqing")) as GameObject;
		bqObj.transform.parent = this.gameObject.transform;
		bqObj.transform.localPosition = new Vector3 (0, 1600, 1);
		bqScript = bqObj.GetComponent<biaoqingScript> ();
	}

	public biaoqingScript getBqScript ()
	{
		return bqScript;
	}

	/*************************桌面常用按钮*********************************/
	/**
    * 邀请好友
    */
	public void inviteFriend ()
	{
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		GlobalDataScript.getInstance ().wechatOperate.inviteFriend ();
	}

	/**
    * 打开聊天窗口
    */
	public void openChatBox ()
	{
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		PrefabManage.loadPerfab ("Prefab/Panel_chatbox");
	}

	/**
    * 打开设置对话框
    */
	public void openGameSettingDialog ()
	{
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		loadPerfab ("Prefab/Panel_Setting");

		SettingScript ss = panelCreateDialog.GetComponent<SettingScript> ();
		if (canClickButtonFlag) {
			ss.canClickButtonFlag = canClickButtonFlag;
			ss.jiesanBtn.GetComponentInChildren<Text> ().text = "申请解散房间";
			ss.type = 2;            
		} else {
			if (0 == getMyIndexFromList ()) { //我是房主
				ss.canClickButtonFlag = canClickButtonFlag;
				ss.jiesanBtn.GetComponentInChildren<Text> ().text = "解散房间";
				ss.type = 3;
				ss.dialog_fanhui = dialog_fanhui;
				dialog_fanhui_text.text = "亲，确定要解散房间吗?";
			} else {
				ss.canClickButtonFlag = canClickButtonFlag;
				ss.jiesanBtn.GetComponentInChildren<Text> ().text = "离开房间";
				ss.type = 3;
				ss.dialog_fanhui = dialog_fanhui;
				dialog_fanhui_text.text = "亲，确定要离开房间吗?";
			}
		}
	}

	public void tuichu ()
	{      
		OutRoomRequestVo vo = new OutRoomRequestVo ();
		vo.roomId = GlobalDataScript.roomVo.roomId;
		string sendMsg = JsonMapper.ToJson (vo);

		CustomSocket.getInstance ().sendMsg (new OutRoomRequest (sendMsg));

		dialog_fanhui.gameObject.SetActive (false);


	}

	public void quxiao ()
	{      
		dialog_fanhui.gameObject.SetActive (false);
	}


	private GameObject panelCreateDialog;
	//界面上打开的dialog
	private void loadPerfab (string perfabName)
	{
		panelCreateDialog = Instantiate (Resources.Load (perfabName)) as GameObject;
		panelCreateDialog.transform.parent = gameObject.transform;
		panelCreateDialog.transform.localScale = Vector3.one;
		//panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
		panelCreateDialog.GetComponent<RectTransform> ().offsetMax = new Vector2 (0f, 0f);
		panelCreateDialog.GetComponent<RectTransform> ().offsetMin = new Vector2 (0f, 0f);
	}

	public void myselfSoundActionPlay ()
	{
		playerItems [0].showChatAction ();
	}

	private List<List<int>> ToList (int[][] param)
	{
		List<List<int>> returnData = new List<List<int>> ();
		for (int i = 0; i < param.Length; i++) {
			List<int> temp = new List<int> ();
			for (int j = 0; j < param [i].Length; j++) {
				temp.Add (param [i] [j]);
			}
			returnData.Add (temp);
		}
		return returnData;
	}

	private int[] ToArray (List<int> param)
	{
		int[] returnData = new int[param.Count];
		for (int i = 0; i < param.Count; i++) {
			returnData [i] = param [i];
		}
		return returnData;
	}
}

