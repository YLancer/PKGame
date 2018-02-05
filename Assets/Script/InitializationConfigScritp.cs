using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using LitJson;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine.SceneManagement;
using System;
using System.Net.Sockets;
using System.Net;

public class InitializationConfigScritp : MonoBehaviour {
	
	int num = 0;
	bool hasPaused   = false;
	void Start () {

		try{
			IPAddress[] IPs = Dns.GetHostAddresses(APIS.socketUrl);
			APIS.socketUrl = IPs [0].ToString ();

			IPAddress[] IPs2 = Dns.GetHostAddresses(APIS.chatSocketUrl);
			APIS.chatSocketUrl = IPs2 [0].ToString ();
		}catch(Exception ex){
			MyDebug.Log (ex.Message);
		}
        MicroPhoneInput.getInstance ();
		GlobalDataScript.getInstance ();
		//CustomSocket.getInstance().Connect();
		//ChatSocket.getInstance ();
		TipsManagerScript.getInstance ().parent = gameObject.transform;
		SoundCtrl.getInstance ();

        //检查更新
		//UpdateScript update = new UpdateScript ();
		//StartCoroutine (update.updateCheck ());

		//ServiceErrorListener seriveError = new ServiceErrorListener();
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		heartbeatThread(); 

		StartCoroutine(LoadGD());

		//预加载场景
		StartCoroutine (LoadRes());
        
	}

	IEnumerator LoadRes(){
		yield return new WaitForEndOfFrame();//加上这么一句就可以先显示加载画面然后再进行加载

		SoundCtrl.getInstance().playBGM(2);
	}

   void	Awake(){
		SocketEventHandle.getInstance().disConnetNotice += disConnetNotice;
		SocketEventHandle.getInstance ().hostUpdateDrawResponse += hostUpdateDrawResponse;
		SocketEventHandle.getInstance ().otherTeleLogin += otherTeleLogin;
		SocketEventHandle.getInstance().cardChangeNotice += cardChangeNotice;

		if (PlayerPrefs.HasKey ("yinyueVolume")) {
			GlobalDataScript.yinyueVolume = PlayerPrefs.GetFloat ("yinyueVolume");
			GlobalDataScript.yinxiaoVolume = PlayerPrefs.GetFloat ("yinxiaoVolume");
		}

        if (PlayerPrefs.HasKey("isYueYu")) {
            int yueyu = PlayerPrefs.GetInt("isYueYu");
            if (yueyu > 0)
                GlobalDataScript.isYueYu = true;
            else
                GlobalDataScript.isYueYu = false;
        }
    }

	Texture2D texture2D; 

	/// <summary>
	/// 加载广告
	/// </summary>
	/// <returns>The image.</returns>
	private IEnumerator LoadGD() { 

		if (GlobalDataScript.isGdOk) {
			yield break;
		}

		//开始下载图片
		WWW www = new WWW(APIS.GD_URL);
		yield return www;
		if (www != null) {
			Texture2D texture2D = www.texture;
			byte[] bytes = texture2D.EncodeToPNG ();
			//将图片赋给场景上的Sprite
			Sprite tempSp = Sprite.Create (texture2D, new Rect (0, 0, texture2D.width, texture2D.height), new Vector2 (0, 0));
			GlobalDataScript.gdSprite = tempSp;
			GlobalDataScript.isGdOk = true;
		} else {
			MyDebug.Log ("没有加载到图片");
		}
	}

    void FixedUpdate()
    {

    }


    private void  disConnetNotice(){
		if (GlobalDataScript.isonLoginPage) {
			//CustomSocket.getInstance ().Connect();
			//ChatSocket.getInstance ().Connect ();
		} else {
			cleaListener ();

			CustomSocket.getInstance ().closeSocket ();

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
                    GlobalDataScript.gamePlayPanel.GetComponent<MyDZPKScript>().exitOrDissoliveRoom();
                else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.AMH)
                    GlobalDataScript.gamePlayPanel.GetComponent<MyDZPKScript>().exitOrDissoliveRoom();
            }

            PrefabManage.loadPerfab ("Prefab/Panel_Start");
            //SceneManager.LoadScene(0);//返回登录页面
		}
	}


	//房卡变化处理
	private void cardChangeNotice(ClientResponse response)
	{
		string msg = "";
		if (GlobalDataScript.goldType) {
			int oldCout = GlobalDataScript.loginResponseData.account.gold;
			GlobalDataScript.loginResponseData.account.gold = int.Parse(response.message);
//			goldText.text = GlobalDataScript.loginResponseData.account.gold + "";
			int count = GlobalDataScript.loginResponseData.account.gold - oldCout;
			msg ="金币"  + (count>=0? "+" + count: count + "");
		} else {
			int oldCout = GlobalDataScript.loginResponseData.account.roomcard;

			if (GlobalDataScript.homePanel != null) {
				GlobalDataScript.homePanel.GetComponent<HomePanelScript> ().cardCountText.text = response.message;
			}

			GlobalDataScript.loginResponseData.account.roomcard = int.Parse(response.message);

			int count = int.Parse (response.message) - oldCout;
			msg ="房卡"  + (count>=0? "+" + count: count + "")+"个";
		}

		//TipsManagerScript.getInstance ().setTips (msg);
	}

//	private void cardChangeNotice2(ClientResponse response)
//	{
//		string msg = "";
//		int count = 0;
//
//		RoomCardVO roomcardvo = JsonMapper.ToObject<RoomCardVO> (response.message);
//
//		if (roomcardvo.type==2) {
//			int oldCout = GlobalDataScript.loginResponseData.account.gold;
//			GlobalDataScript.loginResponseData.account.gold = roomcardvo.cardNum;
//			count = GlobalDataScript.loginResponseData.account.gold - oldCout;
//			msg ="金币"  + (count>=0? "+" + count: count + "");
//			if (GlobalDataScript.homePanel != null) {
//				GlobalDataScript.homePanel.GetComponent<HomePanelScript> ().goldText.text = roomcardvo.cardNum.ToString();
//			}
//		} else {
//			int oldCout = GlobalDataScript.loginResponseData.account.roomcard;
//			GlobalDataScript.loginResponseData.account.roomcard = roomcardvo.cardNum;
//			count = GlobalDataScript.loginResponseData.account.roomcard - oldCout;
//			msg ="钻石"  + (count>=0? "+" + count: count + "")+"个";
//			if (GlobalDataScript.homePanel != null) {
//				GlobalDataScript.homePanel.GetComponent<HomePanelScript> ().cardCountText.text = roomcardvo.cardNum.ToString();
//			}
//		}
//
//
//		if(count != 0)
//			TipsManagerScript.getInstance ().setTips (msg);
//	}

	private void otherTeleLogin(ClientResponse response){
		GlobalDataScript.isAutoLogin = false;
		TipsManagerScript.getInstance ().setTips ("你的账号在其他设备登录");
		disConnetNotice ();
	}



	private void cleaListener(){
		/*
		if (SocketEventHandle.getInstance ().LoginCallBack != null) {
			SocketEventHandle.getInstance ().LoginCallBack = null;
		}
*/
		if (SocketEventHandle.getInstance ().CreateRoomCallBack != null) {
			SocketEventHandle.getInstance ().CreateRoomCallBack = null;
		}

		if (SocketEventHandle.getInstance ().JoinRoomCallBack != null) {
			SocketEventHandle.getInstance ().JoinRoomCallBack = null;
		}

		if (SocketEventHandle.getInstance ().StartGameNotice != null) {
			SocketEventHandle.getInstance ().StartGameNotice = null;
		}

		if (SocketEventHandle.getInstance ().pickCardCallBack != null) {
			SocketEventHandle.getInstance ().pickCardCallBack = null;
		}

		if (SocketEventHandle.getInstance ().otherPickCardCallBack != null) {
			SocketEventHandle.getInstance ().otherPickCardCallBack = null;
		}

		if (SocketEventHandle.getInstance ().putOutCardCallBack != null) {
			SocketEventHandle.getInstance ().putOutCardCallBack = null;
		}

		if (SocketEventHandle.getInstance ().PengCardCallBack != null) {
			SocketEventHandle.getInstance ().PengCardCallBack = null;
		}

		if (SocketEventHandle.getInstance ().ChiCardCallBack != null) {
			SocketEventHandle.getInstance ().ChiCardCallBack = null;
		}

		if (SocketEventHandle.getInstance ().GangCardCallBack != null) {
			SocketEventHandle.getInstance ().GangCardCallBack = null;
		}

		if (SocketEventHandle.getInstance ().HupaiCallBack != null) {
			SocketEventHandle.getInstance ().HupaiCallBack = null;
		}

	
		if (SocketEventHandle.getInstance ().gangCardNotice != null) {
			SocketEventHandle.getInstance ().gangCardNotice = null;
		}



		if (SocketEventHandle.getInstance ().btnActionShow != null) {
			SocketEventHandle.getInstance ().btnActionShow = null;
		}

		if (SocketEventHandle.getInstance ().outRoomCallback != null) {
			SocketEventHandle.getInstance ().outRoomCallback = null;
		}

		if (SocketEventHandle.getInstance ().dissoliveRoomResponse != null) {
			SocketEventHandle.getInstance ().dissoliveRoomResponse = null;
		}

		if (SocketEventHandle.getInstance ().gameReadyNotice != null) {
			SocketEventHandle.getInstance ().gameReadyNotice = null;
		}

	

		if (SocketEventHandle.getInstance ().messageBoxNotice != null) {
			SocketEventHandle.getInstance ().messageBoxNotice = null;
		}



		if (SocketEventHandle.getInstance ().backLoginNotice != null) {
			SocketEventHandle.getInstance ().backLoginNotice = null;
		}
		/*
		if (SocketEventHandle.getInstance ().RoomBackResponse != null) {
			SocketEventHandle.getInstance ().RoomBackResponse = null;
		}
		*/

		if (SocketEventHandle.getInstance ().cardChangeNotice != null) {
			SocketEventHandle.getInstance ().cardChangeNotice = null;
		}



		if (SocketEventHandle.getInstance ().offlineNotice != null) {
			SocketEventHandle.getInstance ().offlineNotice = null;
		}

		if (SocketEventHandle.getInstance ().onlineNotice != null) {
			SocketEventHandle.getInstance ().onlineNotice = null;
		}

		if (SocketEventHandle.getInstance ().giftResponse != null) {
			SocketEventHandle.getInstance ().giftResponse = null;
		}

		if (SocketEventHandle.getInstance ().returnGameResponse != null) {
			SocketEventHandle.getInstance ().returnGameResponse = null;
		}

		if (SocketEventHandle.getInstance ().gameFollowBanderNotice != null) {
			SocketEventHandle.getInstance ().gameFollowBanderNotice = null;
		}

		if (SocketEventHandle.getInstance ().contactInfoResponse != null) {
			SocketEventHandle.getInstance ().contactInfoResponse = null;
		}

		if (SocketEventHandle.getInstance ().zhanjiResponse != null) {
			SocketEventHandle.getInstance ().zhanjiResponse = null;
		}



		if (SocketEventHandle.getInstance ().zhanjiDetailResponse != null) {
			SocketEventHandle.getInstance ().zhanjiDetailResponse = null;
		}

		if (SocketEventHandle.getInstance ().gameBackPlayResponse != null) {
			SocketEventHandle.getInstance ().gameBackPlayResponse = null;
		}

		//20170301 by xief
		if (SocketEventHandle.getInstance ().otherTeleLogin != null) {
			SocketEventHandle.getInstance ().otherTeleLogin = null;
		}

		if (SocketEventHandle.getInstance ().cardChangeNotice != null) {
			SocketEventHandle.getInstance().cardChangeNotice = null;
		}

	}

	System.Timers.Timer t;
	private  void heartbeatTimer(){
		t = new System.Timers.Timer(1000);   //实例化Timer类，设置间隔时间为10000毫秒；   
		t.Elapsed += new System.Timers.ElapsedEventHandler(doSendHeartbeat); //到达时间的时候执行事件；   
		t.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
		t.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；   

	}

	private void heartbeatThread(){
		Thread thread = new Thread (sendHeartbeat);
		thread.IsBackground = true;
		thread.Start();
	}


	private static void sendHeartbeat(){
		if (CustomSocket.getInstance ().isConnected) {
			CustomSocket.getInstance ().sendHeadData ();
			ChatSocket.getInstance ().sendHeadData ();
		}
		Thread.Sleep (20000);
		sendHeartbeat ();
	}

	public  void doSendHeartbeat( object source, System.Timers.ElapsedEventArgs e){
		CustomSocket.getInstance ().sendHeadData ();
		/*
		bool flag = 
		if (!flag) {
			if (t != null) {
				t.Stop ();
			}
		}
		*/
	}


	private void hostUpdateDrawResponse(ClientResponse response){
		int giftTimes =int.Parse(response.message);
		GlobalDataScript.loginResponseData.account.prizecount = giftTimes;
		if(CommonEvent.getInstance().prizeCountChange !=null){
			CommonEvent.getInstance ().prizeCountChange();
		}
	}

}
