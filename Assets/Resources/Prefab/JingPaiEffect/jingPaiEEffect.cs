using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class jingPaiEEffect : MonoBehaviour {

	public List<Text> text;
	public List<Image> image;
	//public Text jingText;
	public Image jingCenter;



	private int loop = 0;
	private int loopMax = 0;
	private int myRoomIndex;

	// Use this for initialization
	void Start () {
		loop = 0;
		loopMax = getLoopMax ();
		myRoomIndex = getMyIndex ();

		//一启动直接显示上精
		int jing1 = GlobalDataScript.roomVo.guiPai;
		int jing2 = GlobalDataScript.roomVo.guiPai2;
		setJingPai (jing1, jing2);
		//jingText.text = "上精";
		jingCenter.overrideSprite = Resources.Load ("images/Jing/shangjing", typeof(Sprite)) as Sprite;
	}

	// Update is called once per frame
	void Update () {

	}

	private void setJingPai(int jing1, int jing2){
		for (int i = 0; i < image.Count/2; i++) {
			image [i*2].overrideSprite = Resources.Load ("Cards/Big/b" + jing1, typeof(Sprite)) as Sprite;
			text [i*2].text = "X" + getJingCount ((i + myRoomIndex) % 4, jing1) + "";
			image [i*2 + 1].overrideSprite = Resources.Load ("Cards/Big/b" + jing2, typeof(Sprite)) as Sprite;
			text [i*2 + 1].text = "X" + getJingCount ((i + myRoomIndex) % 4, jing2) + "";
		}
	}

	private void destroy(){
		if (gameObject) {
			Destroy (gameObject);
		}
	}

	public void jingPaiShow(){ //显示其他精
		if (loop >= loopMax) {
			destroy ();
			return;
		} else { //依次显示下，左，右精
			int jing1 =GlobalDataScript.hupaiResponseVo.otherJing [loop];
			int jing2 = GlobalDataScript.hupaiResponseVo.otherJing [loop + 1];
			if (-1 == jing1 || -1 == jing2) {
				Debug.LogError ("赣州冲关精牌为-1");
				destroy ();
				return;
			}
			setJingPai (jing1, jing2);
			if (0 == loop) {
				//jingText.text = "下精";
				jingCenter.overrideSprite = Resources.Load ("images/Jing/xiajing", typeof(Sprite)) as Sprite;
			} else if (2 == loop) {
				//jingText.text = "左精";
				jingCenter.overrideSprite = Resources.Load ("images/Jing/zuojing", typeof(Sprite)) as Sprite;
			}else if(4 == loop){
				//jingText.text = "右精";
				jingCenter.overrideSprite = Resources.Load ("images/Jing/youjing", typeof(Sprite)) as Sprite;
			}
			loop += 2;
		}
	}



	private int getLoopMax(){
		if (1 == GlobalDataScript.roomVo.shangxiaFanType) { //只有2个下精
			return 2; 
		} else { //还有6个精
			return 6;
		}
	}

	private int getMyIndex () {
		for (int i = 0; i < GlobalDataScript.roomAvatarVoList.Count; i++) {
			if (GlobalDataScript.roomAvatarVoList[i].account.uuid == GlobalDataScript.loginResponseData.account.uuid 
				|| GlobalDataScript.roomAvatarVoList[i].account.openid == GlobalDataScript.loginResponseData.account.openid) {
				return i;
			}
		}
		return 0;
	}

	private int getJingCount(int avaindex, int jing){
		if (-1 == jing) {
			Debug.LogError ("getJingCount():赣州冲关精牌为-1");
			destroy ();
			return 0;
		}

		return GlobalDataScript.hupaiResponseVo.avatarList [avaindex].paiArray[jing];
	}
}
