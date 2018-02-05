using UnityEngine;
using System.Collections;
using System;

namespace cn.sharesdk.unity3d 
{
	[Serializable]
	public class DevInfoSet

	{
		
		public WeChat wechat;
		public WeChatMoments wechatMoments; 
		public WeChatFavorites wechatFavorites;
		

		#if UNITY_ANDROID
		#elif UNITY_IPHONE				
		public WechatSeries wechatSeries;						//iOS端微信系列, 可直接配置微信三个子平台 		[仅支持iOS端]
												 															
		#endif

	}

	public class DevInfo 
	{	
		public bool Enable = true;
	}

	
	
	[Serializable]
	public class WeChat : DevInfo 
	{	
		#if UNITY_ANDROID
		public string SortId = "5";
		public const int type = (int) PlatformType.WeChat;
		public string AppId = "wx5b49cb766fed5380";
		public string AppSecret = "87f964472e76385bd2543ede7c89e911";
		public bool BypassApproval = false;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChat;
		public string app_id = "wx5b49cb766fed5380";
		public string app_secret = "87f964472e76385bd2543ede7c89e911";
		#endif
	}

	[Serializable]
	public class WeChatMoments : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "6";
		public const int type = (int) PlatformType.WeChatMoments;
		public string AppId = "wx5b49cb766fed5380";
		public string AppSecret = "87f964472e76385bd2543ede7c89e911";
		public bool BypassApproval = false;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChatMoments;
		public string app_id = "wx5b49cb766fed5380";
		public string app_secret = "87f964472e76385bd2543ede7c89e911";
		#endif
	}

	[Serializable]
	public class WeChatFavorites : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "7";
		public const int type = (int) PlatformType.WeChatFavorites;
		public string AppId = "wx5b49cb766fed5380";
		public string AppSecret = "87f964472e76385bd2543ede7c89e911";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChatFavorites;
		public string app_id = "wx5b49cb766fed5380";
		public string app_secret = "87f964472e76385bd2543ede7c89e911";
		#endif
	}


    [Serializable]
    public class WechatSeries : DevInfo
    {
    #if UNITY_ANDROID
		    //for android,please set the configuraion in class "Wechat" ,class "WechatMoments" or class "WechatFavorite"
		    //对于安卓端，请在类Wechat,WechatMoments或WechatFavorite中配置相关信息↑	
    #elif UNITY_IPHONE
            public const int type = (int)PlatformType.WechatPlatform;
            public string app_id = "wx5b49cb766fed5380";
            public string app_secret = "87f964472e76385bd2543ede7c89e911";
    #endif
    }

}
