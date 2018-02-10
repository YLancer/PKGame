using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using System;
using LitJson;
using UnityEngine.SceneManagement;


public class CrateRoomSettingScript : MonoBehaviour {
    #region
 //   public GameObject panelZhuanzhuanSetting;
	//public GameObject panelChangshaSetting;
	//public GameObject panelHuashuiSetting;
 //   public GameObject panelGuangDongSetting;
	//public GameObject panelGanZhouSetting;
	//public GameObject panelRuiJinSetting;
	//public GameObject panelHongZhongSetting;
	//public GameObject panelChaoShanSetting;
    
 //   public GameObject panelPdkSetting;
 //   public GameObject panelDevoloping;
	//public GameObject panelDnSetting;
	//public GameObject panelDzpkSetting;
 //   public GameObject panelAmhSetting;
 //   public GameObject Button_zhuanzhuan1;
 //   public GameObject Button_zhuanzhuan;
 //   public GameObject Button_huashui1;
 //   public GameObject Button_huashui;

 //   public GameObject Button_changsha1;
 //   public GameObject Button_changsha;
    
 //   public GameObject Btn_zhuanZ_liang;
 //   public GameObject Btn_zhuanZ_dark;
 //   public GameObject Btn_huaS_liang;
 //   public GameObject Btn_huaS_dark;
	//public GameObject Btn_run_liang;
	//public GameObject Btn_run_dark;
	//public GameObject Btn_ganzhou_liang;
	//public GameObject Btn_ganzhou_dark;
	//public GameObject Btn_ruijin_liang;
	//public GameObject Btn_ruijin_dark;
	//public GameObject Btn_hongzhong_liang;
	//public GameObject Btn_hongzhong_dark;
	//public GameObject Btn_chaoshan_liang;
	//public GameObject Btn_chaoshan_dark;
    
 //   public GameObject Btn_pdk_liang;
	//public GameObject Btn_pdk_dark;
	//public GameObject Btn_dn_liang;
	//public GameObject Btn_dn_dark;
	//public GameObject Btn_dzpk_liang;
	//public GameObject Btn_dzpk_dark;
 //   public GameObject Btn_amh_liang;
 //   public GameObject Btn_amh_dark;

 //   public GameObject Btn_sangong_liang;
	//public GameObject Btn_sangong_dark;
	//public GameObject Btn_mushi_liang;
	//public GameObject Btn_mushi_dark;
    #endregion
    // -lan   斗地主
    public Button Btn_ddz_liang;               
	public Button Btn_ddz_dark;

    public Image watingPanel;

    #region
    public List<Toggle> zhuanzhuanRoomCards;//转转麻将房卡数
	public List<Toggle> changshaRoomCards;//长沙麻将房卡数
	public List<Toggle> huashuiRoomCards;//划水麻将房卡数
	public List<Toggle> guangdongRoomCards;//广东麻将房卡数
	public List<Toggle> ganzhouRoomCards;//赣州麻将房卡数
	public List<Toggle> ruijinRoomCards;//瑞金麻将房卡数

	public List<Toggle> xinchaoshanGameRule;//潮汕麻将规则

    public List<Toggle> zhuanzhuanGameRule;//转转麻将玩法
	public List<Toggle> changshaGameRule;//长沙麻将玩法
	public List<Toggle> huashuiGameRule;//划水麻将玩法
    public List<Toggle> guangdongGameRule;//广东麻将玩法
	public List<Toggle> ganzhouGameRule;//赣州麻将玩法
	public List<Toggle> ruijinGameRule;//瑞金麻将玩法
	public List<Toggle> pdkGameRule;//跑得快玩法
	public List<Toggle> dnGameRule;//斗牛玩法
	public List<Toggle> dzpkGameRule;//德州扑克玩法
    public List<Toggle> amhGameRule;//德州扑克玩法

    public List<Toggle> zhuanzhuanZhuama;//转转麻将抓码个数
	public List<Toggle> changshaZhuama;//长沙麻将抓码个数
	public List<Toggle> huashuixiayu;//划水麻将下鱼条数
    public List<Toggle> guangdongZhuama;//广东麻将抓码个数

    public List<Toggle> guangdongGui;//广东麻将鬼牌
    #endregion

    private int roomCardCount;//房卡数
	private GameObject gameSence;
	private RoomCreateVo sendVo;//创建房间的信息

	public Button createRoom;

	public GameObject[] fangka;
	private string userDefaultSet = null;

    public Slider slider_dzpk;
    public List<Text> difen_DZPK;
    public Slider slider_amh;
    public List<Text> difen_AMH;


    public List<Toggle> ddzRules;
    


    void Start () {
        Btn_ddz_liang.onClick.AddListener(openSettingPanel_DDZ);        //按钮绑定时间—斗地主 
        Btn_ddz_dark.onClick.AddListener(openSettingPanel_DDZ);
        PlayerPrefs.DeleteAll ();
        //PlayerPrefs.DeleteKey ("userDefaultGameType");
        //PlayerPrefs.DeleteKey ("userDefaultSet_ZhuangZhuang");
        //PlayerPrefs.DeleteKey ("userDefaultSet_HuaShui");
        //PlayerPrefs.DeleteKey ("userDefaultSet_ChangSha");
        //PlayerPrefs.DeleteKey ("userDefaultSet_GuangDong");
        //PlayerPrefs.DeleteKey ("userDefaultSet_GanZhou");
        //PlayerPrefs.DeleteKey ("userDefaultSet_RuiJin");
        //PlayerPrefs.DeleteKey ("userDefaultSet_DN");
        //PlayerPrefs.DeleteKey ("userDefaultSet_PDK");
        PlayerPrefs.DeleteKey("userDefaultSet_DDZ");

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
        //showDZPK_difen();
        //showAMH_difen();
    }
    string text_DZPK;
    void showDZPK_difen()
    {
        text_DZPK = "200";
         if (slider_dzpk.value >=0 && slider_dzpk.value <=0.2f){  
                slider_dzpk.value = 0.1f;
            text_DZPK = "200";
        }
        else if(slider_dzpk.value >0.2f && slider_dzpk.value <=0.4f){
            slider_dzpk.value = 0.3f;
            text_DZPK = "500";
        }
        else if (slider_dzpk.value >0.4f && slider_dzpk.value <=0.6f){
            slider_dzpk.value = 0.5f;
            text_DZPK = "1000";
        }
        else if (slider_dzpk.value >0.6f && slider_dzpk.value <=0.8f){
            slider_dzpk.value = 0.7f;
            text_DZPK = "2000";
        }
        else if (slider_dzpk.value > 0.8f && slider_dzpk.value <= 1f){
            slider_dzpk.value = 1f;
            text_DZPK = "5000";
        }
    }
    void showAMH_difen()
    {
        text_DZPK = "200";
        if (slider_amh.value >= 0 && slider_amh.value <= 0.2f)
        {
            slider_amh.value = 0.1f;
            text_DZPK = "200";
        }
        else if (slider_amh.value > 0.2f && slider_amh.value <= 0.4f)
        {
            slider_amh.value = 0.3f;
            text_DZPK = "500";
        }
        else if (slider_amh.value > 0.4f && slider_amh.value <= 0.6f)
        {
            slider_amh.value = 0.5f;
            text_DZPK = "1000";
        }
        else if (slider_amh.value > 0.6f && slider_amh.value <= 0.8f)
        {
            slider_amh.value = 0.7f;
            text_DZPK = "2000";
        }
        else if (slider_amh.value > 0.8f && slider_amh.value <= 1f)
        {
            slider_amh.value = 1f;
            text_DZPK = "5000";
        }
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
		//GlobalDataScript.userGameType = GameType.GameType_MJ_ZhuangZhuang;
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
	 * 打开红中麻将设置面板
	 */ 
	public void openSetingPanel_HongZhong(){
		SoundCtrl.getInstance().playSoundByActionButton(1);
		GlobalDataScript.userGameType = GameType.GameType_MJ_HongZhong;
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
		//TipsManagerScript.getInstance ().setTips ("开发中");
		SoundCtrl.getInstance().playSoundByActionButton(1);
		//return;
	
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
	public void openSetingPanel_ChaoShan()
	{
		SoundCtrl.getInstance().playSoundByActionButton(1);
		GlobalDataScript.userGameType = GameType.GameType_MJ_ChaoShan;
		setGameObjectActive (GlobalDataScript.userGameType);
		loadDefaultSet (GlobalDataScript.userGameType);
		createRoom.onClick.RemoveAllListeners ();
		createRoom.onClick.AddListener (delegate() {
			this.createxinChaoShanRoom();
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

	/**
	 * 打开德州扑克设置面板
	 */
	public void openSetingPanel_DZPK(){
		SoundCtrl.getInstance().playSoundByActionButton(1);
		GlobalDataScript.userGameType = GameType.GameType_PK_DZPK;
		loadDefaultSet (GlobalDataScript.userGameType);
		setGameObjectActive (GlobalDataScript.userGameType);

		createRoom.onClick.RemoveAllListeners ();
		createRoom.onClick.AddListener (delegate() {
			this.createDZPKRoom();
		});
	}

    /**
	 * 打开奥马哈设置面板
	 */
    public void openSetingPanel_AMH()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.userGameType = GameType.GameType_PK_AMH;
        loadDefaultSet(GlobalDataScript.userGameType);
        setGameObjectActive(GlobalDataScript.userGameType);

        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate () {
            this.createAMHRoom();
        });
    }

    //-lan   打开斗地主的游戏界面
    void openSettingPanel_DDZ()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.userGameType = GameType.GameType_PK_DDZ;
        setGameObjectActive(GlobalDataScript.userGameType);
        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate(){this.createDDZRoom(); });
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
//		bool isZimo=false;//自摸
//		bool hasHong=false;//红中赖子
//		bool isSevenDoube =false;//七小对
		//bool isGang = false;
//		int maCount = 0;
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

//		if (zhuanzhuanGameRule [0].isOn) {
//			isZimo = true;
//		}

		//if (zhuanzhuanGameRule [1].isOn) {
		//	isGang = true;
		//}

//		if (zhuanzhuanGameRule [2].isOn) {
//			hasHong = true;
//		}

//		if (zhuanzhuanGameRule [3].isOn) {
//			isSevenDoube = true;
//		}


//		for (int i = 0; i < zhuanzhuanZhuama.Count; i++) {
//			if (zhuanzhuanZhuama [i].isOn) {
//				maCount = 2 * (i + 1);
//				break;
//			}
//		}
		sendVo = new RoomCreateVo ();
		//sendVo.ma = maCount;
		sendVo.roundNumber = roundNumber;
//		sendVo.ziMo = isZimo?1:0;
//		sendVo.hong = hasHong;
//		sendVo.sevenDouble = isSevenDoube;
		//sendVo.roomType = (int)GameType.GameType_MJ_ZhuangZhuang;

		sendVo.roomType = (int)GameType.GameType_MJ_YiChun;

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


//        for (int i = 0; i < guangdongZhuama.Count; i++)
//        {
//            if (guangdongZhuama[i].isOn)
//            {
//                maCount = 2 * (i + 1);
//                break;
//            }
//        }

		if (guangdongZhuama [0].isOn) {
			maCount = 2;
		} else if (guangdongZhuama [1].isOn) {
			maCount = 4;
		} else if (guangdongZhuama [2].isOn) {
			maCount = 6;
		} else if (guangdongZhuama [5].isOn) {
			maCount = 8;
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
		string sendmsgstr = JsonMapper.ToJson(sendVo);
		ReqForCreateRoom (sendmsgstr);
	}

	/**
	 * 创建xin潮汕麻将房间
	 */
	public void createxinChaoShanRoom()
	{
		SoundCtrl.getInstance().playSoundByActionButton(1);
		int gui = 0; //鬼牌
		int maCount = 0;
		sendVo = new RoomCreateVo();         //设置局数  24局 8局 16局


		if (xinchaoshanGameRule[0].isOn)
		{
			sendVo.roundNumber = 8;
		}
		else if (xinchaoshanGameRule[1].isOn)
		{
			sendVo.roundNumber = 16;
		}
		else if(xinchaoshanGameRule[2].isOn)
		{
			sendVo.roundNumber = 24;
		}


		if (xinchaoshanGameRule[3].isOn)        //玩法设置  
		{
			sendVo.gangHu = true;
		}

		if (xinchaoshanGameRule[4].isOn)
		{
			sendVo.genZhuang = true;
		}
		if (xinchaoshanGameRule[5].isOn)
		{
			sendVo.chongZhuang = true;
		}

		if (xinchaoshanGameRule[6].isOn)
		{
			sendVo.pengpengHu = true;
		}

		if (xinchaoshanGameRule[7].isOn)
		{
			sendVo.qiDui = true;
		}

		if (xinchaoshanGameRule[8].isOn)
		{
			sendVo.qiangGangHu = true;
		}

		if (xinchaoshanGameRule[9].isOn)
		{
			sendVo.hunYiSe = true;
		}

		if (xinchaoshanGameRule[10].isOn)
		{
			sendVo.qingYiSe = true;
		}
		if (xinchaoshanGameRule[11].isOn)
		{
			sendVo.gangShangKaiHua = true;
		}

		if (xinchaoshanGameRule[12].isOn)
		{
			sendVo.haoHuaQiDui = true;
		}

		if (xinchaoshanGameRule[13].isOn)
		{
			sendVo.shiSanYao = true;
		}
		if (xinchaoshanGameRule[14].isOn)
		{
			sendVo.tianDiHu = true;
		}
		if (xinchaoshanGameRule[15].isOn)
		{
			sendVo.shuangHaoHua = true;
		}
		if (xinchaoshanGameRule[16].isOn)
		{
			sendVo.sanHaoHua = true;
		}
		if (xinchaoshanGameRule[17].isOn)
		{
			sendVo.shiBaLuoHan = true;
		}
		if (xinchaoshanGameRule[18].isOn)
		{
			sendVo.xiaoSanYuan = true;
		}
		if (xinchaoshanGameRule[19].isOn)
		{
			sendVo.xiaoSiXi = true;
		}
		if (xinchaoshanGameRule[20].isOn)
		{
			sendVo.daSanYuan = true;
		}
		if (xinchaoshanGameRule[21].isOn)
		{
			sendVo.daSiXi = true;
		}
		if (xinchaoshanGameRule[22].isOn)
		{
			sendVo.huaYaoJiu = true;
		}



		if (xinchaoshanGameRule[23].isOn)
		{
			sendVo.fengDing = 5;
		}
		else if (xinchaoshanGameRule[24].isOn)
		{
			sendVo.fengDing = 10;
		}
		else
		{
			sendVo.fengDing = 99999;
		}

		if (xinchaoshanGameRule[31].isOn)
			maCount = 2;
		else if (xinchaoshanGameRule[32].isOn)
			maCount = 4;
		else if (xinchaoshanGameRule[33].isOn)
			maCount = 6;
		else if (xinchaoshanGameRule[30].isOn)
			maCount = 0;

		//if (xinchaoshanZhuama[1].isOn)
		//  sendVo.zhuaMa = true;

		sendVo.ma = maCount;

		if (maCount > 0)
		{
			if (xinchaoshanGameRule[34].isOn)
				sendVo.maGenDifen = true;
			if (xinchaoshanGameRule[35].isOn)
				sendVo.maGenGang = true;
		}

		if (xinchaoshanGameRule[26].isOn)
		{
			gui = 0;
		}
		else if (xinchaoshanGameRule[27].isOn)
		{
			gui = 1;
		}
		else
		{
			if (xinchaoshanGameRule[29].isOn)
				gui = 3; //双鬼
			else
				gui = 2;
		}
		sendVo.gui = gui;

		if (xinchaoshanGameRule [36].isOn) {
			sendVo.AA = false;
		} else {
			sendVo.AA = true;
		}

		sendVo.roomType = 10;
		string sendmsgstr = JsonMapper.ToJson(sendVo);
		if (GlobalDataScript.loginResponseData.account.roomcard > 0)
		{
			watingPanel.gameObject.SetActive(true);

			CustomSocket.getInstance().sendMsg(new CreateRoomRequest(sendmsgstr));
			userDefaultSet = sendmsgstr;
		}
		else
		{
			TipsManagerScript.getInstance().setTips("你的房卡数量不足，不能创建房间");
		}
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
		
		sendVo.gameType = (int)GameTypePK.PDK;
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
		sendVo.niu7fan = dnGameRule [13].isOn;

        sendVo.gameType = (int)GameTypePK.DN;

        string sendmsgstr = JsonMapper.ToJson(sendVo);

		ReqForCreateRoom (sendmsgstr);
	}

	/**
	 * 创建德州扑克房间
	 */
	public void createDZPKRoom()
	{
		sendVo = new RoomCreateVo ();
		if (dzpkGameRule [0].isOn) {
			sendVo.roundNumber = 8;
		} else if (dzpkGameRule [1].isOn) {
			sendVo.roundNumber = 16;
		} else if (dzpkGameRule [2].isOn) {
			sendVo.roundNumber = 24;
		}

        //if (dzpkGameRule [3].isOn) {
        //	sendVo.initFen_dzpk = 200;
        //} else if (dzpkGameRule [4].isOn) {
        //	sendVo.initFen_dzpk = 500;
        //} else if (dzpkGameRule [5].isOn) {
        //	sendVo.initFen_dzpk = 1000;
        //} else if (dzpkGameRule [6].isOn) {
        //	sendVo.initFen_dzpk = 2000;
        //} else if (dzpkGameRule [7].isOn) {
        //	sendVo.initFen_dzpk = 5000;
        //}

        sendVo.initFen_dzpk = int.Parse(text_DZPK);
        sendVo.gameType = (int)GameTypePK.DZPK;
        sendVo.AA = false; //暂时用这个标记区分奥马哈跟德扑

        string sendmsgstr = JsonMapper.ToJson(sendVo);

		ReqForCreateRoom (sendmsgstr);
	}

    /**
	 * 创建奥马哈房间
	 */
    public void createAMHRoom() //TODO 奥马哈设置。
    {
        sendVo = new RoomCreateVo();
        if (amhGameRule[0].isOn)
        {
            sendVo.roundNumber = 8;
        }
        else if (amhGameRule[1].isOn)
        {
            sendVo.roundNumber = 16;
        }
        else if (amhGameRule[2].isOn)
        {
            sendVo.roundNumber = 24;
        }
        #region
        //if (amhGameRule[3].isOn)
        //{
        //    sendVo.initFen_dzpk = 200;
        //}
        //else if (amhGameRule[4].isOn)
        //{
        //    sendVo.initFen_dzpk = 500;
        //}
        //else if (amhGameRule[5].isOn)
        //{
        //    sendVo.initFen_dzpk = 1000;
        //}
        //else if (amhGameRule[6].isOn)
        //{
        //    sendVo.initFen_dzpk = 2000;
        //}
        //else if (amhGameRule[7].isOn)
        //{
        //    sendVo.initFen_dzpk = 5000;
        //}
        #endregion
        sendVo.initFen_dzpk = int.Parse(text_DZPK);
        sendVo.gameType = (int)GameTypePK.DZPK; //暂时能力不足，做成奥马哈寄生在德扑之下。
        sendVo.AA = true; //暂时用这个标记区分奥马哈跟德扑

        string sendmsgstr = JsonMapper.ToJson(sendVo);

        ReqForCreateRoom(sendmsgstr);
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
		string sendmsgstr = JsonMapper.ToJson (sendVo);
		ReqForCreateRoom (sendmsgstr);
	}

	/**
	 * 创建红中麻将房间
	 */
	public void createHongZhongRoom(){
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
		sendVo.hong = true;
		sendVo.xiaYu = maCount;
		sendVo.roundNumber = roundNumber;
		sendVo.ziMo = isZimo?1:0;
		sendVo.roomType = (int)GameType.GameType_MJ_HongZhong;
		sendVo.addWordCard = isFengpai;
		sendVo.sevenDouble = true;
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

    //斗地主玩法   -lan
   void createDDZRoom() 
    {
        sendVo = new RoomCreateVo();
        
        if(ddzRules[0].isOn){
            sendVo.roundNumber = 9;
        }
        else if (ddzRules[1].isOn) {
            sendVo.roundNumber = 18;
        }
        else if(ddzRules[2].isOn) {
            sendVo.roundNumber = 30;
        }

        if(ddzRules[3].isOn){
            sendVo.bombMultiple = 1;
        }
        else if (ddzRules[4].isOn){
            sendVo.bombMultiple = 2;
        }
        else if (ddzRules[5].isOn){
            sendVo.bombMultiple = 3;
        }
        else if (ddzRules[6].isOn){
            sendVo.bombMultiple = 4;
        }

        sendVo.isKick=!ddzRules[8].isOn;
        sendVo.AA = ddzRules[10].isOn;
        sendVo.gameType = (int)GameTypePK.DDZ;                                
        string sendRoomMessage = JsonMapper.ToJson(sendVo);
        print(" +++++createDDZRoom+++++++++++ " + sendRoomMessage);
        ReqForCreateRoom(sendRoomMessage);
    }

    public void onCreateRoomCallback(ClientResponse response){     // response.message 在创建房间阶段为房间号  response.headCode每个人的手牌数
        if (watingPanel != null) {
        	watingPanel.gameObject.SetActive(false);
        }
        MyDebug.Log (response.message+ "----------------onCreateRoomCallbackonCreateRoomCallback----此处已经返回房间号信息ok");
        if(response.message !=null)   //if (response.status == 1)
        {
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

			saveDefaultSet (GlobalDataScript.userGameType, userDefaultSet);

			if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.NULL)
                GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab ("Prefab/Panel_GamePlay");
			//else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.PDK)
   //             GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab ("Prefab/Panel_GamePDK");
			//else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.DN)
   //             GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab ("Prefab/Panel_GameDN");
			//else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.DZPK && GlobalDataScript.roomVo.AA == false)
   //             GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab ("Prefab/Panel_GameDZPK"); //Panel_GameDZPK
   //         else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.DZPK && GlobalDataScript.roomVo.AA == true) //(int)GameTypePK.AMH
   //             GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_GameAMH");
            //  -lan   加载斗地主的预制件   暂定这样
            else if(GlobalDataScript.roomVo.gameType ==(int)GameTypePK.DDZ)
                GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_GamePDK");

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
			//case GameType.GameType_MJ_ZhuangZhuang:
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
			case GameType.GameType_MJ_ChaoShan:
				PlayerPrefs.SetString ("userDefaultSet_ChaoShan", userSet);
				break;	
			case GameType.GameType_PK_PDK:
				PlayerPrefs.SetString ("userDefaultSet_PDK", userSet);
				break;	
			case GameType.GameType_PK_DN:
				PlayerPrefs.SetString ("userDefaultSet_DN", userSet);
				break;
            case GameType.GameType_PK_DDZ:
                PlayerPrefs.SetString ("userDefaultSet_DDZ", userSet);    //-lan
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
		//case GameType.GameType_MJ_ZhuangZhuang:
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
		case GameType.GameType_MJ_HongZhong:
			loadSet_HongZhong();
			break;
		case GameType.GameType_MJ_ChaoShan:
			loadSet_ChaoShan();
			break;
		case GameType.GameType_PK_PDK:
			loadSet_PDK();
			break;
		case GameType.GameType_PK_DN:
			loadSet_DN();
			break;
		case GameType.GameType_PK_DZPK:
			loadSet_DZPK();
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
	public void loadSet_HongZhong(){

	}
	public void loadSet_GanZhou(){
		if (PlayerPrefs.HasKey ("userDefaultSet_GanZhou")) {
			userDefaultSet = PlayerPrefs.GetString("userDefaultSet_GanZhou");
			if (userDefaultSet != null){
				RoomCreateVo roomVo = JsonMapper.ToObject<RoomCreateVo> (userDefaultSet);

				ganzhouRoomCards [0].isOn = 8 == roomVo.roundNumber;
				ganzhouRoomCards [1].isOn = 16 == roomVo.roundNumber;

				ganzhouGameRule [0].isOn = 1 == roomVo.shangxiaFanType;
				ganzhouGameRule [1].isOn = 2 == roomVo.shangxiaFanType;

				ganzhouGameRule [2].isOn = 1 == roomVo.diFen;
				ganzhouGameRule [3].isOn = 2 == roomVo.diFen;

				ganzhouGameRule [4].isOn = roomVo.tongZhuang;
				ganzhouGameRule [5].isOn = !roomVo.tongZhuang;

				ganzhouGameRule [6].isOn = 1 == roomVo.pingHu;
				ganzhouGameRule [7].isOn = 2 == roomVo.pingHu;
				ganzhouGameRule [8].isOn = 3 == roomVo.pingHu;
			}
		}
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

	public void loadSet_ChaoShan(){
		if (PlayerPrefs.HasKey ("userDefaultSet_ChaoShan")) {
			userDefaultSet = PlayerPrefs.GetString("userDefaultSet_ChaoShan");
			if (userDefaultSet != null){
				RoomCreateVo roomVo = JsonMapper.ToObject<RoomCreateVo> (userDefaultSet);

				xinchaoshanGameRule[0].isOn = 8 == roomVo.roundNumber;
				xinchaoshanGameRule[1].isOn = 16 == roomVo.roundNumber;
				xinchaoshanGameRule[2].isOn = 24 == roomVo.roundNumber;

				xinchaoshanGameRule [3].isOn = roomVo.gangHu;
				xinchaoshanGameRule [4].isOn = roomVo.genZhuang;
				xinchaoshanGameRule [5].isOn = roomVo.chongZhuang;
				xinchaoshanGameRule [6].isOn = roomVo.pengpengHu;
				xinchaoshanGameRule [7].isOn = roomVo.qiDui;
				xinchaoshanGameRule [8].isOn = roomVo.qiangGangHu;
				xinchaoshanGameRule [9].isOn = roomVo.hunYiSe;
				xinchaoshanGameRule [10].isOn = roomVo.qingYiSe;
				xinchaoshanGameRule [11].isOn = roomVo.gangShangKaiHua;
				xinchaoshanGameRule [12].isOn = roomVo.haoHuaQiDui;
				xinchaoshanGameRule [13].isOn = roomVo.shiSanYao;
				xinchaoshanGameRule [14].isOn = roomVo.tianDiHu;
				xinchaoshanGameRule [15].isOn = roomVo.shuangHaoHua;
				xinchaoshanGameRule [16].isOn = roomVo.sanHaoHua;
				xinchaoshanGameRule [17].isOn = roomVo.shiBaLuoHan;
				xinchaoshanGameRule [18].isOn = roomVo.xiaoSanYuan;
				xinchaoshanGameRule [19].isOn = roomVo.xiaoSiXi;
				xinchaoshanGameRule [20].isOn = roomVo.daSanYuan;
				xinchaoshanGameRule [21].isOn = roomVo.daSiXi;
				xinchaoshanGameRule [22].isOn = roomVo.huaYaoJiu;
				xinchaoshanGameRule [23].isOn = 5 == roomVo.fengDing;
				xinchaoshanGameRule [24].isOn = 10 == roomVo.fengDing;
				xinchaoshanGameRule [25].isOn = 99999 == roomVo.fengDing;

				xinchaoshanGameRule [26].isOn = 0 == roomVo.gui;
				xinchaoshanGameRule [27].isOn = 1 == roomVo.gui;
				xinchaoshanGameRule [28].isOn = (2 == roomVo.gui || 3 == roomVo.gui);
				xinchaoshanGameRule [29].isOn = 3 == roomVo.gui;

				xinchaoshanGameRule [30].isOn = 0 == roomVo.ma;
				xinchaoshanGameRule [31].isOn = 2 == roomVo.ma;
				xinchaoshanGameRule [32].isOn = 4 == roomVo.ma;
				xinchaoshanGameRule [33].isOn = 6 == roomVo.ma;
				if (roomVo.ma > 0) {
					xinchaoshanGameRule [34].isOn = roomVo.maGenDifen;
					xinchaoshanGameRule [35].isOn = roomVo.maGenGang;
				} else {
					xinchaoshanGameRule [34].isOn = false;
					xinchaoshanGameRule [35].isOn = false;
				}

				xinchaoshanGameRule [36].isOn = !roomVo.AA;
				xinchaoshanGameRule [37].isOn = roomVo.AA;

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

	public void loadSet_DZPK(){
		
	}

	public void openDefaultSetingPanel(){
		GlobalDataScript.userGameType = GameType.GameType_PK_DDZ;                   //GameType_PK_PDK  GameType_MJ_ChaoShan
        GlobalDataScript.goldType = false;
		GameType gameType = GlobalDataScript.userGameType;
		if (PlayerPrefs.HasKey ("userDefaultGameType")) {
			gameType = (GameType)PlayerPrefs.GetInt("userDefaultGameType");
		}
		switch (gameType) {
		case GameType.GameType_MJ_Developing:
			openDeveloping();
			break;
		//case GameType.GameType_MJ_ZhuangZhuang:
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
		case GameType.GameType_MJ_ChaoShan:
			openSetingPanel_ChaoShan ();
			break;
		case GameType.GameType_PK_PDK:   
          openSetingPanel_PDK ();
			break;
		case GameType.GameType_PK_DN:
			openSetingPanel_DN ();
			break;
		case GameType.GameType_PK_DZPK:
			openSetingPanel_DZPK ();
			break;
        case GameType.GameType_PK_DDZ:
            openSettingPanel_DDZ();        // 斗地主   -lan
            break;
            default:
			break;
		}
	}

	public void setGameObjectActive(GameType gt){//设置游戏物体显示的切换
//		Btn_zhuanZ_liang.SetActive(GameType.GameType_MJ_ZhuangZhuang == gt);//GameType.GameType_MJ_ZhuangZhuang == gt
//		Btn_zhuanZ_dark.SetActive(GameType.GameType_MJ_ZhuangZhuang != gt);//GameType.GameType_MJ_ZhuangZhuang != gt
		//Btn_zhuanZ_liang.SetActive(GameType.GameType_MJ_YiChun == gt);
		//Btn_zhuanZ_dark.SetActive(GameType.GameType_MJ_YiChun != gt);
//		Btn_huaS_liang.SetActive(false);//GameType.GameType_MJ_HuaShui == gt
//		Btn_huaS_dark.SetActive(false);//GameType.GameType_MJ_HuaShui != gt
		//Btn_huaS_liang.SetActive(GameType.GameType_MJ_HuaShui == gt);//GameType.GameType_MJ_HuaShui == gt
		//Btn_huaS_dark.SetActive(GameType.GameType_MJ_HuaShui != gt);//GameType.GameType_MJ_HuaShui != gt
		//Btn_ganzhou_liang.SetActive (GameType.GameType_MJ_GanZhou == gt);//
		//Btn_ganzhou_dark.SetActive (GameType.GameType_MJ_GanZhou != gt);//
		//Btn_ruijin_liang.SetActive (GameType.GameType_MJ_RuiJin == gt);
		//Btn_ruijin_dark.SetActive (GameType.GameType_MJ_RuiJin != gt);
		//Btn_hongzhong_liang.SetActive (GameType.GameType_MJ_HongZhong == gt);
		//Btn_hongzhong_dark.SetActive (GameType.GameType_MJ_HongZhong != gt);
		//Btn_run_liang.SetActive(GameType.GameType_MJ_GuangDong == gt);
		//Btn_run_dark.SetActive(GameType.GameType_MJ_GuangDong != gt);
		//Btn_pdk_liang.SetActive (GameType.GameType_PK_PDK == gt);//GameType.GameType_PK_PDK == gt
		//Btn_pdk_dark.SetActive (GameType.GameType_PK_PDK != gt);//GameType.GameType_PK_PDK != gt
		//Btn_dn_liang.SetActive (GameType.GameType_PK_DN == gt);
		//Btn_dn_dark.SetActive (GameType.GameType_PK_DN != gt);
		//Btn_chaoshan_liang.SetActive (GameType.GameType_MJ_ChaoShan == gt);
		//Btn_chaoshan_dark.SetActive (GameType.GameType_MJ_ChaoShan != gt);
		//Btn_dzpk_liang.SetActive (GameType.GameType_PK_DZPK == gt);
		//Btn_dzpk_dark.SetActive (GameType.GameType_PK_DZPK != gt);
  //      Btn_amh_liang.SetActive(GameType.GameType_PK_AMH == gt);
  //      Btn_amh_dark.SetActive(GameType.GameType_PK_AMH != gt);

        //		if (GlobalDataScript.configTemp.pay == 1) {
        //			Btn_ganzhou_liang.SetActive (false);
        //			Btn_ganzhou_dark.SetActive (true);//true
        //		}
        if (GameType.GameType_MJ_Developing == gt) {
			createRoom.gameObject.SetActive (false);
		} else {
			createRoom.gameObject.SetActive (true);
		}


//		panelZhuanzhuanSetting.SetActive(GameType.GameType_MJ_ZhuangZhuang == gt);
		//panelZhuanzhuanSetting.SetActive(GameType.GameType_MJ_YiChun == gt);
		//panelHuashuiSetting.SetActive(GameType.GameType_MJ_HuaShui == gt);
		//panelChangshaSetting.SetActive(GameType.GameType_MJ_ChangSha == gt);
		//panelGuangDongSetting.SetActive(GameType.GameType_MJ_GuangDong == gt);
		//panelGanZhouSetting.SetActive (GameType.GameType_MJ_GanZhou == gt);
		//panelRuiJinSetting.SetActive (GameType.GameType_MJ_RuiJin == gt);
		//panelHongZhongSetting.SetActive (GameType.GameType_MJ_HongZhong == gt);
		//panelChaoShanSetting.SetActive (GameType.GameType_MJ_ChaoShan == gt);
		//panelPdkSetting.SetActive (GameType.GameType_PK_PDK == gt);
		//panelDevoloping.SetActive(GameType.GameType_MJ_Developing == gt);
		//panelDnSetting.SetActive (GameType.GameType_PK_DN == gt);
		//panelDzpkSetting.SetActive (GameType.GameType_PK_DZPK == gt);
  //      panelAmhSetting.SetActive(GameType.GameType_PK_AMH == gt);

        // lan - 斗地主
        Btn_ddz_liang.gameObject.SetActive(GameType.GameType_PK_DDZ == gt);
        Btn_ddz_dark.gameObject.SetActive(GameType.GameType_PK_DDZ != gt);


    }
}
