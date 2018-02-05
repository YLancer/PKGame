using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using System;
using LitJson;
using UnityEngine.SceneManagement;


public class GoldTypeRoomSetingScript : MonoBehaviour {

	public GameObject panelZhuanzhuanSetting;
	public GameObject panelChangshaSetting;
	public GameObject panelHuashuiSetting;
	public GameObject panelGuangDongSetting;
	public GameObject panelGanZhouSetting;
	public GameObject panelRuiJinSetting;
	public GameObject panelPdkSetting;
	public GameObject panelDevoloping;
	public GameObject panelDnSetting;


	public GameObject Button_zhuanzhuan1;
	public GameObject Button_zhuanzhuan;
	public GameObject Button_huashui1;
	public GameObject Button_huashui;

	public GameObject Button_changsha1;
	public GameObject Button_changsha;

	public GameObject Btn_zhuanZ_liang;
	public GameObject Btn_zhuanZ_dark;
	public GameObject Btn_huaS_liang;
	public GameObject Btn_huaS_dark;
	public GameObject Btn_run_liang;
	public GameObject Btn_run_dark;
	public GameObject Btn_ganzhou_liang;
	public GameObject Btn_ganzhou_dark;
	public GameObject Btn_ruijin_liang;
	public GameObject Btn_ruijin_dark;
	public GameObject Btn_pdk_liang;
	public GameObject Btn_pdk_dark;
	public GameObject Btn_dn_liang;
	public GameObject Btn_dn_dark;

	public Image watingPanel;

	public Image button_Create_Sure;


	public List<Toggle> zhuanzhuanRoomCards;//转转麻将房卡数
	public List<Toggle> changshaRoomCards;//长沙麻将房卡数
	public List<Toggle> huashuiRoomCards;//划水麻将房卡数
	public List<Toggle> guangdongRoomCards;//广东麻将房卡数
	public List<Toggle> ganzhouRoomCards;//赣州麻将房卡数
	public List<Toggle> ruijinRoomCards;//瑞金麻将房卡数



	public List<Toggle> zhuanzhuanGameRule;//转转麻将玩法
	public List<Toggle> changshaGameRule;//长沙麻将玩法
	public List<Toggle> huashuiGameRule;//划水麻将玩法
	public List<Toggle> guangdongGameRule;//广东麻将玩法
	public List<Toggle> ganzhouGameRule;//赣州麻将玩法
	public List<Toggle> ruijinGameRule;//瑞金麻将玩法
	public List<Toggle> pdkGameRule;//跑得快玩法
	public List<Toggle> dnGameRule;//斗牛玩法

	public List<Toggle> zhuanzhuanZhuama;//转转麻将抓码个数
	public List<Toggle> changshaZhuama;//长沙麻将抓码个数
	public List<Toggle> huashuixiayu;//划水麻将下鱼条数
	public List<Toggle> guangdongZhuama;//广东麻将抓码个数

	public List<Toggle> guangdongGui;//广东麻将鬼牌

	private int roomCardCount;//房卡数
	private GameObject gameSence;
	private RoomCreateVo sendVo;//创建房间的信息

	public Button createRoom;


	public GameObject[] fangka;

	private string userDefaultSet = null;

	void Start () {

		GlobalDataScript.playCountForGoldType = 0;
		if (GlobalDataScript.configTemp.pay == 0) {
			foreach (GameObject go in fangka) {
				go.SetActive (true);
			}
		} else {
			foreach (GameObject go in fangka) {
				go.SetActive (false);
			}
		}
		openDefaultSetingPanel ();//打开默认房间设置
		SocketEventHandle.getInstance ().CreateRoomCallBack += onCreateRoomCallback;
		SocketEventHandle.getInstance ().JoinRoomCallBack += onJoinRoomCallBack;

		if (SocketEventHandle.getInstance ().serviceErrorNotice != null) {
			SocketEventHandle.getInstance ().serviceErrorNotice = null;
		}
		SocketEventHandle.getInstance ().serviceErrorNotice += serviceResponse;
	}

	public void serviceResponse(ClientResponse response){
		watingPanel.gameObject.SetActive(false);
		TipsManagerScript.getInstance ().setTips (response.message);
	}

	// Update is called once per frame
	void Update () {

	}


	public void cancle() {
		SoundCtrl.getInstance().playSoundByActionButton(1);
		watingPanel.gameObject.SetActive(false);


	}
	/***
	 * 打开转转麻将设置面板
	 */ 
	public void openSetingPanel_ZhuangZhuang(){

		SoundCtrl.getInstance().playSoundByActionButton(1);
//		GlobalDataScript.userGameType = GameType.GameType_MJ_ZhuangZhuang;
		GlobalDataScript.userGameType = GameType.GameType_MJ_YiChun;
		loadDefaultSet (GlobalDataScript.userGameType);
		setGameObjectActive (GlobalDataScript.userGameType);

		createRoom.onClick.RemoveAllListeners ();
		createRoom.onClick.AddListener (delegate() {
			this.createZhuanzhuanRoom();
		});
	}

	/***
	 * 打开长沙麻将设置面板
	 */ 
	public void openSetingPanel_ChangSha(){
		SoundCtrl.getInstance().playSoundByActionButton(1);
		GlobalDataScript.userGameType = GameType.GameType_MJ_ChangSha;
		loadDefaultSet (GlobalDataScript.userGameType);
		setGameObjectActive (GlobalDataScript.userGameType);

		createRoom.onClick.RemoveAllListeners ();
		createRoom.onClick.AddListener (delegate() {
			this.createChangshaRoom();
		});
	}

	/***
	 * 打开划水麻将设置面板
	 */ 
	public void openSetingPanel_HuaShui(){
		SoundCtrl.getInstance().playSoundByActionButton(1);
		GlobalDataScript.userGameType = GameType.GameType_MJ_HuaShui;
		loadDefaultSet (GlobalDataScript.userGameType);
		setGameObjectActive (GlobalDataScript.userGameType);

		createRoom.onClick.RemoveAllListeners ();
		createRoom.onClick.AddListener (delegate() {
			this.createHuashuiRoom();
		});
	}

	/***
	 * 打开广东麻将设置面板
	 */
	public void openSetingPanel_GuangDong()
	{

		SoundCtrl.getInstance().playSoundByActionButton(1);
		GlobalDataScript.userGameType = GameType.GameType_MJ_GuangDong;
		loadDefaultSet (GlobalDataScript.userGameType);
		setGameObjectActive (GlobalDataScript.userGameType);

		createRoom.onClick.RemoveAllListeners ();
		createRoom.onClick.AddListener (delegate() {
			this.createGuangDongRoom();
		});
	}

	/***
	 * 打开赣州麻将设置面板
	 */
	public void openSetingPanel_GanZhou()
	{
		/**
		TipsManagerScript.getInstance ().setTips ("开发中");
		SoundCtrl.getInstance().playSoundByActionButton(1);
		return;
		*/
		GlobalDataScript.userGameType = GameType.GameType_MJ_GanZhou;
		loadDefaultSet (GlobalDataScript.userGameType);
		setGameObjectActive (GlobalDataScript.userGameType);

		createRoom.onClick.RemoveAllListeners ();
		createRoom.onClick.AddListener (delegate() {
			this.createGanZhouRoom();
		});
	}

	/***
	 * 打开瑞金麻将设置面板
	 */
	public void openSetingPanel_RuiJin()
	{
		SoundCtrl.getInstance().playSoundByActionButton(1);
		GlobalDataScript.userGameType = GameType.GameType_MJ_RuiJin;
		setGameObjectActive (GlobalDataScript.userGameType);
		loadDefaultSet (GlobalDataScript.userGameType);
		createRoom.onClick.RemoveAllListeners ();
		createRoom.onClick.AddListener (delegate() {
			this.createRuiJinRoom();
		});
	}


	/***
	 * 打开跑得快设置面板
	 */
	public void openSetingPanel_PDK()
	{
		/**
		TipsManagerScript.getInstance ().setTips ("开发中");
		SoundCtrl.getInstance().playSoundByActionButton(1);
		return;
		*/

		SoundCtrl.getInstance().playSoundByActionButton(1);
		GlobalDataScript.userGameType = GameType.GameType_PK_PDK;
		loadDefaultSet (GlobalDataScript.userGameType);
		setGameObjectActive (GlobalDataScript.userGameType);

		createRoom.onClick.RemoveAllListeners ();
		createRoom.onClick.AddListener (delegate() {
			this.createPDKRoom();
		});
	}

	/***
	 * 打开斗牛设置面板
	 */
	public void openSetingPanel_DN()
	{
		SoundCtrl.getInstance().playSoundByActionButton(1);
		GlobalDataScript.userGameType = GameType.GameType_PK_DN;
		loadDefaultSet (GlobalDataScript.userGameType);
		setGameObjectActive (GlobalDataScript.userGameType);

		createRoom.onClick.RemoveAllListeners ();
		createRoom.onClick.AddListener (delegate() {
			this.createDNRoom();
		});
	}

	public void Button_down()
	{
		SoundCtrl.getInstance().playSoundByActionButton(1);
		Application.OpenURL("http://a.app.qq.com/o/simple.jsp?pkgname=com.pengyoupdk.poker");

	}




	public void openDeveloping(){
		SoundCtrl.getInstance().playSoundByActionButton(1);
		GlobalDataScript.userGameType = GameType.GameType_MJ_Developing;
		loadDefaultSet (GlobalDataScript.userGameType);
		setGameObjectActive (GlobalDataScript.userGameType);
	}

	public void closeDialog(){

		SoundCtrl.getInstance().playSoundByActionButton(1);
		MyDebug.Log ("closeDialog");
		SocketEventHandle.getInstance ().CreateRoomCallBack -= onCreateRoomCallback;
		SocketEventHandle.getInstance ().JoinRoomCallBack -= onJoinRoomCallBack;
		SocketEventHandle.getInstance ().serviceErrorNotice -= serviceResponse;
		Destroy (this);
		Destroy (gameObject);
	}

	private void ReqForCreateRoom(string msg){
		int gold = 10;
		if (GlobalDataScript.userGameType == GameType.GameType_PK_DN) {
			gold = 5;
		}
		if (GlobalDataScript.goldType) {
			if (GlobalDataScript.loginResponseData.account.gold >= gold) {
				watingPanel.gameObject.SetActive(true);
				CustomSocket.getInstance ().sendMsg (new CreateRoomRequest (msg));
				userDefaultSet = msg;
			} else {
				TipsManagerScript.getInstance ().setTips ("你的金币不足，不能匹配到训练场");
			}

		} else {
			if (GlobalDataScript.loginResponseData.account.roomcard > 0) {
				watingPanel.gameObject.SetActive(true);
				CustomSocket.getInstance ().sendMsg (new CreateRoomRequest (msg));
				userDefaultSet = msg;
			} else {
				TipsManagerScript.getInstance ().setTips ("你的房卡数量不足，不能创建房间");
			}
		}
	}

	/**
	 * 创建转转麻将房间
	 */ 
	public void createZhuanzhuanRoom(){
		SoundCtrl.getInstance().playSoundByActionButton(1);
		int roundNumber = 4;//房卡数量
		bool isZimo=false;//自摸
		bool hasHong=false;//红中赖子
		bool isSevenDoube =false;//七小对
		//bool isGang = false;
		int maCount = 0;
		for (int i = 0; i < zhuanzhuanRoomCards.Count; i++) {
			Toggle item = zhuanzhuanRoomCards [i];
			if (item.isOn) {
				if (i == 0) {
					roundNumber = 8;
				} else if (i == 1) {
					roundNumber = 16;
				} 
				break;
			}
		}

		if (zhuanzhuanGameRule [0].isOn) {
			isZimo = true;
		}

		//if (zhuanzhuanGameRule [1].isOn) {
		//	isGang = true;
		//}

		if (zhuanzhuanGameRule [2].isOn) {
			hasHong = true;
		}

		if (zhuanzhuanGameRule [3].isOn) {
			isSevenDoube = true;
		}


		for (int i = 0; i < zhuanzhuanZhuama.Count; i++) {
			if (zhuanzhuanZhuama [i].isOn) {
				maCount = 2 * (i + 1);
				break;
			}
		}
		sendVo = new RoomCreateVo ();
		sendVo.ma = maCount;
		sendVo.roundNumber = roundNumber;
		sendVo.ziMo = isZimo?1:0;
		sendVo.hong = hasHong;
		sendVo.sevenDouble = isSevenDoube;
//		sendVo.roomType = (int)GameType.GameType_MJ_ZhuangZhuang;
		sendVo.roomType = (int)GameType.GameType_MJ_YiChun;
		sendVo.goldType = GlobalDataScript.goldType;
		string sendmsgstr = JsonMapper.ToJson (sendVo);
		ReqForCreateRoom (sendmsgstr);
	}

	/**
	 * 创建广东麻将房间
	 */
	public void createGuangDongRoom()
	{
		SoundCtrl.getInstance().playSoundByActionButton(1);
		int roundNumber = 8;//房卡数量
		bool isGangHu = false;//自摸
		bool hasHong = false;//红中赖子
		bool isSevenDoube = false;//七小对
		bool isFengpai = false;//风牌
		int gui = 0; //鬼牌
		bool gangHuQuanBao = false;
		bool wuGuiX2 = false;

		int maCount = 0;
		bool maGenDiFen = false;
		bool maGenGang = false;
		for (int i = 0; i < guangdongRoomCards.Count; i++)
		{
			Toggle item = guangdongRoomCards[i];
			if (item.isOn)
			{
				if (i == 0)
				{
					roundNumber = 8;
				}
				else if (i == 1)
				{
					roundNumber = 16;
				}
				break;
			}
		}

		if (guangdongGameRule[0].isOn)
		{
			isSevenDoube = true;
		}

		if (guangdongGameRule[1].isOn)
		{
			isFengpai = true;
		}

		if (guangdongGameRule[2].isOn)
		{
			isGangHu = true;
		}

		if (guangdongGameRule[3].isOn)
		{
			gangHuQuanBao = true;
		}

		if (guangdongGameRule[4].isOn)
		{
			wuGuiX2 = true;
		}


		for (int i = 0; i < guangdongZhuama.Count; i++)
		{
			if (guangdongZhuama[i].isOn)
			{
				maCount = 2 * (i + 1);
				break;
			}
		}

		if (maCount > 0) {
			if (guangdongZhuama [3].isOn)
				maGenDiFen = true;
			if (guangdongZhuama [4].isOn)
				maGenGang = true;
		}

		if (guangdongGui [0].isOn) {
			gui = 0;
		} else if (guangdongGui [1].isOn) {
			gui = 1;
		} else {
			if (guangdongGui [3].isOn)
				gui = 3; //双鬼
			else
				gui = 2;
		}

		sendVo = new RoomCreateVo();
		sendVo.ma = maCount;
		sendVo.roundNumber = roundNumber;
		sendVo.ziMo = 1 ;
		sendVo.hong = hasHong;
		sendVo.addWordCard = isFengpai;
		sendVo.sevenDouble = isSevenDoube;
		sendVo.gui = gui;
		sendVo.gangHu = isGangHu;
		sendVo.gangHuQuanBao = gangHuQuanBao;
		sendVo.wuGuiX2 = wuGuiX2;
		sendVo.maGenDifen = maGenDiFen;
		sendVo.maGenGang = maGenGang;

		sendVo.roomType = (int)GameType.GameType_MJ_GuangDong;
		sendVo.goldType = GlobalDataScript.goldType;
		string sendmsgstr = JsonMapper.ToJson(sendVo);
		ReqForCreateRoom (sendmsgstr);
	}

	/**
	 * 创建赣州麻将房间
	 */
	public void createGanZhouRoom()
	{
		SoundCtrl.getInstance().playSoundByActionButton(1);

		sendVo = new RoomCreateVo();
		if (ganzhouRoomCards [0].isOn) {
			sendVo.roundNumber = 8;
		} else {
			sendVo.roundNumber = 16;
		}

		if (ganzhouGameRule [0].isOn) {
			sendVo.shangxiaFanType = 1;
		} else {
			sendVo.shangxiaFanType = 2;
		}

		if (ganzhouGameRule [2].isOn) {
			sendVo.diFen = 1;
		} else {
			sendVo.diFen = 2;
		}

		if (ganzhouGameRule [4].isOn) {
			sendVo.tongZhuang = true;
		} else {
			sendVo.tongZhuang = false;
		}

		if (ganzhouGameRule [6].isOn) {
			sendVo.pingHu = 1;
		} else if (ganzhouGameRule [7].isOn) {
			sendVo.pingHu = 2;
		} else {
			sendVo.pingHu = 3;
		}


		sendVo.roomType = (int)GameType.GameType_MJ_GanZhou;
		sendVo.goldType = GlobalDataScript.goldType;
		string sendmsgstr = JsonMapper.ToJson(sendVo);
		ReqForCreateRoom (sendmsgstr);
	}

	/**
	 * 创建瑞金麻将房间
	 */
	public void createRuiJinRoom()
	{
		SoundCtrl.getInstance().playSoundByActionButton(1);

		sendVo = new RoomCreateVo();
		if (ruijinRoomCards [0].isOn) {
			sendVo.roundNumber = 4;
		}else if (ruijinRoomCards [1].isOn) {
			sendVo.roundNumber = 8;
		} else {
			sendVo.roundNumber = 16;
		}

		if (ruijinGameRule [0].isOn) {
			sendVo.keDianPao = true;
		} else {
			sendVo.keDianPao = false;
		}

		if (ruijinGameRule [1].isOn) {
			sendVo.diFen = 1;
		}else if (ruijinGameRule [2].isOn) {
			sendVo.diFen = 2;
		} else {
			sendVo.diFen = 5;
		}

		if (ruijinGameRule [4].isOn) {
			sendVo.tongZhuang = true;
		} else {
			sendVo.tongZhuang = false;
		}

		if (ruijinGameRule [6].isOn) {
			sendVo.lunZhuang = true;
		} else {
			sendVo.lunZhuang = false;
		}

		sendVo.roomType = (int)GameType.GameType_MJ_RuiJin;
		sendVo.goldType = GlobalDataScript.goldType;
		string sendmsgstr = JsonMapper.ToJson(sendVo);
		ReqForCreateRoom (sendmsgstr);
	}

	/**
	 * 创建跑得快房间
	 */
	public void createPDKRoom()
	{
		sendVo = new RoomCreateVo ();

		if (pdkGameRule [0].isOn)
			sendVo.roundNumber = 10;
		else
			sendVo.roundNumber = 20;

		if (pdkGameRule [2].isOn)
			sendVo.zhang16 = true;
		else
			sendVo.zhang16 = false;

		if (pdkGameRule [4].isOn)
			sendVo.showPai = true;
		else
			sendVo.showPai = false;

		if (pdkGameRule [6].isOn)
			sendVo.xian3 = true;
		else
			sendVo.xian3 = false;

		sendVo.gameType = 1;  //1   (int)GameType.GameType_PK_PDK
		sendVo.goldType = GlobalDataScript.goldType;
		string sendmsgstr = JsonMapper.ToJson(sendVo);
		ReqForCreateRoom (sendmsgstr);
	}

	/**
	 * 创建斗牛房间
	 */
	public void createDNRoom()
	{
		sendVo = new RoomCreateVo ();
		sendVo.roundNumber = dnGameRule [0].isOn ? 10 : 20;
		sendVo.qiang = dnGameRule [3].isOn;
		if (dnGameRule [4].isOn)
			sendVo.diFen = 1;
		else if (dnGameRule [5].isOn)
			sendVo.diFen = 2;
		else if (dnGameRule [6].isOn)
			sendVo.diFen = 3;
		sendVo.ming = dnGameRule [7].isOn;
		sendVo.mengs = dnGameRule [9].isOn ? 1 : 2;
		sendVo.AA = dnGameRule [11].isOn;
		sendVo.goldType = GlobalDataScript.goldType;


		sendVo.gameType = 3;//斗牛  3  (int)GameType.GameType_PK_DN

		string sendmsgstr = JsonMapper.ToJson(sendVo);
		ReqForCreateRoom (sendmsgstr);
	}

	/**
	 * 创建长沙麻将房间
	 */
	public void createChangshaRoom(){
		SoundCtrl.getInstance().playSoundByActionButton(1);
		int roundNumber = 4;//房卡数量
		bool isZimo=false;//自摸
		int maCount = 0;
		for (int i = 0; i < changshaRoomCards.Count; i++) {
			Toggle item = changshaRoomCards [i];
			if (item.isOn) {
				if (i == 0) {
					roundNumber = 8;
				} else if (i == 1) {
					roundNumber = 16;
				} 			
				break;
			}
		}
		if (changshaGameRule [0].isOn) {
			isZimo = true;
		}

		for (int i = 0; i <changshaZhuama.Count; i++) {
			if (changshaZhuama [i].isOn) {
				maCount = 2 * (i + 1);
				break;
			}
		}

		sendVo = new RoomCreateVo ();
		sendVo.ma = maCount;
		sendVo.roundNumber = roundNumber;
		sendVo.ziMo = isZimo?1:0;
		sendVo.roomType = (int)GameType.GameType_MJ_ChangSha;
		sendVo.goldType = GlobalDataScript.goldType;
		string sendmsgstr = JsonMapper.ToJson (sendVo);
		ReqForCreateRoom (sendmsgstr);
	}

	/**
	 * 创建划水麻将房间
	 */
	public void createHuashuiRoom(){
		SoundCtrl.getInstance().playSoundByActionButton(1);
		int roundNumber = 4;//房卡数量
		bool isZimo=false;//自摸
		bool isFengpai =false;//七小对
		int maCount = 0;
		for (int i = 0; i < huashuiRoomCards.Count; i++) {
			Toggle item = huashuiRoomCards [i];
			if (item.isOn) {
				if (i == 0) {
					roundNumber = 8;
				} else if (i == 1) {
					roundNumber = 16;
				} 
				break;
			}
		}
		if (huashuiGameRule [0].isOn) {
			isFengpai = true;
		}
		if (huashuiGameRule [1].isOn) {
			isZimo = true;
		}


		for (int i = 0; i <huashuixiayu.Count; i++) {
			if (huashuixiayu [i].isOn) {
				maCount = 2 * (i + 1)+i;
				break;
			}
		}

		sendVo = new RoomCreateVo ();
		sendVo.xiaYu = maCount;
		sendVo.roundNumber = roundNumber;
		sendVo.ziMo = isZimo?1:0;
		sendVo.roomType = (int)GameType.GameType_MJ_HuaShui;
		sendVo.addWordCard = isFengpai;
		sendVo.sevenDouble = true;
		sendVo.goldType = GlobalDataScript.goldType;
		string sendmsgstr = JsonMapper.ToJson (sendVo);
		ReqForCreateRoom (sendmsgstr);
	}

	//	public void toggleHongClick(){
	//
	//		if (zhuanzhuanGameRule [2].isOn) {
	//			zhuanzhuanGameRule [0].isOn = true;
	//		}
	//	}
	//
	//	public void toggleQiangGangHuClick(){
	//		if (zhuanzhuanGameRule [1].isOn) {
	//			zhuanzhuanGameRule [2].isOn = false;
	//		}
	//	}

	public void onCreateRoomCallback(ClientResponse response){
		if (watingPanel != null) {
			watingPanel.gameObject.SetActive(false);
		}
		MyDebug.Log (response.message);
		if (response.status == 1) {
			//RoomCreateResponseVo responseVO = JsonMapper.ToObject<RoomCreateResponseVo> (response.message);
			int roomid = Int32.Parse(response.message);
			sendVo.roomId = roomid;
			GlobalDataScript.roomVo = sendVo;
			GlobalDataScript.loginResponseData.roomId = roomid;
			//GlobalDataScript.loginResponseData.isReady = true;
			if(GlobalDataScript.roomVo.gameType == 0)//(int)GameType.GameType_PK_PDK
				GlobalDataScript.loginResponseData.main = true;
			GlobalDataScript.loginResponseData.isOnLine = true;
			GlobalDataScript.reEnterRoomData=null;
			//SceneManager.LoadSceneAsync(1);


//			saveDefaultSet (GlobalDataScript.userGameType, userDefaultSet);

			if (GlobalDataScript.roomVo.gameType == 0 ) {//(int)GameType.GameType_PK_PDK
				GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab ("Prefab/Panel_GamePlay");
			} else if (GlobalDataScript.roomVo.gameType == 1) { //(int)GameType.GameType_PK_PDK
				GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab ("Prefab/Panel_GamePDK");
			} else if (GlobalDataScript.roomVo.gameType == 3) {//(int)GameType.GameType_PK_DN
				GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab ("Prefab/Panel_GameDN");
			}

			closeDialog ();

		} else {
			TipsManagerScript.getInstance ().setTips (response.message);
		}
	}

	public void MingChanged(){
		dnGameRule [9].gameObject.SetActive (dnGameRule [7].isOn);
		dnGameRule [10].gameObject.SetActive (dnGameRule [7].isOn);
	}

	public void fanguiChanged(){
		if (guangdongGui [2].isOn) {
			guangdongGui [3].enabled = true;
		} else {
			guangdongGui [3].isOn = false;
			guangdongGui [3].enabled = false;
		}
	}

	public void maChanged(){
		if (guangdongZhuama [0].isOn || guangdongZhuama [1].isOn || guangdongZhuama [2].isOn){
			guangdongZhuama [3].enabled = true;
			guangdongZhuama [4].enabled = true;
		} else {
			guangdongZhuama [3].isOn = false;
			guangdongZhuama [4].isOn = false;
			guangdongZhuama [3].enabled = false;
			guangdongZhuama [4].enabled = false;
		}
	}

	public void saveDefaultSet(GameType gt,string userSet){//保存设置
		if(gt == GameType.GameType_NULL) return;
		PlayerPrefs.SetInt ("userDefaultGameType", (int)gt);
		if (userSet != null) {
			switch (gt) {
//			case GameType.GameType_MJ_ZhuangZhuang:
			case GameType.GameType_MJ_YiChun:
				PlayerPrefs.SetString ("userDefaultSet_ZhuangZhuang", userSet);
				break;
			case GameType.GameType_MJ_HuaShui:
				PlayerPrefs.SetString ("userDefaultSet_HuaShui", userSet);
				break;	
			case GameType.GameType_MJ_ChangSha:
				PlayerPrefs.SetString ("userDefaultSet_ChangSha", userSet);
				break;	
			case GameType.GameType_MJ_GuangDong:
				PlayerPrefs.SetString ("userDefaultSet_GuangDong", userSet);
				break;	
			case GameType.GameType_MJ_GanZhou:
				PlayerPrefs.SetString ("userDefaultSet_GanZhou", userSet);
				break;	
			case GameType.GameType_MJ_RuiJin:
				PlayerPrefs.SetString ("userDefaultSet_RuiJin", userSet);
				break;	
			case GameType.GameType_PK_PDK:
				PlayerPrefs.SetString ("userDefaultSet_GanZhou", userSet);
				break;	
			case GameType.GameType_PK_DN:
				PlayerPrefs.SetString ("userDefaultSet_DN", userSet);
				break;
			default:
				break;
			}
		}
	}
	public void loadDefaultSet(GameType gt) {
		if(gt == GameType.GameType_NULL) return;
		switch (gt) {
		case GameType.GameType_MJ_Developing:
			break;
//		case GameType.GameType_MJ_ZhuangZhuang:
		case GameType.GameType_MJ_YiChun:
			loadSet_ZhuangZhuang();
			break;
		case GameType.GameType_MJ_HuaShui:
			loadSet_HuaShui();
			break;
		case GameType.GameType_MJ_ChangSha:
			loadSet_ChangSha();
			break;	
		case GameType.GameType_MJ_GuangDong:
			loadSet_GuangDong();
			break;
		case GameType.GameType_MJ_GanZhou:
			loadSet_GanZhou();
			break;
		case GameType.GameType_MJ_RuiJin:
			loadSet_RuiJin();
			break;
		case GameType.GameType_PK_PDK:
			loadSet_PDK();
			break;
		case GameType.GameType_PK_DN:
			loadSet_DN();
			break;
		default:
			break;
		}
	}

	public void loadSet_ZhuangZhuang(){

	}
	public void loadSet_HuaShui(){

	}
	public void loadSet_ChangSha(){

	}
	public void loadSet_GuangDong(){

	}
	public void loadSet_GanZhou(){

	}
	public void loadSet_RuiJin(){
		if (PlayerPrefs.HasKey ("userDefaultSet_RuiJin")) {
			userDefaultSet = PlayerPrefs.GetString("userDefaultSet_RuiJin");
			if (userDefaultSet != null){
				RoomCreateVo roomVo = JsonMapper.ToObject<RoomCreateVo> (userDefaultSet);

				ruijinRoomCards [0].isOn = 4 == roomVo.roundNumber;
				ruijinRoomCards [1].isOn = 8 == roomVo.roundNumber;
				ruijinRoomCards [2].isOn = 16 == roomVo.roundNumber;

				ruijinGameRule [0].isOn = roomVo.keDianPao;

				ruijinGameRule [1].isOn = 1 == roomVo.diFen;
				ruijinGameRule [2].isOn = 2 == roomVo.diFen;
				ruijinGameRule [3].isOn = 5 == roomVo.diFen;

				ruijinGameRule [4].isOn = roomVo.tongZhuang;
				ruijinGameRule [5].isOn = !roomVo.tongZhuang;
				ruijinGameRule [6].isOn = roomVo.lunZhuang;
				ruijinGameRule [7].isOn = !roomVo.lunZhuang;
			}
		}
	}
	public void loadSet_PDK(){

	}
	public void loadSet_DN(){
		if (PlayerPrefs.HasKey ("userDefaultSet_DN")) {
			userDefaultSet = PlayerPrefs.GetString("userDefaultSet_DN");
			if (userDefaultSet != null){
				RoomCreateVo roomVo = JsonMapper.ToObject<RoomCreateVo> (userDefaultSet);
				dnGameRule [0].isOn = 10 == roomVo.roundNumber;
				dnGameRule [1].isOn = 20 == roomVo.roundNumber;
				dnGameRule [2].isOn = !roomVo.qiang;
				dnGameRule [3].isOn = roomVo.qiang;
				dnGameRule [4].isOn = 1 == roomVo.diFen;
				dnGameRule [5].isOn = 2 == roomVo.diFen;
				dnGameRule [6].isOn = 3 == roomVo.diFen;
				dnGameRule [7].isOn = roomVo.ming;
				dnGameRule [8].isOn = !roomVo.ming;
				dnGameRule [9].isOn = 1 == roomVo.mengs;
				dnGameRule [10].isOn = 2 == roomVo.mengs;
				dnGameRule [11].isOn = roomVo.AA;
				dnGameRule [12].isOn = !roomVo.AA;
			}
			MingChanged ();
		}
	}

	public void openDefaultSetingPanel(){
		GlobalDataScript.userGameType = GameType.GameType_MJ_RuiJin;
		GameType gameType = GlobalDataScript.userGameType;
		if (PlayerPrefs.HasKey ("userDefaultGameType")) {
			gameType = (GameType)PlayerPrefs.GetInt("userDefaultGameType");
		}
		switch (gameType) {
		case GameType.GameType_MJ_Developing:
			openDeveloping();
			break;
//		case GameType.GameType_MJ_ZhuangZhuang:
		case GameType.GameType_MJ_YiChun:
			openSetingPanel_ZhuangZhuang ();
			break;
		case GameType.GameType_MJ_HuaShui:
			openSetingPanel_HuaShui ();
			break;
		case GameType.GameType_MJ_ChangSha:
			openSetingPanel_ChangSha ();
			break;
		case GameType.GameType_MJ_GuangDong:
			openSetingPanel_GuangDong ();
			break;
		case GameType.GameType_MJ_GanZhou:
			openSetingPanel_GanZhou ();
			break;
		case GameType.GameType_MJ_RuiJin:
			openSetingPanel_RuiJin ();
			break;
		case GameType.GameType_PK_PDK:
			openSetingPanel_PDK ();
			break;
		case GameType.GameType_PK_DN:
			openSetingPanel_DN ();
			break;
		default:
			break;
		}
	}

	public void setGameObjectActive(GameType gt){//设置游戏物体显示的切换
		Btn_zhuanZ_liang.SetActive(false);//GameType.GameType_MJ_ZhuangZhuang == gt
		Btn_zhuanZ_dark.SetActive(false);//GameType.GameType_MJ_ZhuangZhuang != gt
		Btn_huaS_liang.SetActive(false);//GameType.GameType_MJ_HuaShui == gt
		Btn_huaS_dark.SetActive(false);//GameType.GameType_MJ_HuaShui != gt
		Btn_ganzhou_liang.SetActive (GameType.GameType_MJ_GanZhou == gt);//
		Btn_ganzhou_dark.SetActive (GameType.GameType_MJ_GanZhou != gt);//
		Btn_ruijin_liang.SetActive (GameType.GameType_MJ_RuiJin == gt);
		Btn_ruijin_dark.SetActive (GameType.GameType_MJ_RuiJin != gt);
		Btn_run_liang.SetActive(GameType.GameType_MJ_GuangDong == gt);
		Btn_run_dark.SetActive(GameType.GameType_MJ_GuangDong != gt);
		Btn_pdk_liang.SetActive (GameType.GameType_PK_PDK == gt);//GameType.GameType_PK_PDK == gt
		Btn_pdk_dark.SetActive (GameType.GameType_PK_PDK != gt);//GameType.GameType_PK_PDK != gt
		Btn_dn_liang.SetActive (GameType.GameType_PK_DN == gt);
		Btn_dn_dark.SetActive (GameType.GameType_PK_DN != gt);

		button_Create_Sure.gameObject.SetActive(false);

//		panelZhuanzhuanSetting.SetActive(GameType.GameType_MJ_ZhuangZhuang == gt);
		panelZhuanzhuanSetting.SetActive(GameType.GameType_MJ_YiChun == gt);
		panelHuashuiSetting.SetActive(GameType.GameType_MJ_HuaShui == gt);
		panelChangshaSetting.SetActive(GameType.GameType_MJ_ChangSha == gt);
		panelGuangDongSetting.SetActive(GameType.GameType_MJ_GuangDong == gt);
		panelGanZhouSetting.SetActive (GameType.GameType_MJ_GanZhou == gt);
		panelRuiJinSetting.SetActive (GameType.GameType_MJ_RuiJin == gt);
		panelPdkSetting.SetActive (GameType.GameType_PK_PDK == gt);
		panelDevoloping.SetActive(GameType.GameType_MJ_Developing == gt);
		panelDnSetting.SetActive (GameType.GameType_PK_DN == gt);
	}
	public void onJoinRoomCallBack(ClientResponse response)
	{
		watingPanel.gameObject.SetActive(false);
		MyDebug.Log(response);
		if (response.status == 1)
		{
			GlobalDataScript.roomJoinResponseData = JsonMapper.ToObject<RoomCreateVo>(response.message);

			GlobalDataScript.roomVo.addWordCard = GlobalDataScript.roomJoinResponseData.addWordCard;
			GlobalDataScript.roomVo.hong = GlobalDataScript.roomJoinResponseData.hong;
			GlobalDataScript.roomVo.ma = GlobalDataScript.roomJoinResponseData.ma;
			GlobalDataScript.roomVo.name = GlobalDataScript.roomJoinResponseData.name;
			GlobalDataScript.roomVo.roomId = GlobalDataScript.roomJoinResponseData.roomId;
			GlobalDataScript.roomVo.roomType = GlobalDataScript.roomJoinResponseData.roomType;
			GlobalDataScript.roomVo.roundNumber = GlobalDataScript.roomJoinResponseData.roundNumber;
			GlobalDataScript.roomVo.sevenDouble = GlobalDataScript.roomJoinResponseData.sevenDouble;
			GlobalDataScript.roomVo.xiaYu = GlobalDataScript.roomJoinResponseData.xiaYu;
			GlobalDataScript.roomVo.ziMo = GlobalDataScript.roomJoinResponseData.ziMo;
			GlobalDataScript.roomVo.gui = GlobalDataScript.roomJoinResponseData.gui;
			GlobalDataScript.roomVo.gangHu = GlobalDataScript.roomJoinResponseData.gangHu;
			GlobalDataScript.roomVo.gangHuQuanBao = GlobalDataScript.roomJoinResponseData.gangHuQuanBao;
			GlobalDataScript.roomVo.wuGuiX2 = GlobalDataScript.roomJoinResponseData.wuGuiX2;
			GlobalDataScript.roomVo.guiPai = GlobalDataScript.roomJoinResponseData.guiPai;
			GlobalDataScript.roomVo.shangxiaFanType = GlobalDataScript.roomJoinResponseData.shangxiaFanType;
			GlobalDataScript.roomVo.diFen = GlobalDataScript.roomJoinResponseData.diFen;
			GlobalDataScript.roomVo.tongZhuang = GlobalDataScript.roomJoinResponseData.tongZhuang;
			GlobalDataScript.roomVo.pingHu = GlobalDataScript.roomJoinResponseData.pingHu;
			GlobalDataScript.roomVo.keDianPao = GlobalDataScript.roomJoinResponseData.keDianPao;
			GlobalDataScript.roomVo.lunZhuang = GlobalDataScript.roomJoinResponseData.lunZhuang;
			GlobalDataScript.roomVo.gameType = GlobalDataScript.roomJoinResponseData.gameType;
			GlobalDataScript.roomVo.zhang16 = GlobalDataScript.roomJoinResponseData.zhang16;
			GlobalDataScript.roomVo.showPai = GlobalDataScript.roomJoinResponseData.showPai;
			GlobalDataScript.roomVo.xian3 = GlobalDataScript.roomJoinResponseData.xian3;
			GlobalDataScript.roomVo.qiang = GlobalDataScript.roomJoinResponseData.qiang;
			GlobalDataScript.roomVo.ming = GlobalDataScript.roomJoinResponseData.ming;
			GlobalDataScript.roomVo.mengs = GlobalDataScript.roomJoinResponseData.mengs;
			GlobalDataScript.roomVo.AA = GlobalDataScript.roomJoinResponseData.AA;

			GlobalDataScript.surplusTimes = GlobalDataScript.roomJoinResponseData.roundNumber;
			GlobalDataScript.loginResponseData.roomId = GlobalDataScript.roomJoinResponseData.roomId;

			GlobalDataScript.reEnterRoomData=null;

			GlobalDataScript.loadTime = GlobalDataScript.GetTimeStamp() ;

			if(GlobalDataScript.roomVo.gameType == 0 )//(int)GameType.GameType_PK_PDK
				GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab ("Prefab/Panel_GamePlay");
			else if (GlobalDataScript.roomVo.gameType == 1)//(int)GameType.GameType_PK_PDK
				GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab ("Prefab/Panel_GamePDK");
			else if (GlobalDataScript.roomVo.gameType == 3)//(int)GameType.GameType_PK_DN
				GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab ("Prefab/Panel_GameDN");
			//GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab(GlobalDataScript.playObject);
			//SocketEventHandle.getInstance().gameReadyNotice += GlobalDataScript.gamePlayPanel.GetComponent<MyMahjongScript>().gameReadyNotice;
			//GlobalDataScript.gamePlayPanel.GetComponent<MyMahjongScript>().joinToRoom(GlobalDataScript.roomJoinResponseData.playerList);

			connectRetruen = true;
			closeDialog();
		}
		else {
			//clear();
			//TipsManagerScript.getInstance();
			//watingPanel.gameObject.SetActive(true);
			//watingPanel.gameObject.transform.gameObject.SetActive(true);
			//watingPanel.gameObject.transform.FindChild("tip3/Text").GetComponent<Text>().text = response.message;


			TipsManagerScript.getInstance().setTips(response.message);
			closeDialog();
			GlobalDataScript.homePanel.GetComponent<HomePanelScript>().openEnterRoomDialog();

		}

	}
	bool connectRetruen = false;

}
