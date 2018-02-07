using UnityEngine;
using System.Collections;
using cn.sharesdk.unity3d;
using AssemblyCSharp;
using System.IO;
using UnityEngine.UI;
using System;
using LitJson;

/**
 * 微信操作
 */ 
public class WechatOperateScript : MonoBehaviour {
	public ShareSDK shareSdk;
	private string picPath;
    private Text debug_Text = null;
	//
	void Start () {
        debug_Text = GameObject.Find("Debug_Text").GetComponent<Text>();
        debug_Text.text = (" 要开始微信登录了 ");
        if (shareSdk != null) {
			shareSdk.showUserHandler = getUserInforCallback;
			shareSdk.shareHandler = onShareCallBack;
			shareSdk.authHandler = authResultHandler;
		}
	}
	

	void Update () {
	
	}



	public void cancelAuth()
	{
		shareSdk.CancelAuthorize (PlatformType.WeChat);
	}
	/**
	 * 登录，提供给button使用
	 * 
	 */ 
	public void login(){
		
	    shareSdk.GetUserInfo(PlatformType.WeChat);
		//shareSdk.Authorize (PlatformType.WeChat);


	}

	/**
	 * 获取微信个人信息成功回调,登录
	 *
	 */ 
	public void authResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable data){
        //TipsManagerScript.getInstance ().setTips ("获取个人信息成功");
        debug_Text.text = ("成功回调微信信息，开始登录");
		//TipsManagerScript.getInstance ().setTipsBig (data.toJson());
		#if UNITY_IOS
			shareSdk.GetUserInfo(PlatformType.WeChat);
			return;
		#else
			data = shareSdk.GetAuthInfo (PlatformType.WeChat);
		#endif
		if (data != null) {
			
			LoginVo loginvo = new LoginVo ();
			try {
				
				MyDebug.Log (data.toJson ());
				//TipsManagerScript.getInstance ().setTipsBig (data.toJson());

				loginvo.openId = (string)data ["openid"];
				loginvo.nickName = (string)data ["nickname"];
				loginvo.headIcon = (string)data ["icon"];
				loginvo.unionid = (string)data ["unionid"];
				loginvo.province = (string)data ["province"];
				loginvo.city = (string)data ["city"];
				string sex = data ["gender"].ToString ();
				loginvo.sex = int.Parse (sex);
				loginvo.IP = GlobalDataScript.getInstance ().getIpAddress ();
				String msg = JsonMapper.ToJson (loginvo);

				PlayerPrefs.SetString("loginInfo", msg);

				CustomSocket.getInstance ().sendMsg (new LoginRequest (msg));

				/*
				GlobalDataScript.loginVo = loginvo;
				GlobalDataScript.loginResponseData = new AvatarVO ();
				GlobalDataScript.loginResponseData.account = new Account ();
				GlobalDataScript.loginResponseData.account.city = loginvo.city;
				GlobalDataScript.loginResponseData.account.openid = loginvo.openId;
				MyDebug.Log(" loginvo.nickName:"+loginvo.nickName);
				GlobalDataScript.loginResponseData.account.nickname = loginvo.nickName;
				GlobalDataScript.loginResponseData.account.headicon = loginvo.headIcon;
				GlobalDataScript.loginResponseData.account.unionid = loginvo.city;
				GlobalDataScript.loginResponseData.account.sex = loginvo.sex;
				GlobalDataScript.loginResponseData.IP = loginvo.IP;

				//加载头像
				if(GlobalDataScript.loginVo.sex == 2){
					GlobalDataScript.getInstance().headSprite = Resources.Load ("xianlai/public_ui/head_img_female", typeof(Sprite)) as Sprite;
				}else {
					GlobalDataScript.getInstance().headSprite = Resources.Load ("xianlai/public_ui/head_img_male", typeof(Sprite)) as Sprite;
				}
				*/
			} catch (Exception e) {
				MyDebug.Log ("微信接口有变动！" + e.Message);
				TipsManagerScript.getInstance ().setTips ("微信接口有变动！" + e.Message);
				return;
			}
		} else {
			TipsManagerScript.getInstance ().setTips ("微信登录失败");
		}




	}


	/**
	 * 获取微信个人信息成功回调,登录
	 *
	 */ 
	public void getUserInforCallback(int reqID, ResponseState state, PlatformType type, Hashtable data){
        //TipsManagerScript.getInstance ().setTips ("获取个人信息成功");
        debug_Text.text = ("获取个人信息成功");
		//shareSdk.CancelAuthorize (PlatformType.WeChat);
		if (data != null) {
			MyDebug.Log (data.toJson ());
			LoginVo loginvo = new LoginVo ();
			try {

				loginvo.openId = (string)data ["openid"];
				loginvo.nickName = (string)data ["nickname"];
				loginvo.headIcon = (string)data ["headimgurl"];
				loginvo.unionid = (string)data ["unionid"];
				loginvo.province = (string)data ["province"];
				loginvo.city = (string)data ["city"];
				string sex = data ["sex"].ToString ();
				loginvo.sex = int.Parse (sex);
				loginvo.IP = GlobalDataScript.getInstance ().getIpAddress ();
				String msg = JsonMapper.ToJson (loginvo);

				PlayerPrefs.SetString("loginInfo", msg);

				CustomSocket.getInstance ().sendMsg (new LoginRequest (msg));


				/*
				GlobalDataScript.loginVo = loginvo;
				GlobalDataScript.loginResponseData = new AvatarVO ();
				GlobalDataScript.loginResponseData.account = new Account ();
				GlobalDataScript.loginResponseData.account.city = loginvo.city;
				GlobalDataScript.loginResponseData.account.openid = loginvo.openId;
				MyDebug.Log(" loginvo.nickName:"+loginvo.nickName);
				GlobalDataScript.loginResponseData.account.nickname = loginvo.nickName;
				GlobalDataScript.loginResponseData.account.headicon = loginvo.headIcon;
				GlobalDataScript.loginResponseData.account.unionid = loginvo.city;
				GlobalDataScript.loginResponseData.account.sex = loginvo.sex;
				GlobalDataScript.loginResponseData.IP = loginvo.IP;
				*/

			} catch (Exception e) {
				MyDebug.Log ("微信接口有变动！" + e.Message);
				TipsManagerScript.getInstance ().setTips ("请先打开你的微信客户端");
				return;
			}
		} else {
			TipsManagerScript.getInstance ().setTips ("微信登录失败");
		}


	}


	/***
	 * 分享战绩成功回调
	 */ 
	public void onShareCallBack(int reqID,ResponseState state,PlatformType type,Hashtable result){
		if (state == ResponseState.Success) {
			TipsManagerScript.getInstance ().setTips ("分享成功");
			if (GlobalDataScript.isShare) {
				CustomSocket.getInstance ().sendMsg (new ShareRequest ());
				GlobalDataScript.isShare = false;
			}

		} else if(state == ResponseState.Fail){
			MyDebug.Log ("shar fail :" + result ["error_msg"]);
		}
	}

	/**
	 * 分享战绩、战绩
	 */ 
	public void shareAchievementToWeChat(PlatformType platformType){
		StartCoroutine(GetCapture(platformType));
	}

	/**
	 * 执行分享到朋友圈的操作
	 */ 
	private void shareAchievement(PlatformType platformType){
		ShareContent customizeShareParams = new ShareContent();
		customizeShareParams.SetText("");
		customizeShareParams.SetImagePath(picPath);
		customizeShareParams.SetShareType(ContentType.Image);
		customizeShareParams.SetObjectID("");
		customizeShareParams.SetShareContentCustomize(platformType, customizeShareParams);
		shareSdk.ShareContent (platformType, customizeShareParams);
	}

	/**
	 * 截屏
	 * 
	 * 
	 */ 
	private IEnumerator GetCapture(PlatformType platformType)
	{
		yield return new WaitForEndOfFrame();
		if(Application.platform==RuntimePlatform.Android || Application.platform==RuntimePlatform.IPhonePlayer)  

			picPath=Application.persistentDataPath;  

		else if(Application.platform==RuntimePlatform.WindowsPlayer)  

			picPath=Application.dataPath;  

		else if(Application.platform==RuntimePlatform.WindowsEditor)

		{  
			picPath=Application.dataPath;  
			picPath= picPath.Replace("/Assets",null);  
		}   

		picPath = picPath + "/screencapture.png";

		MyDebug.Log ("picPath:" + picPath);

		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(width,height,TextureFormat.RGB24,false);
		tex.ReadPixels(new Rect(0,0,width,height),0,0,true);
		tex.Apply ();
		byte[] imagebytes = tex.EncodeToPNG();//转化为png图
		tex.Compress(false);//对屏幕缓存进行压缩
		MyDebug.Log("imagebytes:"+imagebytes);
		if (File.Exists (picPath)) {
			File.Delete (picPath);
		}
		File.WriteAllBytes(picPath,imagebytes);//存储png图
		Destroy(tex);
		shareAchievement(platformType);
	}




	public void inviteFriend(){
		if(GlobalDataScript.roomVo != null){
			RoomCreateVo roomvo = GlobalDataScript.roomVo;
			GlobalDataScript.totalTimes = roomvo.roundNumber;
			GlobalDataScript.surplusTimes = roomvo.roundNumber;
			string str="" ;
			if (roomvo.gameType == 0) {
				str += "【小麻将 大战场】";
			} else if (roomvo.gameType == 1) {
				
			} else if (roomvo.gameType == 3) {
				str += "【累了困了就牛牛】";
			} else if (roomvo.gameType == 4) {
				str += "【德州扑克】";
			}

			if (roomvo.hong) {
				str += "红中麻将,";
			} else {
				if (roomvo.roomType == 1) {
					str += "转转麻将,";
				} else if (roomvo.roomType == 2) {
					str += "划水麻将,";
				} else if (roomvo.roomType == 3) {
					str += "长沙麻将,";
				} else if (roomvo.roomType == 4) {
					str += "广东麻将,";
				} else if (roomvo.roomType == 5) {
					str += "赣州冲关,";
				} else if (roomvo.roomType == 6) {
					str += "瑞金麻将,";
				} else if (roomvo.gameType == 1) {
					str += "跑得快,";
				} else if (roomvo.gameType == 3) {
					str += "欢乐斗牛,";
				}else if (roomvo.gameType == 10) {
					str += "潮汕麻将,";
				}
            }

			if (roomvo.gameType == 0) {
				str += "一缺三,";
			} else if (roomvo.gameType == 1) {
				str += "一缺二,";
			} else if (roomvo.gameType == 3) {
				str += "一缺四,";
			} else if (roomvo.gameType == 4) {
				str += "一缺五,";
			}

			if (!roomvo.goldType) {
				str += roomvo.roundNumber+"局,";
			}

			if (roomvo.roomType == 5) {
				if (roomvo.shangxiaFanType == 1) {
					str += "上下翻埋地雷,";
				} else {
					str += "上下左右翻精,";
				}

				if (roomvo.diFen == 1) {
					str += "底分1分,";
				} else {
					str += "底分2分,";
				}

				if (roomvo.tongZhuang) {
					str += "通庄,";
				} else {
					str += "分庄闲,";
				}

				if (roomvo.pingHu == 1) {
					str += "可平胡,";
				} else if (roomvo.pingHu == 2) {
					str += "有精点炮不能平胡,";
				} else {
					str += "只可精钓,";
				}
			} else if (roomvo.roomType == 6) {
				if (roomvo.keDianPao)
					str += "可点炮胡,";

				if (roomvo.diFen == 1) {
					str += "底分1分,";
				} else if (roomvo.diFen == 2) {
					str += "底分2分,";
				} else {
					str += "底分5分,";
				}

				if (roomvo.tongZhuang) {
					str += "通庄,";
				} else {
					str += "分庄闲,";
				}
				if (roomvo.lunZhuang) {
					str += "轮庄,";
				} else {
					str += "霸王庄,";
				}
			} else if (roomvo.roomType == 10) {

				if (roomvo.gangHu)
					str += "可接炮胡,";

				if (roomvo.genZhuang) {
					str += "跟庄,";
				}

				if (roomvo.pengpengHu) {
					str += "碰碰胡2倍,";
				}

				if (roomvo.qiDui) {
					str += "七对2倍,";
				}

				if (roomvo.qiangGangHu) {
					str += "抢杠胡2倍,";
				}

				if (roomvo.hunYiSe) {
					str += "混一色2倍,";
				}

				if (roomvo.qingYiSe) {
					str += "清一色2倍,";
				}

				if (roomvo.gangShangKaiHua) {
					str += "杠上开花2倍,";
				}

				if (roomvo.haoHuaQiDui) {
					str += "豪华7对4倍,";
				}

				if (roomvo.shiSanYao) {
					str += "十三幺10倍,";
				}

				if (roomvo.tianDiHu) {
					str += "天地胡10倍,";
				}

				if (roomvo.shuangHaoHua) {
					str += "双豪华6倍,";
				}

				if (roomvo.sanHaoHua) {
					str += "三豪华8倍,";
				}

				if (roomvo.shiBaLuoHan) {
					str += "十八罗汉10倍,";
				}

				if (roomvo.xiaoSanYuan) {
					str += "小三元4倍,";
				}

				if (roomvo.xiaoSanYuan) {
					str += "小四喜4倍,";
				}

				if (roomvo.daSanYuan) {
					str += "大三元6倍,";
				}

				if (roomvo.daSiXi) {
					str += "大四喜6倍,";
				}

				if (roomvo.huaYaoJiu) {
					str += "花幺九6倍,";
				}

				if (roomvo.fengDing == 5) {
					str += "封顶5倍,";
				} else if(roomvo.fengDing == 10) {
					str += "封顶10倍,";
				}else {
					str += "不封顶,";
				}



				if (roomvo.gui == 0)
					str += "无鬼,";
				else if (roomvo.gui == 1)
					str += "白板做鬼,";
				else if (roomvo.gui == 2)
					str += "翻鬼,";
				else if (roomvo.gui == 3)
					str += "翻鬼(双鬼),";

				if (roomvo.wuGuiX2)
					str += "无鬼加翻,";

				if (roomvo.ma > 0) {
					str += "抓码数" + roomvo.ma + "个,";
					if (roomvo.maGenDifen)
						str += "马跟底分,";
					if (roomvo.maGenGang)
						str += "马跟杠,";
				}

				if (roomvo.jiejiegao)
					str += "节节高,";
			} else if (roomvo.gameType == 1) {
				if (GlobalDataScript.roomVo.zhang16)
					str += "每人16张,";
				else
					str += "每人15张,";

				if (GlobalDataScript.roomVo.showPai)
					str += "显示牌,";
				else
					str += "不显示牌,";

				if (GlobalDataScript.roomVo.xian3)
					str += "首轮先出黑桃3,";
			} else if (roomvo.gameType == 3) {
				if(!roomvo.goldType)
					str += roomvo.AA ? "AA制," : "房主付费,";
				str += roomvo.qiang ? "抢庄," : "轮庄,";
				str += "底分" + roomvo.diFen + "分,";
			}else if(roomvo.gameType == 4){
				str += "初始分:" + roomvo.initFen_dzpk;
			} else {
				if (roomvo.roomType == 4)
				{
					if (roomvo.gangHu)
					{
						str += "可抢杠胡,";
					}
				}
				else {
					if (roomvo.ziMo == 1)
					{
						str += "只能自摸,";
					}
					else {
						str += "可抢杠胡,";
					}
				}

				if (roomvo.sevenDouble)
				{
					str += "可胡七对,";
				}

				if (roomvo.addWordCard)
				{
					str += "有风牌,";
				}
				else {                
					str += "无风牌,";			
				}

				if (roomvo.gui==0)
					str += "无鬼,";
				else if (roomvo.gui == 1)
					str += "白板做鬼,";
				else if (roomvo.gui == 2)
					str += "翻鬼,";
				else if (roomvo.gui == 3)
					str += "翻鬼,双鬼,";

				if (roomvo.gangHuQuanBao)
					str += "杠爆全包,";

				if (roomvo.wuGuiX2)
					str += "无鬼加翻,";

				if (roomvo.xiaYu > 0) {
					str += "下鱼"+roomvo.xiaYu+"条,";
				}

				if (roomvo.ma > 0) {
					str += "抓"+roomvo.ma+"个码,";

					if (roomvo.maGenDifen)
						str += "马跟底分,";
					if (roomvo.maGenGang)
						str += "马跟杠,";
				}
				if (roomvo.magnification > 0) {
					str += "倍率"+roomvo.magnification;
				}
			}
			if (roomvo.gameType == 0) {
				str += "兵贵神速,痛快大战300局！";
			} else if (roomvo.gameType == 1) {
				str += "有胆，你就来！";
			} else if (roomvo.gameType == 3) {
				str += "决战千里,牛气冲天！";
			}else if (roomvo.gameType == 4) {
				str += "有胆，你就来！";
			}

			string title;
			if (roomvo.goldType) {
				title ="【" + Application.productName + "】" + "  房间号：训练场";
			} else {
				title ="【" + Application.productName + "】" + "  房间号："+roomvo.roomId;
			}
			ShareContent customizeShareParams = new ShareContent();
			customizeShareParams.SetTitle(title);
			customizeShareParams.SetText (str);
            //配置下载地址
			customizeShareParams.SetUrl(APIS.baseUrl + "/download/index.html");
            //配置分享logo
			customizeShareParams.SetImageUrl(APIS.baseUrl + "/download/logo.png");
			customizeShareParams.SetShareType(ContentType.Webpage);//ContentType.Webpage
			customizeShareParams.SetObjectID("");
			shareSdk.ShowShareContentEditor(PlatformType.WeChat, customizeShareParams);
		}
	}


	public void shareHaoYou(PlatformType platformtype){
		string title = Application.productName + "";
		string str = "深空娱乐——游戏让生活更美好！";
		ShareContent customizeShareParams = new ShareContent();
		customizeShareParams.SetTitle(title);
		customizeShareParams.SetText (str);
		//配置下载地址
		customizeShareParams.SetUrl(APIS.baseUrl + "/download/index.html");
		//配置分享logo
		customizeShareParams.SetImageUrl(APIS.baseUrl + "/download/logo.png");
		customizeShareParams.SetShareType(ContentType.Webpage);
		customizeShareParams.SetObjectID("");
		shareSdk.ShowShareContentEditor(platformtype, customizeShareParams);
	}


	private void testLogin(){
		CustomSocket.getInstance().sendMsg(new LoginRequest(null));
	}

}
