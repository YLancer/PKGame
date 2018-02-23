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


public class MyMahjongScript : MonoBehaviour
{
	private double lastTime = 16;

	public Text versionText;
	public Text Number;
	public Text roomRemark;

	private GameObject effectGame;

	private int otherPengCard;
	private int otherGangCard;
	public ButtonActionScript btnActionScript;
	public List<Transform> parentList;
	public List<Transform> outparentList;
	public List<GameObject> dirGameList;
	//桌面东、南、西、北
	public GameObject table;
	//桌面东南西北整张图片
	public List<PlayerItemScript> playerItems;
	public Text LeavedCastNumText;
	//剩余牌的张数
	public Text LeavedRoundNumText;
	//剩余局数
	public Text lianZhuangText;
	//连庄数
	//public int StartRoundNum;
	public Transform pengGangParenTransformB;
	public Transform pengGangParenTransformL;
	public Transform pengGangParenTransformR;
	public Transform pengGangParenTransformT;
	public Transform chiParenTransformT;
	public Image passButton2;
	public List<AvatarVO> avatarList;
	public Image weipaiImg;
	public Button inviteFriendButton;
	public Text goldText;
	public GameObject gold;

	public Image live1;
	public Image live2;
	public Image live3;
	// public Image tab;
   
	public Image centerImage;

	//公告
	public GameObject noticeGameObject;
	public Text noticeText;

	public GameObject genZhuang;


	//======================================
	private int uuid;
	private float timer = 0;
	private int LeavedCardsNum;
	private int MoPaiCardPoint;
	private List<GameObject> chiCardList;
	private List<List<GameObject>> PengGangCardList;
	//碰杠牌组
	private List<List<GameObject>> PengGangList_L;
	private List<List<GameObject>> PengGangList_T;
	private List<List<GameObject>> PengGangList_R;
	private string effectType;
	private List<List<int>> mineList;
	private int gangKind;
	private int otherGangType;
	private GameObject cardOnTable;
	/// <summary>
	/// 
	/// </summary>
	private int useForGangOrPengOrChi;
	private int selfGangCardPoint;
	public Image dialog_fanhui;
	public Text dialog_fanhui_text;

	public GameObject guiObj;
	public GameObject gui2Obj;

	private GameObject touziObj;
	/// <summary>
	/// 庄家的索引
	/// </summary>
	private int bankerId;
	private int curDirIndex;
	private GameObject curCard;
	/// <summary>
	/// 鬼牌
	/// </summary>
	private int guiPai = -1;
	/// <summary>
	/// 打出来的牌
	/// </summary>
	private GameObject putOutCard;


	private int otherMoCardPoint;
	private GameObject Pointertemp;
	private int putOutCardPoint = -1;
	//打出的牌
	private int putOutCardPointAvarIndex = -1;
	//最后一个打出牌的人的index
	private string outDir;
	private int SelfAndOtherPutoutCard = -1;
	/// <summary>
	/// 当前摸的牌
	/// </summary>
	private GameObject pickCardItem;
	private GameObject otherPickCardItem;
	/// <summary>
	/// 当前的方向字符串
	/// </summary>
	private string curDirString = "B";
	private string lastDirString = "B";

	/// <summary>
	/// 普通胡牌算法
	/// </summary>
	private NormalHuScript norHu;
	/// <summary>
	/// 赖子胡牌算法
	/// </summary>
	private NaiziHuScript naiziHu;

	// Use this for initialization
	private GameToolScript gameTool;
	/**抓码动态面板**/
	private GameObject zhuamaPanel;
	/**游戏单局结束动态面板**/
	//private GameObject singalEndPanel;
	//private List<int> GameOverPlayerCoins;


	private int showTimeNumber = 0;
	private int showNoticeNumber = 0;
	private bool timeFlag = false;
	/// <summary>
	/// 手牌数组，0自己，1-右边。2-上边。3-左边
	/// </summary>
	public List<List<GameObject>> handerCardList;
	/// <summary>
	/// 打在桌子上的牌
	/// </summary>
	public List<List<GameObject>> tableCardList;
	/**后台传过来的杠牌**/
	private string[] gangPaiList;

	/**所有的抓码数据字符串**/
	private string allMas;

	private bool isFirstOpen = true;

	/**是否为抢胡 游戏结束时需置为false**/
	private bool isQiangHu = false;
	/**更否申请退出房间申请**/
	private bool canClickButtonFlag = false;

	private string passType = "";

	//private bool isSelfPickCard = false;
	private string gameRule = "";
	private biaoqingScript bqScript;
	private GameObject bqObj;

	private GameObject gui1_fangui;
	private Image gui1_fanPoint;
	private Image gui1_gui1Point;
	private Image gui1_gui11Point;

	private GameObject gui2_fangui;
	private Image gui2_fanPoint;
	private Image gui2_gui1Point;
	private Image gui2_gui2Point;
	private Image gui2_gui11Point;
	private Image gui2_gui22Point;

	private Slider dianLiangSlider;
	public GameObject Button_invite_friend;

	public GameObject panel_shanghuo;
	public List<GameObject> btn_shanghuo;
	public List<GameObject> btn_piao;

	//cheat
	public GameObject Btn_cheat;
	public GameObject cheatPanel;
	public GameObject cheatContentParent;
	private List<GameObject> cheatObjList;

	void awake ()
	{
		
	}

	void Start ()
	{
		initValue ();
		//在这里开启一个异步任务，    
		//进入loadScene方法。
		//StartCoroutine(loadScene());
		loadScene ();

		//effectType = "tianhu";
		//pengGangHuEffectCtrl ();

		//cheat
		if (GlobalDataScript.loginResponseData.account.isCheat == 1) {
			Btn_cheat.SetActive (true);
		} else {
			Btn_cheat.SetActive (false);
		}
		cheatPanel.SetActive(false);
	}

	public void initValue ()
	{

		//versionText = GameObject.Find ("Text_version").GetComponent<Text> ();
		//btnActionScript = gameObject.GetComponent<ButtonActionScript> ();

		//玩家PlayerItemScript初始化
		/*playerItems = new List<PlayerItemScript> ();
		GameObject player0 = GameObject.Find ("Player_B") as GameObject;
		playerItems.Add (player0.GetComponent<PlayerItemScript> ());

		player0 = GameObject.Find ("Player_R") as GameObject;
		playerItems.Add (player0.GetComponent<PlayerItemScript> ());

		player0 = GameObject.Find ("Player_T") as GameObject;
		playerItems.Add (player0.GetComponent<PlayerItemScript> ());

		player0 = GameObject.Find ("Player_L") as GameObject;
		playerItems.Add (player0.GetComponent<PlayerItemScript> ());
		*/

		//初始化鬼
		//guiObj = GameObject.Find ("guiObj");
		//gui2Obj = GameObject.Find ("gui2Obj");
		guiObj.SetActive (false);
		gui2Obj.SetActive (false);

		//脚本加上去
		//gameObject.GetComponent<ButtonActionScript> ().enabled = true;

		//GameObject Button_invite_friend = GameObject.Find ("Button_invite_friend");
		if (GlobalDataScript.configTemp.pay == 1) {			
			Button_invite_friend.SetActive (false);
		} else {
			Button_invite_friend.SetActive (true);
		}

	}

	IEnumerator loadBiaoQing ()
	{
		yield return new WaitForEndOfFrame ();//加上这么一句就可以先显示加载画面然后再进行加载
		bqObj = Instantiate (Resources.Load ("Prefab/Panel_biaoqing")) as GameObject;
		bqObj.transform.parent = this.gameObject.transform;
		bqObj.transform.localPosition = new Vector3 (0, 1600, 1);
		bqScript = bqObj.GetComponent<biaoqingScript> ();
	}

	public biaoqingScript getBqScript ()
	{
		return bqScript;
	}

	void loadScene ()
	{
		//yield return new WaitForEndOfFrame();//加上这么一句就可以先显示加载画面然后再进行加载

		randShowTime ();
		timeFlag = true;


		//===========================================================================================
		gameTool = new GameToolScript ();
		versionText.text = "V" + Application.version;
		//===========================================================================================

		addListener ();
		initPanel ();
		//initArrayList ();
		//initPerson ();//初始化每个成员1000分



		GlobalDataScript.isonLoginPage = false;

	
		if (GlobalDataScript.reEnterRoomData != null) { //短线重连进入房间
			GlobalDataScript.loginResponseData.roomId = GlobalDataScript.reEnterRoomData.roomId;
			reEnterRoom ();
		} else if (GlobalDataScript.roomJoinResponseData != null) //进入他人房间
            joinToRoom (GlobalDataScript.roomJoinResponseData.playerList);
		else //创建房间
            createRoomAddAvatarVO (GlobalDataScript.loginResponseData);

		//旋转东南西北桌布
		int i = getMyIndexFromList ();
		table.transform.Rotate (0, 0, -90 * i);
		//显示房主
		if (i == 0) {
			playerItems [0].showFangZhu ();
		} else if (i == 1) {
			playerItems [3].showFangZhu ();
		} else if (i == 2) {
			playerItems [2].showFangZhu ();
		} else if (i == 3) {
			playerItems [1].showFangZhu ();
		}

		GlobalDataScript.reEnterRoomData = null;      
		dialog_fanhui.gameObject.SetActive (false);

		#if UNITY_ANDROID && !UNITY_EDITOR
			StartCoroutine("UpdataBattery");
		#endif

		StartCoroutine (loadBiaoQing ());


	}

	IEnumerator UpdataBattery ()
	{
		while (true) {
			//此处的battery是一个百分比数字，比如电量是93%，则这个数字是93
			int dianliang = GetBatteryLevel ();
			dianLiangSlider.value = dianliang;
			MyDebug.Log ("battery::::" + dianliang);
			yield return new WaitForSeconds (300f);
		}
	}

	int GetBatteryLevel ()
	{
		try {
			string CapacityString = System.IO.File.ReadAllText ("/sys/class/power_supply/battery/capacity");
			return int.Parse (CapacityString);
		} catch (Exception e) {
			MyDebug.Log ("Failed to read battery power; " + e.Message);
		}
		return -1;
	}

	void randShowTime ()
	{
		showTimeNumber = (int)(UnityEngine.Random.Range (5000, 10000));
	}

	void initPanel ()
	{
		clean ();
		btnActionScript.cleanBtnShow ();
		//masContaner.SetActive (false);
	}

	public void addListener ()
	{
		SocketEventHandle.getInstance ().StartGameNotice += startGame;
		SocketEventHandle.getInstance ().pickCardCallBack += pickCard;
		SocketEventHandle.getInstance ().otherPickCardCallBack += otherPickCard;
		SocketEventHandle.getInstance ().putOutCardCallBack += otherPutOutCard;
		SocketEventHandle.getInstance ().otherUserJointRoomCallBack += otherUserJointRoom;
		SocketEventHandle.getInstance ().PengCardCallBack += otherPeng;
		SocketEventHandle.getInstance ().ChiCardCallBack += otherChi;
		SocketEventHandle.getInstance ().GangCardCallBack += gangResponse;
		SocketEventHandle.getInstance ().gangCardNotice += otherGang;
		SocketEventHandle.getInstance ().btnActionShow += actionBtnShow;
		SocketEventHandle.getInstance ().HupaiCallBack += hupaiCallBack;
		//	SocketEventHandle.getInstance ().FinalGameOverCallBack += finalGameOverCallBack;
		SocketEventHandle.getInstance ().outRoomCallback += outRoomCallbak;
		SocketEventHandle.getInstance ().dissoliveRoomResponse += dissoliveRoomResponse;
		SocketEventHandle.getInstance ().gameReadyNotice += gameReadyNotice;
		SocketEventHandle.getInstance ().offlineNotice += offlineNotice;
		SocketEventHandle.getInstance ().messageBoxNotice += messageBoxNotice;
		SocketEventHandle.getInstance ().returnGameResponse += returnGameResponse;
		SocketEventHandle.getInstance ().onlineNotice += onlineNotice;
		SocketEventHandle.getInstance ().shanghuoResponse += shanghuoResponse;
		CommonEvent.getInstance ().readyGame += markselfReadyGame;
		CommonEvent.getInstance ().closeGamePanel += exitOrDissoliveRoom;
		SocketEventHandle.getInstance ().micInputNoticeOther += micInputNotice;
		SocketEventHandle.getInstance ().gameFollowBanderNotice += gameFollowBanderNotice;
		SocketEventHandle.getInstance ().cheatCallBack += cheatCallBack;//cheat

	}

	private void removeListener ()
	{
		SocketEventHandle.getInstance ().StartGameNotice -= startGame;
		SocketEventHandle.getInstance ().pickCardCallBack -= pickCard;
		SocketEventHandle.getInstance ().otherPickCardCallBack -= otherPickCard;
		SocketEventHandle.getInstance ().putOutCardCallBack -= otherPutOutCard;
		SocketEventHandle.getInstance ().otherUserJointRoomCallBack -= otherUserJointRoom;
		SocketEventHandle.getInstance ().PengCardCallBack -= otherPeng;
		SocketEventHandle.getInstance ().ChiCardCallBack -= otherChi;
		SocketEventHandle.getInstance ().GangCardCallBack -= gangResponse;
		SocketEventHandle.getInstance ().gangCardNotice -= otherGang;
		SocketEventHandle.getInstance ().btnActionShow -= actionBtnShow;
		SocketEventHandle.getInstance ().HupaiCallBack -= hupaiCallBack;
		//SocketEventHandle.getInstance ().FinalGameOverCallBack -= finalGameOverCallBack;
		SocketEventHandle.getInstance ().outRoomCallback -= outRoomCallbak;
		SocketEventHandle.getInstance ().dissoliveRoomResponse -= dissoliveRoomResponse;
		SocketEventHandle.getInstance ().gameReadyNotice -= gameReadyNotice;
		SocketEventHandle.getInstance ().offlineNotice -= offlineNotice;
		SocketEventHandle.getInstance ().onlineNotice -= onlineNotice;
		SocketEventHandle.getInstance ().shanghuoResponse -= shanghuoResponse;
		SocketEventHandle.getInstance ().messageBoxNotice -= messageBoxNotice;
		SocketEventHandle.getInstance ().returnGameResponse -= returnGameResponse;
		CommonEvent.getInstance ().readyGame -= markselfReadyGame;
		CommonEvent.getInstance ().closeGamePanel -= exitOrDissoliveRoom;
		SocketEventHandle.getInstance ().micInputNoticeOther -= micInputNotice;
		SocketEventHandle.getInstance ().gameFollowBanderNotice -= gameFollowBanderNotice;
		SocketEventHandle.getInstance ().cheatCallBack -= cheatCallBack;
	}


	private void initArrayList ()
	{
		mineList = new List<List<int>> ();
		handerCardList = new List<List<GameObject>> ();
		tableCardList = new List<List<GameObject>> ();
		for (int i = 0; i < 4; i++) {
			handerCardList.Add (new List<GameObject> ());
			tableCardList.Add (new List<GameObject> ());
		}

		PengGangList_L = new List<List<GameObject>> ();
		PengGangList_R = new List<List<GameObject>> ();
		PengGangList_T = new List<List<GameObject>> ();
		PengGangCardList = new List<List<GameObject>> ();


	}

	/**
	private void initPerson(){
		GameOverPlayerCoins = new List<int> (4);
		GameOverPlayerCoins.Add(1000);
		GameOverPlayerCoins.Add(1000);
		GameOverPlayerCoins.Add(1000);
		GameOverPlayerCoins.Add(1000);
	}
	*/
	/// <summary>
	/// Cards the select.
	/// </summary>
	/// <param name="obj">Object.</param>
	public void cardSelect (GameObject obj)
	{
		for (int i = 0; i < handerCardList [0].Count; i++) {
			if (handerCardList [0] [i] == null) {
				handerCardList [0].RemoveAt (i);
				i--;
			} else {
				handerCardList [0] [i].transform.localPosition = new Vector3 (handerCardList [0] [i].transform.localPosition.x, -292f); //从右到左依次对齐
				handerCardList [0] [i].transform.GetComponent<bottomScript> ().selected = false;
			}
		}
		if (obj != null) {
			obj.transform.localPosition = new Vector3 (obj.transform.localPosition.x, -272f);
			obj.transform.GetComponent<bottomScript> ().selected = true;
		}
	}

	/// <summary>
	/// 开始游戏
	/// </summary>
	/// <param name="response">Response.</param>
	public void startGame (ClientResponse response)
	{
		GlobalDataScript.roomAvatarVoList = avatarList;
		//GlobalDataScript.surplusTimes -= 1;
		StartGameVO sgvo = JsonMapper.ToObject<StartGameVO> (response.message);
		bankerId = sgvo.GrabAvatarIndex;
		GlobalDataScript.roomVo.guiPai = sgvo.gui;
		GlobalDataScript.roomVo.guiPai2 = sgvo.gui2;
        
		cleanGameplayUI ();
		//开始游戏后不显示
		MyDebug.Log ("startGame");
		GlobalDataScript.surplusTimes--;
		curDirString = getDirection (bankerId);
		if (!GlobalDataScript.goldType) {
			LeavedRoundNumText.text = GlobalDataScript.surplusTimes + "/" + GlobalDataScript.roomVo.roundNumber;//刷新剩余圈数
		} else {
			LeavedRoundNumText.text = GlobalDataScript.playCountForGoldType + "";//刷新剩余圈数
		}

		if (sgvo.lianZhuang == 0) {
			lianZhuangText.text = "0";
		} else {
			lianZhuangText.text = sgvo.lianZhuang.ToString ();
		}

		if (!isFirstOpen) {
			btnActionScript = gameObject.GetComponent<ButtonActionScript> ();
			initPanel ();
			initArrayList ();
			avatarList [bankerId].main = true;
		}

		GlobalDataScript.finalGameEndVo = null;
		GlobalDataScript.mainUuid = avatarList [bankerId].account.uuid;
		initArrayList ();
		curDirString = getDirection (bankerId);
		playerItems [curDirIndex].setbankImgEnable (true);
		SetDirGameObjectAction (bankerId);
		isFirstOpen = false;
		GlobalDataScript.isOverByPlayer = false;

		mineList = sgvo.paiArray;

		UpateTimeReStart ();
		displayTouzi (sgvo.touzi, sgvo.gui, sgvo.gui2);//显示骰子  
		//displayGuiPai(sgvo.gui);

		setAllPlayerReadImgVisbleToFalse ();
		initMyCardListAndOtherCard (13, 13, 13);

		ShowLeavedCardsNumForInit ();

		if (curDirString == DirectionEnum.Bottom) {
			//isSelfPickCard = true;
			GlobalDataScript.isDrag = true;
		} else {
			//isSelfPickCard = false;
			GlobalDataScript.isDrag = false;
		}
	}

	private void cleanGameplayUI ()
	{
		canClickButtonFlag = true;
		//weipaiImg.transform.gameObject.SetActive(false);
		table.SetActive (true);
		Number.gameObject.SetActive (true);
		inviteFriendButton.transform.gameObject.SetActive (false);
		live1.transform.gameObject.SetActive (true);
		live2.transform.gameObject.SetActive (true);
		if (GlobalDataScript.roomVo.roomType == 6 && !GlobalDataScript.roomVo.tongZhuang)
			live3.transform.gameObject.SetActive (true);
		//  tab.transform.gameObject.SetActive(true);
		centerImage.transform.gameObject.SetActive (true);

	}


	public void ShowLeavedCardsNumForInit ()
	{
		RoomCreateVo roomCreateVo = GlobalDataScript.roomVo;

		bool hong = (bool)roomCreateVo.hong;
		int RoomType = (int)roomCreateVo.roomType;
		if (RoomType == 1) {//转转麻将
			LeavedCardsNum = 108;
			if (hong) {
				LeavedCardsNum = 112;
			}
		} else if (RoomType == 2) {//划水麻将
			LeavedCardsNum = 108;
			if (roomCreateVo.addWordCard) {
				LeavedCardsNum = 136;
			}
		} else if (RoomType == 3) {
			LeavedCardsNum = 108;
		} else if (RoomType == 4) {
			LeavedCardsNum = 108;
			if (roomCreateVo.addWordCard) {
				LeavedCardsNum = 136;
			} else {
				if (roomCreateVo.gui == 1)
					LeavedCardsNum = 112;
			}
		} else if (RoomType == 5) {			
			LeavedCardsNum = 136;
		} else if (RoomType == 6) {			
			LeavedCardsNum = 140;
		} else {
			LeavedCardsNum = 136;
		}
		LeavedCardsNum = LeavedCardsNum - 53;
		LeavedCastNumText.text = (LeavedCardsNum) + "";

	}

	public void CardsNumChange ()
	{
		LeavedCardsNum--;
		if (LeavedCardsNum < 0) {
			LeavedCardsNum = 0;
		}
		LeavedCastNumText.text = LeavedCardsNum + "";
	}

	/// <summary>
	/// 别人摸牌通知
	/// </summary>
	/// <param name="response">Response.</param>
	public void otherPickCard (ClientResponse response)
	{
		UpateTimeReStart ();
		JsonData json = JsonMapper.ToObject (response.message);
		//下一个摸牌人的索引
		int avatarIndex = (int)json ["avatarIndex"];
		MyDebug.Log ("otherPickCard avatarIndex = " + avatarIndex);
		otherPickCardAndCreate (avatarIndex);
		SetDirGameObjectAction (avatarIndex);
		CardsNumChange ();
	}

	private void otherPickCardAndCreate (int avatarIndex)
	{
		//getDirection (avatarIndex);
		int myIndex = getMyIndexFromList ();
		int seatIndex = avatarIndex - myIndex;
		if (seatIndex < 0) {
			seatIndex = 4 + seatIndex;
		}
		curDirString = playerItems [seatIndex].dir;
		//SetDirGameObjectAction ();
		otherMoPaiCreateGameObject (curDirString);
	}

	public void otherMoPaiCreateGameObject (string dir)
	{
		Vector3 tempVector3 = new Vector3 (0, 0);
		//Transform tempParent = null;
		switch (dir) {
		case DirectionEnum.Top://上
			//tempParent = topParent.transform;
			tempVector3 = new Vector3 (-273, 0f);
			break;
		case DirectionEnum.Left://左
			//tempParent = leftParent.transform;
			tempVector3 = new Vector3 (0, -173f);

			break;
		case DirectionEnum.Right://右
			//tempParent = rightParent.transform;
			tempVector3 = new Vector3 (0, 183f);
			break;
		}

		String path = "prefab/card/Bottom_" + dir;
		MyDebug.Log ("path  = " + path);
		otherPickCardItem = createGameObjectAndReturn (path, parentList [getIndexByDir (dir)], tempVector3);//实例化当前摸的牌
		otherPickCardItem.transform.localScale = Vector3.one;//原大小

	}

	/// <summary>
	/// 自己摸牌
	/// </summary>
	/// <param name="response">Response.</param>
	public void pickCard (ClientResponse response)
	{
		UpateTimeReStart ();
		CardVO cardvo = JsonMapper.ToObject<CardVO> (response.message);
		MoPaiCardPoint = cardvo.cardPoint;
		MyDebug.Log ("摸牌" + MoPaiCardPoint);
		SelfAndOtherPutoutCard = MoPaiCardPoint; 
		useForGangOrPengOrChi = cardvo.cardPoint;
		putCardIntoMineList (MoPaiCardPoint);
		moPai ();
		curDirString = DirectionEnum.Bottom;
		SetDirGameObjectAction (getMyIndexFromList ());
		CardsNumChange ();
		//checkHuOrGangOrPengOrChi (MoPaiCardPoint,2);
		GlobalDataScript.isDrag = true;
		//	isSelfPickCard = true;
	}

	/// <summary>
	/// 胡，杠，碰，吃，pass按钮显示.
	/// </summary>
	/// <param name="response">Response.</param>
	public void actionBtnShow (ClientResponse response)
	{
		try {
			GlobalDataScript.isDrag = false;
			string[] strs = response.message.Split (new char[1]{ ',' });
			if (curDirString == DirectionEnum.Bottom) {
				passType = "selfPickCard";
			} else {
				passType = "otherPickCard";
			}

			for (int i = 0; i < strs.Length; i++) {
				if (strs [i].Equals ("hu")) {
					btnActionScript.showBtn (1);

				} else if (strs [i].Contains ("qianghu")) {
				

					SelfAndOtherPutoutCard = int.Parse (strs [i].Split (new char[1]{ ':' }) [1]);
				

					btnActionScript.showBtn (1);
					isQiangHu = true;
				} else if (strs [i].Contains ("peng")) {
					btnActionScript.showBtn (3);
					putOutCardPoint = int.Parse (strs [i].Split (new char[1]{ ':' }) [2]);
				} else if (strs [i].Contains ("chi")) {
					btnActionScript.showBtn (4);
					putOutCardPoint = int.Parse (strs [i].Split (new char[1]{ ':' }) [1]);
				}
				if (strs [i].Contains ("gang")) {				
					btnActionScript.showBtn (2);
					gangPaiList = strs [i].Split (new char[1]{ ':' });
					List<string> gangPaiListTemp = gangPaiList.ToList ();
					gangPaiListTemp.RemoveAt (0);
					gangPaiList = gangPaiListTemp.ToArray ();
				}
			}
		} catch (Exception e) {

		}
	}

	private void initMyCardListAndOtherCard (int topCount, int leftCount, int rightCount)
	{
		dispalySelfhanderCard (mineList);//显示自己的手牌

		/*
		for (int a = 0; a < mineList [0].Count; a++) {//我的牌13张
			if (mineList [0] [a] > 0) {
				for (int b = 0; b < mineList [0] [a]; b++) {
					GameObject gob = Instantiate (Resources.Load ("prefab/card/Bottom_B")) as GameObject;
					//GameObject.Instantiate ("");
					if (gob != null) {//
						gob.transform.SetParent (parentList [0]);//设置父节点
						gob.transform.localScale = new Vector3 (1.1f, 1.1f, 1);
						gob.GetComponent<bottomScript> ().onSendMessage += cardChange;//发送消息fd
						gob.GetComponent<bottomScript> ().reSetPoisiton += cardSelect;
						gob.GetComponent<bottomScript> ().setPoint (a, -1, -1);//设置指针                                
						SetPosition (false);
						handerCardList [0].Add (gob);//增加游戏对象
					} else {
						Debug.Log ("--> gob is null");//游戏对象为空
					}
				}

			}
		}
		*/

		initOtherCardList (DirectionEnum.Left, leftCount);
		initOtherCardList (DirectionEnum.Right, rightCount);
		initOtherCardList (DirectionEnum.Top, topCount);

		if (bankerId == getMyIndexFromList ()) {
			SetPosition (true);//设置位置
			MyDebug.Log ("初始化数据自己为庄家");
			//	checkHuPai();
		} else {
			SetPosition (false);
			otherPickCardAndCreate (bankerId);
		}
	}

	//显示自己手牌中鬼的标志
	private void initMyCardListAndOtherCard_gui ()
	{		
		for (int a = 0; a < handerCardList [0].Count; a++) {
			GameObject gob = handerCardList [0] [a];
			bottomScript bts = gob.GetComponent<bottomScript> ();
			bts.setPoint (bts.getPoint (), GlobalDataScript.roomVo.guiPai, GlobalDataScript.roomVo.guiPai2);
		}
	}


	private void setAllPlayerReadImgVisbleToFalse ()
	{
		for (int i = 0; i < playerItems.Count; i++) {
			playerItems [i].readyImg.enabled = false;
		}
	}

	private void setAllPlayerHuImgVisbleToFalse ()
	{
		for (int i = 0; i < playerItems.Count; i++) {
			playerItems [i].setHuFlagHidde ();
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
		case DirectionEnum.Top: //上
			result = 2;
			break;
		case DirectionEnum.Left: //左
			result = 3;
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

	/// <summary>
	/// 
	/// </summary>
	/// <param name="initDirection"></param>
	private void initOtherCardList (string initDiretion, int count) //初始化
	{
		for (int i = 0; i < count; i++) {
			GameObject temp = Instantiate (Resources.Load ("Prefab/card/Bottom_" + initDiretion)) as GameObject; //实例化当前牌
			if (temp != null) { //有可能没牌了
				temp.transform.SetParent (parentList [getIndexByDir (initDiretion)]); //父节点
				temp.transform.localScale = Vector3.one;
				switch (initDiretion) {

				case DirectionEnum.Top: //上
					temp.transform.localPosition = new Vector3 (-204 + 38 * i, 0); //位置   
					handerCardList [2].Add (temp);
					temp.transform.localScale = Vector3.one; //原大小
					break;
				case DirectionEnum.Left: //左
					temp.transform.localPosition = new Vector3 (0, -105 + i * 30); //位置   
					temp.transform.SetSiblingIndex (0);
					handerCardList [3].Add (temp);
					break;
				case DirectionEnum.Right: //右
					temp.transform.localPosition = new Vector3 (0, 119 - i * 30); //位置     
					handerCardList [1].Add (temp);
					break;
				}
			}

		}
	}

	/// <summary>
	/// 
	/// </summary>
	public void moPai () //摸牌
	{
		pickCardItem = Instantiate (Resources.Load ("prefab/card/Bottom_B")) as GameObject; //实例化当前摸的牌
		MyDebug.Log ("摸牌 === >> " + MoPaiCardPoint);
		if (pickCardItem != null) { //有可能没牌了
			pickCardItem.name = "pickCardItem";
			pickCardItem.transform.SetParent (parentList [0]); //父节点
			pickCardItem.transform.localScale = new Vector3 (1.1f, 1.1f, 1);//原大小
			pickCardItem.transform.localPosition = new Vector3 (580f, -292f); //位置
			pickCardItem.GetComponent<bottomScript> ().onSendMessage += cardChange; //发送消息
			pickCardItem.GetComponent<bottomScript> ().reSetPoisiton += cardSelect;
			pickCardItem.GetComponent<bottomScript> ().setPoint (MoPaiCardPoint, GlobalDataScript.roomVo.guiPai, GlobalDataScript.roomVo.guiPai2); //得到索引
            
			insertCardIntoList (pickCardItem);
		}
		MyDebug.Log ("moPai  goblist count === >> " + handerCardList [0].Count);

	}

	public void putCardIntoMineList (int cardPoint)
	{
		if (mineList [0] [cardPoint] < 4) {
			mineList [0] [cardPoint]++;

		}
	}

	public void pushOutFromMineList (int cardPoint)
	{

		if (mineList [0] [cardPoint] > 0) {
			mineList [0] [cardPoint]--;
		}
	}

	/// <summary>
	/// 接收到其它人的出牌通知
	/// </summary>
	/// <param name="response">Response.</param>
	public void otherPutOutCard (ClientResponse response)
	{		
		JsonData json = JsonMapper.ToObject (response.message);
		int cardPoint = (int)json ["cardIndex"];
		int curAvatarIndex = (int)json ["curAvatarIndex"];
		putOutCardPointAvarIndex = getIndexByDir (getDirection (curAvatarIndex));
		MyDebug.Log ("otherPickCard avatarIndex = " + curAvatarIndex);
		useForGangOrPengOrChi = cardPoint;
		if (otherPickCardItem != null) {
			int dirIndex = getIndexByDir (getDirection (curAvatarIndex));
			Destroy (otherPickCardItem);
			otherPickCardItem = null;

		} else {
			int dirIndex = getIndexByDir (getDirection (curAvatarIndex));
			GameObject obj = handerCardList [dirIndex] [0];
			handerCardList [dirIndex].RemoveAt (0);
			Destroy (obj);

		}
		createPutOutCardAndPlayAction (cardPoint, curAvatarIndex);
	}

	/// <summary>
	/// 创建打来的的牌对象，并且开始播放动画
	/// </summary>
	/// <param name="cardPoint">Card point.</param>
	/// <param name="curAvatarIndex">Current avatar index.</param>
	private void createPutOutCardAndPlayAction (int cardPoint, int curAvatarIndex)
	{
		//SoundCtrl.getInstance ().playSoundByActionButton (4);

		MyDebug.Log ("put out cardPoint" + cardPoint + "-----------------------1---------------");
		;
		SoundCtrl.getInstance ().playSound (cardPoint, avatarList [curAvatarIndex].account.sex);
		Vector3 tempVector3 = new Vector3 (0, 0);
		MyDebug.Log ("put out cardPoint" + cardPoint + "-----------------------2--------------");
		outDir = getDirection (curAvatarIndex);
		switch (outDir) {
		case DirectionEnum.Top: //上
			tempVector3 = new Vector3 (0, 130f);
			break;
		case DirectionEnum.Left: //左
			tempVector3 = new Vector3 (-370, 0f);
			break;
		case DirectionEnum.Right: //右
			tempVector3 = new Vector3 (420f, 0f);
			break;
		case DirectionEnum.Bottom:
			tempVector3 = new Vector3 (0, -100f);
			break;
		}
		MyDebug.Log ("put out cardPoint" + cardPoint + "-----------------------3--------------");
		GameObject tempGameObject = createGameObjectAndReturn ("Prefab/card/PutOutCard", parentList [0], tempVector3);
		tempGameObject.name = "putOutCard";
		MyDebug.Log ("put out cardPoint" + cardPoint + "----------------------4--------------");
		MyDebug.Log ("put out cardPoint" + tempGameObject + "----------------------4.......--------------");
		tempGameObject.transform.localScale = Vector3.one;
		MyDebug.Log ("put out cardPoint" + cardPoint + "----------------------4.1--------------");
		tempGameObject.GetComponent<TopAndBottomCardScript> ();
		MyDebug.Log ("put out cardPoint" + tempGameObject + "----------------------4.......--------------");
		tempGameObject.GetComponent<TopAndBottomCardScript> ().setPoint (cardPoint);
		putOutCardPoint = cardPoint;
		MyDebug.Log ("put out cardPoint" + cardPoint + "----------------------5--------------");
		SelfAndOtherPutoutCard = cardPoint;
		putOutCard = tempGameObject;
		destroyPutOutCard (cardPoint);
		MyDebug.Log ("put out cardPoint" + cardPoint + "---------------------6--------------");
		if (putOutCard != null) {
			Destroy (putOutCard, 1f);
		}
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
		for (int i = 0; i < 4; i++) {
			myselfIndex++;
			if (myselfIndex >= 4) {
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
					return DirectionEnum.Top;
				} else {
					MyDebug.Log ("getDirection == L");
					curDirIndex = 3;
					return DirectionEnum.Left;
				}
			}
		}
		MyDebug.Log ("getDirection == B");
		curDirIndex = 0;
		return DirectionEnum.Bottom;
	}

	/// <summary>
	/// 设置红色箭头的显示方向
	/// </summary>
	public void SetDirGameObjectAction (int avatarIndex = 0) //设置方向
	{
		//UpateTimeReStart();
		for (int i = 0; i < dirGameList.Count; i++) {
			dirGameList [i].SetActive (false);
		}
		//dirGameList[getIndexByDir(curDirString)].SetActive(true);
		dirGameList [avatarIndex].SetActive (true);

	}

	public void ThrowBottom (int index)//
	{
		GameObject temp = null;
		String path = "";
		Vector3 poisVector3 = Vector3.one;
		MyDebug.Log ("put out cardPoint" + index + "---ThrowBottom---");
		if (outDir == DirectionEnum.Bottom) {
			path = "Prefab/ThrowCard/TopAndBottomCard";
			poisVector3 = new Vector3 (-261 + tableCardList [0].Count % 14 * 37, (int)(tableCardList [0].Count / 14) * 67f);
			GlobalDataScript.isDrag = false;
		} else if (outDir == DirectionEnum.Right) {
			path = "Prefab/ThrowCard/ThrowCard_R";
			poisVector3 = new Vector3 ((int)(-tableCardList [1].Count / 13 * 54f), -180f + tableCardList [1].Count % 13 * 28);
		} else if (outDir == DirectionEnum.Top) {
			path = "Prefab/ThrowCard/TopAndBottomCard";
			poisVector3 = new Vector3 (289f - tableCardList [2].Count % 14 * 37, -(int)(tableCardList [2].Count / 14) * 67f);
		} else if (outDir == DirectionEnum.Left) {
			path = "Prefab/ThrowCard/ThrowCard_L";
			poisVector3 = new Vector3 (tableCardList [3].Count / 13 * 54f, 152f - tableCardList [3].Count % 13 * 28);
			//     parenTransform = leftOutParent;
		}

		temp = createGameObjectAndReturn (path, outparentList [curDirIndex], poisVector3);
		temp.transform.localScale = Vector3.one;
		if (outDir == DirectionEnum.Right || outDir == DirectionEnum.Left) {
			temp.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (index);
		} else {
			temp.GetComponent<TopAndBottomCardScript> ().setPoint (index);
		}

		cardOnTable = temp;
		//temp.transform.SetAsLastSibling();
		tableCardList [getIndexByDir (outDir)].Add (temp);
		if (outDir == DirectionEnum.Right) {
			temp.transform.SetSiblingIndex (0);
		}
		//丢牌上
		//顶针下
		setPointGameObject (temp);
	}

	public void otherPeng (ClientResponse response)//其他人碰牌
	{
		btnActionScript.cleanChiShow ();
		UpateTimeReStart ();
		OtherPengGangBackVO cardVo = JsonMapper.ToObject<OtherPengGangBackVO> (response.message);
		otherPengCard = cardVo.cardPoint;
		lastDirString = curDirString;
		curDirString = getDirection (cardVo.avatarId);
		print ("Current Diretion==========>" + curDirString);
		SetDirGameObjectAction (cardVo.avatarId);
		effectType = "peng";
		pengGangHuEffectCtrl ();
		SoundCtrl.getInstance ().playSoundByAction ("peng", avatarList [cardVo.avatarId].account.sex);
		if (cardOnTable != null) {
			reSetOutOnTabelCardPosition (cardOnTable);
			Destroy (cardOnTable);
		}


		if (curDirString == DirectionEnum.Bottom) {  //==============================================自己碰牌
			mineList [0] [putOutCardPoint]++;
			mineList [1] [putOutCardPoint] = 2;
			int removeCount = 0;
			for (int i = 0; i < handerCardList [0].Count; i++) {
				GameObject temp = handerCardList [0] [i];
				int tempCardPoint = temp.GetComponent<bottomScript> ().getPoint ();
				if (tempCardPoint == putOutCardPoint) {

					handerCardList [0].RemoveAt (i);
					Destroy (temp);
					i--;
					removeCount++;
					if (removeCount == 2) {
						break;
					}
				}
			}
			SetPosition (true);
			bottomPeng ();
		
		} else {//==============================================其他人碰牌
			List<GameObject> tempCardList = handerCardList [getIndexByDir (curDirString)];
			string path = "Prefab/PengGangCard/PengGangCard_" + curDirString;
			if (tempCardList != null) {
				MyDebug.Log ("tempCardList.count======前" + tempCardList.Count);
				for (int i = 0; i < 2; i++) {//消除其他的人牌碰牌长度
					GameObject temp = tempCardList [0];
					Destroy (temp);
					tempCardList.RemoveAt (0);

				}
				MyDebug.Log ("tempCardList.count======前" + tempCardList.Count);

				otherPickCardItem = tempCardList [0];
				gameTool.setOtherCardObjPosition (tempCardList, curDirString, 1);
				//Destroy (tempCardList [0]);
				tempCardList.RemoveAt (0);
			}
			Vector3 tempvector3 = new Vector3 (0, 0, 0);
			List<GameObject> tempList = new List<GameObject> ();

			switch (curDirString) {
			case DirectionEnum.Right:
				for (int i = 0; i < 3; i++) {
					GameObject obj = Instantiate (Resources.Load (path)) as GameObject;
					obj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (cardVo.cardPoint);
					if (i == 1) {
						obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (getIndexByDir (lastDirString));
					}
					tempvector3 = new Vector3 (0, -122 + PengGangList_R.Count * 95 + i * 26f);
					//+ new Vector3(0, i * 26, 0);
					obj.transform.parent = pengGangParenTransformR.transform;
					obj.transform.SetSiblingIndex (0);
					obj.transform.localScale = Vector3.one;
					obj.transform.localPosition = tempvector3;
					tempList.Add (obj);
				}
				break;
			case DirectionEnum.Top:
				for (int i = 0; i < 3; i++) {
					GameObject obj = Instantiate (Resources.Load (path)) as GameObject;
					obj.GetComponent<TopAndBottomCardScript> ().setPoint (cardVo.cardPoint);
					if (i == 1) {
						obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (getIndexByDir (lastDirString));
					}
					tempvector3 = new Vector3 (251 - PengGangList_T.Count * 120f + i * 37, 0, 0);
					obj.transform.parent = pengGangParenTransformT.transform;
					obj.transform.localScale = Vector3.one;
					obj.transform.localPosition = tempvector3;
					tempList.Add (obj);
				}
				break;
			case DirectionEnum.Left:
				for (int i = 0; i < 3; i++) {
					GameObject obj = Instantiate (Resources.Load (path)) as GameObject;
					obj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (cardVo.cardPoint);
					if (i == 1) {
						obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (getIndexByDir (lastDirString));
					}
					tempvector3 = new Vector3 (0, 122 - PengGangList_L.Count * 95f - i * 26f, 0);
					obj.transform.parent = pengGangParenTransformL.transform;
					obj.transform.localScale = Vector3.one;
					obj.transform.localPosition = tempvector3;
					tempList.Add (obj);
				}
				break;
			}
			addListToPengGangList (curDirString, tempList);
		}
	}

	public int[] paixu (int cardPoint, int onePoint, int twoPoint)
	{
		int[] result = new int[3];
		if (cardPoint < onePoint) {
			if (cardPoint < twoPoint) {
				result [0] = cardPoint;
				if (onePoint < twoPoint) {
					result [1] = onePoint;
					result [2] = twoPoint;
				} else {
					result [1] = twoPoint;
					result [2] = onePoint;
				}
			} else {
				result [0] = twoPoint;
				result [1] = cardPoint;
				result [2] = onePoint;
			}
		} else {
			if (cardPoint > twoPoint) {
				result [2] = cardPoint;
				if (onePoint < twoPoint) {
					result [0] = onePoint;
					result [1] = twoPoint;
				} else {
					result [0] = twoPoint;
					result [1] = onePoint;
				}
			} else {
				result [0] = onePoint;
				result [1] = cardPoint;
				result [2] = twoPoint;
			}
		}

		return result;
	}

	public void otherChi (ClientResponse response)//吃牌
	{
		UpateTimeReStart ();
		OtherChiBackVO cardVo = JsonMapper.ToObject<OtherChiBackVO> (response.message);
		otherPengCard = cardVo.cardPoint;
		int[] chiList = paixu (cardVo.cardPoint, cardVo.onePoint, cardVo.twoPoint);
		chiList [0] = cardVo.onePoint;
		chiList [1] = cardVo.cardPoint;
		chiList [2] = cardVo.twoPoint;
		lastDirString = curDirString;
		curDirString = getDirection (cardVo.avatarId);
		print ("Current Diretion==========>" + curDirString);
		SetDirGameObjectAction (cardVo.avatarId);
		effectType = "chi";
		pengGangHuEffectCtrl ();
		SoundCtrl.getInstance ().playSoundByAction ("chi", avatarList [cardVo.avatarId].account.sex);
		if (cardOnTable != null) {
			reSetOutOnTabelCardPosition (cardOnTable);
			Destroy (cardOnTable);
		}


		if (curDirString == DirectionEnum.Bottom) {  //==============================================自己碰牌
			mineList [0] [putOutCardPoint]++;
			mineList [1] [putOutCardPoint] = 4;
			int removeCount1 = 0;
			int removeCount2 = 0;
			for (int i = 0; i < handerCardList [0].Count; i++) {
				GameObject temp = handerCardList [0] [i];
				int tempCardPoint = temp.GetComponent<bottomScript> ().getPoint ();
				if (tempCardPoint == cardVo.onePoint && removeCount1 == 0) {
					handerCardList [0].RemoveAt (i);
					Destroy (temp);
					i--;
					removeCount1++;
				}
				if (tempCardPoint == cardVo.twoPoint && removeCount2 == 0) {
					handerCardList [0].RemoveAt (i);
					Destroy (temp);
					i--;
					removeCount2++;
				}
			}
			SetPosition (true);
			bottomChi (chiList);

		} else {//==============================================其他人吃牌
			List<GameObject> tempCardList = handerCardList [getIndexByDir (curDirString)];
			string path = "Prefab/PengGangCard/PengGangCard_" + curDirString;
			if (tempCardList != null) {
				MyDebug.Log ("tempCardList.count======前" + tempCardList.Count);
				for (int i = 0; i < 2; i++) {//消除其他的人牌碰牌长度
					GameObject temp = tempCardList [0];
					Destroy (temp);
					tempCardList.RemoveAt (0);

				}
				MyDebug.Log ("tempCardList.count======前" + tempCardList.Count);

				otherPickCardItem = tempCardList [0];
				gameTool.setOtherCardObjPosition (tempCardList, curDirString, 1);
				//Destroy (tempCardList [0]);
				tempCardList.RemoveAt (0);
			}
			Vector3 tempvector3 = new Vector3 (0, 0, 0);
			List<GameObject> tempList = new List<GameObject> ();

			switch (curDirString) {
			case DirectionEnum.Right:
				for (int i = 0; i < 3; i++) {
					GameObject obj = Instantiate (Resources.Load (path)) as GameObject;
					obj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (chiList [i]);
					if (i == 1) {
						obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (getIndexByDir (lastDirString));
					}
					tempvector3 = new Vector3 (0, -122 + PengGangList_R.Count * 95 + i * 26f);
					//+ new Vector3(0, i * 26, 0);
					obj.transform.parent = pengGangParenTransformR.transform;
					obj.transform.SetSiblingIndex (0);
					obj.transform.localScale = Vector3.one;
					obj.transform.localPosition = tempvector3;
					tempList.Add (obj);
				}
				break;
			case DirectionEnum.Top:
				for (int i = 0; i < 3; i++) {
					GameObject obj = Instantiate (Resources.Load (path)) as GameObject;
					obj.GetComponent<TopAndBottomCardScript> ().setPoint (chiList [i]);
					if (i == 1) {
						obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (getIndexByDir (lastDirString));
					}
					tempvector3 = new Vector3 (251 - PengGangList_T.Count * 120f + i * 37, 0, 0);
					obj.transform.parent = pengGangParenTransformT.transform;
					obj.transform.localScale = Vector3.one;
					obj.transform.localPosition = tempvector3;
					tempList.Add (obj);
				}
				break;
			case DirectionEnum.Left:
				for (int i = 0; i < 3; i++) {
					GameObject obj = Instantiate (Resources.Load (path)) as GameObject;
					obj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (chiList [i]);
					if (i == 1) {
						obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (getIndexByDir (lastDirString));
					}
					tempvector3 = new Vector3 (0, 122 - PengGangList_L.Count * 95f - i * 26f, 0);
					obj.transform.parent = pengGangParenTransformL.transform;
					obj.transform.localScale = Vector3.one;
					obj.transform.localPosition = tempvector3;
					tempList.Add (obj);
				}
				break;
			}
			addListToPengGangList (curDirString, tempList);
		}
	}

	private void bottomPeng ()
	{
		List<GameObject> templist = new List<GameObject> ();
		for (int j = 0; j < 3; j++) {
			GameObject obj1 = createGameObjectAndReturn ("Prefab/PengGangCard/PengGangCard_B",
				                  pengGangParenTransformB.transform,
				                  new Vector3 (-370 + PengGangCardList.Count * 190 + j * 60f, 0));
			obj1.GetComponent<TopAndBottomCardScript> ().setPoint (putOutCardPoint);
			if (j == 1) {
				obj1.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (getIndexByDir (lastDirString));
			}
			obj1.transform.localScale = Vector3.one;
			templist.Add (obj1);
		}
		PengGangCardList.Add (templist);
		GlobalDataScript.isDrag = true;
	}

	private void bottomChi (int[] chiList)
	{
		List<GameObject> templist = new List<GameObject> ();
		for (int j = 0; j < 3; j++) {
			GameObject obj1 = createGameObjectAndReturn ("Prefab/PengGangCard/PengGangCard_B",
				                  pengGangParenTransformB.transform,
				                  new Vector3 (-370 + PengGangCardList.Count * 190 + j * 60f, 0));
			obj1.GetComponent<TopAndBottomCardScript> ().setPoint (chiList [j]);
			if (j == 1) {
				obj1.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (getIndexByDir (lastDirString));
			}
			obj1.transform.localScale = Vector3.one;
			templist.Add (obj1);
		}
		PengGangCardList.Add (templist);
		GlobalDataScript.isDrag = true;
	}

	private void  pengGangHuEffectCtrl ()
	{
		float time1 = 2.1f;
		if (effectType == "peng") {  // add 0828
			//pengEffectGame.SetActive (true);
			effectGame = PrefabManage.loadPerfab ("Prefab/Panel_peng_effect", 3);
			Invoke ("HidePengGangHuEff", time1);
		} else if (effectType == "gang") {
			//gangEffectGame.SetActive (true);
			effectGame = PrefabManage.loadPerfab ("Prefab/Panel_gang_effect", 3);
			Invoke ("HidePengGangHuEff", time1);
		}else if (effectType == "chi") {
			//chiEffectGame.SetActive (true);
			effectGame = PrefabManage.loadPerfab ("Prefab/Panel_chi_effect", 3);
			Invoke ("HidePengGangHuEff", time1);
		}

		return;

		float time = 2.1f;
		if (effectType == "peng") {
			//pengEffectGame.SetActive (true);
			effectGame = PrefabManage.loadPerfab ("Prefab/Panel_peng_effect");
			Invoke ("HidePengGangHuEff", time);
		} else if (effectType == "gang") {
			//gangEffectGame.SetActive (true);
			effectGame = PrefabManage.loadPerfab ("Prefab/Panel_gang_effect");
			Invoke ("HidePengGangHuEff", time);
		} else if (effectType == "hu") {
			//huEffectGame.SetActive (true);
			effectGame = PrefabManage.loadPerfab ("Prefab/Panel_hu_effect");
			Invoke ("HidePengGangHuEff", time);
		} else if (effectType == "liuju") {
			//liujuEffectGame.SetActive (true);
			effectGame = PrefabManage.loadPerfab ("Prefab/Panel_liuju_effect");
			Invoke ("HidePengGangHuEff", time);
		} else if (effectType == "chi") {
			//chiEffectGame.SetActive (true);
			effectGame = PrefabManage.loadPerfab ("Prefab/Panel_chi_effect");
			Invoke ("HidePengGangHuEff", time);
		} else if (effectType == "zimo") {
			//chiEffectGame.SetActive (true);
			effectGame = PrefabManage.loadPerfab ("Prefab/Panel_zimo_effect");
			Invoke ("HidePengGangHuEff", time);
		} else if (effectType == "pinghu") {
			//chiEffectGame.SetActive (true);
			effectGame = PrefabManage.loadPerfab ("Prefab/Panel_pinghu_effect");
			Invoke ("HidePengGangHuEff", time);
		} else if (effectType == "tianhu") {
			//chiEffectGame.SetActive (true);
			effectGame = PrefabManage.loadPerfab ("Prefab/Panel_tianhu_effect");
			Invoke ("showHuoJian", time);
		} else if (effectType == "dihu") {
			//chiEffectGame.SetActive (true);
			effectGame = PrefabManage.loadPerfab ("Prefab/Panel_dihu_effect");
			Invoke ("showHuoJian", time);
		} else if (effectType == "fei") {
			//chiEffectGame.SetActive (true);
			effectGame = PrefabManage.loadPerfab ("Prefab/Panel_fei_effect");
			Invoke ("showHuoJian", time);
		}
			

	}

	private void showHuoJian ()
	{
		DestroyImmediate (effectGame);

		effectGame = Instantiate (Resources.Load ("Prefab/Panel_huojian_effect")) as GameObject;
		effectGame.transform.parent = GlobalDataScript.getInstance ().canvsTransfrom;

		Invoke ("HidePengGangHuEff", 2f);
	}

	private void HidePengGangHuEff ()
	{
		Destroy (effectGame);
	}

	private void otherGang (ClientResponse response) //其他人杠牌
	{
		btnActionScript.cleanChiShow ();
		GangNoticeVO gangNotice = JsonMapper.ToObject<GangNoticeVO> (response.message);
		otherGangCard = gangNotice.cardPoint;
		otherGangType = gangNotice.type;
		string path = "";
		string path2 = "";
		Vector3 tempvector3 = new Vector3 (0, 0, 0);
		lastDirString = curDirString;
		curDirString = getDirection (gangNotice.avatarId);
		effectType = "gang";
		pengGangHuEffectCtrl ();
		SetDirGameObjectAction (gangNotice.avatarId);
		SoundCtrl.getInstance ().playSoundByAction ("gang", avatarList [gangNotice.avatarId].account.sex);
		List<GameObject> tempCardList = null;


		//确定牌背景（明杠，暗杠）
		switch (curDirString) {
		case DirectionEnum.Right:
			tempCardList = handerCardList [1];
			path = "Prefab/PengGangCard/PengGangCard_R";
			path2 = "Prefab/PengGangCard/GangBack_L&R";
			break;
		case DirectionEnum.Top:
			tempCardList = handerCardList [2];
			path = "Prefab/PengGangCard/PengGangCard_T";
			path2 = "Prefab/PengGangCard/GangBack_T";
			break;
		case DirectionEnum.Left:
			tempCardList = handerCardList [3];
			path = "Prefab/PengGangCard/PengGangCard_L";
			path2 = "Prefab/PengGangCard/GangBack_L&R";
			break;
		}


		List<GameObject> tempList = new List<GameObject> ();
		if (getPaiInpeng (otherGangCard, curDirString) == -1) {


			//删除玩家手牌，当玩家碰牌牌组里面的有碰牌时，不用删除手牌
			for (int i = 0; i < 3; i++) {
				GameObject temp = tempCardList [0];
				tempCardList.RemoveAt (0);
				Destroy (temp);
			}
			SetPosition (false);

			if (tempCardList != null) {
				gameTool.setOtherCardObjPosition (tempCardList, curDirString, 2);
			}

			//创建杠牌，当玩家碰牌牌组里面的无碰牌，才创建

			if (otherGangType == 0) {
				if (cardOnTable != null) {
					reSetOutOnTabelCardPosition (cardOnTable);
					Destroy (cardOnTable);
				}
				for (int i = 0; i < 4; i++) { //实例化其他人杠牌
					GameObject obj1 = Instantiate (Resources.Load (path)) as GameObject;


					switch (curDirString) {
					case DirectionEnum.Right:
						obj1.GetComponent< TopAndBottomCardScript > ().setLefAndRightPoint (otherGangCard);
						if (i == 3) {
							tempvector3 = new Vector3 (0f, -122 + PengGangList_R.Count * 95 + 33f);
							obj1.transform.parent = pengGangParenTransformR.transform;
							obj1.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (getIndexByDir (lastDirString));
						} else {
							tempvector3 = new Vector3 (0, -122 + PengGangList_R.Count * 95 + i * 28f);
							obj1.transform.parent = pengGangParenTransformR.transform;
							obj1.transform.SetSiblingIndex (0);
						}

						break;
					case DirectionEnum.Top:
						obj1.GetComponent< TopAndBottomCardScript > ().setPoint (otherGangCard);
						if (i == 3) {
							tempvector3 = new Vector3 (251 - PengGangList_T.Count * 120f + 37f, 20f);
							obj1.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (getIndexByDir (lastDirString));
						} else {
							tempvector3 = new Vector3 (251 - PengGangList_T.Count * 120f + i * 37, 0f);
						}

						obj1.transform.parent = pengGangParenTransformT.transform;
						break;
					case DirectionEnum.Left:
						obj1.GetComponent< TopAndBottomCardScript > ().setLefAndRightPoint (otherGangCard);
						if (i == 3) {
							tempvector3 = new Vector3 (0f, 122 - PengGangList_L.Count * 95f - 18f);
							obj1.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (getIndexByDir (lastDirString));
						} else {
							tempvector3 = new Vector3 (0f, 122 - PengGangList_L.Count * 95f - i * 28f);
						}

						obj1.transform.parent = pengGangParenTransformL.transform;
						break;
					}
					obj1.transform.localScale = Vector3.one;
					obj1.transform.localPosition = tempvector3;
					tempList.Add (obj1);
				}
			} else if (otherGangType == 1) {
				Destroy (otherPickCardItem);
				for (int j = 0; j < 4; j++) {
					GameObject obj2;
					if (j == 3) {
//						obj2 = Instantiate (Resources.Load (path2)) as GameObject;
						obj2 = Instantiate (Resources.Load (path)) as GameObject;
					} else {
						obj2 = Instantiate (Resources.Load (path2)) as GameObject;
					}

					switch (curDirString) {
					case DirectionEnum.Right:
						obj2.transform.parent = pengGangParenTransformR.transform;
						if (j == 3) {
							tempvector3 = new Vector3 (0f, -122 + PengGangList_R.Count * 95 + 33f);
							obj2.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (otherGangCard);

						} else {
							tempvector3 = new Vector3 (0, -122 + PengGangList_R.Count * 95 + j * 28);
							obj2.transform.SetSiblingIndex (0);
						}

						break;
					case DirectionEnum.Top:
						obj2.transform.parent = pengGangParenTransformT.transform;
						if (j == 3) {
							tempvector3 = new Vector3 (251 - PengGangList_T.Count * 120f + 37f, 10f);
							obj2.GetComponent<TopAndBottomCardScript> ().setPoint (otherGangCard);
						} else {
							tempvector3 = new Vector3 (251 - PengGangList_T.Count * 120f + j * 37, 0f);
						}

						break;
					case DirectionEnum.Left:
						obj2.transform.parent = pengGangParenTransformL.transform;
						if (j == 3) {
							tempvector3 = new Vector3 (0f, 122 - PengGangList_L.Count * 95f - 18f, 0);
							obj2.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (otherGangCard);
						} else {
							tempvector3 = new Vector3 (0, 122 - PengGangList_L.Count * 95 - j * 28f, 0);
						}

						break;
					}

					obj2.transform.localScale = Vector3.one;
					obj2.transform.localPosition = tempvector3;
					tempList.Add (obj2);
				}
			

			}
			addListToPengGangList (curDirString, tempList);
			//Destroy (otherPickCardItem);

		} else if (getPaiInpeng (otherGangCard, curDirString) != -1) {/////////end of if(getPaiInpeng(otherGangCard,curDirString) == -1)

			int gangIndex = getPaiInpeng (otherGangCard, curDirString);

			if (otherPickCardItem != null) {
				Destroy (otherPickCardItem);
			}

			GameObject objTemp = Instantiate (Resources.Load (path)) as GameObject;
			objTemp.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (getIndexByDir (lastDirString));
			switch (curDirString) {
			case DirectionEnum.Top:
				objTemp.transform.parent = pengGangParenTransformT.transform;
				tempvector3 = new Vector3 (251 - gangIndex * 120f + 37f, 20f);
				objTemp.GetComponent<TopAndBottomCardScript> ().setPoint (otherGangCard);
				PengGangList_T [gangIndex].Add (objTemp);
				break;
			case DirectionEnum.Left:
				objTemp.transform.parent = pengGangParenTransformL.transform;
				tempvector3 = new Vector3 (0f, 122 - gangIndex * 95f - 26f, 0);
				objTemp.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (otherGangCard);

				PengGangList_L [gangIndex].Add (objTemp);
				break;
			case DirectionEnum.Right:
				objTemp.transform.parent = pengGangParenTransformR.transform;
				tempvector3 = new Vector3 (0f, -122 + gangIndex * 95f + 26f);
				objTemp.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (otherGangCard);

				PengGangList_R [gangIndex].Add (objTemp);
				break;
			}
			objTemp.transform.localScale = Vector3.one;
			objTemp.transform.localPosition = tempvector3;

		}



	}


	private void addListToPengGangList (string dirString, List<GameObject> tempList)
	{
		switch (dirString) {
		case DirectionEnum.Right:
			PengGangList_R.Add (tempList);
			break;
		case DirectionEnum.Top:
			PengGangList_T.Add (tempList);
			break;
		case DirectionEnum .Left:
			PengGangList_L.Add (tempList);
			break;
		}
	}

	/**
	 * 
	 * 判断碰牌的牌组里面是否包含某个牌，用于判断是否实例化一张牌还是三张牌
	 * cardpoint：牌点
	 * direction：方向
	 * 返回-1  代表没有牌
	 * 其余牌在list的位置
	 */
	private int getPaiInpeng (int cardPoint, string direction)
	{
		List<List<GameObject>> jugeList = new List<List<GameObject>> ();
		switch (direction) {
		case DirectionEnum.Bottom://自己
			jugeList = PengGangCardList;
			break;
		case DirectionEnum.Right:
			jugeList = PengGangList_R;
			break;
		case DirectionEnum.Left:
			jugeList = PengGangList_L;
			break;
		case DirectionEnum.Top:
			jugeList = PengGangList_T;
			break;
		}

		if (jugeList == null || jugeList.Count == 0) {

			return -1;
		} 

		//循环遍历比对点数
		for (int i = 0; i < jugeList.Count; i++) {

			try {
				if (jugeList [i] [0].GetComponent<TopAndBottomCardScript> ().getPoint () == cardPoint) {
					return i;
				}
			} catch (Exception e) {
				return -1;
			}

		}

		return -1;
	}


	private void setPointGameObject (GameObject parent)
	{
		if (parent != null) {
			if (Pointertemp == null) {
				Pointertemp = Instantiate (Resources.Load ("Prefab/Pointer")) as GameObject;
			}
			Pointertemp.transform.SetParent (parent.transform);
			Pointertemp.transform.localScale = Vector3.one;
			Pointertemp.transform.localPosition = new Vector3 (0f, parent.transform.GetComponent<RectTransform> ().sizeDelta.y / 2 + 10);
		}
	}
	//顶针实现
	/// <summary>
	/// 自己打出来的牌
	/// </summary>
	/// <param name="obj">Object.</param>
	public void cardChange (GameObject obj)//
	{
		int handCardCount = handerCardList [0].Count - 1;
		if (handCardCount == 13 || handCardCount == 10 || handCardCount == 7 || handCardCount == 4 || handCardCount == 1) {
			if (GlobalDataScript.isCanChu == false)
				return;
			GlobalDataScript.isDrag = false;
			obj.GetComponent<bottomScript> ().onSendMessage -= cardChange;
			obj.GetComponent<bottomScript> ().reSetPoisiton -= cardSelect;
			MyDebug.Log ("card change over");
			int putOutCardPointTemp = obj.GetComponent<bottomScript> ().getPoint ();//将当期打出牌的点数传出
			pushOutFromMineList (putOutCardPointTemp);                         //将牌的索引从minelist里面去掉
			handerCardList [0].Remove (obj);
			MyDebug.Log ("cardchange  goblist count = > " + handerCardList [0].Count);
			Destroy (obj);
			SetPosition (false);
			createPutOutCardAndPlayAction (putOutCardPointTemp, getMyIndexFromList ());//讲拖出牌进行第一段动画的播放
			outDir = DirectionEnum.Bottom;
			//========================================================================
			CardVO cardvo = new CardVO ();
			cardvo.cardPoint = putOutCardPointTemp;
			putOutCardPointAvarIndex = getIndexByDir (getDirection (getMyIndexFromList ()));
			CustomSocket.getInstance ().sendMsg (new PutOutCardRequest (cardvo));
		}

	}

	private void cardGotoTable () //动画第二段
	{
		MyDebug.Log ("==cardGotoTable=Invoke=====>");

		if (outDir == DirectionEnum.Bottom) {
			if (putOutCard != null) {
				putOutCard.transform.DOLocalMove (new Vector3 (-261f + tableCardList [0].Count * 39, -133f), 0.4f);
				putOutCard.transform.DOScale (new Vector3 (0.5f, 0.5f), 0.4f);
			}
		} else if (outDir == DirectionEnum.Right) {
			if (putOutCard != null) {
				putOutCard.transform.DOLocalRotate (new Vector3 (0, 0, 95), 0.4f);
				putOutCard.transform.DOLocalMove (new Vector3 (448f, -140f + tableCardList [1].Count * 28), 0.4f);
				putOutCard.transform.DOScale (new Vector3 (0.5f, 0.5f), 0.4f);
			}
		} else if (outDir == DirectionEnum.Top) {
			if (putOutCard != null) {
				putOutCard.transform.DOLocalMove (new Vector3 (250f - tableCardList [2].Count * 39, 173f), 0.4f);
				putOutCard.transform.DOScale (new Vector3 (0.5f, 0.5f), 0.4f);
			}
		} else if (outDir == DirectionEnum.Left) {
			if (putOutCard != null) {
				putOutCard.transform.DOLocalRotate (new Vector3 (0, 0, -95), 0.4f);
				putOutCard.transform.DOLocalMove (new Vector3 (-364f, 160f - tableCardList [3].Count * 28), 0.4f);
				putOutCard.transform.DOScale (new Vector3 (0.5f, 0.5f), 0.4f);
			}
		}
		Invoke ("destroyPutOutCard", 0.5f);
	}

	public void insertCardIntoList (GameObject item)//插入牌的方法
	{
		if (item != null) {
			int curCardPoint = item.GetComponent<bottomScript> ().getPoint ();//得到当前牌指针
			for (int i = 0; i < handerCardList [0].Count; i++) {//i<游戏物体个数 自增
				int cardPoint = handerCardList [0] [i].GetComponent<bottomScript> ().getPoint ();//得到所有牌指针
				if (GlobalDataScript.roomVo.roomType == 6) {
					//如果是鬼牌
					if (curCardPoint == GlobalDataScript.roomVo.guiPai) {
						handerCardList [0].Insert (0, item);//在
						return;
					} else if (curCardPoint == 34) {
						if (cardPoint >= GlobalDataScript.roomVo.guiPai && cardPoint != GlobalDataScript.roomVo.guiPai) {//牌指针>=当前牌的时候插入
							handerCardList [0].Insert (i, item);//在
							return;
						}
					} else {
						if (cardPoint >= curCardPoint && cardPoint != GlobalDataScript.roomVo.guiPai) {//牌指针>=当前牌的时候插入
							if (cardPoint != 34) {
								handerCardList [0].Insert (i, item);//在
								return;
							} else {
								if (GlobalDataScript.roomVo.guiPai >= curCardPoint) {
									handerCardList [0].Insert (i, item);//在
									return;
								}
							}

						}
					}

				} else {
					if (cardPoint >= curCardPoint) {//牌指针>=当前牌的时候插入
						handerCardList [0].Insert (i, item);//在
						return;
					}
				}

			}
			handerCardList [0].Add (item);//游戏对象列表添加当前牌
		}
		item = null;
	}

	public void SetPosition (bool flag)//设置位置
	{
		int count = handerCardList [0].Count;
		//int startX = 594 - count*79;
		int startX = 594 - count * 80;
		if (flag) {
			for (int i = 0; i < count - 1; i++) {
				handerCardList [0] [i].transform.localPosition = new Vector3 (startX + i * 80f, -292f); //从左到右依次对齐
			}
			handerCardList [0] [count - 1].transform.localPosition = new Vector3 (580f, -292f); //从左到右依次对齐

		} else {
			for (int i = 0; i < count; i++) {
				handerCardList [0] [i].transform.localPosition = new Vector3 (startX + i * 80f - 80f, -292f); //从左到右依次对齐
			}
		}
	}

	/// <summary>
	/// 销毁出的牌，并且检测是否可以碰
	/// </summary>
	private void destroyPutOutCard (int cardPoint)
	{
		ThrowBottom (cardPoint);

		if (outDir != DirectionEnum.Bottom) {
			gangKind = 0;
			//checkHuOrGangOrPengOrChi (Point,1);
		}

	}

	void Update ()
	{
		timer -= Time.deltaTime;
		if (timer < 0) {
			timer = 0;
			//UpateTimeReStart();
		}
		Number.text = Math.Floor (timer) + "";

		if (timeFlag) {
			showTimeNumber--;
			if (showTimeNumber < 0) {
				timeFlag = false;
				showTimeNumber = 0;
				playNoticeAction ();
			}
		}
	}

	private void playNoticeAction ()
	{
		noticeGameObject.SetActive (true);


		if (GlobalDataScript.noticeMegs != null && GlobalDataScript.noticeMegs.Count != 0) {
			noticeText.transform.localPosition = new Vector3 (500, noticeText.transform.localPosition.y);
			noticeText.text = GlobalDataScript.noticeMegs [showNoticeNumber];
			float time = noticeText.text.Length * 0.5f + 422f / 56f;

			Tweener tweener = noticeText.transform.DOLocalMove (
				                  new Vector3 (-noticeText.text.Length * 28, noticeText.transform.localPosition.y), time)
				.OnComplete (moveCompleted);
			tweener.SetEase (Ease.Linear);
			//tweener.SetLoops(-1);
		}
	}

	void moveCompleted ()
	{
		showNoticeNumber++;
		if (showNoticeNumber == GlobalDataScript.noticeMegs.Count) {
			showNoticeNumber = 0;
		}
		noticeGameObject.SetActive (false);
		randShowTime ();
		timeFlag = true;
	}

	/// <summary>
	/// 重新开始计时
	/// </summary>
	void UpateTimeReStart ()
	{
		timer = 16;
	}

	/// <summary>
	/// 点击放弃按钮
	/// </summary>
	public void myPassBtnClick ()
	{

		passButton2.gameObject.SetActive (false);
		for (int i = 0; i < chiParenTransformT.childCount; i++) {
			GameObject go = chiParenTransformT.GetChild (i).gameObject;
			Destroy (go);
		}

		//GlobalDataScript.isDrag = true;
		btnActionScript.cleanBtnShow ();
		//nextMoPai();
		/*
		if(isSelfPickCard ){
			GlobalDataScript.isDrag = true;
			isSelfPickCard = false;
		}
		*/
		if (passType == "selfPickCard") {
			GlobalDataScript.isDrag = true;
		}
		passType = "";
		CustomSocket.getInstance ().sendMsg (new GaveUpRequest ());
	}

	public void myPengBtnClick ()
	{
		GlobalDataScript.isDrag = true;
		UpateTimeReStart ();
		CardVO cardvo = new CardVO ();
		cardvo.cardPoint = putOutCardPoint;
		CustomSocket.getInstance ().sendMsg (new PengCardRequest (cardvo));
		btnActionScript.cleanBtnShow ();
	}

	public void myChiBtnClick ()
	{
		GlobalDataScript.isDrag = true;
		UpateTimeReStart ();
		List<List<int>> result = new List<List<int>> ();
		if (GlobalDataScript.roomVo.roomType == 6)
			result = checkChi_ruijin (putOutCardPoint);
		else
			result = checkChi (putOutCardPoint);
		
		if (result.Count == 1) {
			CardVO cardvo = new CardVO ();
			cardvo.cardPoint = putOutCardPoint;
			cardvo.onePoint = result [0] [0];
			cardvo.twoPoint = result [0] [1];
			CustomSocket.getInstance ().sendMsg (new ChiRequest (cardvo));
		} else {
			chiCardList = new List<GameObject> ();
			passButton2.gameObject.SetActive (true);
			foreach (List<int> re in result) {
				GameObject obj1 = createGameObjectAndReturn ("Prefab/Panel_chi",
					                  chiParenTransformT.transform,
					                  new Vector3 (-102 * chiCardList.Count - 34, 0));
				obj1.GetComponent<PanelChiScript> ().setChiPoint (putOutCardPoint, re [0], re [1]);
				obj1.transform.localScale = Vector3.one;
				chiCardList.Add (obj1);
			}
		}

		btnActionScript.cleanBtnShow ();
	}

	public List<List<int>> checkChi_ruijin (int cardIndex)
	{
		int[] cardList = new int[35];
		for (int i = 0; i < handerCardList [0].Count; i++) {
			GameObject temp = handerCardList [0] [i];
			int tempCardPoint = temp.GetComponent<bottomScript> ().getPoint ();
			cardList [tempCardPoint]++;
		}
		int gui = GlobalDataScript.roomVo.guiPai;

		if (cardIndex == gui)//鬼不参与吃牌
			return new List<List<int>> ();
		if (cardIndex == 34)
			cardIndex = gui;
		
		if (GlobalDataScript.roomVo.roomType == 6) {			
			//假宝替换成实际牌
			cardList [gui] = 0; //真宝不参与吃
			if (cardList [34] > 0) { 
				cardList [gui] = cardList [34];
				cardList [34] = 0;
			}
		}
			
		List<List<int>> result = new List<List<int>> ();
		List<int> list = new List<int> ();
		if (cardIndex >= 0 && cardIndex <= 26) {
			int count = cardIndex / 9;
			cardIndex = cardIndex % 9;
			if (cardIndex >= 0 && cardIndex <= 8) {
				if (cardIndex == 0 && cardList [1 + 9 * count] >= 1 && cardList [2 + 9 * count] >= 1) {
					list = new List<int> ();
					list.Add (1 + 9 * count);
					list.Add (2 + 9 * count);
					result.Add (list);
				}
				if (cardIndex == 1 && (cardList [0 + 9 * count] >= 1 && cardList [2 + 9 * count] >= 1)) {
					list = new List<int> ();
					list.Add (0 + 9 * count);
					list.Add (2 + 9 * count);
					result.Add (list);
				} 
				if (cardIndex == 1 && (cardList [3 + 9 * count] >= 1 && cardList [2 + 9 * count] >= 1)) {
					list = new List<int> ();
					list.Add (3 + 9 * count);
					list.Add (2 + 9 * count);
					result.Add (list);
				} 
				if (cardIndex == 8 && cardList [7 + 9 * count] >= 1 && cardList [6 + 9 * count] >= 1) {
					list = new List<int> ();
					list.Add (7 + 9 * count);
					list.Add (6 + 9 * count);
					result.Add (list);
				} 
				if (cardIndex == 7 && (cardList [8 + 9 * count] >= 1 && cardList [6 + 9 * count] >= 1)) {
					list = new List<int> ();
					list.Add (8 + 9 * count);
					list.Add (6 + 9 * count);
					result.Add (list);
				} 
				if (cardIndex == 7 && (cardList [5 + 9 * count] >= 1 && cardList [6 + 9 * count] >= 1)) {
					list = new List<int> ();
					list.Add (5 + 9 * count);
					list.Add (6 + 9 * count);
					result.Add (list);
				} 
				if (cardIndex >= 2 && cardIndex <= 6) {
					if (cardList [cardIndex - 1 + 9 * count] >= 1 && cardList [cardIndex + 1 + 9 * count] >= 1) {
						list = new List<int> ();
						list.Add (cardIndex - 1 + 9 * count);
						list.Add (cardIndex + 1 + 9 * count);
						result.Add (list);
					}

					if (cardList [cardIndex - 1 + 9 * count] >= 1 && cardList [cardIndex - 2 + 9 * count] >= 1) {
						list = new List<int> ();
						list.Add (cardIndex - 1 + 9 * count);
						list.Add (cardIndex - 2 + 9 * count);
						result.Add (list);
					}

					if (cardList [cardIndex + 1 + 9 * count] >= 1 && cardList [cardIndex + 2 + 9 * count] >= 1) {
						list = new List<int> ();
						list.Add (cardIndex + 1 + 9 * count);
						list.Add (cardIndex + 2 + 9 * count);
						result.Add (list);
					}
				}
			} 
		}//东南西北 
		else if (cardIndex >= 27 && cardIndex <= 30) {			
			for (int i = 27; i < 31; i++) {
				if (cardList [i] >= 1 && i != cardIndex) {
					for (int j = i + 1; j < 31; j++) {
						if (cardList [j] >= 1 && j != cardIndex && i != j) {
							list = new List<int> ();
							list.Add (i);
							list.Add (j);
							result.Add (list);
						}
					}
				}
			}
		}//中发白
		else if (cardIndex >= 31 && cardIndex <= 33) {
			for (int i = 31; i < 34; i++) {
				if (cardList [i] >= 1 && i != cardIndex) {
					for (int j = i + 1; j < 34; j++) {
						if (cardList [j] >= 1 && j != cardIndex && i != j) {
							list = new List<int> ();
							list.Add (i);
							list.Add (j);
							result.Add (list);
						}
					}
				}
			}
		}
		List<List<int>> result2 = new List<List<int>> ();

		if (cardList [gui] > 0) {
			foreach (List<int> re in result) {
				list = new List<int> ();
				for (int i = 0; i < re.Count; i++) {
					if (re [i] == gui)
						list.Add (34);
					else
						list.Add (re [i]);
				}
				result2.Add (list);
			}
		} else {
			result2 = result;
		}


		return result2;
	}

	public List<List<int>> checkChi (int cardIndex)
	{
		int[] cardList = new int[34];
		for (int i = 0; i < handerCardList [0].Count; i++) {
			GameObject temp = handerCardList [0] [i];
			int tempCardPoint = temp.GetComponent<bottomScript> ().getPoint ();
			cardList [tempCardPoint]++;
		}
			
		List<List<int>> result = new List<List<int>> ();
		List<int> list = new List<int> ();
		if (cardIndex >= 0 && cardIndex <= 26) {
			int count = cardIndex / 9;
			cardIndex = cardIndex % 9;
			if (cardIndex >= 0 && cardIndex <= 8) {
				if (cardIndex == 0 && cardList [1 + 9 * count] >= 1 && cardList [2 + 9 * count] >= 1) {
					list = new List<int> ();
					list.Add (1 + 9 * count);
					list.Add (2 + 9 * count);
					result.Add (list);
				} 
				if (cardIndex == 1 && (cardList [0 + 9 * count] >= 1 && cardList [2 + 9 * count] >= 1)) {
					list = new List<int> ();
					list.Add (0 + 9 * count);
					list.Add (2 + 9 * count);
					result.Add (list);
				} 
				if (cardIndex == 1 && (cardList [3 + 9 * count] >= 1 && cardList [2 + 9 * count] >= 1)) {
					list = new List<int> ();
					list.Add (3 + 9 * count);
					list.Add (2 + 9 * count);
					result.Add (list);
				} 
				if (cardIndex == 8 && cardList [7 + 9 * count] >= 1 && cardList [6 + 9 * count] >= 1) {
					list = new List<int> ();
					list.Add (7 + 9 * count);
					list.Add (6 + 9 * count);
					result.Add (list);
				} 
				if (cardIndex == 7 && (cardList [8 + 9 * count] >= 1 && cardList [6 + 9 * count] >= 1)) {
					list = new List<int> ();
					list.Add (8 + 9 * count);
					list.Add (6 + 9 * count);
					result.Add (list);
				} 
				if (cardIndex == 7 && (cardList [5 + 9 * count] >= 1 && cardList [6 + 9 * count] >= 1)) {
					list = new List<int> ();
					list.Add (5 + 9 * count);
					list.Add (6 + 9 * count);
					result.Add (list);
				} 
				if (cardIndex >= 2 && cardIndex <= 6) {
					if (cardList [cardIndex - 1 + 9 * count] >= 1 && cardList [cardIndex + 1 + 9 * count] >= 1) {
						list = new List<int> ();
						list.Add (cardIndex - 1 + 9 * count);
						list.Add (cardIndex + 1 + 9 * count);
						result.Add (list);
					}

					if (cardList [cardIndex - 1 + 9 * count] >= 1 && cardList [cardIndex - 2 + 9 * count] >= 1) {
						list = new List<int> ();
						list.Add (cardIndex - 1 + 9 * count);
						list.Add (cardIndex - 2 + 9 * count);
						result.Add (list);
					}

					if (cardList [cardIndex + 1 + 9 * count] >= 1 && cardList [cardIndex + 2 + 9 * count] >= 1) {
						list = new List<int> ();
						list.Add (cardIndex + 1 + 9 * count);
						list.Add (cardIndex + 2 + 9 * count);
						result.Add (list);
					}
				}
			} 
		}//东南西北 
		else if (cardIndex >= 27 && cardIndex <= 30) {			
			for (int i = 27; i < 31; i++) {
				if (cardList [i] >= 1 && i != cardIndex) {
					for (int j = i + 1; j < 31; j++) {
						if (cardList [j] >= 1 && j != cardIndex && i != j) {
							list = new List<int> ();
							list.Add (i);
							list.Add (j);
							result.Add (list);
						}
					}
				}
			}
		}//中发白
		else if (cardIndex >= 31 && cardIndex <= 33) {
			for (int i = 31; i < 34; i++) {
				if (cardList [i] >= 1 && i != cardIndex) {
					for (int j = i + 1; j < 34; j++) {
						if (cardList [j] >= 1 && j != cardIndex && i != j) {
							list = new List<int> ();
							list.Add (i);
							list.Add (j);
							result.Add (list);
						}
					}
				}
			}
		}

		return result;
	}

	public void gangResponse (ClientResponse response)
	{
		UpateTimeReStart ();
		lastDirString = curDirString;
		GangBackVO gangBackVo = JsonMapper.ToObject<GangBackVO> (response.message);
		gangKind = gangBackVo.type;
		int Num = 0;
		bool pengOrNot = false;
		//checkHuOrGangOrPengOrChi (MoPaiCardPoint,2);
		//	GlobalDataScript.isDrag = true;

		if (gangBackVo.cardList.Count == 0) {
			/*创建一个摸的牌***/
			/**
			SelfAndOtherPutoutCard = gangBackVo.cardList[0]; 
			//useForGangOrPengOrChi = gangBackVo.cardList[0];
			putCardIntoMineList (gangBackVo.cardList[0]);
			moPai ();
			curDirString = DirectionEnum.Bottom;
			SetDirGameObjectAction ();
			CardsNumChange();
			**/


			if (gangKind == 0) {//明杠
				mineList [1] [selfGangCardPoint] = 3;
				/**杠牌点数**/
				//int gangpaiPonitTemp = gangBackVo.cardList [0];
				if (getPaiInpeng (selfGangCardPoint, DirectionEnum.Bottom) == -1) {//杠牌不在碰牌数组以内，一定为别人打得牌

					//销毁别人打的牌
					if (putOutCard != null) {
						Destroy (putOutCard);
					}
					if (cardOnTable != null) {
						reSetOutOnTabelCardPosition (cardOnTable);
						Destroy (cardOnTable);

					}

					//销毁手牌中的三张牌
					int removeCount = 0;
					for (int i = 0; i < handerCardList [0].Count; i++) {
						GameObject temp = handerCardList [0] [i];
						int tempCardPoint = handerCardList [0] [i].GetComponent<bottomScript> ().getPoint ();
						if (selfGangCardPoint == tempCardPoint) {
							handerCardList [0].RemoveAt (i);
							Destroy (temp);
							i--;
							removeCount++;
							if (removeCount == 3) {
								break;
							}
						}
					}

					//创建杠牌序列

					List<GameObject> gangTempList = new List<GameObject> ();
					for (int i = 0; i < 4; i++) {
						GameObject obj = createGameObjectAndReturn ("Prefab/PengGangCard/PengGangCard_B",
							                 pengGangParenTransformB.transform, new Vector3 (-370f + PengGangCardList.Count * 190f + i * 60f, 0));
						obj.GetComponent<TopAndBottomCardScript> ().setPoint (selfGangCardPoint);
						obj.transform.localScale = Vector3.one;
						if (i == 3) {

							obj.transform.localPosition = new Vector3 (-310f + PengGangCardList.Count * 190f, 24f);
							obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (getIndexByDir (lastDirString));
						}
						gangTempList.Add (obj);
					}

					//添加到杠牌数组里面
					PengGangCardList.Add (gangTempList);

				} else {//在碰牌数组以内，则一定是自摸的牌

					for (int i = 0; i < handerCardList [0].Count; i++) {
						if (handerCardList [0] [i].GetComponent<bottomScript> ().getPoint () == selfGangCardPoint) {
							GameObject temp = handerCardList [0] [i];
							handerCardList [0].RemoveAt (i);
							Destroy (temp);
							break;
						}

					}

					int index = getPaiInpeng (selfGangCardPoint, DirectionEnum.Bottom);
					//将杠牌放到对应位置
					GameObject obj = createGameObjectAndReturn ("Prefab/PengGangCard/PengGangCard_B",
						                 pengGangParenTransformB.transform, new Vector3 (-370f + PengGangCardList.Count * 190f + 0 * 60f, 0));
					obj.GetComponent<TopAndBottomCardScript> ().setPoint (selfGangCardPoint);
					obj.transform.localScale = Vector3.one;
					obj.transform.localPosition = new Vector3 (-310f + index * 190f, 24f);
					PengGangCardList [index].Add (obj);

				}
				//MoPaiCardPoint = gangBackVo.cardList [0];
				//putCardIntoMineList (gangBackVo.cardList [0]);


			} else if (gangKind == 1) { //===================================================================================暗杠

				mineList [1] [selfGangCardPoint] = 4;
				int removeCount = 0;

				for (int i = 0; i < handerCardList [0].Count; i++) {
					GameObject temp = handerCardList [0] [i];
					int tempCardPoint = handerCardList [0] [i].GetComponent<bottomScript> ().getPoint ();
					if (selfGangCardPoint == tempCardPoint) {
						handerCardList [0].RemoveAt (i);
						Destroy (temp);
						i--;
						removeCount++;
						if (removeCount == 4) {
							break;
						}
					}
				}
				List<GameObject> tempGangList = new List<GameObject> ();
				for (int i = 0; i < 4; i++) {

					if (i < 3) {
						GameObject obj = createGameObjectAndReturn ("Prefab/PengGangCard/gangBack",
							                 pengGangParenTransformB.transform, new Vector3 (-370 + PengGangCardList.Count * 190f + i * 60, 0));
						tempGangList.Add (obj);
					} else if (i == 3) {
						GameObject obj1 = createGameObjectAndReturn ("Prefab/PengGangCard/PengGangCard_B",
							                  pengGangParenTransformB.transform, new Vector3 (-310f + PengGangCardList.Count * 190f, 24f));
						obj1.GetComponent<TopAndBottomCardScript> ().setPoint (selfGangCardPoint);
						tempGangList.Add (obj1);
					}

				}

				PengGangCardList.Add (tempGangList);
			}
		} else if (gangBackVo.cardList.Count == 2) {

		}
		SetPosition (false);
		// moPai();
		//CardsNumChange();
		//GlobalDataScript.isDrag = true;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="path"></param>
	/// <param name="parent"></param>
	/// <param name="position"></param>
	/// <returns></returns>
	private GameObject createGameObjectAndReturn (string path, Transform parent, Vector3 position)
	{
		GameObject obj = Instantiate (Resources.Load (path)) as GameObject;
		obj.transform.SetParent (parent);
		//  obj.transform.parent = parent;
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = position;
		return obj;
	}

	public void myGangBtnClick ()
	{
		//useForGangOrPengOrChi = int.Parse (gangPaiList [0]);
		GlobalDataScript.isDrag = true;
		if (gangPaiList.Length == 1) {
			useForGangOrPengOrChi = int.Parse (gangPaiList [0]);
			selfGangCardPoint = useForGangOrPengOrChi;

		} else {//多张牌
			useForGangOrPengOrChi = int.Parse (gangPaiList [0]);
			selfGangCardPoint = useForGangOrPengOrChi;
		}

		CustomSocket.getInstance ().sendMsg (new GangCardRequest (useForGangOrPengOrChi, 0));
		MyDebug.Log ("==myGangBtnClick=Invoke=====>");
		SoundCtrl.getInstance ().playSoundByAction ("gang", GlobalDataScript.loginResponseData.account.sex);
		btnActionScript.cleanBtnShow ();
		effectType = "gang";
		pengGangHuEffectCtrl ();
		gangPaiList = null;
		return;


	}

	/// <summary>
	/// 清理桌面
	/// </summary>
	public void clean ()
	{
		cleanArrayList (handerCardList);
		cleanArrayList (tableCardList);
		cleanArrayList (PengGangList_L);
		cleanArrayList (PengGangCardList); 
		cleanArrayList (PengGangList_R);
		cleanArrayList (PengGangList_T);
		if (mineList != null) {
			mineList.Clear ();
		}

		if (curCard != null) {
			Destroy (curCard);
		}


		if (putOutCard != null) {
			Destroy (putOutCard);
		}

		if (pickCardItem != null) {
			Destroy (pickCardItem);
		}

		if (otherPickCardItem != null) {
			Destroy (otherPickCardItem);
		}

		guiObj.SetActive (false);
		gui2Obj.SetActive (false);
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

	public void setRoomRemark ()
	{
		RoomCreateVo roomvo = GlobalDataScript.roomVo;
		if (GlobalDataScript.goldType && GlobalDataScript.reEnterRoomData == null) {
			GlobalDataScript.totalTimes = 100000;
			foreach (AvatarVO ava in avatarList) {
				int gold = 10;
				if (roomvo.gameType == 3)
					gold = 5;
				int count = ava.account.gold / gold;
				if (GlobalDataScript.totalTimes > count) {
					GlobalDataScript.totalTimes = count;
				}
			}
			GlobalDataScript.surplusTimes = GlobalDataScript.totalTimes;
		} else {
			GlobalDataScript.totalTimes = roomvo.roundNumber;
			GlobalDataScript.surplusTimes = roomvo.roundNumber;
		}
		if (GlobalDataScript.goldType) {
			gold.SetActive (true);
			goldText.text = GlobalDataScript.loginResponseData.account.gold + "";
			live2.sprite = Resources.Load ("xianlai/play_scene/ywjs", typeof(Sprite)) as Sprite;
		}
		string roomInfo = "";
		if (GlobalDataScript.goldType) {
			roomInfo += "房间号<训练场>";

		} else {
			roomInfo += "房间号<" + roomvo.roomId + ">";
		}

		roomRemark.text = roomInfo;

		string str = "";
		if (roomvo.hong) {
			str += "逼胡麻将";
		} else if (roomvo.roomType == 1) {
			str += "转转麻将";
		} else if (roomvo.roomType == 2) {
			str += "划水麻将";
		} else if (roomvo.roomType == 3) {
			str += "长沙麻将";
		} else if (roomvo.roomType == 4) {
			str += "广东麻将";
		} else if (roomvo.roomType == 5) {
			str += "赣州冲关";
		} else if (roomvo.roomType == 6) {
			str += "瑞金麻将";
		} else if (roomvo.roomType == 7) {
			str += "红中麻将";
		} else if (roomvo.roomType == 10) {
			str += "潮汕麻将";
		} else if (roomvo.roomType == 11) {
			str += "宜春麻将";
		}

		if (!GlobalDataScript.goldType)
			str += "、圈数<" + roomvo.roundNumber + ">";

		if (roomvo.roomType == 4) {

			if (roomvo.gangHu)
				str += "、可抢杠胡";
			if (roomvo.addWordCard) {
				str += "、有风牌";
			} else {
				str += "、无风牌";
			}

			if (roomvo.sevenDouble)
				str += "、可胡七对";
			
			if (roomvo.gangHuQuanBao)
				str += "、杠爆全包";

			if (roomvo.wuGuiX2)
				str += "、无鬼加翻";
			
			if (roomvo.gui == 0)
				str += "、无鬼";
			else if (roomvo.gui == 1)
				str += "、白板做鬼";
			else if (roomvo.gui == 2)
				str += "、翻鬼";
			else if (roomvo.gui == 3)
				str += "、翻鬼 双鬼";

			if (roomvo.ma > 0) {
				str += "、抓码数" + roomvo.ma + "个";
				if (roomvo.maGenDifen)
					str += "、马跟底分";
				if (roomvo.maGenGang)
					str += "、马跟杠";
			}
		} else if (roomvo.roomType == 5) {

			if (roomvo.shangxiaFanType == 1) {
				str += "、上下翻埋地雷";
			} else {
				str += "、上下左右翻精";
			}

			if (roomvo.diFen == 1) {
				str += "、底分1分";
			} else {
				str += "、底分2分";
			}

			if (roomvo.tongZhuang) {
				str += "、通庄";
			} else {
				str += "、分庄闲";
			}

			if (roomvo.pingHu == 1) {
				str += "、可平胡";
			} else if (roomvo.pingHu == 2) {
				str += "、有精点炮不能平胡";
			} else {
				str += "、只可精钓";
			}
		} else if (roomvo.roomType == 6) {

			if (roomvo.keDianPao)
				str += "、可点炮胡";
			
			if (roomvo.diFen == 1) {
				str += "、底分1分";
			} else if (roomvo.diFen == 2) {
				str += "、底分2分";
			} else {
				str += "、底分5分";
			}

			if (roomvo.tongZhuang) {
				str += "、通庄";
			} else {
				str += "、分庄闲";
			}

			if (roomvo.lunZhuang) {
				str += "、轮庄";
			} else {
				str += "、霸王庄";
			}
		
		} else if (roomvo.roomType == 10) {

			if (roomvo.gangHu)
				str += "、可接炮胡";

			if (roomvo.genZhuang) {
				str += "、跟庄";
			}

			if (roomvo.pengpengHu) {
				str += "、碰碰胡2倍";
			}

			if (roomvo.qiDui) {
				str += "、七对2倍";
			}

			if (roomvo.qiangGangHu) {
				str += "、抢杠胡2倍";
			}

			if (roomvo.hunYiSe) {
				str += "、混一色2倍";
			}

			if (roomvo.qingYiSe) {
				str += "、清一色2倍";
			}

			if (roomvo.gangShangKaiHua) {
				str += "、杠上开花2倍";
			}

			if (roomvo.haoHuaQiDui) {
				str += "、豪华7对4倍";
			}

			if (roomvo.shiSanYao) {
				str += "、十三幺10倍";
			}

			if (roomvo.tianDiHu) {
				str += "、天地胡10倍";
			}

			if (roomvo.shuangHaoHua) {
				str += "、双豪华6倍";
			}

			if (roomvo.sanHaoHua) {
				str += "、三豪华8倍";
			}

			if (roomvo.shiBaLuoHan) {
				str += "、十八罗汉10倍";
			}

			if (roomvo.xiaoSanYuan) {
				str += "、小三元4倍";
			}

			if (roomvo.xiaoSanYuan) {
				str += "、小四喜4倍";
			}

			if (roomvo.daSanYuan) {
				str += "、大三元6倍";
			}

			if (roomvo.daSiXi) {
				str += "、大四喜6倍";
			}

			if (roomvo.huaYaoJiu) {
				str += "、花幺九6倍";
			}

			if (roomvo.fengDing == 5) {
				str += "、封顶5倍";
			} else if(roomvo.fengDing == 10) {
				str += "、封顶10倍";
			}else {
				str += "、不封顶";
			}
				
			if (roomvo.gui == 0)
				str += "、无鬼";
			else if (roomvo.gui == 1)
				str += "、白板做鬼";
			else if (roomvo.gui == 2)
				str += "、翻鬼";
			else if (roomvo.gui == 3)
				str += "、翻鬼(双鬼)";

			if (roomvo.wuGuiX2)
				str += "、无鬼加翻";

			if (roomvo.ma > 0) {
				str += "、抓码数" + roomvo.ma + "个";
				if (roomvo.maGenDifen)
					str += "、马跟底分";
				if (roomvo.maGenGang)
					str += "、马跟杠";
			}

			if (roomvo.jiejiegao)
				str += "、节节高";
			if (roomvo.AA) {
				str += "、AA制";
			} else {
				str += "、房主付费";
			}
		} else if(roomvo.roomType == 11){
			
		} else {
			
			if (roomvo.ziMo == 1) {
				str += "、只能自摸";
			} else {
				str += "、可抢杠胡";
			}
			if (roomvo.sevenDouble && roomvo.roomType != (int)GameType.GameType_MJ_HuaShui) {
				str += "、可胡七对";
			}

			if (roomvo.addWordCard) {
				str += "、有风牌";
			}
			if (roomvo.xiaYu > 0) {
				str += "、下鱼数" + roomvo.xiaYu + "条";
			}

			if (roomvo.ma > 0) {
				str += "、抓码数" + roomvo.ma + "个";

			}
		}
		if (roomvo.magnification > 0) {
			str += "、倍率：" + roomvo.magnification + "";
		}

		gameRule = str;
	}

	public void showGameRule ()
	{
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		TipsManagerScript.getInstance ().setTips (gameRule);
	}

	private void addAvatarVOToList (AvatarVO avatar)
	{
		if (avatarList == null) {
			avatarList = new List<AvatarVO> ();
		}
		avatarList.Add (avatar);
		setSeat (avatar);

		if (GlobalDataScript.goldType) {
			GlobalDataScript.totalTimes = 100000;
			foreach (AvatarVO ava in avatarList) {
				int gold = 10;
				if (GlobalDataScript.roomVo.gameType == 3)
					gold = 5;
				int count = ava.account.gold / gold;
				if (GlobalDataScript.totalTimes > count) {
					GlobalDataScript.totalTimes = count;
				}
			}
			GlobalDataScript.surplusTimes = GlobalDataScript.totalTimes;
		}
	}

	public void createRoomAddAvatarVO (AvatarVO avatar)
	{
		avatar.scores = 0;
		addAvatarVOToList (avatar);
		setRoomRemark ();
		readyGame ();
	
		markselfReadyGame ();
	
	}


	public void joinToRoom (List<AvatarVO> avatars)
	{
		avatarList = avatars;
		for (int i = 0; i < avatars.Count; i++) {
			setSeat (avatars [i]);
		}
		setRoomRemark ();
		readyGame ();
		markselfReadyGame ();
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
				seatIndex = 4 + seatIndex;
			}
			playerItems [seatIndex].setAvatarVo (avatar);
		}

	}

	/// <summary>
	/// Gets my index from list.
	/// </summary>
	/// <returns>The my index from list.</returns>
	public int getMyIndexFromList ()
	{
		if (avatarList != null) {
			for (int i = 0; i < avatarList.Count; i++) {
				if (avatarList [i].account.uuid == GlobalDataScript.loginResponseData.account.uuid || avatarList [i].account.openid == GlobalDataScript.loginResponseData.account.openid) {
					GlobalDataScript.loginResponseData.account.uuid = avatarList [i].account.uuid;
					MyDebug.Log ("数据正常返回" + i);
					return i;
				}

			}
		}

		MyDebug.Log ("数据异常返回0");
		return 0;
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

	public void otherUserJointRoom (ClientResponse response)
	{
		AvatarVO avatar = JsonMapper.ToObject<AvatarVO> (response.message);
		addAvatarVOToList (avatar);
	}


	/**
	 * 胡牌请求
	 */ 
	public void hupaiRequest ()
	{
		//20170211
		if (SelfAndOtherPutoutCard == -1) {
			int count = handerCardList [0].Count;
			bottomScript bts = handerCardList [0] [count - 1].GetComponent<bottomScript> ();
			SelfAndOtherPutoutCard = bts.getPoint ();
		}
		if (SelfAndOtherPutoutCard != -1) {
			int cardPoint = SelfAndOtherPutoutCard;//需修改成正确的胡牌cardpoint
			CardVO requestVo = new CardVO ();
			requestVo.cardPoint = cardPoint;
			if (isQiangHu) {
				requestVo.type = "qianghu";
				isQiangHu = false;
			}
			string sendMsg = JsonMapper.ToJson (requestVo);
			CustomSocket.getInstance ().sendMsg (new HupaiRequest (sendMsg));
			btnActionScript.cleanBtnShow ();
		}


		//模拟胡牌操作
		//ClientResponse response = new ClientResponse();
		//HupaiResponseItem itemData = new HupaiResponseItem();
		//itemData.cardlist = new int[2][27]{{},{}}
	}



	/**
	 * 胡牌请求回调
	 */ 
	private void hupaiCallBack (ClientResponse response)
	{
		live1.gameObject.SetActive (false);
		live2.gameObject.SetActive (false);
		live3.gameObject.SetActive (false);

		//删除这句，未区分胡家是谁
		GlobalDataScript.hupaiResponseVo = new HupaiResponseVo ();
		GlobalDataScript.hupaiResponseVo = JsonMapper.ToObject<HupaiResponseVo> (response.message);

		if (GlobalDataScript.goldType) {
			GlobalDataScript.playCountForGoldType++;
		}

		string scores = GlobalDataScript.hupaiResponseVo.currentScore;
		hupaiCoinChange (scores);
		if (GlobalDataScript.goldType) {
			int gold = 10;
			if (GlobalDataScript.roomVo.gameType == 3)
				gold = 5;
			gold = int.Parse (goldText.text) - gold;
			goldText.text = gold + "";
		}

		if (GlobalDataScript.hupaiResponseVo.type == "0") {
			
			for (int i = 0; i < GlobalDataScript.hupaiResponseVo.avatarList.Count; i++) {
				int huType = checkAvarHupai (GlobalDataScript.hupaiResponseVo.avatarList [i]);
				if (huType == 1) {//平胡
					playerItems [getIndexByDir (getDirection (i))].setHuFlagDisplay ();
					SoundCtrl.getInstance ().playSoundByAction ("hupinghu", avatarList [i].account.sex);

					effectType = "pinghu";
					pengGangHuEffectCtrl ();
				} else if (huType == 2) {
					playerItems [getIndexByDir (getDirection (i))].setHuFlagDisplay ();
					SoundCtrl.getInstance ().playSoundByAction ("zimo", avatarList [i].account.sex);

					effectType = "zimo";
					pengGangHuEffectCtrl ();
				} else if (huType == 3) {
					playerItems [getIndexByDir (getDirection (i))].setHuFlagDisplay ();
					SoundCtrl.getInstance ().playSoundByAction ("hutianhu", avatarList [i].account.sex);

					effectType = "tianhu";
					pengGangHuEffectCtrl ();
				} else if (huType == 4) {
					playerItems [getIndexByDir (getDirection (i))].setHuFlagDisplay ();
					SoundCtrl.getInstance ().playSoundByAction ("hudihu", avatarList [i].account.sex);

					effectType = "dihu";
					pengGangHuEffectCtrl ();
				} else if (huType == 5) {
					playerItems [getIndexByDir (getDirection (i))].setHuFlagDisplay ();
					SoundCtrl.getInstance ().playSoundByAction ("hufei", avatarList [i].account.sex);

					effectType = "fei";
					pengGangHuEffectCtrl ();
				} else {
					playerItems [getIndexByDir (getDirection (i))].setHuFlagHidde ();
				}

			}

			allMas = GlobalDataScript.hupaiResponseVo.allMas;
			if (GlobalDataScript.roomVo.roomType == (int)GameType.GameType_MJ_ZhuangZhuang
			    || GlobalDataScript.roomVo.roomType == (int)GameType.GameType_MJ_ChangSha) {//转转麻将显示抓码信息
				if (GlobalDataScript.roomVo.ma > 0 && allMas != null && allMas.Length > 0) {
//					zhuamaPanel = PrefabManage.loadPerfab ("prefab/Panel_ZhuaMa");
//					zhuamaPanel.GetComponent<ZhuMaScript> ().arrageMas (allMas, avatarList, GlobalDataScript.hupaiResponseVo.validMas);
//					Invoke ("openGameOverPanelSignal", 7);
					Invoke ("openGameOverPanelSignal", 3);
				} else {
					Invoke ("openGameOverPanelSignal", 3);
				}

			} else if (GlobalDataScript.roomVo.roomType == 4 
				||GlobalDataScript.roomVo.roomType == 8
				||GlobalDataScript.roomVo.roomType == 10) {//广东麻将显示抓码信息
				if (GlobalDataScript.roomVo.ma > 0 && allMas != null && allMas.Length > 0) {
//					zhuamaPanel = PrefabManage.loadPerfab ("prefab/Panel_ZhuaMa");
//					zhuamaPanel.GetComponent<ZhuMaScript> ().arrageMas (allMas, avatarList, GlobalDataScript.hupaiResponseVo.validMas, GlobalDataScript.roomVo.roomType);
//					Invoke ("openGameOverPanelSignal", 7);
					Invoke ("openGameOverPanelSignal", 3);
				} else {
					Invoke ("openGameOverPanelSignal", 3);
				}

			} else if (5 == GlobalDataScript.roomVo.roomType) { //赣州冲关显示精牌
				Invoke ("showJingPai", 3);
			} else {
				Invoke ("openGameOverPanelSignal", 3);
			}


		} else if (GlobalDataScript.hupaiResponseVo.type == "1") {

			SoundCtrl.getInstance ().playSoundByAction ("liuju", GlobalDataScript.loginResponseData.account.sex);
			effectType = "liuju";
			pengGangHuEffectCtrl ();

			if (5 == GlobalDataScript.roomVo.roomType) { //赣州冲关显示精牌
				Invoke ("showJingPai", 3);
			} else {
				Invoke ("openGameOverPanelSignal", 3);
			}
		} else {
			Invoke ("openGameOverPanelSignal", 1);
		}



	}

	/**
	 *检测某人是否胡牌 
	 */
	public int checkAvarHupai (HupaiResponseItem itemData)
	{
		string hupaiStr = itemData.totalInfo.hu;
		HuipaiObj hupaiObj = new HuipaiObj ();
		if (hupaiStr != null && hupaiStr.Length > 0) {
			hupaiObj.uuid = hupaiStr.Split (new char[1]{ ':' }) [0];
			hupaiObj.cardPiont = int.Parse (hupaiStr.Split (new char[1]{ ':' }) [1]);
			hupaiObj.type = hupaiStr.Split (new char[1]{ ':' }) [2];
			//增加判断是否是自己胡牌的判断

			if (hupaiStr.Contains ("d_other")) {//排除一炮多响的情况
				return 0;
			} else if (hupaiStr.Contains ("zi_common")) {
				return 2;

			} else if (hupaiStr.Contains ("d_self")) {
				return 1;
			} else if (hupaiObj.type == "qiyise") {
				return 1;
			} else if (hupaiObj.type == "zi_qingyise") {
				return 2;
			} else if (hupaiObj.type == "qixiaodui") {
				return 1;
			} else if (hupaiObj.type == "self_qixiaodui") {
				return 2;
			} else if (hupaiObj.type == "gangshangpao") {
				return 1;
			} else if (hupaiObj.type == "gangshanghua") {
				return 2;
			} else if (hupaiObj.type == "tian_hu") {
				return 3;//天胡
			} else if (hupaiObj.type == "di_hu") {
				return 4;//地胡
			} else if (hupaiObj.type == "jing_diao") {
				return 5;//飞
			}


		}
		return 0;
	}



	/**
	public void huPaiCoinChanges(HupaiResponseItem hupaiResponseItem,int index,int avarIndex)
	{
		int totalScore = hupaiResponseItem.totalScore;
		int  curentScore  =  int.Parse(playerItems[index].scoreText.text);
		playerItems[index].scoreText.text = curentScore+ totalScore +"";
		avatarList [avarIndex].scores = curentScore + totalScore;
		//GameOverPlayerCoins[index].ToString();
	}
*/

	private void hupaiCoinChange (string scores)
	{
		string[] scoreList = scores.Split (new char[1]{ ',' });
		if (scoreList != null && scoreList.Length > 0) {
			for (int i = 0; i < scoreList.Length - 1; i++) {
				string itemstr = scoreList [i];
				int uuid = int.Parse (itemstr.Split (new char[1]{ ':' }) [0]);
				int score = int.Parse (itemstr.Split (new char[1]{ ':' }) [1]) + 0;
				playerItems [getIndexByDir (getDirection (getIndex (uuid)))].scoreText.text = score + "";
				avatarList [getIndex (uuid)].scores = score;
			}
		}

	}


	private void openGameOverPanelSignal ()
	{	
		//单局结算
		setAllPlayerHuImgVisbleToFalse ();
		if (zhuamaPanel != null) {
			Destroy (zhuamaPanel.GetComponent<ZhuMaScript> ());
			Destroy (zhuamaPanel);
		}

		//GlobalDataScript.singalGameOver = PrefabManage.loadPerfab("prefab/Panel_Game_Over");
		GameObject obj = PrefabManage.loadPerfab ("Prefab/Panel_Game_Over");

		getDirection (bankerId);
		playerItems [curDirIndex].setbankImgEnable (false);
		if (handerCardList != null && handerCardList.Count > 0 && handerCardList [0].Count > 0) {
			for (int i = 0; i < handerCardList [0].Count; i++) {
				handerCardList [0] [i].GetComponent<bottomScript> ().onSendMessage -= cardChange;
				handerCardList [0] [i].GetComponent<bottomScript> ().reSetPoisiton -= cardSelect;
			}
		}

		initPanel ();
		obj.GetComponent<GameOverScript> ().setDisplayContent (0, avatarList, allMas, GlobalDataScript.hupaiResponseVo.validMas);
		GlobalDataScript.singalGameOverList.Add (obj);
		allMas = "";//初始化码牌数据为空
		avatarList [bankerId].main = false;


		//20171103
		for (int i = 0; i < 4; i++) {
			playerItems [i].SetShanghuo (false);
		}
	}

	/**

	//全局结束请求回调
	private void finalGameOverCallBack(ClientResponse response){
		GlobalDataScript.finalGameEndVo = JsonMapper.ToObject<FinalGameEndVo> (response.message);
		Invoke ("finalGameOver",12);
	}

	private void finalGameOver(){

		loadPerfab ("prefab/Panel_Game_Over", 1);
		initPanel ();
		weipaiImg.transform.gameObject.SetActive(false);
		inviteFriendButton.transform.gameObject.SetActive (false);
		live1.transform.gameObject.SetActive (true);
		live2.transform.gameObject.SetActive (true);
		centerImage.transform.gameObject.SetActive (true);

		Destroy (GlobalDataScript.singalGameOver.GetComponent<GameOverScript> ());
		Destroy (GlobalDataScript.singalGameOver);
		exitOrDissoliveRoom ();
	}
	*/


	private void  loadPerfab (string perfabName, int openFlag)
	{
		GameObject obj = PrefabManage.loadPerfab (perfabName);
		obj.GetComponent<GameOverScript> ().setDisplayContent (openFlag, avatarList, allMas, GlobalDataScript.hupaiResponseVo.validMas);
	}

	private void reSetOutOnTabelCardPosition (GameObject cardOnTable)
	{
		MyDebug.Log ("putOutCardPointAvarIndex===========:" + putOutCardPointAvarIndex);
		if (putOutCardPointAvarIndex != -1) {
			int objIndex = tableCardList [putOutCardPointAvarIndex].IndexOf (cardOnTable);
			if (objIndex != -1) {
				tableCardList [putOutCardPointAvarIndex].RemoveAt (objIndex);
				return;
			}
		}

	}


 

	/***
	 * 退出房间请求
	 */
	public void quiteRoom ()
	{

		//LeaveRoomScript lrs = dialog_fanhui.GetComponent<LeaveRoomScript>();
		if (bankerId == getMyIndexFromList ()) {
			dialog_fanhui_text.text = "亲，确定要解散房间吗?";
			//lrs.setTip("亲，确定要解散房间吗?");
		} else {
			dialog_fanhui_text.text = "亲，确定要离开房间吗?";
			//lrs.setTip("亲，确定要离开房间吗?");
		}

		dialog_fanhui.gameObject.SetActive (true);
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
						markselfReadyGame ();
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


	private string dissoliveRoomType = "0";

	public void dissoliveRoomRequest ()
	{
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		if (canClickButtonFlag) {
			dissoliveRoomType = "0";
			TipsManagerScript.getInstance ().loadDialog ("申请解散房间", "你确定要申请解散房间？", doDissoliveRoomRequest, cancle);
		} else {
			TipsManagerScript.getInstance ().setTips ("还没有开始游戏，不能申请退出房间");
		}
	}

	/**
    * 打开聊天窗口
    */
	public void showChat ()
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
			if (bankerId == getMyIndexFromList ()) { //我是房主
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

	/***
	 * 申请解散房间回调
	 */
	GameObject dissoDialog;

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
			
		
			if (zhuamaPanel != null && GlobalDataScript.isonApplayExitRoomstatus) {
				Destroy (zhuamaPanel.GetComponent<ZhuMaScript> ());
				Destroy (zhuamaPanel);
			}
			GlobalDataScript.isonApplayExitRoomstatus = false;
			if (dissoDialog != null) {
				GlobalDataScript.isOverByPlayer = true;
				dissoDialog.GetComponent<VoteScript> ().removeListener ();
				Destroy (dissoDialog.GetComponent<VoteScript> ());
				Destroy (dissoDialog);
			}
		
		}  
	}


	/**
	 * 申请或同意解散房间请求
	 * 
	 */ 
	public void  doDissoliveRoomRequest ()
	{
		DissoliveRoomRequestVo dissoliveRoomRequestVo = new DissoliveRoomRequestVo ();
		dissoliveRoomRequestVo.roomId = GlobalDataScript.loginResponseData.roomId;
		dissoliveRoomRequestVo.type = dissoliveRoomType;
		string sendMsg = JsonMapper.ToJson (dissoliveRoomRequestVo);
		CustomSocket.getInstance ().sendMsg (new DissoliveRoomRequest (sendMsg));
		GlobalDataScript.isonApplayExitRoomstatus = true;
	}

	private void cancle ()
	{

	}

	private void cancle1 ()
	{
		dissoliveRoomType = "2";
		doDissoliveRoomRequest ();
	}

	public void exitOrDissoliveRoom ()
	{
		/*
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
			GlobalDataScript.loadTime = GlobalDataScript.GetTimeStamp ();
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
		*/

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

	public void gameReadyNotice (ClientResponse response)
	{
		//===============================================
		JsonData json = JsonMapper.ToObject (response.message);
		int avatarIndex = Int32.Parse (json ["avatarIndex"].ToString ());
		int myIndex = getMyIndexFromList ();
		int seatIndex = avatarIndex - myIndex;
		if (seatIndex < 0) {
			seatIndex = 4 + seatIndex;
		}
		playerItems [seatIndex].readyImg.enabled = true;
		avatarList [avatarIndex].isReady = true;

		string readyUid = json ["readyUid"].ToString ();
		//20170310 增加一句结算时 某玩家断线后重连 看不到他人准备 
		for (int i = 0; i < avatarList.Count; i++) {
			if (readyUid.IndexOf (avatarList [i].account.uuid.ToString ()) != -1) {
				avatarList [i].isReady = true;
				setSeat (avatarList [i]);
			}
		}
	}


	private void gameFollowBanderNotice (ClientResponse response)
	{
		genZhuang.SetActive (true);
		Invoke ("hideGenzhuang", 2f);
	}

	private void hideGenzhuang ()
	{
		genZhuang.SetActive (false);
	}

	/*************************断线重连*********************************/
	public void reEnterRoom ()
	{
		
		if (GlobalDataScript.reEnterRoomData != null) {
			//显示房间基本信息

			/*
			GlobalDataScript.roomVo.addWordCard = GlobalDataScript.reEnterRoomData.addWordCard;
			GlobalDataScript.roomVo.hong = GlobalDataScript.reEnterRoomData.hong;
			GlobalDataScript.roomVo.name = GlobalDataScript.reEnterRoomData.name;
			GlobalDataScript.roomVo.roomId = GlobalDataScript.reEnterRoomData.roomId;
			GlobalDataScript.roomVo.roomType = GlobalDataScript.reEnterRoomData.roomType;
			GlobalDataScript.roomVo.roundNumber = GlobalDataScript.reEnterRoomData.roundNumber;
			GlobalDataScript.roomVo.sevenDouble = GlobalDataScript.reEnterRoomData.sevenDouble;
			GlobalDataScript.roomVo.xiaYu = GlobalDataScript.reEnterRoomData.xiaYu;
			GlobalDataScript.roomVo.ziMo = GlobalDataScript.reEnterRoomData.ziMo;
			GlobalDataScript.roomVo.magnification = GlobalDataScript.reEnterRoomData.magnification;
			GlobalDataScript.roomVo.ma = GlobalDataScript.reEnterRoomData.ma;
			GlobalDataScript.roomVo.gui = GlobalDataScript.reEnterRoomData.gui;
			GlobalDataScript.roomVo.gangHu = GlobalDataScript.reEnterRoomData.gangHu;
			GlobalDataScript.roomVo.gangHuQuanBao = GlobalDataScript.reEnterRoomData.gangHuQuanBao;
			GlobalDataScript.roomVo.wuGuiX2 = GlobalDataScript.reEnterRoomData.wuGuiX2;
			GlobalDataScript.roomVo.guiPai = GlobalDataScript.reEnterRoomData.guiPai;
			GlobalDataScript.roomVo.guiPai2 = GlobalDataScript.reEnterRoomData.guiPai2;
			GlobalDataScript.roomVo.maGenDifen = GlobalDataScript.reEnterRoomData.maGenDifen;
			GlobalDataScript.roomVo.maGenGang = GlobalDataScript.reEnterRoomData.maGenGang;
			GlobalDataScript.roomVo.shangxiaFanType = GlobalDataScript.reEnterRoomData.shangxiaFanType;
			GlobalDataScript.roomVo.diFen = GlobalDataScript.reEnterRoomData.diFen;
			GlobalDataScript.roomVo.tongZhuang = GlobalDataScript.reEnterRoomData.tongZhuang;
			GlobalDataScript.roomVo.pingHu = GlobalDataScript.reEnterRoomData.pingHu;
			GlobalDataScript.roomVo.keDianPao = GlobalDataScript.reEnterRoomData.keDianPao;
			GlobalDataScript.roomVo.lunZhuang = GlobalDataScript.reEnterRoomData.lunZhuang;
			GlobalDataScript.roomVo.goldType = GlobalDataScript.reEnterRoomData.goldType;
			*/

			GlobalDataScript.roomVo = GlobalDataScript.reEnterRoomData;

			avatarList = GlobalDataScript.reEnterRoomData.playerList;
			setRoomRemark ();
			//设置座位


			GlobalDataScript.roomAvatarVoList = GlobalDataScript.reEnterRoomData.playerList;
			for (int i = 0; i < avatarList.Count; i++) {
				setSeat (avatarList [i]);
				if (avatarList [i].main) {
					GlobalDataScript.mainUuid = avatarList [i].account.uuid;
					bankerId = i;
				}
			}

			recoverOtherGlobalData ();
			int[][] selfPaiArray = GlobalDataScript.reEnterRoomData.playerList [getMyIndexFromList ()].paiArray;
			if (selfPaiArray == null || selfPaiArray.Length == 0) {//游戏还没有开始
//				int myIndexTmp = getMyIndexFromList ();//XXX
//				bool readytmp = GlobalDataScript.reEnterRoomData.playerList [myIndexTmp].isReady;//XXX
				if(GlobalDataScript.reEnterRoomData.playerList [getMyIndexFromList ()].isReady == false){//20171103
//				if (!readytmp) {
					readyGame ();
				} 
			} else {//牌局已开始
				if (GlobalDataScript.roomVo.roomType == 11) {
					int myIndexTmp = getMyIndexFromList ();
					if (avatarList [myIndexTmp].piao == -1) {
						cleanGameplayUI ();
						panel_shanghuo.SetActive (true);
						btn_piao [0].SetActive (true);
						btn_piao [1].SetActive (true);
						btn_piao [2].SetActive (true);

						if (avatarList [myIndexTmp].shanghuo == true) {
							btn_shanghuo [0].SetActive (false);
							btn_shanghuo [1].SetActive (false);
						} else {
							btn_shanghuo [0].SetActive (true);
							btn_shanghuo [1].SetActive (true);
						}

						for (int i = 0; i < avatarList.Count; i++) {
							if (avatarList [i].shanghuo) {
								int seatIndex = i - myIndexTmp;
								if (seatIndex < 0) {
									seatIndex = 4 + seatIndex;
								}
								playerItems [seatIndex].SetShanghuo(true);
							}

						}

						return;
					}

					for (int i = 0; i < avatarList.Count; i++) {
						if (avatarList [i].shanghuo) {
							int seatIndex = i - myIndexTmp;
							if (seatIndex < 0) {
								seatIndex = 4 + seatIndex;
							}
							playerItems [seatIndex].SetShanghuo(true);
						}

					}

				}


				setAllPlayerReadImgVisbleToFalse ();
				cleanGameplayUI ();
				initArrayList ();

				//显示打牌数据
				displayTableCards ();
                
				//显示碰牌
				displayOtherHandercard ();//显示其他玩家的手牌
				displayallGangCard ();//显示杠牌
				displayPengCard ();//显示碰牌
				displayChiCard ();//显示吃牌
				mineList = ToList (GlobalDataScript.reEnterRoomData.playerList [getMyIndexFromList ()].paiArray);
				dispalySelfhanderCard (mineList);//显示自己的手牌
				//显示鬼牌
				displayGuiPai ();

				CustomSocket.getInstance ().sendMsg (new CurrentStatusRequest ());
			}

			//cheat
			if (GlobalDataScript.loginResponseData.account.isCheat == 1) {
				Btn_cheat.SetActive (true);
			} else {
				Btn_cheat.SetActive (false);
			}
			cheatPanel.SetActive(false);
		}

	}




	//恢复其他全局数据
	private void recoverOtherGlobalData ()
	{
		int selfIndex = getMyIndexFromList ();
		GlobalDataScript.loginResponseData.account.roomcard = GlobalDataScript.reEnterRoomData.playerList [selfIndex].account.roomcard;//恢复房卡数据，此时主界面还没有load所以无需操作界面显示

	}




	private void dispalySelfhanderCard (List<List<int>> mineList)
	{
		//排序
		if (GlobalDataScript.roomVo.roomType == 6) {
			if (mineList [0] [GlobalDataScript.roomVo.guiPai] > 0) {
				for (int j = 0; j < mineList [0] [GlobalDataScript.roomVo.guiPai]; j++) {
					GameObject gob = Instantiate (Resources.Load ("prefab/card/Bottom_B")) as GameObject;
					//GameObject.Instantiate ("");

					if (gob != null) {//
						gob.transform.SetParent (parentList [0]);//设置父节点
						gob.transform.localScale = new Vector3 (1.1f, 1.1f, 1);
						gob.GetComponent<bottomScript> ().onSendMessage += cardChange;//发送消息fd
						gob.GetComponent<bottomScript> ().reSetPoisiton += cardSelect;
						gob.GetComponent<bottomScript> ().setPoint (GlobalDataScript.roomVo.guiPai, GlobalDataScript.roomVo.guiPai, GlobalDataScript.roomVo.guiPai2);//设置指针                                                                                                                 
						handerCardList [0].Add (gob);//增加游戏对象
					}
				}
			}

			mineList [0] [GlobalDataScript.roomVo.guiPai] = mineList [0] [34];
			mineList [0] [34] = 0;
		}

		// 
		for (int i = 0; i < mineList [0].Count; i++) {
			if (mineList [0] [i] > 0) {
				for (int j = 0; j < mineList [0] [i]; j++) {
					GameObject gob = Instantiate (Resources.Load ("prefab/card/Bottom_B")) as GameObject;
					//GameObject.Instantiate ("");

					if (gob != null) {//
						gob.transform.SetParent (parentList [0]);//设置父节点
						gob.transform.localScale = new Vector3 (1.1f, 1.1f, 1);
						gob.GetComponent<bottomScript> ().onSendMessage += cardChange;//发送消息fd
						gob.GetComponent<bottomScript> ().reSetPoisiton += cardSelect;
						if (GlobalDataScript.roomVo.roomType == 6) {
							if (i == GlobalDataScript.roomVo.guiPai) {
								gob.GetComponent<bottomScript> ().setPoint (34, GlobalDataScript.roomVo.guiPai, GlobalDataScript.roomVo.guiPai2);//设置指针  
							} else {
								gob.GetComponent<bottomScript> ().setPoint (i, GlobalDataScript.roomVo.guiPai, GlobalDataScript.roomVo.guiPai2);//设置指针  
							}
						} else {
							gob.GetComponent<bottomScript> ().setPoint (i, GlobalDataScript.roomVo.guiPai, GlobalDataScript.roomVo.guiPai2);//设置指针  
						}						                                                                                                               
						handerCardList [0].Add (gob);//增加游戏对象
					}
				}

			}
		}
		SetPosition (false);
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

	public void myselfSoundActionPlay ()
	{
		playerItems [0].showChatAction ();
	}


	/**显示打牌数据在桌面**/
	private void displayTableCards ()
	{
		//List<int[]> chupaiList = new List<int[]> ();
		for (int i = 0; i < GlobalDataScript.reEnterRoomData.playerList.Count; i++) {
			int[] chupai = GlobalDataScript.reEnterRoomData.playerList [i].chupais;
			outDir = getDirection (getIndex (GlobalDataScript.reEnterRoomData.playerList [i].account.uuid));
			if (chupai != null && chupai.Length > 0) {
				for (int j = 0; j < chupai.Length; j++) {
					ThrowBottom (chupai [j]);
				}
			}

		}
	}

	/**显示桌面鬼牌**/
	private void displayTouzi (int touzi, int gui, int gui2)
	{
		displayFanGui (touzi, gui, gui2);
		return;
		if (gui != -1 && GlobalDataScript.roomVo.roomType == 4 
			&& (GlobalDataScript.roomVo.gui == 2 || GlobalDataScript.roomVo.gui == 3)) { //显示骰子
			int r1 = touzi / 10;
			int r2 = touzi % 10;
			touziObj = PrefabManage.loadPerfab ("Prefab/Panel_touzi");
			TouziActionScript bts = touziObj.GetComponent<TouziActionScript> ();
			bts.setResult (r1, r2);
			touziObj.SetActive (true);

			Invoke ("displayGuiPai", 3.2f);
		} else {
			displayGuiPai ();
		}
	}

	//显示翻鬼动画——仿闲来广东麻将
	private void displayFanGui (int touzi, int gui, int gui2)
	{
		if (gui != -1 && (GlobalDataScript.roomVo.roomType == 4 
			|| GlobalDataScript.roomVo.roomType == 10)
			&& (GlobalDataScript.roomVo.gui == 2 
				|| GlobalDataScript.roomVo.gui == 3)) { 
			//显示骰子
			int iGui = 0;
			if (gui == 0)
				iGui = 8;
			else if (gui < 9)
				iGui = gui - 1;
			else if (gui == 9)
				iGui = 17;
			else if (gui < 18)
				iGui = gui - 1;
			else if (gui == 18)
				iGui = 26;
			else if (gui < 27)
				iGui = gui - 1;
			else if (gui == 27)
				iGui = 30;
			else if (gui < 31)
				iGui = gui - 1;
			else if (gui == 31)
				iGui = 33;
			else if (gui < 34)
				iGui = gui - 1;
			Sprite sp = new Sprite ();
			if (GlobalDataScript.roomVo.gui == 2) {

				gui1_fangui = PrefabManage.loadPerfab ("Prefab/Panel_FanGui1", 2);
				gui1_fangui.GetComponent<fanguiScript> ().setFanPoint (iGui);
				gui1_fangui.GetComponent<fanguiScript> ().setGui1 (gui);
				gui1_fangui.SetActive (true);
			}

			if (GlobalDataScript.roomVo.gui == 3) {
				gui2_fangui = PrefabManage.loadPerfab ("Prefab/Panel_FanGui2", 2);
				gui2_fangui.GetComponent<fanguiScript> ().setFanPoint (iGui);
				gui2_fangui.GetComponent<fanguiScript> ().setGui1 (gui);
				gui2_fangui.GetComponent<fanguiScript> ().setGui2 (gui2);
				gui2_fangui.SetActive (true);
			}

			Invoke ("displayGuiPai", 2.5f);

		} else if (GlobalDataScript.roomVo.roomType == 5) {
			gui2_fangui = PrefabManage.loadPerfab ("Prefab/Panel_FanGui2", 2);
			gui2_fangui.GetComponent<fanguiScript> ().setFanPoint (gui);
			gui2_fangui.GetComponent<fanguiScript> ().setGui1 (gui);
			gui2_fangui.GetComponent<fanguiScript> ().setGui2 (gui2);
			gui2_fangui.SetActive (true);

			Invoke ("displayGuiPai", 2.5f);
		} else if (GlobalDataScript.roomVo.roomType == 6) {
			//显示骰子
			int iGui = 0;
			if (gui == 0)
				iGui = 8;
			else if (gui < 9)
				iGui = gui - 1;
			else if (gui == 9)
				iGui = 17;
			else if (gui < 18)
				iGui = gui - 1;
			else if (gui == 18)
				iGui = 26;
			else if (gui < 27)
				iGui = gui - 1;
			else if (gui == 27)
				iGui = 30;
			else if (gui < 31)
				iGui = gui - 1;
			else if (gui == 31)
				iGui = 33;
			else if (gui < 34)
				iGui = gui - 1;
			else if (gui == 34)
				iGui = 34;
			
			gui1_fangui = PrefabManage.loadPerfab ("Prefab/Panel_FanGui1", 2);
			gui1_fangui.GetComponent<fanguiScript> ().setFanPoint (iGui);
			gui1_fangui.GetComponent<fanguiScript> ().setGui1 (gui);
			gui1_fangui.SetActive (true);

			Invoke ("displayGuiPai", 2.5f);
		} else {
			displayGuiPai ();
		}
	}

	/**显示桌面鬼牌**/
	private void displayGuiPai ()
	{
		Destroy (gui1_fangui);
		Destroy (gui2_fangui);
		//touziObj.SetActive (false);
		Destroy (touziObj);


		int gui = GlobalDataScript.roomVo.guiPai;
		int gui2 = GlobalDataScript.roomVo.guiPai2;
		if (gui != -1 && (GlobalDataScript.roomVo.hong || GlobalDataScript.roomVo.gui > 0)) { //显示鬼牌         
			bottomScript bts = guiObj.GetComponent<bottomScript> ();
			bts.setGuiPoint (gui);
			guiObj.SetActive (true);

			if (GlobalDataScript.roomVo.gui == 3) {
				bottomScript bts2 = gui2Obj.GetComponent<bottomScript> ();
				bts2.setGuiPoint (gui2);
				gui2Obj.SetActive (true);
			}
		} else if (GlobalDataScript.roomVo.roomType == 5) {
			bottomScript bts = guiObj.GetComponent<bottomScript> ();
			bts.setGuiPoint (gui);
			guiObj.SetActive (true);

			bottomScript bts2 = gui2Obj.GetComponent<bottomScript> ();
			bts2.setGuiPoint (gui2);
			gui2Obj.SetActive (true);
		} else if (GlobalDataScript.roomVo.roomType == 6) {

			//显示骰子
			int iGui = 0;
			if (gui == 0)
				iGui = 8;
			else if (gui < 9)
				iGui = gui - 1;
			else if (gui == 9)
				iGui = 17;
			else if (gui < 18)
				iGui = gui - 1;
			else if (gui == 18)
				iGui = 26;
			else if (gui < 27)
				iGui = gui - 1;
			else if (gui == 27)
				iGui = 30;
			else if (gui < 31)
				iGui = gui - 1;
			else if (gui == 31)
				iGui = 33;
			else if (gui < 34)
				iGui = gui - 1;
			else if (gui == 34)
				iGui = 34;

			bottomScript bts = guiObj.GetComponent<bottomScript> ();
			bts.setPoint (iGui, -1, -1);
			guiObj.SetActive (true);

			bottomScript bts2 = gui2Obj.GetComponent<bottomScript> ();
			bts2.setGuiPoint (gui);
			//gui2Obj.SetActive (true);
		}

		initMyCardListAndOtherCard_gui ();
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

	/**显示杠牌**/
	private void displayallGangCard ()
	{
		for (int i = 0; i < GlobalDataScript.reEnterRoomData.playerList.Count; i++) {
			int[] paiArrayType = GlobalDataScript.reEnterRoomData.playerList [i].paiArray [1];
			string dirstr = getDirection (getIndex (GlobalDataScript.reEnterRoomData.playerList [i].account.uuid));
			if (paiArrayType.Contains<int> (2)) {
				string gangString = GlobalDataScript.reEnterRoomData.playerList [i].huReturnObjectVO.totalInfo.gang;
				if (gangString != null) {
					string[] gangtemps = gangString.Split (new char[1]{ ',' });
					for (int j = 0; j < gangtemps.Length; j++) {
						string item = gangtemps [j];
						GangpaiObj gangpaiObj = new GangpaiObj ();
						gangpaiObj.uuid = item.Split (new char[1]{ ':' }) [0];
						gangpaiObj.cardPiont = int.Parse (item.Split (new char[1]{ ':' }) [1]);
						gangpaiObj.type = item.Split (new char[1]{ ':' }) [2];
						//增加判断是否为自己的杠牌的操作
						GlobalDataScript.reEnterRoomData.playerList [i].paiArray [0] [gangpaiObj.cardPiont] -= 4;

						int index = getIndex (int.Parse (gangpaiObj.uuid));
						if (gangpaiObj.type == "an") {
							doDisplayPengGangCard (dirstr, gangpaiObj.cardPiont, 4, 1);

						} else {
							doDisplayPengGangCard (dirstr, gangpaiObj.cardPiont, 4, 0, InitPosition (getMyIndexFromList ()) [index]);

						}
					}
				}
			}

		}
	}

	private void displayPengCard ()
	{
		for (int i = 0; i < GlobalDataScript.reEnterRoomData.playerList.Count; i++) {
			string gangString = GlobalDataScript.reEnterRoomData.playerList [i].huReturnObjectVO.totalInfo.peng;
			int[] indexs = new int[34]; 
			for (int j = 0; j < 34; j++) {
				indexs [j] = -1;
			}
			if (gangString != null) {
				string[] gangtemps = gangString.Split (',');
				for (int j = 0; j < gangtemps.Length; j++) {
					string temp = gangtemps [j];
					string[] temps = temp.Split (':');
					int point = int.Parse (temps [0]);
					int index = int.Parse (temps [1]);
					indexs [point] = index;
				}
			}
			int[] paiArrayType = GlobalDataScript.reEnterRoomData.playerList [i].paiArray [1];
			string dirstr = getDirection (getIndex (GlobalDataScript.reEnterRoomData.playerList [i].account.uuid));
			if (paiArrayType.Contains<int> (1) || paiArrayType.Contains<int> (14)) {
				for (int j = 0; j < paiArrayType.Length; j++) {
					if ((paiArrayType [j] == 1 || paiArrayType [j] == 14) && GlobalDataScript.reEnterRoomData.playerList [i].paiArray [0] [j] > 0) {
						GlobalDataScript.reEnterRoomData.playerList [i].paiArray [0] [j] -= 3;
						//doDisplayPengGangCard (dirstr, j, 3, 2, indexs [j]);
						doDisplayPengGangCard (dirstr, j, 3, 2, InitPosition (getMyIndexFromList ())[indexs[j]] );
					}
				}
			}
		}
	}


	/**显示吃牌**/
	private void displayChiCard ()
	{
		for (int i = 0; i < GlobalDataScript.reEnterRoomData.playerList.Count; i++) {
			int[] paiArrayType = GlobalDataScript.reEnterRoomData.playerList [i].paiArray [1];
			string dirstr = getDirection (getIndex (GlobalDataScript.reEnterRoomData.playerList [i].account.uuid));
			string gangString = GlobalDataScript.reEnterRoomData.playerList [i].huReturnObjectVO.totalInfo.chi;
			if (gangString != null) {
				string[] gangtemps = gangString.Split (new char[1]{ ',' });
				for (int j = 0; j < gangtemps.Length; j++) {
					string item = gangtemps [j];
					string[] itemArray = item.Split (new char[1]{ ':' });
					int[] card = new int[3];
					card [0] = int.Parse (itemArray [1]);
					card [1] = int.Parse (itemArray [0]);
					card [2] = int.Parse (itemArray [2]);
					//card = paixu (card [0], card [1], card [2]);
					int index = int.Parse (itemArray [3]);
					//手牌减去已经吃的牌
					GlobalDataScript.reEnterRoomData.playerList [i].paiArray [0] [card [0]] -= 1;
					GlobalDataScript.reEnterRoomData.playerList [i].paiArray [0] [card [1]] -= 1;
					GlobalDataScript.reEnterRoomData.playerList [i].paiArray [0] [card [2]] -= 1;
					doDisplayChiCard (dirstr, card, 3, 0, InitPosition (getMyIndexFromList ()) [index]);
				}
			}
		}
	}

	private int[] InitPosition (int myIndex)
	{
		int[] position = new int[4];
		for (int i = myIndex; i < 4; i++) {
			position [i] = i - myIndex;
		}
		for (int i = myIndex - 1; i > -1; i--) {
			position [i] = 4 - (myIndex - i);
		}
		return position;
	}

	/**
	 * 显示杠碰牌
	 * cloneCount 代表clone的次数  若为3则表示碰   若为4则表示杠
	 */ 
	private void doDisplayPengGangCard (string dirstr, int point, int cloneCount, int flag, int index = -1)
	{
		List<GameObject> gangTempList;
		switch (dirstr) {
		case DirectionEnum.Bottom:
			gangTempList = new List<GameObject> ();
			for (int i = 0; i < cloneCount; i++) {
				GameObject obj;
				if (i < 3) {
					if (flag != 1) {
						obj = createGameObjectAndReturn ("Prefab/PengGangCard/PengGangCard_B",
							pengGangParenTransformB.transform, new Vector3 (-370f + PengGangCardList.Count * 190f + i * 60f, 0));
						obj.GetComponent<TopAndBottomCardScript> ().setPoint (point);
						obj.transform.localScale = Vector3.one;
						if (i == 1 && cloneCount == 3) {
							if (index != -1) {
								obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (index);
							}
						}
					} else {
						obj = createGameObjectAndReturn ("Prefab/PengGangCard/gangBack",
							pengGangParenTransformB.transform, new Vector3 (-370f + PengGangCardList.Count * 190f + i * 60f, 0));
						obj.transform.localScale = Vector3.one;
					}
				} else {
					obj = createGameObjectAndReturn ("Prefab/PengGangCard/PengGangCard_B",
						pengGangParenTransformB.transform, new Vector3 (-370f + PengGangCardList.Count * 190f + i * 60f, 0));
					obj.GetComponent<TopAndBottomCardScript> ().setPoint (point);
					obj.transform.localScale = Vector3.one;
					obj.transform.localPosition = new Vector3 (-310f + PengGangCardList.Count * 190f, 24f);
					if (flag != 1) {
						if (index != -1) {
							obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (index);
						}
					}
				}


				gangTempList.Add (obj);
			}
			PengGangCardList.Add (gangTempList);
			break;
		case DirectionEnum.Top:
			gangTempList = new List<GameObject> ();
			for (int i = 0; i < cloneCount; i++) {
				GameObject obj;
				if (i < 3) {
					if (flag != 1) {
						obj = createGameObjectAndReturn ("Prefab/PengGangCard/PengGangCard_T",
							pengGangParenTransformT.transform, new Vector3 (-370f + PengGangList_T.Count * 190f + i * 60f, 0));
						obj.transform.parent = pengGangParenTransformT.transform;
						obj.GetComponent<TopAndBottomCardScript> ().setPoint (point);
						if (i == 1 && cloneCount == 3) {
							if (index != -1) {
								obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (index);
							}
						}
					} else {
						obj = createGameObjectAndReturn ("Prefab/PengGangCard/GangBack_T",
							pengGangParenTransformT.transform, new Vector3 (-370f + PengGangCardList.Count * 190f + i * 60f, 0));
						obj.transform.localScale = Vector3.one;
					}
					obj.transform.localPosition = new Vector3 (251 - PengGangList_T.Count * 120f + i * 37, 0f);
				} else {
					if (flag != 1) {
						obj = createGameObjectAndReturn ("Prefab/PengGangCard/PengGangCard_T",
							pengGangParenTransformT.transform, new Vector3 (-370f + PengGangList_T.Count * 190f + i * 60f, 0));
						obj.transform.parent = pengGangParenTransformT.transform;
						obj.GetComponent<TopAndBottomCardScript> ().setPoint (point);
						if (index != -1) {
							obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (index);
						}
					} else {
						obj = createGameObjectAndReturn ("Prefab/PengGangCard/GangBack_T",
							pengGangParenTransformT.transform, new Vector3 (-370f + PengGangList_T.Count * 190f + i * 60f, 0));
						obj.transform.localScale = Vector3.one;
					}
					
					//obj.GetComponent<TopAndBottomCardScript> ().setPoint (point);
					obj.transform.localPosition = new Vector3 (251 - PengGangList_T.Count * 120f + 37f, 20f);

				} 
				gangTempList.Add (obj);
			}
			PengGangList_T.Add (gangTempList);
			break;
		case DirectionEnum.Left:
			gangTempList = new List<GameObject> ();
			for (int i = 0; i < cloneCount; i++) {
				GameObject obj;
				if (i < 3) {
					if (flag != 1) {
						obj = createGameObjectAndReturn ("Prefab/PengGangCard/PengGangCard_L",
							pengGangParenTransformL.transform, new Vector3 (-370f + PengGangList_L.Count * 190f + i * 60f, 0));
						obj.transform.parent = pengGangParenTransformL.transform;
						obj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (point);
						if (i == 1 && cloneCount == 3) {
							if (index != -1) {
								obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (index);
							}
						}
					} else {
						obj = createGameObjectAndReturn ("Prefab/PengGangCard/GangBack_L&R",
							pengGangParenTransformL.transform, new Vector3 (-370f + PengGangList_L.Count * 190f + i * 60f, 0));
					}
					obj.transform.localPosition = new Vector3 (0f, 122 - PengGangList_L.Count * 95f - i * 28f);
				} else {
					if (flag != 1) {
						obj = createGameObjectAndReturn ("Prefab/PengGangCard/PengGangCard_L",
							pengGangParenTransformL.transform, new Vector3 (-370f + PengGangList_L.Count * 190f + i * 60f, 0));
						obj.transform.parent = pengGangParenTransformL.transform;
						obj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (point);
						if (index != -1) {
							obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (index);
						}
					} else {
						obj = createGameObjectAndReturn ("Prefab/PengGangCard/GangBack_L&R",
							pengGangParenTransformL.transform, new Vector3 (-370f + PengGangList_L.Count * 190f + i * 60f, 0));
					}
					//obj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (point);
					obj.transform.localPosition = new Vector3 (0f, 122 - PengGangList_L.Count * 95f - 18f);

				}


				gangTempList.Add (obj);
			}
			PengGangList_L.Add (gangTempList);
			break;
		case DirectionEnum.Right:
			gangTempList = new List<GameObject> ();
			for (int i = 0; i < cloneCount; i++) {
				GameObject obj;
				if (i < 3) {
					if (flag != 1) {
						obj = createGameObjectAndReturn ("Prefab/PengGangCard/PengGangCard_R",
							pengGangParenTransformR.transform, new Vector3 (-370f + PengGangList_R.Count * 190f + i * 60f, 0));
						obj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (point);
						obj.transform.parent = pengGangParenTransformR.transform;
						if (i == 1 && cloneCount == 3) {
							if (index != -1) {
								obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (index);
							}
						}
					} else {
						obj = createGameObjectAndReturn ("Prefab/PengGangCard/GangBack_L&R",
							pengGangParenTransformR.transform, new Vector3 (-370f + PengGangList_R.Count * 190f + i * 60f, 0));
					}
					obj.transform.localPosition = new Vector3 (0, -122 + PengGangList_R.Count * 95 + i * 28f);

					obj.transform.SetSiblingIndex (0);
				} else {
					if (flag != 1) {
						obj = createGameObjectAndReturn ("Prefab/PengGangCard/PengGangCard_R",
							pengGangParenTransformR.transform, new Vector3 (-370f + PengGangList_R.Count * 190f + i * 60f, 0));
						obj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (point);
						obj.transform.parent = pengGangParenTransformR.transform;
						if (index != -1) {
							obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (index);
						}
					} else {
						obj = createGameObjectAndReturn ("Prefab/PengGangCard/GangBack_L&R",
							pengGangParenTransformR.transform, new Vector3 (-370f + PengGangList_R.Count * 190f + i * 60f, 0));
					}
					//obj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (point);
					obj.transform.localPosition = new Vector3 (0f, -122 + PengGangList_R.Count * 95 + 33f);
				}

				gangTempList.Add (obj);
			}
			PengGangList_R.Add (gangTempList);
			break;
		}
	}

	/**
	 * 显示吃牌
	 * cloneCount 代表clone的次数  若为3则表示碰   若为4则表示杠
	 */ 
	private void doDisplayChiCard (string dirstr, int[] point, int cloneCount, int flag, int index = -1)
	{
		List<GameObject> gangTempList;
		switch (dirstr) {
		case DirectionEnum.Bottom:
			gangTempList = new List<GameObject> ();
			for (int i = 0; i < cloneCount; i++) {
				GameObject obj;
				if (flag != 1) {
					obj = createGameObjectAndReturn ("Prefab/PengGangCard/PengGangCard_B",
						pengGangParenTransformB.transform, new Vector3 (-370f + PengGangCardList.Count * 190f + i * 60f, 0));
					if (i == 1) {
						if (index != -1) {
							obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (index);
						}
					}
					obj.GetComponent<TopAndBottomCardScript> ().setPoint (point [i]);
					obj.transform.localScale = Vector3.one;
				} else {
					obj = createGameObjectAndReturn ("Prefab/PengGangCard/gangBack",
						pengGangParenTransformB.transform, new Vector3 (-370f + PengGangCardList.Count * 190f + i * 60f, 0));
					obj.transform.localScale = Vector3.one;
				}
					
				gangTempList.Add (obj);
			}
			PengGangCardList.Add (gangTempList);
			break;
		case DirectionEnum.Top:
			gangTempList = new List<GameObject> ();
			for (int i = 0; i < cloneCount; i++) {
				GameObject obj;
				if (flag != 1) {
					obj = createGameObjectAndReturn ("Prefab/PengGangCard/PengGangCard_T",
						pengGangParenTransformT.transform, new Vector3 (-370f + PengGangList_T.Count * 190f + i * 60f, 0));
					obj.transform.parent = pengGangParenTransformT.transform;
					obj.GetComponent<TopAndBottomCardScript> ().setPoint (point [i]);
					if (i == 1) {
						if (index != -1) {
							obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (index);
						}
					}
				} else {
					obj = createGameObjectAndReturn ("Prefab/PengGangCard/GangBack_T",
						pengGangParenTransformT.transform, new Vector3 (-370f + PengGangCardList.Count * 190f + i * 60f, 0));
					obj.transform.localScale = Vector3.one;
				}
				obj.transform.localPosition = new Vector3 (251 - PengGangList_T.Count * 120f + i * 37, 0f);

				gangTempList.Add (obj);
			}
			PengGangList_T.Add (gangTempList);
			break;
		case DirectionEnum.Left:
			gangTempList = new List<GameObject> ();
			for (int i = 0; i < cloneCount; i++) {
				GameObject obj;
				if (flag != 1) {
					obj = createGameObjectAndReturn ("Prefab/PengGangCard/PengGangCard_L",
						pengGangParenTransformL.transform, new Vector3 (-370f + PengGangList_L.Count * 190f + i * 60f, 0));
					obj.transform.parent = pengGangParenTransformL.transform;
					obj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (point [i]);
					if (i == 1) {
						if (index != -1) {
							obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (index);
						}
					}
				} else {
					obj = createGameObjectAndReturn ("Prefab/PengGangCard/GangBack_L&R",
						pengGangParenTransformL.transform, new Vector3 (-370f + PengGangList_L.Count * 190f + i * 60f, 0));
				}
				obj.transform.localPosition = new Vector3 (0f, 122 - PengGangList_L.Count * 95f - i * 28f);

				gangTempList.Add (obj);
			}
			PengGangList_L.Add (gangTempList);
			break;
		case DirectionEnum.Right:
			gangTempList = new List<GameObject> ();
			for (int i = 0; i < cloneCount; i++) {
				GameObject obj;
				if (flag != 1) {
					obj = createGameObjectAndReturn ("Prefab/PengGangCard/PengGangCard_R",
						pengGangParenTransformR.transform, new Vector3 (-370f + PengGangList_R.Count * 190f + i * 60f, 0));
					obj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (point [i]);
					obj.transform.parent = pengGangParenTransformR.transform;
					if (i == 1) {
						if (index != -1) {
							obj.GetComponent<TopAndBottomCardScript> ().ShowIndexIcon (index);
						}
					}
				} else {
					obj = createGameObjectAndReturn ("Prefab/PengGangCard/GangBack_L&R",
						pengGangParenTransformR.transform, new Vector3 (-370f + PengGangList_R.Count * 190f + i * 60f, 0));
				}
				obj.transform.localPosition = new Vector3 (0, -122 + PengGangList_R.Count * 95 + i * 28f);

				obj.transform.SetSiblingIndex (0);

				gangTempList.Add (obj);
			}
			PengGangList_R.Add (gangTempList);
			break;
		}
	}

	public void inviteFriend ()
	{
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		GlobalDataScript.getInstance ().wechatOperate.inviteFriend ();
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
		case DirectionEnum.Top:
			playerItems [2].GetComponent<PlayerItemScript> ().setPlayerOffline ();
			break;
		case DirectionEnum.Left:
			playerItems [3].GetComponent<PlayerItemScript> ().setPlayerOffline ();
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
		case DirectionEnum.Top:
			playerItems [2].GetComponent<PlayerItemScript> ().setPlayerOnline ();
			break;
		case DirectionEnum.Left:
			playerItems [3].GetComponent<PlayerItemScript> ().setPlayerOnline ();
			break;

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
				seatIndex = 4 + seatIndex;
			}            

			int code = int.Parse (arr [1]);
			//传输性别  大于3000为女
			if (code > 3000) {
				code = code - 3000;
				playerItems [seatIndex].showChatMessage (code);
				SoundCtrl.getInstance ().playMessageBoxSound (code, 0, 3);
			} else {
				code = code - 1000;
				playerItems [seatIndex].showChatMessage (code);
				SoundCtrl.getInstance ().playMessageBoxSound (code, 1, 3);
			}
		} else if (int.Parse (arr [0]) == 2) {
			int uuid = int.Parse (arr [2]);
			int myIndex = getMyIndexFromList ();
			int curAvaIndex = getIndex (uuid);
			int seatIndex = curAvaIndex - myIndex;
			if (seatIndex < 0) {
				seatIndex = 4 + seatIndex;
			}
			playerItems [seatIndex].showChatMessage (arr [1]);
		} else if (int.Parse (arr [0]) == 3) {
			int uuid = int.Parse (arr [2]);
			int myIndex = getMyIndexFromList ();
			int curAvaIndex = getIndex (uuid);
			int seatIndex = curAvaIndex - myIndex;
			if (seatIndex < 0) {
				seatIndex = 4 + seatIndex;
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
			//SoundCtrl.getInstance ().playMessageBoxSound (code, sex, 2);
			playerItems [seatIndex].showBiaoQing (bqScript.getBiaoqing (code - 1));//int.Parse (arr [1])
		} else if(int.Parse (arr [0]) == 4){
			int srcuuid = int.Parse (arr [2]);
			int destuuid = int.Parse (arr [3]);
			int myIndex = getMyIndexFromList ();
			int srcAvaIndex = getIndex (srcuuid);
			int destAvaIndex = getIndex (destuuid);

			int destSeatIndex = destAvaIndex - myIndex;
			if (destSeatIndex < 0) {
				destSeatIndex = 4 + destSeatIndex;
			}
			int srcSeatIndex = srcAvaIndex - myIndex;
			if (srcSeatIndex < 0) {
				srcSeatIndex = 4 + srcSeatIndex;
			}
			//XXX
			playerItems [destSeatIndex].showMfbq (srcSeatIndex, destSeatIndex, arr[1]);
		}


	}


	/*显示自己准备*/
	private void markselfReadyGame ()
	{
		playerItems [0].readyImg.transform.gameObject.SetActive (true);
	}

	/**
    *准备游戏
	*/
	public void readyGame ()
	{
		CustomSocket.getInstance ().sendMsg (new GameReadyRequest ());
	}

	public void micInputNotice (int sendUUid)
	{
		
		//int sendUUid = int.Parse (response.message);
		//StartCoroutine (inputNotice(sendUUid));

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

	private IEnumerator inputNotice (int sendUUid)
	{
		AudioSource playAudio = GameObject.Find ("GamePlayAudio").GetComponent<AudioSource> ();
		//排麦序，当有人在语音时，等待。
		while (playAudio.isPlaying) {
			yield return new WaitForSeconds (0.1f);
		}

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

	public void returnGameResponse (ClientResponse response)
	{
		//1.显示剩余牌的张数和圈数
		JsonData returnJsonData = JsonMapper.ToObject (response.message);
		string surplusCards = returnJsonData ["surplusCards"].ToString ();
		LeavedCastNumText.text = surplusCards;
		LeavedCardsNum = int.Parse (surplusCards);
		int lian = int.Parse (returnJsonData ["lianZhuang"].ToString ());
		if (lian == 0) {
			lianZhuangText.text = "0";
		} else {
			lianZhuangText.text = lian.ToString ();
		}
		int gameRound = int.Parse (returnJsonData ["gameRound"].ToString ());
		GlobalDataScript.surplusTimes = gameRound;
		if (GlobalDataScript.goldType) {
			GlobalDataScript.playCountForGoldType = GlobalDataScript.roomVo.roundNumber - gameRound - 1;
			LeavedRoundNumText.text	= GlobalDataScript.playCountForGoldType + "";
		} else
			LeavedRoundNumText.text = gameRound + "/" + GlobalDataScript.roomVo.roundNumber;



		int curAvatarIndexTemp = -1;//当前出牌人的索引
		int pickAvatarIndexTemp = -1; //当前摸牌人的索引
		int putOffCardPointTemp = -1;//当前打得点数
		int currentCardPointTemp = -1;//当前摸的点数


		//不是自己摸牌

		curAvatarIndexTemp = int.Parse (returnJsonData ["curAvatarIndex"].ToString ());//当前打牌人的索引
		putOffCardPointTemp = int.Parse (returnJsonData ["putOffCardPoint"].ToString ());//当前打得点数

		putOutCardPointAvarIndex = getIndexByDir (getDirection (curAvatarIndexTemp));

		putOutCardPoint = putOffCardPointTemp;//碰
		//useForGangOrPengOrChi = putOutCardPoint;//杠
		//	selfGangCardPoint = useForGangOrPengOrChi;
		SelfAndOtherPutoutCard = putOutCardPoint;
		pickAvatarIndexTemp = int.Parse (returnJsonData ["pickAvatarIndex"].ToString ()); //当前摸牌牌人的索引
		/**这句代码有可能引发catch  所以后面的 SelfAndOtherPutoutCard = currentCardPointTemp; 可能不执行**/
		try {
			if (returnJsonData ["pickAvatarIndex"] != null) {
				String strCurrent = returnJsonData ["currentCardPoint"].ToString ();
				currentCardPointTemp = int.Parse (strCurrent);//当前摸得的点数  
				SelfAndOtherPutoutCard = currentCardPointTemp; 
			}
		} catch (Exception ex) {
			
		}





		if (pickAvatarIndexTemp == getMyIndexFromList ()) {//自己摸牌
			if (currentCardPointTemp == -2) {
				MoPaiCardPoint = handerCardList [0] [handerCardList [0].Count - 1].GetComponent<bottomScript> ().getPoint ();
				SelfAndOtherPutoutCard = MoPaiCardPoint; 
				useForGangOrPengOrChi = curAvatarIndexTemp;
				Destroy (handerCardList [0] [handerCardList [0].Count - 1]);
				handerCardList [0].Remove (handerCardList [0] [handerCardList [0].Count - 1]);
				SetPosition (false);
				putCardIntoMineList (MoPaiCardPoint);
				moPai ();
				curDirString = DirectionEnum.Bottom;
				SetDirGameObjectAction (getMyIndexFromList ());
				GlobalDataScript.isDrag = true;	
				MyDebug.Log ("自己摸牌");

			} else {
				if ((handerCardList [0].Count) % 3 != 1) {
					MoPaiCardPoint = currentCardPointTemp;
					MyDebug.Log ("摸牌" + MoPaiCardPoint);
					SelfAndOtherPutoutCard = MoPaiCardPoint; 
					useForGangOrPengOrChi = curAvatarIndexTemp;
					for (int i = 0; i < handerCardList [0].Count; i++) {
						if (handerCardList [0] [i].GetComponent<bottomScript> ().getPoint () == currentCardPointTemp) {
							Destroy (handerCardList [0] [i]);
							handerCardList [0].Remove (handerCardList [0] [i]);
							break;
						}
					}
					SetPosition (false);
					putCardIntoMineList (MoPaiCardPoint);
					moPai ();
					curDirString = DirectionEnum.Bottom;
					SetDirGameObjectAction (getMyIndexFromList ());
					GlobalDataScript.isDrag = true;	
				}

			}

		} else { //别人摸牌
			curDirString = getDirection (pickAvatarIndexTemp);
			//	otherMoPaiCreateGameObject (curDirString);
			SetDirGameObjectAction (pickAvatarIndexTemp);
		}			

		try {
			//光标指向打牌人
			int dirindex = getIndexByDir (getDirection (curAvatarIndexTemp));
			cardOnTable = tableCardList [dirindex] [tableCardList [dirindex].Count - 1];
			if (tableCardList [dirindex] == null || tableCardList [dirindex].Count == 0) {//刚启动

			} else {
				//otherPickCardItem = handerCardList[dirindex][0];
				//	gameTool.setOtherCardObjPosition(handerCardList[dirindex],getDirection(curAvatarIndexTemp) , 1);
				GameObject temp = tableCardList [dirindex] [tableCardList [dirindex].Count - 1]; 
				setPointGameObject (temp);
			}
		} catch (Exception ex) {
		}
	}

	//赣州冲关精牌显示
	private void showJingPai ()
	{
		GameObject go = Instantiate (Resources.Load ("prefab/JingPaiEffect/JingPaiEffect")) as GameObject;
		go.transform.parent = gameObject.transform;
		go.transform.localPosition = new Vector2 (-56, -6);

		//其他操作
		if (1 == GlobalDataScript.roomVo.shangxiaFanType) {
			Invoke ("openGameOverPanelSignal", 3);
		} else {
			Invoke ("openGameOverPanelSignal", 6);
		}

	}


	public void shanghuoResponse (ClientResponse  response)
	{
		cleanGameplayUI ();
		JsonData json = JsonMapper.ToObject (response.message);
		int avatarIndex = Int32.Parse (json ["avatarIndex"].ToString ());

		if (avatarIndex == -1) {
			GlobalDataScript.roomAvatarVoList = avatarList;
			int myIndex = getMyIndexFromList ();
			if (GlobalDataScript.surplusTimes % 4 == 0) {//shanghuo
				avatarList [myIndex].shanghuo = false;
			}


			panel_shanghuo.SetActive (true);
			btn_piao [0].SetActive (true);
			btn_piao [1].SetActive (true);
			btn_piao [2].SetActive (true);

			if (avatarList [myIndex].shanghuo == true) {
				btn_shanghuo [0].SetActive (false);
				btn_shanghuo [1].SetActive (false);
			} else {
				btn_shanghuo [0].SetActive (true);
				btn_shanghuo [1].SetActive (true);
			}
		} else {
			bool shanghuo = Boolean.Parse (json ["shanghuo"].ToString ());
			int piao = Int32.Parse (json ["piao"].ToString ());
			avatarList [avatarIndex].shanghuo = shanghuo;
			avatarList [avatarIndex].piao = piao;
			int myIndex = getMyIndexFromList ();
			int seatIndex = avatarIndex - myIndex;
			if (seatIndex < 0) {
				seatIndex = 4 + seatIndex;
			}

			playerItems [seatIndex].SetShanghuo(shanghuo);

		}
	}

	private bool shanghuo = false;
	private int piao = -1;
	private bool ifClickShanghuo = false;
	public void clickShanghuo(int i){
		switch (i) {
		case 0:
			shanghuo = true;
			ifClickShanghuo = true;
			btn_shanghuo [0].SetActive (false);
			btn_shanghuo [1].SetActive (false);
			break;
		case 1:
			shanghuo = false;
			ifClickShanghuo = true;
			btn_shanghuo [0].SetActive (false);
			btn_shanghuo [1].SetActive (false);
			break;
		case 2:
			piao = 0;
			btn_piao [0].SetActive (false);
			btn_piao [1].SetActive (false);
			btn_piao [2].SetActive (false);
			break;
		case 3:
			piao = 1;
			btn_piao [0].SetActive (false);
			btn_piao [1].SetActive (false);
			btn_piao [2].SetActive (false);
			break;
		case 4:
			piao = 2;
			btn_piao [0].SetActive (false);
			btn_piao [1].SetActive (false);
			btn_piao [2].SetActive (false);
			break;
		}
		if (piao != -1) {
			int myIndex = getMyIndexFromList ();
			if (avatarList [myIndex].shanghuo == true) {
				CustomSocket.getInstance ().sendMsg (new ShanghuoRequest (true, piao));
				ifClickShanghuo = false;
				piao = -1;
			} else {
				if (ifClickShanghuo) {
					CustomSocket.getInstance ().sendMsg (new ShanghuoRequest (shanghuo, piao));
					ifClickShanghuo = false;
					piao = -1;
				}
			}
		}

	}


	//cheat
	public void cheatCallBack (ClientResponse response){
		CheatResponseVO cheatVo = JsonMapper.ToObject<CheatResponseVO> (response.message);

		if (cheatVo.state == -1 && cheatVo.paiArray != null) {//显示所有剩余牌
			cheatPanel.SetActive (true);
			showAllCards (cheatVo.paiArray);
		} else if (cheatVo.paiArray == null && cheatVo.state > -1) {//替换处理
			if (pickCardItem != null) {

				for (int i = 0; i < handerCardList [0].Count; i++) {
					GameObject delObj = handerCardList [0] [i];//
					if (delObj == pickCardItem) {//
						handerCardList [0].RemoveAt (i);
						break;
					}
				}
				pickCardItem.GetComponent<bottomScript> ().setPoint (cheatVo.state, GlobalDataScript.roomVo.guiPai);
				SelfAndOtherPutoutCard = cheatVo.state;
				insertCardIntoList (pickCardItem);
			}

		} else if (cheatVo.state == -2) {
			TipsManagerScript.getInstance().setTips("只有在自己摸牌时");
		}

	}

	public void showAllCards(List <int> paiList){
		//int []pai = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1};
		//int count = pai.Length;
		cheatObjList = new List<GameObject>();
		int count = paiList.Count;
		for (int i = 0; i < count; i++) {
			GameObject obj = Instantiate (Resources.Load ("prefab/card/CheatCard")) as GameObject;
			if (obj != null) {
				obj.transform.SetParent (cheatContentParent.transform);
				obj.transform.localScale = new Vector3 (1, 1, 1);
				obj.GetComponent<cheatScript> ().setPoint (paiList[i], GlobalDataScript.roomVo.guiPai);
				obj.GetComponent<cheatScript> ().onSendMessage += cheatCardSendMsg;//发送消息fd
				//obj.transform.localPosition = new Vector3 (0, 0);
				cheatObjList.Add(obj);
			}
		}

	}


	public void closeCheatPanel(){
		cheatPanel.SetActive (false);
		if (cheatObjList != null) {
			foreach (GameObject obj in cheatObjList) {
				Destroy (obj);
			}
			cheatObjList.Clear ();
			/*for (int i = 0; i < cheatObjList.Count; i++) {
				if (cheatObjList [i] == null) {

				} 
			}*/
		}
	}


	//cheat 0831 //@##
	public void cheatCardSendMsg(GameObject obj){
		CardVO cardvo = new CardVO ();
		cardvo.cardPoint = obj.GetComponent<cheatScript> ().getPoint();
		CustomSocket.getInstance ().sendMsg (new CheatRequest(cardvo));
		closeCheatPanel ();
	}

	public void clickBtnCheat(){
		//cheat 0831
		if (GlobalDataScript.isDrag && pickCardItem != null) {
			if (cheatObjList == null || cheatObjList.Count == 0) {

				CardVO cardvo = new CardVO ();
				cardvo.cardPoint = -1;
				CustomSocket.getInstance ().sendMsg (new CheatRequest (cardvo));
			}
		} else {
			TipsManagerScript.getInstance().setTips("只有在自己摸牌时");
		}


	}


}