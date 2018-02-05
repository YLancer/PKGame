using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using LitJson;

public class BroadcastScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GlobalDataScript.configTemp = new ConfigVo();
		addListener ();
	}



	// Update is called once per frame
	void Update () {

	}


	private void addListener(){
		SocketEventHandle.getInstance ().gameBroadcastNotice += gameBroadcastNotice;
		SocketEventHandle.getInstance ().gameConfigNotice += gameConfigNotice;
		SocketEventHandle.getInstance ().JoinRoomIPCallBack += joinroomIpTip;
	}

	private void removeListener(){
		SocketEventHandle.getInstance ().gameBroadcastNotice -= gameBroadcastNotice;
		SocketEventHandle.getInstance ().JoinRoomIPCallBack -= joinroomIpTip;
	}


	private void gameConfigNotice(ClientResponse response){
		GlobalDataScript.configTemp = JsonMapper.ToObject<ConfigVo>(response.message);
		#if UNITY_ANDROID
			GlobalDataScript.configTemp.pay = 0;
		#endif

		GlobalDataScript.configTemp.pay = 0;
	}

	private void gameBroadcastNotice(ClientResponse response){
		string noticeString = response.message;
		string[] noticeList = noticeString.Split (new char[1]{ '*' });
		//	List<string> notices = new List<string> ();
		if (noticeList != null)
		{
			GlobalDataScript.noticeMegs = new List<string> ();
			for (int i=0 ;i<noticeList.Length ;i++){
				GlobalDataScript.noticeMegs .Add (noticeList[i]);
			}
			if (CommonEvent.getInstance ().DisplayBroadcast != null) {
				CommonEvent.getInstance ().DisplayBroadcast ();
			}
		}

	}
	private void joinroomIpTip(ClientResponse response){
		//TipsManagerScript.getInstance ().setTips ("该房间有ip相同者，请注意");
		TipsManagerScript.getInstance ().loadDialog ("同IP警告", "该房间有相同IP的玩家，请注意", cancel, cancel);
	}

	public void cancel() 
	{

	}
}
