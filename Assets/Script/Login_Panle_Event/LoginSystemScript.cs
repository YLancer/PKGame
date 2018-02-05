using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using LitJson;
using System.Collections.Generic;
using cn.sharesdk.unity3d;
using System.Threading;
using System;

public class LoginSystemScript : MonoBehaviour
{
	

	//public ShareSDK shareSdk;
	private GameObject panelCreateDialog;

	public Toggle agreeProtocol;

	public Text versionText;

	private int tapCount = 0;
//点击次数
	public GameObject watingPanel;


	public GameObject loginBtn;
	public GameObject youkeBtn;

	public Text loginText;
	void Start ()
	{
		//

		#if UNITY_IPHONE && !UNITY_EDITOR
		if(GlobalDataScript.getInstance().wechatOperate.shareSdk.IsClientValid(PlatformType.WechatPlatform)){
			youkeBtn.SetActive(false);
			loginBtn.SetActive(true);
		}else {
			youkeBtn.SetActive(true);
			loginBtn.SetActive(false);
		}
		#else 
			youkeBtn.SetActive(false);
			loginBtn.SetActive(true);
		#endif


		//shareSdk.showUserHandler = getUserInforCallback;//注册获取用户信息回调
		CustomSocket.hasStartTimer = false;
		SocketEventHandle.getInstance ().LoginCallBack += LoginCallBack;
		SocketEventHandle.getInstance ().RoomBackResponse += RoomBackResponse;


		GlobalDataScript.isonLoginPage = true;
		versionText.text = "版本号：" + Application.version;
		//WxPayImpl test = new WxPayImpl(gameObject);
		//test.callTest ("dddddddddddddddddddddddddddd");
		StartCoroutine (ConnectTime1 (3f, 1));

		//每隔0.1秒執行一次定時器
		InvokeRepeating ("isConnected", 0.1f, 0.1f);

		GlobalDataScript.headTime = GlobalDataScript.GetTimeStamp ();

	}

	private void isConnected ()
	{
		if (CustomSocket.getInstance ().isConnected) {
			watingPanel.SetActive (false);
			this.CancelInvoke ();//取消定时器的执行


			if (PlayerPrefs.HasKey ("loginInfo")) {
				#if UNITY_EDITOR
					//login ();
				#else
					login();
				#endif
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey (KeyCode.Escape)) { //Android系统监听返回键，由于只有Android和ios系统所以无需对系统做判断
			if (panelCreateDialog == null) {
				//panelCreateDialog = Instantiate (Resources.Load("Prefab/Panel_Exit")) as GameObject;
				//panelCreateDialog.transform.parent = gameObject.transform;
				//panelCreateDialog.transform.localScale = Vector3.one;
				//panelCreateDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
				//panelCreateDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
			}
			
		} 
	}

	int count = 0;

	IEnumerator ConnectTime1 (float time, byte type)
	{
		connectRetruen = false;

		if (watingPanel != null) {
			watingPanel.SetActive (true);
			watingPanel.GetComponentInChildren<Text> ().text = "正在连接服务器...";
		}
			
		CustomSocket.hasStartTimer = false;
		CustomSocket.getInstance ().Connect ();
		ChatSocket.getInstance ().Connect ();
		GlobalDataScript.isonLoginPage = true;

		yield return new WaitForSeconds (time);
		if (!connectRetruen) {//超过5秒还没连接成功显示失败          
			if (type == 1) {
				watingPanel.SetActive (false);
			} else if (type == 2) {
                 
			}
		}
	}


	public void login ()
	{
		MyDebug.Log ("----------------1-------------------");
		if (!CustomSocket.getInstance ().isConnected) {

			StartCoroutine (ConnectTime1 (3f, 1));
			InvokeRepeating ("isConnected", 0.1f, 0.1f);

			tapCount = 0;
			MyDebug.Log ("----------------2------------------");
			return;
		}

		GlobalDataScript.reinitData ();//初始化界面数据
		if (agreeProtocol.isOn) {
			MyDebug.Log ("----------------3------------------");
			doLogin ();
			watingPanel.GetComponentInChildren<Text> ().text = "进入游戏中";
			watingPanel.SetActive (true);

			StartCoroutine (ConnectTime (5f, 1));
		} else {
			MyDebug.Log ("请先同意用户使用协议");
			TipsManagerScript.getInstance ().setTips ("请先同意用户使用协议");
		}

		tapCount += 1;
		Invoke ("resetClickNum", 10f);
	}

	public void youkeLogin(){
		watingPanel.GetComponentInChildren<Text> ().text = "进入游戏中";
		watingPanel.SetActive (true);

		CustomSocket.getInstance ().sendMsg (new LoginRequest (null));
	}

	bool connectRetruen = false;

  
	IEnumerator ConnectTime (float time, byte type)
	{
		connectRetruen = false;
		yield return new WaitForSeconds (time);
		if (!connectRetruen) {//超过5秒还没连接成功显示失败

			if (watingPanel != null) {
				watingPanel.SetActive (false);
			}
		}
	}

	public void doLogin ()
	{
		if (loginText.isActiveAndEnabled) {
			CustomSocket.getInstance ().sendMsg (new LoginRequest (loginText.text, loginText.text));
			return;
		}
        
		if (PlayerPrefs.HasKey ("loginInfo")) {
			string msg = PlayerPrefs.GetString ("loginInfo");
			CustomSocket.getInstance ().sendMsg (new LoginRequest (msg));
		} else {
			#if UNITY_EDITOR
			//用于测试 不用微信登录
			CustomSocket.getInstance ().sendMsg (new LoginRequest (null));
			#else
        	GlobalDataScript.getInstance ().wechatOperate.login ();
			#endif
		}
	}


	public void LoginCallBack (ClientResponse response)
	{
		
		if (response.status == 1) {

			if (GlobalDataScript.homePanel != null) {
				GlobalDataScript.homePanel.GetComponent<HomePanelScript> ().removeListener ();
				Destroy (GlobalDataScript.homePanel);
			}


			if (GlobalDataScript.gamePlayPanel != null) {
				if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.NULL)
					GlobalDataScript.gamePlayPanel.GetComponent<MyMahjongScript> ().exitOrDissoliveRoom ();
				else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.PDK)
					GlobalDataScript.gamePlayPanel.GetComponent<MyPDKScript> ().exitOrDissoliveRoom ();
				else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.DN)
					GlobalDataScript.gamePlayPanel.GetComponent<MyDNScript> ().exitOrDissoliveRoom ();
				else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.DZPK)
					GlobalDataScript.gamePlayPanel.GetComponent<MyDZPKScript> ().exitOrDissoliveRoom ();
                else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.AMH)
                    GlobalDataScript.gamePlayPanel.GetComponent<MyDZPKScript>().exitOrDissoliveRoom();
            }


            GlobalDataScript.loginResponseData = JsonMapper.ToObject<AvatarVO> (response.message);
			ChatSocket.getInstance ().sendMsg (new LoginChatRequest (GlobalDataScript.loginResponseData.account.uuid));

			GlobalDataScript.loadTime = GlobalDataScript.GetTimeStamp ();
			panelCreateDialog = Instantiate (Resources.Load ("Prefab/Panel_Home")) as GameObject;
			panelCreateDialog.transform.parent = GlobalDataScript.getInstance ().canvsTransfrom;
			panelCreateDialog.transform.localScale = Vector3.one;
			panelCreateDialog.GetComponent<RectTransform> ().offsetMax = new Vector2 (0f, 0f);
			panelCreateDialog.GetComponent<RectTransform> ().offsetMin = new Vector2 (0f, 0f);
			GlobalDataScript.homePanel = panelCreateDialog;

			GlobalDataScript.homePanel.GetComponent<HomePanelScript> ().initUI ();
			removeListener ();
			Destroy (this);
			Destroy (gameObject);
		}

		if (watingPanel != null) {
			//watingPanel.SetActive(false);
		}

	}

	GameObject Panel_xieyi;

	public void xieyi ()
	{
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		Panel_xieyi = PrefabManage.loadPerfab ("Prefab/Panel_xieyi");



	}

	public void closexieyi ()
	{
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		if (Panel_xieyi != null)
			Panel_xieyi.SetActive (false);


	}

	private void removeListener ()
	{
		SocketEventHandle.getInstance ().LoginCallBack -= LoginCallBack;
		SocketEventHandle.getInstance ().RoomBackResponse -= RoomBackResponse;
	}

	private void RoomBackResponse (ClientResponse response)
	{
		if (GlobalDataScript.homePanel != null) {
			GlobalDataScript.homePanel.GetComponent<HomePanelScript> ().removeListener ();
			Destroy (GlobalDataScript.homePanel);
		}
		GlobalDataScript.reEnterRoomData = JsonMapper.ToObject<RoomCreateVo> (response.message);
		GlobalDataScript.goldType = GlobalDataScript.reEnterRoomData.goldType;

		if (GlobalDataScript.gamePlayPanel != null) {
			if (GlobalDataScript.reEnterRoomData.gameType == (int)GameTypePK.NULL)
				GlobalDataScript.gamePlayPanel.GetComponent<MyMahjongScript> ().exitOrDissoliveRoom ();
			else if (GlobalDataScript.reEnterRoomData.gameType == (int)GameTypePK.PDK)
				GlobalDataScript.gamePlayPanel.GetComponent<MyPDKScript> ().exitOrDissoliveRoom ();
			else if (GlobalDataScript.reEnterRoomData.gameType == (int)GameTypePK.DN)
				GlobalDataScript.gamePlayPanel.GetComponent<MyDNScript> ().exitOrDissoliveRoom ();
			else if (GlobalDataScript.reEnterRoomData.gameType == (int)GameTypePK.DZPK)
				GlobalDataScript.gamePlayPanel.GetComponent<MyDZPKScript> ().exitOrDissoliveRoom ();
            else if (GlobalDataScript.reEnterRoomData.gameType == (int)GameTypePK.AMH)
                GlobalDataScript.gamePlayPanel.GetComponent<MyDZPKScript>().exitOrDissoliveRoom();
        }

		for (int i = 0; i < GlobalDataScript.reEnterRoomData.playerList.Count; i++) {
			AvatarVO itemData =	GlobalDataScript.reEnterRoomData.playerList [i];
			if (itemData.account.openid == GlobalDataScript.loginResponseData.account.openid) {
				GlobalDataScript.loginResponseData.account.uuid = itemData.account.uuid;
				GlobalDataScript.loginResponseData.account.roomcard = itemData.account.roomcard;
				GlobalDataScript.loginResponseData.account.gold = itemData.account.gold;
				GlobalDataScript.loginResponseData.account.isCheat = itemData.account.isCheat;
				ChatSocket.getInstance ().sendMsg (new LoginChatRequest (GlobalDataScript.loginResponseData.account.uuid));
				break;
			}
		}
		if(GlobalDataScript.reEnterRoomData.gameType == (int)GameTypePK.NULL)
            GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab ("Prefab/Panel_GamePlay");
		else if(GlobalDataScript.reEnterRoomData.gameType == (int)GameTypePK.PDK)
            GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab ("Prefab/Panel_GamePDK");
		else if(GlobalDataScript.reEnterRoomData.gameType == (int)GameTypePK.DN)
            GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab ("Prefab/Panel_GameDN");
		else if(GlobalDataScript.reEnterRoomData.gameType == (int)GameTypePK.DZPK)
            GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab ("Prefab/Panel_GameDZPK");
        else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.AMH)
            GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_GameAMH");

        removeListener ();
		Destroy (this);
		Destroy (gameObject);
	
	}


	private void resetClickNum ()
	{
		tapCount = 0;
	}


}
