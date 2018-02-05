using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;


public class PlayerContent_DN : MonoBehaviour {

	public Image imgFangZhu;
	public Image imgBanker;
	public Text nameTxt;
	public Text fenTxt;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void setUI(HupaiResponseItem itemData){
		if (itemData.uuid == GlobalDataScript.mainUuid)
			imgBanker.gameObject.SetActive(true);
		else
			imgBanker.gameObject.SetActive(false);
		nameTxt.text = itemData.nickname;
		fenTxt.text = itemData.totalScore.ToString ();
	}
}
