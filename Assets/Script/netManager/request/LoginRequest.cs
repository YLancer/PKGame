using System;
using LitJson;
using System.Collections;
using UnityEngine;

namespace AssemblyCSharp
{
	public class LoginRequest:ClientRequest
	{
		
		public LoginRequest (string data)
		{
			MyDebug.Log ("----------------4------------------");
			headCode = APIS.LOGIN_REQUEST;

			LoginVo loginvo = new LoginVo ();

			if (data == null) {
				
				System.Random ran = new System.Random ();
				string str = ran.Next (100, 1000) + "for" + ran.Next (2000, 5000);
				loginvo.openId = "111111";
				MyDebug.Log ("----------------5------------------");

				loginvo.nickName = "111111";
				loginvo.headIcon = "imgico221";
				loginvo.unionid = "12732233";
				loginvo.province = "21sfsd";
				loginvo.city = "afafsdf";
				loginvo.sex = 1;
				loginvo.IP = GlobalDataScript.getInstance ().getIpAddress ();
				data = JsonMapper.ToJson (loginvo);

				PlayerPrefs.SetString ("loginInfo", data);

			}
//			if(PlayerPrefs.HasKey ("loginInfo")){
//				PlayerPrefs.DeleteKey ("loginInfo");
//			}
			loginvo = new LoginVo ();
			loginvo = JsonMapper.ToObject<LoginVo> (data);

			GlobalDataScript.loginVo = loginvo;
			GlobalDataScript.loginResponseData = new AvatarVO ();
			GlobalDataScript.loginResponseData.account = new Account ();
			GlobalDataScript.loginResponseData.account.city = loginvo.city;
			GlobalDataScript.loginResponseData.account.openid = loginvo.openId;
			GlobalDataScript.loginResponseData.account.nickname = loginvo.nickName;
			GlobalDataScript.loginResponseData.account.headicon = loginvo.headIcon;
			GlobalDataScript.loginResponseData.account.unionid = loginvo.city;
			GlobalDataScript.loginResponseData.account.sex = loginvo.sex;
			GlobalDataScript.loginResponseData.IP = loginvo.IP;

			//加载头像
			if (GlobalDataScript.loginVo.sex == 2) {
				GlobalDataScript.getInstance ().headSprite = Resources.Load ("xianlai/public_ui/head_img_female", typeof(Sprite)) as Sprite;
			} else {
				GlobalDataScript.getInstance ().headSprite = Resources.Load ("xianlai/public_ui/head_img_male", typeof(Sprite)) as Sprite;
			}

			MyDebug.Log ("----------------6------------------" + messageContent);
			messageContent = data;

		}


		public LoginRequest (string openid, string nickname)
		{
			MyDebug.Log ("----------------4------------------");
			headCode = APIS.LOGIN_REQUEST;

			LoginVo loginvo = new LoginVo ();


			System.Random ran = new System.Random ();
			string str = ran.Next (100, 1000) + "for" + ran.Next (2000, 5000);
			loginvo.openId = openid;
			MyDebug.Log ("----------------5------------------");

			loginvo.nickName = nickname;
			loginvo.headIcon = "imgico221";
			loginvo.unionid = openid;
			loginvo.province = "21sfsd";
			loginvo.city = "afafsdf";
			loginvo.sex = 1;
			loginvo.IP = GlobalDataScript.getInstance ().getIpAddress ();
			string data = JsonMapper.ToJson (loginvo);

			GlobalDataScript.loginVo = loginvo;
			GlobalDataScript.loginResponseData = new AvatarVO ();
			GlobalDataScript.loginResponseData.account = new Account ();
			GlobalDataScript.loginResponseData.account.city = loginvo.city;
			GlobalDataScript.loginResponseData.account.openid = loginvo.openId;
			GlobalDataScript.loginResponseData.account.nickname = loginvo.nickName;
			GlobalDataScript.loginResponseData.account.headicon = loginvo.headIcon;
			GlobalDataScript.loginResponseData.account.unionid = loginvo.city;
			GlobalDataScript.loginResponseData.account.sex = loginvo.sex;
			GlobalDataScript.loginResponseData.IP = loginvo.IP;

			//加载头像
			if (GlobalDataScript.loginVo.sex == 2) {
				GlobalDataScript.getInstance ().headSprite = Resources.Load ("xianlai/public_ui/head_img_female", typeof(Sprite)) as Sprite;
			} else {
				GlobalDataScript.getInstance ().headSprite = Resources.Load ("xianlai/public_ui/head_img_male", typeof(Sprite)) as Sprite;
			}

			messageContent = data;

		}

		/**用于重新登录使用**/


		//退出登录
		public LoginRequest ()
		{
			headCode = APIS.QUITE_LOGIN;
			if (GlobalDataScript.loginResponseData != null) {
				messageContent = GlobalDataScript.loginResponseData.account.uuid + "";
			}

		}


	}
}

