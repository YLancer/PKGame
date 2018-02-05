using UnityEngine;
using System.Collections;
using cn.sharesdk.unity3d;
using AssemblyCSharp;

public class ShareScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void shareHaoYou(){
		GlobalDataScript.getInstance ().wechatOperate.shareHaoYou (PlatformType.WeChat);
		close ();
	}

	public void sharePengYouQuan(){
		GlobalDataScript.getInstance ().wechatOperate.shareHaoYou (PlatformType.WeChatMoments);
		GlobalDataScript.isShare = true;
		close ();
	}

	public void close()
	{
		Destroy (this);
		Destroy (gameObject);
	}
}

