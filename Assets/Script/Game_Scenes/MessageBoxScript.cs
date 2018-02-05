using UnityEngine;
using System.Collections;
using DG.Tweening;
using AssemblyCSharp;
using UnityEngine.UI;

public class MessageBoxScript : MonoBehaviour {
	MyMahjongScript myMaj;
	MyPDKScript myPdk;
	MyDNScript myDN;
	MyDZPKScript myDzpk;
    MyDZPKScript myAmh;
    public Text msg;


	// Use this for initialization
	void Start () {
		//SocketEventHandle.getInstance ().messageBoxNotice += messageBoxNotice;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void btnClick(int index){         
        SoundCtrl.getInstance ().playMessageBoxSound(index, GlobalDataScript.loginResponseData.account.sex,3);
		CustomSocket.getInstance ().sendMsg (new MessageBoxRequest(1, GlobalDataScript.loginResponseData.account.sex == 1 ? (index+1000).ToString(): (index + 3000).ToString(),GlobalDataScript.loginResponseData.account.uuid));
		if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.NULL) {
			if (myMaj == null) {
				myMaj = GameObject.Find ("Panel_GamePlay(Clone)").GetComponent<MyMahjongScript> ();
			}
			if (myMaj != null) {
				myMaj.playerItems [0].showChatMessage (index);
			}
		} else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.PDK) {
			if (myPdk == null) {
				myPdk = GameObject.Find ("Panel_GamePDK(Clone)").GetComponent<MyPDKScript> ();
			}
			if (myPdk != null) {
				myPdk.playerItems [0].showChatMessage (index);
			}
		} else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.DN) {
			if (myDN == null) {
				myDN = GameObject.Find ("Panel_GameDN(Clone)").GetComponent<MyDNScript> ();
			}
			if (myDN != null) {
				myDN.playerItems [0].showChatMessage (index);
			}
		} else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.DZPK) {
			if (myDzpk == null) {
				myDzpk = GameObject.Find ("Panel_GameDZPK(Clone)").GetComponent<MyDZPKScript> ();
			}
			if (myDzpk != null) {
				myDzpk.playerItems [0].showChatMessage (index);
			}
        } else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.AMH) {
            if (myAmh == null)
            {
                myAmh = GameObject.Find("Panel_GameAMH(Clone)").GetComponent<MyDZPKScript>();
            }
            if (myAmh != null)
            {
                myAmh.playerItems[0].showChatMessage(index);
            }
        }
        hidePanel ();
	}


    public void sendMsg() {
        if (msg.text.Length == 0)
            return;
        SoundCtrl.getInstance().playSoundByActionButton(1);
        CustomSocket.getInstance().sendMsg(new MessageBoxRequest(2, msg.text, GlobalDataScript.loginResponseData.account.uuid));
		if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.NULL) {
			if (myMaj == null) {
				myMaj = GameObject.Find ("Panel_GamePlay(Clone)").GetComponent<MyMahjongScript> ();
			}
			if (myMaj != null) {
				myMaj.playerItems [0].showChatMessage (msg.text);
			}
		} else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.PDK) {
			if (myPdk == null) {
				myPdk = GameObject.Find ("Panel_GamePDK(Clone)").GetComponent<MyPDKScript> ();
			}
			if (myPdk != null) {
				myPdk.playerItems [0].showChatMessage (msg.text);
			}
		} else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.DN) {
			if (myDN == null) {
				myDN = GameObject.Find ("Panel_GameDN(Clone)").GetComponent<MyDNScript> ();
			}
			if (myDN != null) {
				myDN.playerItems [0].showChatMessage (msg.text);
			}
		} else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.DZPK) {
			if (myDzpk == null) {
				myDzpk = GameObject.Find ("Panel_GameDZPK(Clone)").GetComponent<MyDZPKScript> ();
			}
			if (myDzpk != null) {
				myDzpk.playerItems [0].showChatMessage (msg.text);
			}
        } else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.AMH) {
            if (myAmh == null)
            {
                myAmh = GameObject.Find("Panel_GameAMH(Clone)").GetComponent<MyDZPKScript>();
            }
            if (myAmh != null)
            {
                myAmh.playerItems[0].showChatMessage(msg.text);
            }
        }

        hidePanel();
    }

    public void sendBiaoQing(string indexStr)
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
		int sex = GlobalDataScript.loginResponseData.account.sex;
		string tmp = null;
		if (sex == 1) {
			tmp = "" + (1000 + int.Parse (indexStr));
		} else {
			tmp = "" + (3000 + int.Parse (indexStr));
		}
		CustomSocket.getInstance().sendMsg(new MessageBoxRequest(3, tmp, GlobalDataScript.loginResponseData.account.uuid));
		if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.NULL) {
			if (myMaj == null) {
				myMaj = GameObject.Find ("Panel_GamePlay(Clone)").GetComponent<MyMahjongScript> ();
			}
			if (myMaj != null) {
				int index = int.Parse (indexStr) - 1;

				//SoundCtrl.getInstance ().playMessageBoxSound (index + 1, sex, 2);
				myMaj.playerItems [0].showBiaoQing (myMaj.getBqScript ().getBiaoqing (index));
			}
		} else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.PDK) {
			if (myPdk == null) {
				myPdk = GameObject.Find ("Panel_GamePDK(Clone)").GetComponent<MyPDKScript> ();
			}
			if (myPdk != null) {
				int index = int.Parse (indexStr) - 1;

				//SoundCtrl.getInstance ().playMessageBoxSound (index + 1, sex, 2);
				myPdk.playerItems [0].showBiaoQing (myPdk.getBqScript ().getBiaoqing (index));
			}
		} else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.DN) {
			if (myDN == null) {
				myDN = GameObject.Find ("Panel_GameDN(Clone)").GetComponent<MyDNScript> ();
			}
			if (myDN != null) {
				int index = int.Parse (indexStr) - 1;
//				if(!(GlobalDataScript.roomVo.gameType == 3))
//					SoundCtrl.getInstance ().playMessageBoxSound (index + 1, sex, 2);
				myDN.playerItems [0].showBiaoQing (myDN.getBqScript ().getBiaoqing (index));
			}
		} else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.DZPK) {
			if (myDzpk == null) {
				myDzpk = GameObject.Find ("Panel_GameDZPK(Clone)").GetComponent<MyDZPKScript> ();
			}
			if (myDzpk != null) {
				int index = int.Parse (indexStr) - 1;
				//
				//SoundCtrl.getInstance ().playMessageBoxSound (index + 1, sex, 2);
				myDzpk.playerItems [0].showBiaoQing (myDzpk.getBqScript ().getBiaoqing (index));
			}
        } else if (GlobalDataScript.roomVo.gameType == (int)GameTypePK.AMH) {
            if (myAmh == null)
            {
                myAmh = GameObject.Find("Panel_GameAMH(Clone)").GetComponent<MyDZPKScript>();
            }
            if (myAmh != null)
            {
                int index = int.Parse(indexStr) - 1;
                //
                //SoundCtrl.getInstance ().playMessageBoxSound (index + 1, sex, 2);
                myAmh.playerItems[0].showBiaoQing(myAmh.getBqScript().getBiaoqing(index));
            }
        }

        hidePanel();
    }

    public void showPanel(){
		SoundCtrl.getInstance().playSoundByActionButton(1);
		gameObject.transform.DOLocalMove (new Vector3(0,0), 0.1f);
	}

	public void hidePanel(){		 
		Destroy (this);
		Destroy (gameObject);
		//gameObject.transform.DOLocalMove (new Vector3(0,1600), 0.1f);
	}

    public void messageBoxNotice(ClientResponse response) {
        string[] arr = response.message.Split(new char[1] { '|' });
        int code = int.Parse(arr[1]);
        //传输性别  大于3000为女
        if (code > 3000)
        {
            code = code - 3001;
            SoundCtrl.getInstance().playMessageBoxSound(code, 0);
        }
        else {
            code = code - 1001;
            SoundCtrl.getInstance().playMessageBoxSound(code, 1);
        }    
	}

	public void Destroy(){
		//SocketEventHandle.getInstance ().messageBoxNotice -= messageBoxNotice;
	}
}
