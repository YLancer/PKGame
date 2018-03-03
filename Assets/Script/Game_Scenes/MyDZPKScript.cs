using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;
using LitJson;


public class MyDZPKScript : MonoBehaviour {

	public List<PlayerItemScript> playerItems;   //从6人改为10人   位置是从右起   L-
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
	public Text goldText;
	public GameObject gold;

	public Button readyBtn;
	public Button inviteFriendButton;

	private int bankerId;
	private int curDirIndex;
	private int currentIndex = -1;
	//当前出牌人
	private string curDirString = "B";
	//当前的方向字符串

	private bool isFirstOpen = true;

	public pdkButtonActionScript btnActionScript;

	private List<List<int>> mineList;

	public List<Transform> handTransformList;    //手牌的位置： 第7位是桌面发牌即为handTransformList[6]   L-改至11位是桌面发牌即为handTransformList[10]
    public List<Transform> ThrowTransformList;
	public Transform deskTransform;

	public List<List<GameObject>> handerCardList;             // handCardList[i] 是人    handlist[i][j]是牌

	private float timer = 0;
	private bool isGameStart = false;

	private int[] lastCard;
	//上家、或上上家最后出的牌

	// Use this for initialization

	public GameObject panel_dzpk_zhu;
	public GameObject panel_dzpk_Jiazhu;
	public GameObject btn_rangpai;
	public GameObject btn_genzhu;
	public GameObject btn_jiazhu;
	public GameObject btn_jiazhu_an;
	public Text Text_genZhu;
	public Text text_dichi;
	public List<GameObject> obj_chiList;//20171013新增
	public List<Text> text_chiList;//20171013新增
	public List<GameObject> dichiZhu_dzpk;
	public List<Text> Text_jiazhu_dzpk;
	//public Text Text_sliderzhu_dzpk;
	private int dichi_dzpk = 0;
	private List<int> chiList_dzpk = new List<int>();//20171013新增, 所有的池，0为主池，其余为边池
	public List<GameObject> obj_jiazhu_dzpk;
	public Slider slideZhu;
	private int[] sliderZhuArray = {1,2,3,5,7,10,15,20,30,40,50,75,100};
	private List<int> curSliderZhuList = new List<int>();
	private GameObject[] shoupaiMove;
	public List<GameObject> girsList;
	private bool isAutoRang = false;//记录时间到了后，是否可以自动让牌


//	public GameObject panel_Qiang;
//	public GameObject panel_Zhus;
//	public GameObject panel_Niu;
//	public GameObject panel_RankNiu;
//	public GameObject panel_Tips;

	private bool hasQiang = false;
	private bool hasZhu = false;
	private int hasZhus = 0;
	private int hasQiangs = 0;
	void Start ()
	{
		init ();

		if (GlobalDataScript.reEnterRoomData != null) { //断线重连进入房间
			GlobalDataScript.loginResponseData.roomId = GlobalDataScript.reEnterRoomData.roomId;
			reEnterRoom ();
		} else if (GlobalDataScript.roomJoinResponseData != null) {//进入他人房间
			joinToRoom (GlobalDataScript.roomJoinResponseData.playerList);
		} else {//创建房间
			createRoomAddAvatarVO (GlobalDataScript.loginResponseData);
		}

		showFangzhu ();
		loadBiaoQing ();

		//显示发牌员
		//int lastNum = GlobalDataScript.roomVo.roomId % 10;
		//int girlIndex = lastNum % 3;
		//for (int i = 0; i < girsList.Count; i++) {
		//	if (i == girlIndex) {
		//		girsList [i].SetActive (true);
		//	} else {
		//		girsList [i].SetActive (false);
		//	}
		//}
	}

	public void init ()
	{
//		GlobalDataScript.isonLoginPage = false;
		addListener ();
		versionText.text = "V" + Application.version;
	}

	public void showFangzhu ()//考虑放到
	{
		//显示房主
		int myIndex = getMyIndexFromList ();
		if (myIndex == 0) {
			playerItems [0].showFangZhu ();
		}else if(myIndex >=1 && myIndex <=9)
        {
            playerItems[playerItems.Count - myIndex].showFangZhu();     // L-
        } 
        //else if (myIndex == 1) {
		//	playerItems [5].showFangZhu ();
		//} else if (myIndex == 2) {
		//	playerItems [4].showFangZhu ();
		//} else if (myIndex == 3) {
		//	playerItems [3].showFangZhu ();
		//}else if (myIndex == 4) {
		//	playerItems [2].showFangZhu ();
		//}else if (myIndex == 5) {
		//	playerItems [1].showFangZhu ();
		//}
	}

	void Update () {
		timer -= Time.deltaTime;
		if (timer < 0) {
			timer = 0;
		}

		if (isGameStart && currentIndex >= 0) {
			playerItems [currentIndex].showClockText (Math.Floor (timer) + "");
		}

		if (isGameStart && currentIndex == 0 && (int)Math.Floor (timer) == 0) {
//			bool isSbjiazhu = false;
//			for (int i = 0; i < playerItems.Count; i++) {
//				if (playerItems [i].getCurZhu () > 0) {
//					isSbjiazhu = true;
//					break;
//				}
//			}

			if (isAutoRang) {
				cleanDZPKBtn ();
				CustomSocket.getInstance ().sendMsg (new DZPK_rangPaiRequest ());
			} else {
				cleanDZPKBtn ();
				CustomSocket.getInstance ().sendMsg (new DZPK_qiPaiRequest ());
			}
		}


//		panel_Tips.GetComponent<TipsScript_DN> ().setTime ((int)Math.Floor (timer));
//		if ((int)Math.Floor (timer) == 0) {
//			if (!hasQiang && GlobalDataScript.roomVo.qiang) {
////				qiangZhuang (false);
//			} else if (GlobalDataScript.mainUuid != GlobalDataScript.loginResponseData.account.uuid && !hasZhu){
////				xiaZhu (1);
//			}
//			else if (GlobalDataScript.loginResponseData.stateForNiu == 7) {
////				hasNiu (GlobalDataScript.loginResponseData.niu > 0);
//			}
//		}
//		if (inviteFriendButton.isActiveAndEnabled) {
//			if (avatarList != null && avatarList.Count == 5)
//				inviteFriendButton.gameObject.SetActive (true);
//		}
	}

	public void addListener ()
	{
		SocketEventHandle.getInstance ().outRoomCallback += outRoomCallbak;
		SocketEventHandle.getInstance ().otherUserJointRoomCallBack += otherUserJointRoom;
		SocketEventHandle.getInstance ().messageBoxNotice += messageBoxNotice;
		SocketEventHandle.getInstance ().gameReadyNotice += gameReadyNotice;
		SocketEventHandle.getInstance ().StartGameNotice += startGame;
		SocketEventHandle.getInstance ().serviceErrorNotice += serviceErrorNotice;
		SocketEventHandle.getInstance ().returnGameResponse += returnGameResponse;
		SocketEventHandle.getInstance ().dissoliveRoomResponse += dissoliveRoomResponse;
		SocketEventHandle.getInstance ().offlineNotice += offlineNotice;
		SocketEventHandle.getInstance ().onlineNotice += onlineNotice;
		SocketEventHandle.getInstance ().HupaiCallBack += hupaiCallBack;
		SocketEventHandle.getInstance ().HupaiBackDzpkCallBack += hupaiBackDzpkCallBack;
		SocketEventHandle.getInstance ().FinalGameOverCallBack += finalGameOverCallBack;
		CommonEvent.getInstance ().closeGamePanel += exitOrDissoliveRoom;
		SocketEventHandle.getInstance ().micInputNoticeOther += micInputNotice;

		SocketEventHandle.getInstance().DZPK_genZhuResponse += DZPK_genZhuResponse;
		SocketEventHandle.getInstance().DZPK_rangPaiResponse += DZPK_rangPaiResponse;
		SocketEventHandle.getInstance().DZPK_qiPaiResponse += DZPK_qiPaiResponse;
		SocketEventHandle.getInstance().DZPK_jiaZhuResponse += DZPK_jiaZhuResponse;
		SocketEventHandle.getInstance().DZPK_putOffResponse += DZPK_putOffResponse;
		SocketEventHandle.getInstance().DZPK_faPaiResponse += DZPK_faPaiResponse;

	}


	private void removeListener ()
	{
		SocketEventHandle.getInstance ().outRoomCallback -= outRoomCallbak;
		SocketEventHandle.getInstance ().otherUserJointRoomCallBack -= otherUserJointRoom;
		SocketEventHandle.getInstance ().messageBoxNotice -= messageBoxNotice;
		SocketEventHandle.getInstance ().gameReadyNotice -= gameReadyNotice;
		SocketEventHandle.getInstance ().StartGameNotice -= startGame;
		SocketEventHandle.getInstance ().serviceErrorNotice -= serviceErrorNotice;
		SocketEventHandle.getInstance ().returnGameResponse -= returnGameResponse;
		SocketEventHandle.getInstance ().dissoliveRoomResponse -= dissoliveRoomResponse;
		SocketEventHandle.getInstance ().offlineNotice -= offlineNotice;
		SocketEventHandle.getInstance ().onlineNotice -= onlineNotice;
		SocketEventHandle.getInstance ().HupaiCallBack -= hupaiCallBack;
		SocketEventHandle.getInstance ().FinalGameOverCallBack -= finalGameOverCallBack;
		CommonEvent.getInstance ().closeGamePanel -= exitOrDissoliveRoom;
		SocketEventHandle.getInstance ().micInputNoticeOther -= micInputNotice;

		SocketEventHandle.getInstance().DZPK_genZhuResponse -= DZPK_genZhuResponse;
		SocketEventHandle.getInstance().DZPK_rangPaiResponse -= DZPK_rangPaiResponse;
		SocketEventHandle.getInstance().DZPK_qiPaiResponse -= DZPK_qiPaiResponse;
		SocketEventHandle.getInstance().DZPK_jiaZhuResponse -= DZPK_jiaZhuResponse;
		SocketEventHandle.getInstance().DZPK_putOffResponse -= DZPK_putOffResponse;
		SocketEventHandle.getInstance().DZPK_faPaiResponse -= DZPK_faPaiResponse;
	}


	public void otherUserJointRoom (ClientResponse response)
	{
		AvatarVO avatar = JsonMapper.ToObject<AvatarVO> (response.message);
		addAvatarVOToList (avatar);
	}


	public void micInputNotice (int sendUUid)
	{
		if (sendUUid > 0) {
			for (int i = 0; i < playerItems.Count; i++) {
				if (playerItems [i].getUuid () != -1) {
					if (sendUUid == playerItems [i].getUuid ()) {
						playerItems [i].showChatAction ();
					}
				}
			}
		}
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
				seatIndex = 9 + seatIndex;             // 5->9 L
			}            

			int code = int.Parse (arr [1]);
			//传输性别  大于3000为女
			if (code > 3000) {
				code = code - 3000;
				playerItems [seatIndex].showChatMessage (code);
				SoundCtrl.getInstance ().playMessageBoxSound (code, 0,3);
			} else {
				code = code - 1000;
				playerItems [seatIndex].showChatMessage (code);
				SoundCtrl.getInstance ().playMessageBoxSound (code, 1,3);
			}
		} else if (int.Parse (arr [0]) == 2) {
			int uuid = int.Parse (arr [2]);
			int myIndex = getMyIndexFromList ();
			int curAvaIndex = getIndex (uuid);
			int seatIndex = curAvaIndex - myIndex;
			if (seatIndex < 0) {
				seatIndex = 9 + seatIndex;     // 5->9 L
            }
			playerItems [seatIndex].showChatMessage (arr [1]);
		} else if (int.Parse (arr [0]) == 3) {
			int uuid = int.Parse (arr [2]);
			int myIndex = getMyIndexFromList ();
			int curAvaIndex = getIndex (uuid);
			int seatIndex = curAvaIndex - myIndex;
			if (seatIndex < 0) {
				seatIndex = 9 + seatIndex;     // 5->9 L
            }
			int code = int.Parse (arr [1]);
			int sex = 0;
			//传输性别  大于3000为女
			if (code > 3000) {
				code = code - 3000;
				sex = 0;
			} else {
				code = code - 1000;
				sex = 1;
			}
			if(!(GlobalDataScript.roomVo.gameType == 3))
				SoundCtrl.getInstance ().playMessageBoxSound (code, sex,2);
			playerItems [seatIndex].showBiaoQing (bqScript.getBiaoqing ( code- 1));//int.Parse (arr [1])
		} else if(int.Parse (arr [0]) == 4){
			int srcuuid = int.Parse (arr [2]);
			int destuuid = int.Parse (arr [3]);
			int myIndex = getMyIndexFromList ();
			int srcAvaIndex = getIndex (srcuuid);
			int destAvaIndex = getIndex (destuuid);

			int destSeatIndex = destAvaIndex - myIndex;
			if (destSeatIndex < 0) {
				destSeatIndex = 9 + destSeatIndex;    // 5->9 L
            }
			int srcSeatIndex = srcAvaIndex - myIndex;
			if (srcSeatIndex < 0) {
				srcSeatIndex = 9 + srcSeatIndex;     // 5->9 L
            }
			//XXX
			playerItems [destSeatIndex].showMfbq (srcSeatIndex, destSeatIndex, arr[1]);
		}

	}

	public void startGame (ClientResponse response)
	{
		GlobalDataScript.roomAvatarVoList = avatarList;
        print(" startGame " + avatarList.Count);
		StartGameVO sgvo = JsonMapper.ToObject<StartGameVO> (response.message);
//		GlobalDataScript.count_Players_DN = avatarList.Count;
//		GlobalDataScript.roomVo.guiPai = sgvo.gui;
//		GlobalDataScript.roomVo.guiPai2 = sgvo.gui2;

		cleanGameplayUI ();
		cleanDesk_dzpk ();

		//开始游戏后不显示
		MyDebug.Log ("startGame");
		GlobalDataScript.surplusTimes--;

		if (GlobalDataScript.goldType) {
			Number.text = GlobalDataScript.playCountForGoldType + "";
		}else
			Number.text = GlobalDataScript.surplusTimes + "/" + GlobalDataScript.roomVo.roundNumber;

		//bankerId = sgvo.GrabAvatarIndex;
		avatarList [bankerId].main = true;
		PlayerItemScript bankerItem = getPlayItem (bankerId);
		bankerItem.setbankImgEnable (true);

		GlobalDataScript.finalGameEndVo = null;
		initArrayList ();

		isFirstOpen = false;
		GlobalDataScript.isOverByPlayer = false;
		int selfIndex = getMyIndexFromList ();

		mineList = sgvo.paiArray;

        setAllPlayerReadImgVisbleToFalse ();
		//所有玩家发牌移动动画
		float time = 0;
		shoupaiMove = new GameObject[7];
        if(GlobalDataScript.roomVo.AA ==false)   //德扑发两张牌  
        {
            for (int i = 0; i < 2; i++){                        
                for (int j = 0; j < avatarList.Count; j++){
                    if (avatarList[j].scores > GlobalDataScript.roomVo.initFen_dzpk / 100)
                    {
                        int seat = getIndexByDir(getDirection(j));
                        StartCoroutine(DisplayFaPaiMove(seat, time, i));
                        time += 0.4f;
                    }
                }
            }
        }
        else if(GlobalDataScript.roomVo.AA == true)  //奥马哈发四张牌  L-
        {
            for (int i = 0; i < 4; i++) {                       
                for (int j = 0; j < avatarList.Count; j++) {
                    if (avatarList[j].scores > GlobalDataScript.roomVo.initFen_dzpk / 100)
                    {
                        int seat = getIndexByDir(getDirection(j));
                        StartCoroutine(DisplayFaPaiMove(seat, time, i));
                        time += 0.4f;
                    }
                }
            }
        }
		


		//DisplayHandCard ();


		//更新底池显示
		int xiaomangzhu = GlobalDataScript.roomVo.initFen_dzpk / 200;
		dichi_dzpk = xiaomangzhu * 3;
//		text_dichi.text = dichi_dzpk + "";
		obj_chiList [0].SetActive (true);
//		text_chiList[0].text = dichi_dzpk + "";

		//更新大盲和小盲的分数
		int xiaomangIndex = sgvo.xiaomangIndex;
		int damangIndex = sgvo.damangIndex;
		int curAvatarIndex = sgvo.curAvatarIndex;

		int xiaoseat = getIndexByDir (getDirection (xiaomangIndex));
		int daseat = getIndexByDir (getDirection (damangIndex));
		PlayerItemScript playerxiao = getPlayItem (xiaomangIndex);
		if (playerxiao != null) {
			//playerxiao.updateScoreText (-xiaomangzhu);
			playerxiao.showDZPKdaxiaoMang (xiaomangzhu, xiaoseat);
		}

		PlayerItemScript playerda = getPlayItem (damangIndex);
		if (playerda != null) {
			//playerda.updateScoreText (-xiaomangzhu*2);
			playerda.showDZPKdaxiaoMang (xiaomangzhu*2, daseat);
		}

		initJiaZhuText ();//加注按钮赋值，与创建房间的初始分有关
		initSliderZhuArray();//设置滑动条初始数组，与创建房间的初始分有关

//		int firstPutoff = (bankerId + 3) % avatarList.Count;
		string dirstr = getDirection (curAvatarIndex);

		if (dirstr == DirectionEnum.Bottom) {  //如果是庄家并且是在bottom位置 先出牌  跑得快  //斗牛  不是庄家要下注
			if (curAvatarIndex == xiaomangIndex) {
				showBtnAndGen (xiaomangzhu);
			} else {
				showBtnAndGen (xiaomangzhu*2);
			}
		} else {
			cleanDZPKBtn ();
		}
		currentIndex = curDirIndex;

		UpdateTimeReStart ();
		isGameStart = true;

		isAutoRang = false;

	}

	private void cleanGameplayUI ()
	{
		canClickButtonFlag = true;
		inviteFriendButton.transform.gameObject.SetActive (false);
	}

	private void initArrayList ()
	{
		mineList = new List<List<int>> ();
		handerCardList = new List<List<GameObject>> ();
		for (int i = 0; i < 11; i++) {                    // 7->11  L-
			handerCardList.Add (new List<GameObject> ());
		}
	}

	private void setAllPlayerReadImgVisbleToFalse ()
	{
		for (int i = 0; i < 10; i++) {                     //  6-10
			playerItems [i].readyImg.enabled = false;
		}
	}

	IEnumerator DisplayFaPaiMove(int seat, float time, int cardIndex){
		yield return new WaitForSeconds (time);
		//移动
//		GameObject obj = Instantiate (Resources.Load ("prefab/dzpk/dzpk_shoupai_move")) as GameObject;
//		if (obj != null) {
//			SoundCtrl.getInstance ().playSoundByDZPK ("fapai");
//			obj.transform.SetParent (gameObject.transform);
//			obj.transform.localScale = new Vector3 (1, 1, 1);
//			obj.transform.localPosition = new Vector3 (6,170,0);
//			obj.transform.SetSiblingIndex (2);
//			Debug.LogError ("seat:" + seat);
//			shoupaiMove [seat] = obj;
//			Vector3 vec = getHandVector (seat);
//			StartCoroutine(MoveToPosition(obj, vec, seat));
//		}

		GameObject obj = Instantiate (Resources.Load ("prefab/dn/HandCard_s_DN")) as GameObject;
		if (obj != null) {
			SoundCtrl.getInstance ().playSoundByDZPK ("fapai");
			obj.transform.SetParent (gameObject.transform);
			obj.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			obj.transform.localPosition = new Vector3 (6,180,0);
			obj.transform.SetSiblingIndex (5);
			//Debug.LogError ("seat:" + seat);
			//shoupaiMove [seat] = obj;
			Vector3 pos = getHandCardPosition (seat, cardIndex);

			StartCoroutine(MoveToPosition(obj, pos, seat, cardIndex));
		}
	}

	IEnumerator MoveToPosition(GameObject obj, Vector3 dest, int seat, int cardIndex) {
		while(obj.transform.localPosition != dest){
			obj.transform.localPosition = Vector3.MoveTowards (obj.transform.localPosition, dest, 1700*Time.deltaTime);
			yield return 0;
		}
		Destroy (obj);
		if (seat == 0 && cardIndex == 1) {
			DisplayHandCard ();
		}
		if (seat != 0) {
			GameObject gob = Instantiate (Resources.Load ("prefab/dn/HandCard_s_DN")) as GameObject;    //发牌  牌只能看到背面
			if (gob != null) {
				gob.transform.SetParent (handTransformList [seat]);
				gob.transform.localScale = new Vector3 (1, 1, 1);
				handerCardList [seat].Add (gob);
			}
		}
	}

	private Vector3 getHandCardPosition(int i, int cardIndex){             //位置调整
		Vector3 vec = new Vector3 (0,0,0);
		switch (i) {
		case 0:
			if (cardIndex == 0) {
				//vec = new Vector3 (20, -196, 0);
				vec = new Vector3 (-30, -290, 0);
			} else {
				//vec = new Vector3 (120, -196, 0);
				vec = new Vector3 (-30, -290, 0);
			}
			break;
		case 1:
			vec = new Vector3 (285, -290, 0);
			break;
		case 2:
			vec = new Vector3 (500, -95, 0);
			break;
		case 3:
			vec = new Vector3 (470, 135, 0);
			break;
		case 4:
			vec = new Vector3 (230, 220, 0);
			break;
		case 5:
			vec = new Vector3 (-80 , 210, 0);
			break;
        case 6:                                 //新增|   暂定  L-
            vec = new Vector3(-330, 250, 0);
           break;
        case 7:                                
            vec = new Vector3(-540, 120, 0);
            break;
        case 8:                              
            vec = new Vector3(-550, -100, 0);
            break;
        case 9:                                 
            vec = new Vector3(-350, -285, 0);
            break;
            default:
			break;
		}
		return vec;
	}

	private void DisplayHandCard(bool hasShowed = false){                    //显示手里的牌  L-
//		if (shoupaiMove [0] != null) {
//			Destroy (shoupaiMove [0]);
//			shoupaiMove [0] = null;
//		}

		for (int i = 0; i < 52; i++) {                                
			if (mineList [0] [i] == 1) {
				GameObject gob = Instantiate (Resources.Load ("prefab/dn/HandCard_s_DN")) as GameObject;
				gob.GetComponent<pdkCardScript> ().isSmall = true;
				if (gob != null) {
					gob.transform.SetParent (handTransformList [0]);
					gob.transform.localScale = new Vector3 (1, 1, 1);
					gob.GetComponent<pdkCardScript> ().setPoint (i, 4);
					handerCardList [0].Add (gob);
				}
			}
		}
		float time = 0;
		for (int i = 0; i < handerCardList [0].Count; i++) {
			GameObject go = handerCardList [0] [i];
			StartCoroutine (FanPai(time, go));
			go.GetComponent<pdkCardScript> ().hasShow = true;
			time += 0.3f;
		}
	}

	IEnumerator FanPai(float time, GameObject go){
		yield return new WaitForSeconds (time);
		if (go.GetComponent<pdkCardScript> ().cardPoint != -1) {
			go.GetComponent<pdkCardScript> ().startPlay ();
		}
	}





	public void serviceErrorNotice (ClientResponse response)
	{
		TipsManagerScript.getInstance ().setTips (response.message);
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
		case DirectionEnum.TopRight:
			playerItems [2].GetComponent<PlayerItemScript> ().setPlayerOffline ();
			break;
		case DirectionEnum.TopLeft:
			playerItems [3].GetComponent<PlayerItemScript> ().setPlayerOffline ();
			break;
		case DirectionEnum.Left:
			playerItems [4].GetComponent<PlayerItemScript> ().setPlayerOffline ();
			break;
		case DirectionEnum.LeftBottom:
			playerItems [5].GetComponent<PlayerItemScript> ().setPlayerOffline ();
			break;
        case DirectionEnum.Player_7:                                                      //L-
            playerItems[6].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
        case DirectionEnum.Player_8:
            playerItems[7].GetComponent<PlayerItemScript>().setPlayerOffline();
            break;
        case DirectionEnum.Player_9:
            playerItems[8].GetComponent<PlayerItemScript>().setPlayerOffline();
            break;
        case DirectionEnum.Player_10:
            playerItems[9].GetComponent<PlayerItemScript>().setPlayerOffline();
            break;
        }

		//申请解散房间过程中，有人掉线，直接不能解散房间
		/**
		if (GlobalDataScript.isonApplayExitRoomstatus) {
			if (dissoDialog != null) {
				dissoDialog.GetComponent<VoteScript> ().removeListener ();
				Destroy (dissoDialog.GetComponent<VoteScript> ());
				Destroy (dissoDialog);
			}
			TipsManagerScript.getInstance ().setTips ("由于" + avatarList [index].account.nickname + "离线，系统不能解散房间。");

		}
		*/
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
            playerItems[1].GetComponent<PlayerItemScript>().setPlayerOffline();
            break;
        case DirectionEnum.TopRight:
            playerItems[2].GetComponent<PlayerItemScript>().setPlayerOffline();
            break;
        case DirectionEnum.TopLeft:
            playerItems[3].GetComponent<PlayerItemScript>().setPlayerOffline();
            break;
        case DirectionEnum.Left:
            playerItems[4].GetComponent<PlayerItemScript>().setPlayerOffline();
            break;
        case DirectionEnum.LeftBottom:
            playerItems[5].GetComponent<PlayerItemScript>().setPlayerOffline();
            break;
            case DirectionEnum.Player_7:                                                       //L-
            playerItems[6].GetComponent<PlayerItemScript>().setPlayerOffline();
            break;
        case DirectionEnum.Player_8:
            playerItems[7].GetComponent<PlayerItemScript>().setPlayerOffline();
            break;
        case DirectionEnum.Player_9:
            playerItems[8].GetComponent<PlayerItemScript>().setPlayerOffline();
            break;
        case DirectionEnum.Player_10:
            playerItems[9].GetComponent<PlayerItemScript>().setPlayerOffline();
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

			//exitOrDissoliveRoom ();//0928
			Invoke ("exitOrDissoliveRoom", 4f);

		}
	}

	/// <summary>
	/// 重新开始计时
	/// </summary>
	void UpdateTimeReStart ()   // 每个玩家操作可考虑时间段
	{
		timer = 24;
	}

	public void returnGameResponse (ClientResponse response)
	{
		JsonData json = JsonMapper.ToObject (response.message);
		int gameRound = int.Parse (json ["gameRound"].ToString ());
		GlobalDataScript.surplusTimes = gameRound;

		if (GlobalDataScript.goldType) {
			Number.text = GlobalDataScript.playCountForGoldType + "";
		}else
			Number.text = GlobalDataScript.surplusTimes + "/" + GlobalDataScript.roomVo.roundNumber;
		

//		dichi_dzpk = (int)json ["diChi"];
//		text_dichi.text = dichi_dzpk.ToString ();

		ChiList_DZPKVO chiListVo = JsonMapper.ToObject<ChiList_DZPKVO> (response.message);
		chiList_dzpk = chiListVo.chiList;

		for(int i = 0; i< chiList_dzpk.Count; i++){
			obj_chiList [i].SetActive (true);
			text_chiList [i].text = chiList_dzpk [i].ToString ();
		}

		int curAvatarIndex = (int)json ["curAvatarIndex"];

		FaPai_DZPKVO favo = JsonMapper.ToObject<FaPai_DZPKVO> (response.message);

		List<int> deskCard = favo.deskCard;
		for (int i = 0; i < deskCard.Count; i++) {
			GameObject gob = Instantiate (Resources.Load ("prefab/pdk/HandCard_s")) as GameObject;
			if (gob != null) {
				gob.transform.SetParent (handTransformList [10]);         // 6->10  发牌位置由第七位改为第十一位
				gob.transform.localScale = new Vector3 (1, 1, 1);
				gob.GetComponent<pdkCardScript> ().setPoint (deskCard[i], 4);
				handerCardList [10].Add (gob);            // 桌面增加牌
			}
		}


		if (curAvatarIndex != -1) {//1008
			int outIndex = getIndexByDir (getDirection (curAvatarIndex));

			UpdateTimeReStart ();
			currentIndex = outIndex;

			if (outIndex == 0) {//自己
				int zhu = (int)json ["zhu"];
				if (zhu > 0) {
					showBtnAndGen (zhu);
				} else {
					showBtnAndRang ();
				}
			} else {//别人

			}
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
		case DirectionEnum .LeftBottom: 
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


	/**
    *准备游戏
	*/
	public void  readyGame ()
	{
		cleanDesk_dzpk ();


		CustomSocket.getInstance ().sendMsg (new GameReadyRequest ());
	}


	private void cleanDesk_dzpk(){
		readyBtn.gameObject.SetActive (false);//1008
		if (handerCardList != null && handerCardList.Count > 0) {
			cleanArrayList (handerCardList);
			/**
			for (int i = 0; i < handerCardList.Count; i++) {
				List<GameObject> hands = handerCardList [i];
				for (int j = 0; j < hands.Count; j++) {
					//hands [j].GetComponent<pdkCardScript> ().onDragSelect -= dragSelect;
					//hands [j].GetComponent<pdkCardScript> ().onCardSelect -= cardSelect;
					Destroy (hands [j]);
				}
			}
			*/
		}
		getDirection (bankerId);
		if(avatarList[bankerId] != null)
			avatarList [bankerId].main = false;
		playerItems [curDirIndex].setbankImgEnable (false);
		for (int i = 0; i < playerItems.Count; i++) {
			playerItems [i].cleanDZPKAlltext ();
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

	/// 根据一个人在数组里的索引，得到这个人所在的方位，L-左，T-上,R-右，B-下（自己的方位永远都是在下方）
	private string getDirection (int avatarIndex)
	{
		string result = DirectionEnum.Bottom;
		int myselfIndex = getMyIndexFromList ();
		if (myselfIndex == avatarIndex) {
			MyDebug.Log ("getDirection == B");
			curDirIndex = 0;
			return result;
		}
        //从自己开始计算，下一位的索引                   玩家位置的索引 0-9  供10个人
        for (int i = 0; i < 10; i++) {                   // 6->10    位置   L-
			myselfIndex++;
			if (myselfIndex >= 10) {
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

		MyDebug.Log ("getDirection == B");
		curDirIndex = 0;
		return DirectionEnum.Bottom;
	}

	public void gameReadyNotice (ClientResponse response)
	{
		JsonData json = JsonMapper.ToObject (response.message);
		int avatarIndex = Int32.Parse (json ["avatarIndex"].ToString ());
		int myIndex = getMyIndexFromList ();
		int seatIndex = avatarIndex - myIndex;
		if (seatIndex < 0) {
			seatIndex = 10 + seatIndex;            // 6->10      L-
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

	/// <summary>
	/// 设置当前角色的座位
	/// </summary>
	/// <param name="avatar">Avatar.</param>
	private void setSeat (AvatarVO avatar)
	{
        //游戏结束后用的数据，勿删！！！

        //GlobalDataScript.palyerBaseInfo.Add (avatar.account.uuid, avatar.account);
		if (avatar.account.uuid == GlobalDataScript.loginResponseData.account.uuid) {
			playerItems [0].gameObject.SetActive (true);
			playerItems [0].setAvatarVo (avatar);
		} else {
			int myIndex = getMyIndexFromList ();
			int curAvaIndex = avatarList.IndexOf (avatar);
			int seatIndex = curAvaIndex - myIndex;
			if (seatIndex < 0) {
				if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.NULL)
					seatIndex = 4 + seatIndex;
				else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.PDK)
					seatIndex = 3 + seatIndex;
				else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.DN) //斗牛  最多五个 5个轮回才能回到正确的位置
					seatIndex = 5 + seatIndex;
				else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.DZPK)   // 德州扑克和奥马哈都是十人   6->10  L-
					seatIndex = 10 + seatIndex;
				else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.AMH)
					seatIndex = 10 + seatIndex;
			}
			playerItems [seatIndex].gameObject.SetActive (true);
			playerItems [seatIndex].setAvatarVo (avatar);
		}

	}



	public void createRoomAddAvatarVO (AvatarVO avatar)
	{
		avatar.scores = GlobalDataScript.roomVo.initFen_dzpk;//dzpk
		addAvatarVOToList (avatar);
		setRoomRemark ();
//		SetTip ();
	}

	private void addAvatarVOToList (AvatarVO avatar)
	{
		if (avatarList == null) {
			avatarList = new List<AvatarVO> ();
		}
		avatarList.Add (avatar);
		setSeat (avatar);
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
					index = getIndexByDir (getDirection (index));
					if(handerCardList != null)
						cleanList (handerCardList [index]);

					for (int i = 0; i < playerItems.Count; i++) {
						playerItems [i].setAvatarVo (null);
						playerItems [i].gameObject.SetActive (false);
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

	/// <summary>
	/// 清理桌面
	/// </summary>
	public void clean ()
	{
		//cleanArrayList (handerCardList);

		//去除打出的牌
		/**
		for (int i = 0; i < 5; i++) {
			cleanChuPai (i);
		}
		*/


		if (mineList != null) {
			mineList.Clear ();
		}

	}

	public void joinToRoom (List<AvatarVO> avatars)
	{
		avatarList = avatars;
		for (int i = 0; i < avatars.Count; i++) {
			setSeat (avatars [i]);
		}
		setRoomRemark ();
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


		roomInfo += "、初始分:" + GlobalDataScript.roomVo.initFen_dzpk;

		roomRemark.text = roomInfo;
	}

	/*************************断线重连*********************************/
	public void reEnterRoom ()
	{
		
		if (GlobalDataScript.reEnterRoomData != null) {
			//显示房间基本信息
			GlobalDataScript.roomVo.roundNumber = GlobalDataScript.reEnterRoomData.roundNumber;
			GlobalDataScript.roomVo.roomId = GlobalDataScript.reEnterRoomData.roomId;
			GlobalDataScript.roomVo.gameType = GlobalDataScript.reEnterRoomData.gameType;
			GlobalDataScript.roomVo.initFen_dzpk = GlobalDataScript.reEnterRoomData.initFen_dzpk;
			setRoomRemark ();

			//确定自己和其它玩家
			avatarList = GlobalDataScript.reEnterRoomData.playerList;
			GlobalDataScript.loginResponseData = avatarList[getMyIndexFromList()];
			GlobalDataScript.count_Players_DN = avatarList.Count;
			GlobalDataScript.roomAvatarVoList = GlobalDataScript.reEnterRoomData.playerList;
			for (int i = 0; i < avatarList.Count; i++) {
				AvatarVO ava = avatarList [i];
				setSeat (ava);
				
				if (ava.main) {
					bankerId = i;
					GlobalDataScript.mainUuid = ava.account.uuid;
					playerItems [getIndexByDir (getDirection (bankerId))].setbankImgEnable (true);//新一局游戏还没开始庄家是上一局的庄家不显示，因已经准备好，只是别人还没准备好
				}
				if (avatarList [i].account.uuid == GlobalDataScript.loginResponseData.account.uuid
				    && ava.isReady) {
					readyBtn.gameObject.SetActive (false);
				}
//					playerItems [getIndexByDir (getDirection (i))].readyImg.enabled =true;
			}

			//恢复房卡数据，此时主界面还没有load所以无需操作界面显示
			GlobalDataScript.loginResponseData.account.roomcard = GlobalDataScript.reEnterRoomData.playerList [getMyIndexFromList ()].account.roomcard;


			int[][] selfPaiArray = GlobalDataScript.loginResponseData.paiArray;
			if (selfPaiArray == null || selfPaiArray.Length == 0) {//游戏还没有开始

			} else {//牌局已开始
				readyBtn.gameObject.SetActive (false);//德州扑克

				UpdateTimeReStart ();
				isGameStart = true;

				setAllPlayerReadImgVisbleToFalse ();
				cleanGameplayUI ();

				initJiaZhuText ();//加注按钮赋值，与创建房间的初始分有关
				initSliderZhuArray();//设置滑动条初始数组，与创建房间的初始分有关

				initArrayList ();


				//显示打牌数据
				mineList = ToList (selfPaiArray);
				DisplayHandCard ();

				for (int i = 0; i < 2; i++) {
					for (int j = 0; j < avatarList.Count; j++) {
						if (avatarList [j].scores > GlobalDataScript.roomVo.initFen_dzpk / 100) {
							int seat = getIndexByDir (getDirection (j));
							if (seat == 0) {
								continue;
							}
							GameObject obj = Instantiate (Resources.Load ("prefab/dn/HandCard_s_DN")) as GameObject;
							if (obj != null) {
								obj.transform.SetParent (handTransformList [seat]);
								obj.transform.localScale = new Vector3 (1, 1, 1);
								handerCardList [seat].Add (obj);
							}
						}
					}
				}
			}
		}

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


	/**
	 * 结束
	 */
	private void finalGameOverCallBack(ClientResponse response){
		timer = 0;
		readyBtn.gameObject.SetActive (false);
		//GlobalDataScript.surplusTimes = 0; //0829@##
		GlobalDataScript.finalGameEndVo = JsonMapper.ToObject<FinalGameEndVo> (response.message);


//		GameObject obj = PrefabManage.loadPerfab ("Prefab/Panel_Game_Over");
//		obj.GetComponent<GameOverScript> ().setDisplayContent_pdk (1, avatarList, null, null, 4);
//		GlobalDataScript.singalGameOverList.Add (obj);
//		avatarList [bankerId].main = false;

		Invoke ("openGameOverPanelAll", 3f);



	}
	private void openGameOverPanelAll (){
		GameObject obj = PrefabManage.loadPerfab ("Prefab/Panel_Game_Over");
		obj.GetComponent<GameOverScript> ().setDisplayContent_pdk (1, avatarList, null, null, 4);
		GlobalDataScript.singalGameOverList.Add (obj);
		avatarList [bankerId].main = false;
	}


	/**
	 * 赢牌请求回调
	 */
	public void hupaiCallBack(ClientResponse response){
		currentIndex = -1;
		cleanClock ();
		readyBtn.gameObject.SetActive (false);

		if (GlobalDataScript.goldType)
			GlobalDataScript.playCountForGoldType++;
		//删除这句，未区分胡家是谁
		GlobalDataScript.hupaiResponseVo = new HupaiResponseVo ();
		GlobalDataScript.hupaiResponseVo = JsonMapper.ToObject<HupaiResponseVo> (response.message);



		if (GlobalDataScript.hupaiResponseVo.type == "2") {//解散

			//exitOrDissoliveRoom ();

		} else {//0:正常赢牌;1:只剩一个未弃牌

			//注移动
			for(int i = 0; i < playerItems.Count; i++){
				if (playerItems [i].getCurZhu () > 0) {
					GameObject obj = Instantiate (Resources.Load ("prefab/dzpk/winZhuCoinMove")) as GameObject;
					obj.transform.SetParent (gameObject.transform);
					obj.transform.localScale = new Vector3 (1.3f, 1.3f, 1.3f);
					obj.transform.localPosition = getZhuCoinPosition(i);
					Vector3 dest = new Vector3 (0, 148, 0);
					StartCoroutine (ZhuCoinMoveToPosition (obj, dest));
				}
			}

			//先清空本轮的注
			zhuClean ();
			Invoke ("diChiUpdate", 1f);


			Invoke ("openGameOverPanelSignal", 1.5f);
		}


	}

	private void openGameOverPanelSignal ()
	{//单局结算

		for (int i = 0; i < playerItems.Count-1; i++) {
			playerItems [i].cleanDZPKAlltext ();
		}

		displayAllPlayerCard (GlobalDataScript.hupaiResponseVo.avatarList);

		//显示德州扑克牌型
		displayAllPlayerType (GlobalDataScript.hupaiResponseVo.avatarList);

		string scores = GlobalDataScript.hupaiResponseVo.currentScore;
		hupaiCoinChange (scores);                                                         //  出问题
		showWinPlayer (GlobalDataScript.hupaiResponseVo.winuuid);

		dichi_dzpk = 0;
		//20171013新增
		text_dichi.text = "0";
		for (int i = 0; i < text_chiList.Count; i++) {
			text_chiList[i].text = "0";
		}
		obj_chiList [0].SetActive (true);
		for (int i = 1; i < obj_chiList.Count; i++) {
			obj_chiList [i].SetActive (false);
		}

		avatarList [bankerId].main = false;
		if (avatarList [getMyIndexFromList()].scores < GlobalDataScript.roomVo.initFen_dzpk / 100) {
			readyBtn.gameObject.SetActive (false);
		} else {
			readyBtn.gameObject.SetActive (true);
		}

	}


	private void hupaiCoinChange (string scores)
	{
		string[] scoreList = scores.Split (new char[1]{ ',' });
        print(" scores -----------" + scores+">>>>"+ scoreList.Length);
        if (scoreList != null && scoreList.Length > 0) {
			for (int i = 0; i < scoreList.Length - 1; i++) {
				string itemstr = scoreList [i];
				int uuid = int.Parse (itemstr.Split (new char[1]{ ':' }) [0]);
				int score = int.Parse (itemstr.Split (new char[1]{ ':' }) [1]) + 0;


				if (score > 0) {


//					int index = getIndex (uuid);
//					PlayerItemScript player = getPlayItem (index);
//					if (player != null) {
//						player.showDZPKSuccess ();
//						player.updateScoreText(score);
//					}
//					string dir = getDirection (index);
//					int seat = getIndexByDir (dir);

//					playerItems [getIndexByDir (getDirection (getIndex (uuid)))].showDZPKSuccess ();
					playerItems [getIndexByDir (getDirection (getIndex (uuid)))].setScoreText(score);        //  L-  出问题
                }
	
			}
		}
	}

	private void showWinPlayer (string uuids){
		string[] uuidList = uuids.Split (new char[1]{ ',' });
		if (uuidList != null && uuidList.Length > 0) {
			//sound
			SoundCtrl.getInstance ().playSoundByDZPK ("win");
			for (int i = 0; i < uuidList.Length; i++) {
				string itemstr = uuidList [i];
				//Debug.LogError ("winer:" + itemstr);
				int uuid = int.Parse (itemstr);

				//20171027新增动画
				int seat = getIndexByDir (getDirection (getIndex (uuid)));

				GameObject obj = Instantiate (Resources.Load ("prefab/dzpk/winZhuCoinMove")) as GameObject;
				obj.transform.SetParent (gameObject.transform);
				obj.transform.localScale = new Vector3 (1.3f, 1.3f, 1.3f);
				obj.transform.localPosition = new Vector3 (0, 148, 0);
				Vector3 dest = getPlayerPosition(seat);
				StartCoroutine (winZhuMoveToPlayer (obj, dest, seat));
				//playerItems [getIndexByDir (getDirection (getIndex (uuid)))].showDZPKSuccess ();
			}
		}
	}

	private void displayAllPlayerCard(List<HupaiResponseItem> avaList){
		List<HupaiResponseItem> avatarList = avaList;
		//foreach (HupaiResponseItem item in GlobalDataScript.hupaiResponseVo.avatarList) {
		for (int i = 0; i < avatarList.Count; i++) {
			int[] paiArray = avatarList [i].paiArray;
			int myIndex = getMyIndexFromList ();
            print("myIndex ++++++++ " + myIndex);
			if (i == myIndex) {
				continue;
			}
			int seatIndex = i - myIndex;
			if (seatIndex < 0) {
				seatIndex = 10 + seatIndex;           //  6->10  L-
			}

			//删除
			while(handerCardList [seatIndex].Count > 0){
				GameObject tmp = handerCardList[seatIndex][0];
				Destroy (tmp);
				handerCardList [seatIndex].RemoveAt (0);
			}

			for (int j = 0; j < 52; j++) {
				if (paiArray [j] == 1) {
					GameObject gob = Instantiate (Resources.Load ("prefab/dn/HandCard_s_DN")) as GameObject;
					if (gob != null) {
						gob.transform.SetParent (handTransformList [seatIndex]);
						gob.transform.localScale = new Vector3 (1, 1, 1);
						gob.GetComponent<pdkCardScript> ().setPoint (j, 4);
						handerCardList [seatIndex].Add (gob);
					}
				}
			}

//			if (shoupaiMove [seatIndex] != null) {
//				Destroy (shoupaiMove [seatIndex]);
//				shoupaiMove [seatIndex] = null;
//			}
			float time = 0;
			for (int j = 0; j < handerCardList [seatIndex].Count; j++) {
				GameObject go = handerCardList [seatIndex] [j];
				StartCoroutine (FanPai(time, go));
				go.GetComponent<pdkCardScript> ().hasShow = true;
				time += 0.3f;
			}
		}

	}


	private void displayAllPlayerType(List<HupaiResponseItem> avaList){
		List<HupaiResponseItem> avatarList = avaList;
		//foreach (HupaiResponseItem item in GlobalDataScript.hupaiResponseVo.avatarList) {
		for (int i = 0; i < avatarList.Count; i++) {
			
//			int myIndex = getMyIndexFromList ();
//
//			int seatIndex = i - myIndex;
//			if (seatIndex < 0) {
//				seatIndex = 6 + seatIndex;
//			}

			string hu = avatarList [i].totalInfo.hu;
			int type = -1;
			if (hu != null && hu != "") {
				type = int.Parse (hu);
			}
			PlayerItemScript player = getPlayItem (i);
			if (player != null) {
				player.showDZPKType (type);
			}
		}

	}



	/**
	 * 德州扑克断线重连，胡牌特殊处理
	 */
	public void hupaiBackDzpkCallBack(ClientResponse response){
		currentIndex = -1;
		cleanClock ();

		if (GlobalDataScript.goldType)
			GlobalDataScript.playCountForGoldType++;
		//删除这句，未区分胡家是谁
		GlobalDataScript.hupaiResponseVo = new HupaiResponseVo ();
		GlobalDataScript.hupaiResponseVo = JsonMapper.ToObject<HupaiResponseVo> (response.message);



		if (GlobalDataScript.hupaiResponseVo.type == "2") {//解散

			//exitOrDissoliveRoom ();

		} else {//0:正常赢牌;1:只剩一个未弃牌

			Invoke ("openGameOverPanelSignal1", 1f);
		}
	}

	private void openGameOverPanelSignal1 ()
	{//单局结算

		for (int i = 0; i < playerItems.Count; i++) {
			playerItems [i].cleanDZPKAlltext ();
		}

		displayAllPlayerCard (GlobalDataScript.hupaiResponseVo.avatarList);

		//显示德州扑克牌型
		displayAllPlayerType (GlobalDataScript.hupaiResponseVo.avatarList);

		string scores = GlobalDataScript.hupaiResponseVo.currentScore;
		hupaiCoinChange1 (scores);

		dichi_dzpk = 0;
		text_dichi.text = "0";
		//20171013新增
		for (int i = 0; i < text_chiList.Count; i++) {
			text_chiList[i].text = "0";
		}
		obj_chiList [0].SetActive (true);
		for (int i = 1; i < obj_chiList.Count; i++) {
			obj_chiList [i].SetActive (false);
		}

		avatarList [bankerId].main = false;
		readyBtn.gameObject.SetActive (true);
	}

	private void hupaiCoinChange1 (string scores)
	{
		string[] scoreList = scores.Split (new char[1]{ ',' });
		if (scoreList != null && scoreList.Length > 0) {
			for (int i = 0; i < scoreList.Length - 1; i++) {
				string itemstr = scoreList [i];
				int uuid = int.Parse (itemstr.Split (new char[1]{ ':' }) [0]);
				int score = int.Parse (itemstr.Split (new char[1]{ ':' }) [1]) + 0;


				if (score > 0) {
					playerItems [getIndexByDir (getDirection (getIndex (uuid)))].showDZPKSuccess ();
				}

			}
		}
	}


	/**
	 * 发牌
	 */
	public void DZPK_faPaiResponse(ClientResponse response){
		FaPai_DZPKVO favo = JsonMapper.ToObject<FaPai_DZPKVO> (response.message);
		List<int> deskCard = favo.deskCard;
		int count = handerCardList [10].Count;       // 桌面发牌位置

		isAutoRang = true;

		//20171026新增 一轮结束注移动
		for(int i = 0; i < playerItems.Count; i++){
			if (playerItems [i].getCurZhu () > 0) {
				GameObject obj = Instantiate (Resources.Load ("prefab/dzpk/zhuCoinMove")) as GameObject;
				obj.transform.SetParent (gameObject.transform);
				obj.transform.localScale = new Vector3 (1.3f, 1.3f, 1.3f);
				obj.transform.localPosition = getZhuCoinPosition(i);
				Vector3 dest = new Vector3 (0, 148, 0);
				StartCoroutine (ZhuCoinMoveToPosition (obj, dest));
			}
		}

		//先清空本轮的注
		zhuClean ();
		Invoke ("diChiUpdate", 1f);





//		GameObject obj = Instantiate (Resources.Load ("prefab/dzpk/zhuCoinMove")) as GameObject;
//		if (obj != null) {
//			obj.transform.SetParent (gameObject.transform);
//			obj.transform.localScale = new Vector3 (1.3f, 1.3f, 1.3f);
//			obj.transform.localPosition = new Vector3 (0, 0, 0);
//			Vector3 vec = getZhuCoinPosition (seat);
//
//			StartCoroutine (ZhuCoinMoveToPosition (obj, vec, zhu));
//		}

		//20171026新增发牌
		StartCoroutine (fapaiPlay (deskCard));

//		float time = 0;
//		for (int i = 0; i < deskCard.Count; i++) {



			//int countNum = handerCardList [6].Count;
			//移动
			//GameObject obj = Instantiate (Resources.Load ("prefab/dn/HandCard_s_DN")) as GameObject;
			//if (obj != null) {
			//	SoundCtrl.getInstance ().playSoundByDZPK ("fapai");
			//	obj.transform.SetParent (gameObject.transform);
			//	obj.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			//	obj.transform.localPosition = new Vector3 (6,180,0);
			//	obj.transform.SetSiblingIndex (5);
			//	Vector3 vec = getDeskCardPosition (countNum+1);

			//	StartCoroutine(DeskMoveToPosition(obj, vec, deskCard[i]));
			//}

//			StartCoroutine(DisplayDeskCardMove (time, deskCard[i]));
//			time += 0.2f;
//
//		}






//		for (int i = 0; i < deskCard.Count; i++) {
//			GameObject gob = Instantiate (Resources.Load ("prefab/dn/HandCard_s_DN")) as GameObject;
//			if (gob != null) {
//				gob.transform.SetParent (handTransformList [6]);
//				gob.transform.localScale = new Vector3 (1, 1, 1);
//				gob.GetComponent<pdkCardScript> ().setPoint (deskCard[i], 4);
//				handerCardList [6].Add (gob);
//			}
//		}
//
//		float time = 0;
//		for (int i = count; i < handerCardList [6].Count; i++) {
//			GameObject go = handerCardList [6] [i];
//			StartCoroutine (FanPai(time, go));
//			go.GetComponent<pdkCardScript> ().hasShow = true;
//			time += 0.3f;
//		}

		//Invoke ("zhuClean", 2f);
//		for (int i = 0; i < playerItems.Count; i++) {
//			playerItems [i].cleanDZPKtext ();
//		}


	}

	IEnumerator DeskMoveToPosition(GameObject obj, Vector3 dest, int cardIndex) {
		while(obj.transform.localPosition != dest){
			obj.transform.localPosition = Vector3.MoveTowards (obj.transform.localPosition, dest, 1200*Time.deltaTime);
			yield return 0;
		}
		Destroy (obj);

		DisplayDeskCard (cardIndex);

	}
	private void DisplayDeskCard (int cardIndex){
		GameObject gob = Instantiate (Resources.Load ("prefab/dn/HandCard_s_DN")) as GameObject;
		if (gob != null) {
			gob.transform.SetParent (handTransformList [10]);  //l  6->10
			gob.transform.localScale = new Vector3 (1, 1, 1);
			gob.GetComponent<pdkCardScript> ().setPoint (cardIndex, 4);
			handerCardList [10].Add (gob);

			StartCoroutine (FanPai(0, gob));
			gob.GetComponent<pdkCardScript> ().hasShow = true;
		}

	}
    //桌面五张牌出现的位置设置
	private Vector3 getDeskCardPosition(int i){   
		Vector3 vec = new Vector3 (0,0,0);
		switch (i) {
		case 1:
			vec = new Vector3 (-199, 3, 0);
			break;
		case 2:
			vec = new Vector3 (-106, 3, 0);
			break;
		case 3:
			vec = new Vector3 (-6, 3, 0);
			break;
		case 4:
			vec = new Vector3 (83, 3, 0);
			break;
		case 5:
			vec = new Vector3 (178, 3, 0);
			break;
		default:
			break;
		}
		return vec;
	}

	IEnumerator DisplayDeskCardMove(float time, int cardIndex){
		//yield return 0;
		yield return new WaitForSeconds (time);

		int countNum = handerCardList [10].Count;         //6 ->10   L-
        GameObject obj = Instantiate (Resources.Load ("prefab/dn/HandCard_s_DN")) as GameObject;
		if (obj != null) {
			SoundCtrl.getInstance ().playSoundByDZPK ("fapai");
			obj.transform.SetParent (gameObject.transform);
			obj.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			obj.transform.localPosition = new Vector3 (6,180,0);
			obj.transform.SetSiblingIndex (5);
			Vector3 vec = getDeskCardPosition (countNum+1);

			StartCoroutine(DeskMoveToPosition(obj, vec, cardIndex));
		}
	}

	private Vector3 getPlayerPosition (int i){      // L-
		Vector3 vec = new Vector3 (0,0,0);
		switch (i) {
		case 0:
            vec = new Vector3(-145, -235, 0);    //vec = new Vector3 (-108, -218, 0);原来6人位置
			break;
		case 1:
            vec = new Vector3(205, -235, 0);   //vec = new Vector3 (-425, -191, 0);
            break;
		case 2:
			vec = new Vector3 (500, -210, 0);  //vec = new Vector3(-513, 11.4f, 0);
                break;
		case 3:
			vec = new Vector3 (590, 70, 0);    // vec = new Vector3 (-400, 209, 0);
                break;
		case 4:
			vec = new Vector3 (380, 265, 0);      //vec = new Vector3 (412, 199, 0);
                break;
		case 5:
			vec = new Vector3 (100, 265, 0);      //vec = new Vector3 (533, -30, 0);
                break;
        case 6:
            vec = new Vector3(-445, 265, 0);      
            break;
        case 7:
            vec = new Vector3(-590,70,0);     
            break;
        case 8:
            vec = new Vector3(-580,-205,0);     
            break;
        case 9:
            vec = new Vector3(-390,-240,0);      
            break;
            default:
			break;
		}
		return vec;
	}

	private Vector3 getZhuCoinPosition (int i){
		Vector3 vec = new Vector3 (0,0,0);
		switch (i) {
		case 0:
			vec = new Vector3 (-100, -155, 0);    //vec = new Vector3 (-38, -119, 0);   L-
                break;
		case 1:
			vec = new Vector3 (250, -155, 0);    //vec = new Vector3 (-343, -99, 0);
                break;
		case 2:
			vec = new Vector3 (415, -115, 0);    //vec = new Vector3 (-422, 97, 0);
                break;
		case 3: 
			vec = new Vector3 (450, 50, 0);     //vec = new Vector3 (-306, 280, 0);
                break;
		case 4:
			vec = new Vector3 (320, 145, 0);     //vec = new Vector3 (341, 264, 0);
                break;
		case 5:
			vec = new Vector3 (90, 145, 0);      //vec = new Vector3 (445, 52, 0);
                break;
        case 6:
            vec = new Vector3(-330, 145, 0);      
            break;
        case 7:
            vec = new Vector3(-445, 20, 0);     
            break;
        case 8:
            vec = new Vector3(-395, -130, 0);      
            break;
        case 9:
            vec = new Vector3(-295, -155, 0);      
            break;
            default:
			break;
		}
		return vec;
	}

	IEnumerator winZhuMoveToPlayer(GameObject obj, Vector3 dest, int seat) {
		while(obj.transform.localPosition != dest){
			obj.transform.localPosition = Vector3.MoveTowards (obj.transform.localPosition, dest, 900*Time.deltaTime);
			yield return 0;
		}
		Destroy (obj);

		playerItems [seat].showDZPKSuccess ();
	}

	IEnumerator ZhuCoinMoveToPosition(GameObject obj, Vector3 dest) {
		while(obj.transform.localPosition != dest){
			obj.transform.localPosition = Vector3.MoveTowards (obj.transform.localPosition, dest, 700*Time.deltaTime);
			yield return 0;
		}
		Destroy (obj);

		//
	}
	private void diChiUpdate(){
		for(int i = 0; i< chiList_dzpk.Count; i++){
			obj_chiList [i].SetActive (true);
			text_chiList [i].text = chiList_dzpk [i].ToString ();
		}
	}

	IEnumerator fapaiPlay(List<int> deskCard){
		yield return new WaitForSeconds (1f);
		float time = 0;
		for (int i = 0; i < deskCard.Count; i++) {
			//			int countNum = handerCardList [6].Count;
			//			//移动
			//			GameObject obj = Instantiate (Resources.Load ("prefab/dn/HandCard_s_DN")) as GameObject;
			//			if (obj != null) {
			//				SoundCtrl.getInstance ().playSoundByDZPK ("fapai");
			//				obj.transform.SetParent (gameObject.transform);
			//				obj.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			//				obj.transform.localPosition = new Vector3 (6,180,0);
			//				obj.transform.SetSiblingIndex (5);
			//				Vector3 vec = getDeskCardPosition (countNum+1);
			//
			//				StartCoroutine(DeskMoveToPosition(obj, vec, deskCard[i]));
			//			}

			StartCoroutine(DisplayDeskCardMove (time, deskCard[i]));
			time += 0.2f;

		}
	}





	private void zhuClean(){
		for (int i = 0; i < playerItems.Count; i++) {
			playerItems [i].cleanDZPKtext ();
		}
	}

	private void cleanClock ()
	{
		foreach (PlayerItemScript pis in playerItems) {
			pis.showClock (false);
		}
	}

	/**
	 * 出牌操作
	 */
	public void DZPK_putOffResponse(ClientResponse response){
		cleanClock ();
		UpdateTimeReStart ();

		JsonData json = JsonMapper.ToObject (response.message);
		int avaIndex = int.Parse (json ["avatarIndex"].ToString ());
		int type = int.Parse (json ["type"].ToString ());//0:让牌, 1:跟注,
		int genzhu = int.Parse (json ["genzhu"].ToString ());

		PlayerItemScript player = getPlayItem (avaIndex);
		if (player != null) {
			//自己
			if (player == playerItems [0]) {
				if (type == 0) {
					isAutoRang = true;
					showBtnAndRang ();
				} else {
					isAutoRang = false;
					showBtnAndGen (genzhu);

				}
			}
		}
		currentIndex = curDirIndex;

	}

	/**
	 * 跟注
	 */
	public void DZPK_genZhuResponse(ClientResponse response){
		JsonData json = JsonMapper.ToObject (response.message);
		int avaIndex = int.Parse (json ["avatarIndex"].ToString ());
		int genzhu = int.Parse (json ["genzhu"].ToString ());

		ChiList_DZPKVO chiListVo = JsonMapper.ToObject<ChiList_DZPKVO> (response.message);
		chiList_dzpk = chiListVo.chiList;
		int seat = getIndexByDir (getDirection (avaIndex));
		PlayerItemScript player = getPlayItem (avaIndex);
		if (player != null) {
			//player.updateScoreText (-genzhu);
			player.showDZPKGenZhu (genzhu, seat);
		}
		dichi_dzpk += genzhu;
		text_dichi.text = dichi_dzpk + "";
		//20171013新增

//		for(int i = 0; i< chiList_dzpk.Count; i++){
//			obj_chiList [i].SetActive (true);
//			text_chiList [i].text = chiList_dzpk [i].ToString ();
//		}

	}

	/**
	 * 让牌
	 */
	public void DZPK_rangPaiResponse(ClientResponse response){
		JsonData json = JsonMapper.ToObject (response.message);
		int avaIndex = int.Parse (json ["avatarIndex"].ToString ());
		PlayerItemScript player = getPlayItem (avaIndex);
		if (player != null) {
			player.showDZPKRangPai ();
		}
	}

	/**
	 * 弃牌
	 */
	public void DZPK_qiPaiResponse(ClientResponse response){
//		int avaIndex = int.Parse (response.message);
		JsonData json = JsonMapper.ToObject (response.message);
		int avaIndex = int.Parse (json ["avatarIndex"].ToString ());

		ChiList_DZPKVO chiListVo = JsonMapper.ToObject<ChiList_DZPKVO> (response.message);//20171027新增
		chiList_dzpk = chiListVo.chiList;

		PlayerItemScript player = getPlayItem (avaIndex);
		if (player != null) {
			player.showDZPKQiPai ();
		}
	}

	/**
	 * 加注
	 */
	public void DZPK_jiaZhuResponse(ClientResponse response){
		JsonData json = JsonMapper.ToObject (response.message);
		int avaIndex = int.Parse (json ["avatarIndex"].ToString ());
		int zhu = int.Parse (json ["zhu"].ToString ());

		ChiList_DZPKVO chiListVo = JsonMapper.ToObject<ChiList_DZPKVO> (response.message);
		chiList_dzpk = chiListVo.chiList;
		int seat = getIndexByDir (getDirection (avaIndex));
		PlayerItemScript player = getPlayItem (avaIndex);
		if (player != null) {
			//player.updateScoreText (-zhu);
			player.showDZPKJiaZhu (zhu, seat);
		}
		dichi_dzpk += zhu;
		text_dichi.text = dichi_dzpk + "";

		//20171013新增
//		for(int i = 0; i< chiList_dzpk.Count; i++){
//			obj_chiList [i].SetActive (true);
//			text_chiList [i].text = chiList_dzpk [i].ToString ();
//		}
	}

	/**
	 * 全下
	 */
	public void DZPK_quanXiaResponse(ClientResponse response){
		JsonData json = JsonMapper.ToObject (response.message);


	}


	private PlayerItemScript getPlayItem(int index){
//		int uuid = int.Parse (msg);
//		int index = getIndex (uuid);
		string dirstr = getDirection (index);
		PlayerItemScript res = null;
		switch (dirstr) {
		case DirectionEnum.Bottom:
			res = playerItems [0];
			break;
		case DirectionEnum.Right:
			res = playerItems [1];
			break;
		case DirectionEnum.TopRight:
			res = playerItems [2];
			break;
		case DirectionEnum.TopLeft:
			res = playerItems [3];
			break;
		case DirectionEnum.Left:
			res = playerItems [4];
			break;
		case DirectionEnum.LeftBottom:
			res = playerItems [5];
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
	}


	private void cleanDZPKBtn(){
		panel_dzpk_zhu.SetActive (false);
	}

	private void showBtnAndGen(int genzhu){
		panel_dzpk_zhu.SetActive (true);
		btn_genzhu.SetActive (true);

		btn_rangpai.SetActive (false);

		int curscore = avatarList [getMyIndexFromList ()].scores;
		if (curscore > genzhu) {
			Text_genZhu.text = "跟" + genzhu;
			btn_jiazhu.SetActive (true);
			btn_jiazhu_an.SetActive (false);
		} else {
			Text_genZhu.text = "全下";
			btn_jiazhu.SetActive (false);
			btn_jiazhu_an.SetActive (true);
		}


		setDiChiZhuBtnShow (genzhu);
		setJiaZhuBtnShow (genzhu);




//		for (int i = 0; i < Text_jiazhu_dzpk.Count; i++) {
//			Text_jiazhu_dzpk [i].text = genzhu * (i + 1) + "";
//		}
	}

	private void showBtnAndRang(){
		panel_dzpk_zhu.SetActive (true);
		btn_genzhu.SetActive (false);
		btn_jiazhu.SetActive (true);
		btn_jiazhu_an.SetActive (false);
		btn_rangpai.SetActive (true);

		setDiChiZhuBtnShow ();
		setJiaZhuBtnShow ();


//		int xiaomangZhu = GlobalDataScript.roomVo.initFen_dzpk / 200;
//		for (int i = 0; i < Text_jiazhu_dzpk.Count; i++) {
//			Text_jiazhu_dzpk [i].text = xiaomangZhu * (i + 1) + "";
//		}
	}

	private void setDiChiZhuBtnShow(){
		int curscore = avatarList [getMyIndexFromList ()].scores;
		if (curscore >= dichi_dzpk / 2) {
			dichiZhu_dzpk [0].SetActive (true);
			dichiZhu_dzpk [1].SetActive (false);
		} else {
			dichiZhu_dzpk [0].SetActive (false);
			dichiZhu_dzpk [1].SetActive (true);
		}

		if (curscore >= dichi_dzpk) {
			dichiZhu_dzpk [2].SetActive (true);
			dichiZhu_dzpk [3].SetActive (false);
		} else {
			dichiZhu_dzpk [2].SetActive (false);
			dichiZhu_dzpk [3].SetActive (true);
		}
	}

	private void setDiChiZhuBtnShow(int genzhu){
		int curscore = avatarList [getMyIndexFromList ()].scores;
		if (curscore >= dichi_dzpk / 2 && dichi_dzpk / 2 > genzhu) {
			dichiZhu_dzpk [0].SetActive (true);
			dichiZhu_dzpk [1].SetActive (false);
		} else {
			dichiZhu_dzpk [0].SetActive (false);
			dichiZhu_dzpk [1].SetActive (true);
		}

		if (curscore >= dichi_dzpk && dichi_dzpk > genzhu) {
			dichiZhu_dzpk [2].SetActive (true);
			dichiZhu_dzpk [3].SetActive (false);
		} else {
			dichiZhu_dzpk [2].SetActive (false);
			dichiZhu_dzpk [3].SetActive (true);
		}
	}


	private void initJiaZhuText(){
		int initFen = GlobalDataScript.roomVo.initFen_dzpk;

		Text_jiazhu_dzpk [0].text = initFen / 20 + "";
		Text_jiazhu_dzpk [1].text = initFen / 20 + "";
		Text_jiazhu_dzpk [2].text = initFen / 10 + "";
		Text_jiazhu_dzpk [3].text = initFen / 10 + "";
		Text_jiazhu_dzpk [4].text = initFen / 4 + "";
		Text_jiazhu_dzpk [5].text = initFen / 4 + "";
		Text_jiazhu_dzpk [6].text = initFen / 2 + "";
		Text_jiazhu_dzpk [7].text = initFen / 2 + "";
		Text_jiazhu_dzpk [8].text = initFen + "";
		Text_jiazhu_dzpk [9].text = initFen + "";
	}

	private void initSliderZhuArray(){
		sliderZhuArray = new int[] {1,2,3,5,7,10,15,20,30,40,50,75,100};
		int initFen = GlobalDataScript.roomVo.initFen_dzpk;
		for (int i = 0; i < sliderZhuArray.Length; i++) {
			sliderZhuArray [i] *= (initFen / 100);
		}
	}

	private void setJiaZhuBtnShow(){
		int curscore = avatarList [getMyIndexFromList ()].scores;
		int initFen = GlobalDataScript.roomVo.initFen_dzpk;

		if (curscore > initFen / 20) {
			obj_jiazhu_dzpk [0].SetActive (true);
			obj_jiazhu_dzpk [1].SetActive (false);
		} else {
			obj_jiazhu_dzpk [0].SetActive (false);
			obj_jiazhu_dzpk [1].SetActive (true);
		}

		if (curscore > initFen / 10) {
			obj_jiazhu_dzpk [2].SetActive (true);
			obj_jiazhu_dzpk [3].SetActive (false);
		} else {
			obj_jiazhu_dzpk [2].SetActive (false);
			obj_jiazhu_dzpk [3].SetActive (true);
		}

		if (curscore > initFen / 4) {
			obj_jiazhu_dzpk [4].SetActive (true);
			obj_jiazhu_dzpk [5].SetActive (false);
		} else {
			obj_jiazhu_dzpk [4].SetActive (false);
			obj_jiazhu_dzpk [5].SetActive (true);
		}

		if (curscore > initFen / 2) {
			obj_jiazhu_dzpk [6].SetActive (true);
			obj_jiazhu_dzpk [7].SetActive (false);
		} else {
			obj_jiazhu_dzpk [6].SetActive (false);
			obj_jiazhu_dzpk [7].SetActive (true);
		}

		if (curscore > initFen) {
			obj_jiazhu_dzpk [8].SetActive (true);
			obj_jiazhu_dzpk [9].SetActive (false);
		} else {
			obj_jiazhu_dzpk [8].SetActive (false);
			obj_jiazhu_dzpk [9].SetActive (true);
		}

		//滑动条加注,数值与当前跟注和玩家当前剩余的注有关
		setCurSlideZhuList (curscore);
		slideZhu.maxValue = curSliderZhuList.Count-1;

		slideZhu.value = 0;
		Text_jiazhu_dzpk [Text_jiazhu_dzpk.Count-1].text = curSliderZhuList[0] + "";
	}

	private void setJiaZhuBtnShow(int genzhu){
		int curscore = avatarList [getMyIndexFromList ()].scores;
		int initFen = GlobalDataScript.roomVo.initFen_dzpk;

		if (curscore > initFen / 20 && initFen / 20 > genzhu) {
			obj_jiazhu_dzpk [0].SetActive (true);
			obj_jiazhu_dzpk [1].SetActive (false);
		} else {
			obj_jiazhu_dzpk [0].SetActive (false);
			obj_jiazhu_dzpk [1].SetActive (true);
		}

		if (curscore > initFen / 10 && initFen / 10 > genzhu) {
			obj_jiazhu_dzpk [2].SetActive (true);
			obj_jiazhu_dzpk [3].SetActive (false);
		} else {
			obj_jiazhu_dzpk [2].SetActive (false);
			obj_jiazhu_dzpk [3].SetActive (true);
		}

		if (curscore > initFen / 4 && initFen / 4 > genzhu) {
			obj_jiazhu_dzpk [4].SetActive (true);
			obj_jiazhu_dzpk [5].SetActive (false);
		} else {
			obj_jiazhu_dzpk [4].SetActive (false);
			obj_jiazhu_dzpk [5].SetActive (true);
		}

		if (curscore > initFen / 2 && initFen / 2 > genzhu) {
			obj_jiazhu_dzpk [6].SetActive (true);
			obj_jiazhu_dzpk [7].SetActive (false);
		} else {
			obj_jiazhu_dzpk [6].SetActive (false);
			obj_jiazhu_dzpk [7].SetActive (true);
		}

		if (curscore > initFen && initFen > genzhu) {
			obj_jiazhu_dzpk [8].SetActive (true);
			obj_jiazhu_dzpk [9].SetActive (false);
		} else {
			obj_jiazhu_dzpk [8].SetActive (false);
			obj_jiazhu_dzpk [9].SetActive (true);
		}

		//滑动条加注,数值与当前跟注和玩家当前剩余的注有关
		setCurSlideZhuList(curscore, genzhu * 2);
		slideZhu.maxValue = curSliderZhuList.Count-1;
		slideZhu.value = 0;
		Text_jiazhu_dzpk [Text_jiazhu_dzpk.Count-1].text = curSliderZhuList[0] + "";
	}


	private void setCurSlideZhuList(int curscore){
		curSliderZhuList.Clear ();
		for (int i = 0; i < sliderZhuArray.Length; i++) {
			if (sliderZhuArray [i] < curscore) {
				curSliderZhuList.Add (sliderZhuArray [i]);
			}
		}
		curSliderZhuList.Add (curscore);
	}

	private void setCurSlideZhuList(int curscore, int genzhu){
		curSliderZhuList.Clear ();
		curSliderZhuList.Add (genzhu);
		for (int i = 0; i < sliderZhuArray.Length; i++) {
			if (sliderZhuArray [i] > genzhu && sliderZhuArray [i] < curscore) {
				curSliderZhuList.Add (sliderZhuArray [i]);
			}
		}
		if (curSliderZhuList.Count == 1) {
			curSliderZhuList.Clear ();
		}
		curSliderZhuList.Add (curscore);
	}


	public void qiPai(){
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		cleanDZPKBtn ();
		CustomSocket.getInstance ().sendMsg (new DZPK_qiPaiRequest());
	}

	public void genZhu(){
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		cleanDZPKBtn ();
		CustomSocket.getInstance ().sendMsg (new DZPK_genZhuRequest());
	}

	public void rangPai(){
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		cleanDZPKBtn ();
		CustomSocket.getInstance ().sendMsg (new DZPK_rangPaiRequest());
	}

	public void jiaZhu(){/////
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		cleanDZPKBtn();
		panel_dzpk_Jiazhu.SetActive (true);
		
	}

	public void jiaZhuNumber(int number){
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		cleanDZPKBtn();
		panel_dzpk_Jiazhu.SetActive (false);
		string zhu = Text_jiazhu_dzpk [number*2].text;
		CustomSocket.getInstance ().sendMsg (new DZPK_jiaZhuRequest(zhu));
	}

	public void allDiChi(){
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		cleanDZPKBtn ();
		CustomSocket.getInstance ().sendMsg (new DZPK_jiaZhuRequest(dichi_dzpk + ""));
	}

	public void halfDiChi(){
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		cleanDZPKBtn ();
		CustomSocket.getInstance ().sendMsg (new DZPK_jiaZhuRequest(dichi_dzpk/2 + ""));
	}

	public void cancelJiaZhu(){
		panel_dzpk_Jiazhu.SetActive (false);
		panel_dzpk_zhu.SetActive (true);
	}

	//加注，滑动条事件
	public void slideZhuChange(){
		int index = (int)slideZhu.value;
		int value = curSliderZhuList [index];
		Text_jiazhu_dzpk [Text_jiazhu_dzpk.Count-1].text = value + "";

//		if (index == (int)slideZhu.maxValue) {
//			Text_jiazhu_dzpk [Text_jiazhu_dzpk.Count-1].text = "全下";
//		}
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

	public void myselfSoundActionPlay ()
	{
		playerItems [0].showChatAction ();
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



}
