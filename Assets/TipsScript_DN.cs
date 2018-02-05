using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TipsScript_DN : MonoBehaviour {

	// Use this for initialization
	public Image tipImg;
	public Text clockTxt;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	 public void setTip(int tip){
		if (tip >= 0) {
			tipImg.gameObject.SetActive (true);
			tipImg.sprite = Resources.Load ("dn/tip" + tip, typeof(Sprite)) as Sprite;
		} else {
			tipImg.gameObject.SetActive (false);
		}

	}
	public void setTime(int time = -1){
		if (time >= 0) {
			clockTxt.text = "" + time;
		} else {
			clockTxt.text = "";
		}
	}
}
