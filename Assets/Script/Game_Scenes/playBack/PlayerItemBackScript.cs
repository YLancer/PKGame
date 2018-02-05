using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class PlayerItemBackScript : MonoBehaviour {
	public Image headerIcon;
	public Image bankerImg;
	public Text nameText;
	public Image readyImg;
	public Text scoreText;
	public string dir;
	public GameObject HuFlag;
	public GameObject pengEffect;
	public GameObject gangEffect;
	public GameObject huEffect;
	// Use this for initialization
	private PlayerBackVO avatarvo;
	public Image yaobuqiImage;
	public Image qiangImg;
	public Image zhuImg;
	public Image niuImg;


	public Text text_dzpk;
	public GameObject qipaiImg_dzpk;
	public Text avaZhu_dzpk;
	private int avaCurZhu_dzpk = 0;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void setQiang(int qiang){
		if (qiang == 1) {
			qiangImg.gameObject.SetActive (true);
			qiangImg.sprite = Resources.Load("dn/qiang1", typeof(Sprite)) as Sprite;
		} else if ((qiang == 0)) {
			qiangImg.gameObject.SetActive (true);
			qiangImg.sprite = Resources.Load("dn/qiang0", typeof(Sprite)) as Sprite;
		} else {
			qiangImg.gameObject.SetActive (false);
		}
	}
	public void setZhu(int zhu){
		if (zhu > 0) {
			zhuImg.gameObject.SetActive (true);
			zhuImg.sprite = Resources.Load("dn/zhu"+zhu, typeof(Sprite)) as Sprite;
		}else {
			zhuImg.gameObject.SetActive (false);
		}
	}
	public void setNiu(int niu){
		/**
		if (niu == 10) {
			niuTxt.text = "牛牛";
		} else if (niu == 11) {
			niuTxt.text = "五花牛";
		} else if (niu == 12) {
			niuTxt.text = "炸弹";
		} else if (niu == 13) {
			niuTxt.text = "顺牛";
		} else if (niu > 0) {
			niuTxt.text = "牛" + niu;
		} else if (niu == 0) {
			niuTxt.text = "没牛";
		} else {
			niuTxt.text = "";
		}
		*/
		if (niu >= 0) {
			niuImg.gameObject.SetActive (true);
			niuImg.sprite = Resources.Load("dn/niu"+niu, typeof(Sprite)) as Sprite;
		} else {
			niuImg.gameObject.SetActive (false);
		}
	}

	public void setAvatarVo(PlayerBackVO value){
		if (value != null) {
			avatarvo = value;
			nameText.text = avatarvo.accountName;
			scoreText.text = avatarvo.socre+"";
			StartCoroutine (LoadImg());

		} else {
			nameText.text = "";
			readyImg.enabled = false;
			bankerImg.enabled = false;
			headerIcon.sprite = Resources.Load("Image/morentouxiang",typeof(Sprite)) as Sprite;
		}
	}

	/// <summary>
	/// 加载头像
	/// </summary>
	/// <returns>The image.</returns>
	private IEnumerator LoadImg() { 

		if (avatarvo.headIcon.IndexOf ("http") == -1) {
			headerIcon.sprite = GlobalDataScript.getInstance().headSprite;
			yield break;
		}

		//开始下载图片
		WWW www = new WWW(avatarvo.headIcon);
		yield return www;
		if (www != null) {
			Texture2D texture2D = www.texture;
			byte[] bytes = texture2D.EncodeToPNG ();
			//将图片赋给场景上的Sprite
			Sprite tempSp = Sprite.Create (texture2D, new Rect (0, 0, texture2D.width, texture2D.height), new Vector2 (0, 0));
			headerIcon.sprite = tempSp;
		} else {
			MyDebug.Log ("没有加载到图片");
		}
	}
	/// <summary>
	/// Shows the hu effect.
	/// </summary>
	public void showHuEffect(){
		huEffect.SetActive (true);
		HuFlag.SetActive (true);
	}

	public void hideHuImage(){
		HuFlag.SetActive (false);
	}
	/// <summary>
	/// Shows the peng effect.
	/// </summary>
	public void showPengEffect(){
		pengEffect.SetActive (true);
	}
	/// <summary>
	/// Shows the gang effect.
	/// </summary>
	public void showGangEffect(){
		gangEffect.SetActive (true);
	}

	public int getSex(){
		return avatarvo.sex;
	}
	public void showYaoBuQi(bool isShow){
		yaobuqiImage.gameObject.SetActive (isShow);
	}

	//德州扑克
	public void showDZPKGenZhu(int genzhu){
		text_dzpk.text = "跟注";
		//		avaZhu_dzpk.text = int.Parse("avaZhu_dzpk.text") + genzhu +"";
		avaCurZhu_dzpk += (genzhu); 
		avaZhu_dzpk.text = avaCurZhu_dzpk + "";
	}
	public void showDZPKRangPai(){
		text_dzpk.text = "让牌";
		avaZhu_dzpk.text = "";
	}
	public void showDZPKQiPai(){
		//
		cleanDZPKtext();
		qipaiImg_dzpk.SetActive(true);
		text_dzpk.text = "弃牌";
		avaZhu_dzpk.text = "";
	}
	public void showDZPKJiaZhu(int zhu){
		text_dzpk.text = "加注";
		//		avaZhu_dzpk.text = int.Parse("avaZhu_dzpk.text") + zhu +"";
		avaCurZhu_dzpk += (zhu); 
		avaZhu_dzpk.text = avaCurZhu_dzpk + "";
	}

	public void showDZPKSuccess(){
		//text_dzpk.text = "胜利";
		HuFlag.SetActive (true);
	}

	public void cleanDZPKtext(){
		//text_dzpk.text = "";
		avaCurZhu_dzpk = 0;
		avaZhu_dzpk.text = "";
	}

	public void cleanDZPKAlltext(){
		text_dzpk.text = "";
		avaCurZhu_dzpk = 0;
		avaZhu_dzpk.text = "";
		qipaiImg_dzpk.SetActive(false);
		HuFlag.SetActive (false);
	}

	public void frontGenZhu(int genzhu){
		text_dzpk.text = "";
		//		avaZhu_dzpk.text = int.Parse("avaZhu_dzpk.text") + genzhu +"";
		avaCurZhu_dzpk -= (genzhu); 
		avaZhu_dzpk.text = avaCurZhu_dzpk + "";
	}

	public void frontRangPai(){
		text_dzpk.text = "";
		avaZhu_dzpk.text = "";
	}

	public void frontQiPai(){
		//
		cleanDZPKtext();
		qipaiImg_dzpk.SetActive(false);
		text_dzpk.text = "";
		avaZhu_dzpk.text = "";
	}
	public void frontJiaZhu(int zhu){
		text_dzpk.text = "";
		//		avaZhu_dzpk.text = int.Parse("avaZhu_dzpk.text") + zhu +"";
		avaCurZhu_dzpk -= (zhu); 
		avaZhu_dzpk.text = avaCurZhu_dzpk + "";
	}
	public void frontSuccess(){
		//text_dzpk.text = "胜利";
		HuFlag.SetActive (false);
	}

}
