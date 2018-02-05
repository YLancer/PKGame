using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System;


public class ShowUserInfoScript : MonoBehaviour {
	public Image headIcon;
	public Text Ip;
	public Text ID;
	public Text name;

	private Texture2D texture2D;
	private string headIconPath;
	private AvatarVO ava;
	private MyMahjongScript myMaj;
	private MyPDKScript myPdk;
	private MyDNScript myDN;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setUIData(AvatarVO  userInfo){
		ava = userInfo;
		headIconPath = userInfo.account.headicon;
		Ip.text = "IP:"+ userInfo.IP;
		ID.text = "ID:"+  userInfo.account.uuid +"";
		name.text ="昵称:"+ userInfo.account.nickname;
		StartCoroutine (LoadImg());

	}



	private IEnumerator LoadImg() {
		//开始下载图片
		if (headIconPath != null && headIconPath != "") {

			if (headIconPath.IndexOf ("http") == -1) {
				headIcon.sprite = GlobalDataScript.getInstance().headSprite;
				yield break;
			}

//			WWW www = new WWW(headIconPath);
//			yield return www;
//			//下载完成，保存图片到路径filePath
//			try {
//				texture2D = www.texture;
//				byte[] bytes = texture2D.EncodeToPNG();
//				//将图片赋给场景上的Sprite
//				Sprite tempSp = Sprite.Create(texture2D, new Rect(0,0,texture2D.width,texture2D.height),new Vector2(0,0));
//				headIcon.sprite = tempSp;
//
//			} catch (Exception e){
//
//				MyDebug.Log ("LoadImg"+e.Message);
//			}


			if (FileIO.wwwSpriteImage.ContainsKey(headIconPath))
			{
				headIcon.sprite = FileIO.wwwSpriteImage[headIconPath];
				yield break;
			}
			else {


				//开始下载图片
				WWW www = new WWW(headIconPath);
				yield return www;
				//下载完成，保存图片到路径filePath
				texture2D = www.texture;
				byte[] bytes = texture2D.EncodeToPNG();

				//将图片赋给场景上的Sprite
				Sprite tempSp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));
				headIcon.sprite = tempSp;
				FileIO.wwwSpriteImage.Add(headIconPath, tempSp);
			}






		}

	}

	public void closeUserInfoPanel(){
		Destroy (this);
		Destroy (gameObject);
	}

	public void clickmfbq(string indexStr){

		SoundCtrl.getInstance().playSoundByActionButton(1);
		if (ava == null) {
			return;
		}
		CustomSocket.getInstance().sendMsg(new MessageBoxRequest(4, indexStr, GlobalDataScript.loginResponseData.account.uuid, ava.account.uuid));
//		if (myMaj == null) {
//			myMaj = GameObject.Find("Panel_GamePlay(Clone)").GetComponent<MyMahjongScript>();
//		}
//		if (myMaj != null) {
//			int myIndex = myMaj.getMyIndexFromList ();
//			int destAvaIndex = myMaj.getIndex (ava.account.uuid);
//			int seatIndex = destAvaIndex - myIndex;
//			if (seatIndex < 0) {
//				seatIndex = 4 + seatIndex;
//			}
//
//			myMaj.playerItems [seatIndex].showMfbq (0, seatIndex, indexStr);
//		}

		if (GlobalDataScript.roomVo.gameType == 0) {
			if (myMaj == null) {
				myMaj = GameObject.Find ("Panel_GamePlay(Clone)").GetComponent<MyMahjongScript> ();
			}
			if (myMaj != null) {
				int myIndex = myMaj.getMyIndexFromList ();
				int destAvaIndex = myMaj.getIndex (ava.account.uuid);
				int seatIndex = destAvaIndex - myIndex;
				if (seatIndex < 0) {
					seatIndex = 4 + seatIndex;
				}
	
				myMaj.playerItems [seatIndex].showMfbq (0, seatIndex, indexStr);
			}
		} else if (GlobalDataScript.roomVo.gameType == 1) {
			if (myPdk == null) {
				myPdk = GameObject.Find ("Panel_GamePDK(Clone)").GetComponent<MyPDKScript> ();
			}
			if (myPdk != null) {
				int myIndex = myPdk.getMyIndexFromList ();
				int destAvaIndex = myPdk.getIndex (ava.account.uuid);
				int seatIndex = destAvaIndex - myIndex;
				if (seatIndex < 0) {
					seatIndex = 3 + seatIndex;
				}

				myPdk.playerItems [seatIndex].showMfbq (0, seatIndex, indexStr);
			}
		} else if (GlobalDataScript.roomVo.gameType == 3) {
			if (myDN == null) {
				myDN = GameObject.Find ("Panel_GameDN(Clone)").GetComponent<MyDNScript> ();
			}
			if (myDN != null) {
				int myIndex = myDN.getMyIndexFromList ();
				int destAvaIndex = myDN.getIndex (ava.account.uuid);
				int seatIndex = destAvaIndex - myIndex;
				if (seatIndex < 0) {
					seatIndex = 5 + seatIndex;
				}

				myDN.playerItems [seatIndex].showMfbq (0, seatIndex, indexStr);
			}
		}


		closeUserInfoPanel ();
	}
}
