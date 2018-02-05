using System;
using LitJson;
using System.Collections;
using UnityEngine;

namespace AssemblyCSharp
{
	public class ShareRequest:ClientRequest
	{
		//微信分享送房卡
		public ShareRequest (){
			headCode = APIS.WECHAT_SHARE;
			messageContent =  "";
		}
	}
}

