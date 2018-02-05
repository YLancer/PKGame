using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using AssemblyCSharp;

public class DNOverScript : MonoBehaviour {
	public Text roomTxt;
	public Text jushuTxt;
	public Text timeTxt;
	public List<GameObject> players;
	private GameObject dnGO;

	// Use this for initialization
	void Start () {
		if (GlobalDataScript.loginResponseData.fen < 0) {
			gameObject.GetComponent<Image> ().sprite = Resources.Load ("xianlai/end_report/overFail", typeof(Sprite)) as Sprite;
			SoundCtrl.getInstance ().playSound_DN (false);
		} else {
			SoundCtrl.getInstance ().playSound_DN (true);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void setUI(GameObject gob){
		timeTxt.text=DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
		roomTxt.text = "房间号 " + GlobalDataScript.roomVo.roomId;
		if (GlobalDataScript.goldType) {
			jushuTxt.text = "局数：" + "" + GlobalDataScript.playCountForGoldType + "局";
		} else {
			jushuTxt.text = "局数：" + (GlobalDataScript.totalTimes - GlobalDataScript.surplusTimes) + "/" + GlobalDataScript.totalTimes;
		}
		if (GlobalDataScript.hupaiResponseVo != null && GlobalDataScript.hupaiResponseVo.avatarList.Count > 0) {
			for (int i = 0; i < GlobalDataScript.hupaiResponseVo.avatarList.Count; i++) {
				HupaiResponseItem itemdata = GlobalDataScript.hupaiResponseVo.avatarList [i];
				GameObject go = players [i];
				go.SetActive (true);
				go.GetComponent<PlayerContent_DN> ().setUI (itemdata);
			}
		}
		dnGO = gob;
	}
	public void CloseDialog(){
		if(dnGO != null)
			dnGO.GetComponent<MyDNScript> ().setAllPlayerDNImgVisbleToFalse ();
		Destroy (this);
		Destroy (gameObject);
	}
}
