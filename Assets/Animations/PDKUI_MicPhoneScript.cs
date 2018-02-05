using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class PDKUI_MicPhoneScript : MonoBehaviour
{
	public float WholeTime=10f;
	public GameObject InputGameObject;
	private Boolean btnDown = false;
	public GameObject circle;
	public MyPDKScript myScript;
	void Start ()
	{
	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate(){
		if (btnDown)
		{
			WholeTime -= Time.deltaTime;
			circle.GetComponent<Slider>().value = WholeTime;
			if (WholeTime <= 0)
			{
				OnPointerUp ();
			}
		}
	}

	public   long GetTimeStamp()
	{
		TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
		return Convert.ToInt64(ts.TotalSeconds);
	}

	public  long time = 0;
	public void OnPointerDown()
	{
		try{
			Debug.Log("-----------down ");
			SoundCtrl.getInstance().playBGM(0);
			if (myScript.avatarList != null && myScript.avatarList.Count >1) {
				time =  GetTimeStamp();

				btnDown = true;
				InputGameObject.SetActive(true);
				MicroPhoneInput.getInstance ().StartRecord (getUserList ());
			}else{
				TipsManagerScript.getInstance ().setTips ("房间里只有你一个人，不能发送语音");
				SoundCtrl.getInstance().playBGM(2);
			}
		}catch{

			TipsManagerScript.getInstance ().setTips ("您的设备不支持录音功能");
		}

	}

	public void OnPointerUp()
	{
		if (btnDown) {
			btnDown = false;
			InputGameObject.SetActive (false);
			WholeTime = 10;
			if (myScript.avatarList != null && myScript.avatarList.Count > 1) {

				long currentTime =  GetTimeStamp();
				if (time != 0) {
					if ((currentTime - time) > 0){


					}else {
						SoundCtrl.getInstance().playBGM(2);
						return;
						TipsManagerScript.getInstance().setTips("录音时间太短哦");
						//TipsManagerScript.getInstance ().setTips ("录音时间不能少于0秒哦");

					}

				}



				MicroPhoneInput.getInstance ().StopRecord ();
				myScript.myselfSoundActionPlay ();
			} else {

			}
		}

		//
		StartCoroutine(DelayedCallback(5));
		//

	}

	private IEnumerator DelayedCallback(float time)
	{
		Debug.Log("***************-----------***********---------+time =" + time);
		time = 5;//这里获取不了声音的时间
		yield return new WaitForSeconds(time);
		// callback();
		SoundCtrl.getInstance().playBGM(2);
	}

	private List<int> getUserList(){
		List<int> userList = new List<int> ();
		for(int i=0;i<myScript.avatarList.Count;i++){
			if (myScript.avatarList [i].account.uuid != GlobalDataScript.loginResponseData.account.uuid) {
				userList.Add (myScript.avatarList[i].account.uuid);
			}
		}
		return userList;
	}
}
