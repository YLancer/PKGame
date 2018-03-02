using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using AssemblyCSharp;
using cn.sharesdk.unity3d;
using System.IO;
using System.Collections.Generic;

public class GameOverScript : MonoBehaviour {
	/**时间显示条**/
	public Text timeText;

	/**房间号**/
	public Text roomNoText;

	/**连庄数**/
	public Text lianZhuangText;

	/**局数**/
	public Text jushuText;

	/**玩法显示**/
	//public Text wanfa;
  
	/***单局面板**/
	public GameObject signalEndPanel;

	/***单局面板_跑得快**/
	public GameObject signalEndPanel_pdk;

	/***单局面板_斗牛**/
	public GameObject signalEndPanel_dn;

	/***单局面板_德州扑克奥马哈**/
	public GameObject signalEndPanel_dzpk;

	/***全局面板**/
	public GameObject finalEndPanel;

	/**分享单局结束战绩按钮**/
	public GameObject shareSiganlButton;

	/**继续游戏按钮**/
	public GameObject continueGame;

	/**分享全局结束战绩按钮**/
	public GameObject shareFinalButton;
    /**返回按钮**/
    public GameObject button_Delete;

    public Button closeButton;

	public Text title;

	public GameObject panelJingArraysh; //赣州冲关上精
	public GameObject panelJingArray; //赣州冲关下，左，右精
	public Text leftJing;//赣州冲关左精文字显示
	public Text rightJing;//赣州冲关右精文字显示

	public GameObject maObjList;


	private List<AvatarVO> mAvatarvoList;
	private List<int> mas_0;
	private List<int> mas_1;
	private List<int> mas_2;
	private List<int> mas_3;
	private  List<List<int>> allMasList;
	private List<int> mValidMas;

	/**0表示打开单局结束模板，1表示全局结束模板**/
	private int mDispalyFlag;

	private string picPath;//图片存储路径


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/**
	 * 设置面板的显示内容
	 * dispalyFlag:0------>表示单据结束； 1--------->全局结束
	 */ 
	public void setDisplayContent(int dispalyFlag,List<AvatarVO> personList,string allMas,List<int> validMas){
        MyDebug.Log("--dispalyFlag----------0----" + dispalyFlag);
		mAvatarvoList = personList;
		mDispalyFlag = dispalyFlag;
		mValidMas = validMas; 
		initRoomBaseInfo ();
        MyDebug.Log("--dispalyFlag----------1----" + dispalyFlag);
        
		if (GlobalDataScript.roomVo.roomType == 6 && !GlobalDataScript.roomVo.tongZhuang) {
			lianZhuangText.gameObject.SetActive (true);
			lianZhuangText.text = "连庄：" + GlobalDataScript.hupaiResponseVo.lianZhuang.ToString ();
		}
		if (GlobalDataScript.goldType) {
			jushuText.text = "局数：" + GlobalDataScript.playCountForGoldType;
		} else {
			jushuText.text = "局数：" + (GlobalDataScript.totalTimes - GlobalDataScript.surplusTimes) + "/" + GlobalDataScript.totalTimes;
		}

		if (dispalyFlag == 0) {
			allMasList = new List<List<int>> ();
			mas_0 = new List<int> ();
			mas_1 = new List<int> ();
			mas_2 = new List<int> ();
			mas_3 = new List<int> ();
			signalEndPanel.SetActive (true);
			finalEndPanel.SetActive (false);
			continueGame.SetActive (true);
			shareFinalButton.SetActive (false);
            button_Delete.SetActive(false);
			if(5 == GlobalDataScript.roomVo.roomType){
				panelJingArray.SetActive (true);  //赣州冲关精牌
				panelJingArraysh.SetActive (true);  //赣州冲关精牌
			}else{
				panelJingArray.SetActive (false);
				panelJingArraysh.SetActive (false);
			}
			///左下角马牌
			maObjList.SetActive (false);

			if (GlobalDataScript.roomVo.roomType == (int)GameType.GameType_MJ_ZhuangZhuang
			   || GlobalDataScript.roomVo.roomType == (int)GameType.GameType_MJ_ChangSha
			   || GlobalDataScript.roomVo.roomType == 4
			   || GlobalDataScript.roomVo.roomType == 8
			   || GlobalDataScript.roomVo.roomType == 10) {
				if (GlobalDataScript.roomVo.ma > 0 && allMas != null && allMas.Length > 0) {
					maObjList.SetActive (true);
					List<string> paiList = new List<string>();
					string uuid = allMas.Split(new char[1] { ':' })[0];
					string[] paiArray = allMas.Split(new char[1] { ':' });
					paiList = new List<string>(paiArray);
					paiList.RemoveAt(0);

					for (int i = 0; i < paiList.Count; i++) {
						GameObject itemTemp = Instantiate (Resources.Load ("Prefab/ThrowCard/ZhongMa")) as GameObject;
						itemTemp.transform.parent = maObjList.transform;
						itemTemp.GetComponent<TopAndBottomCardScript> ().setPoint (int.Parse(paiList [i]));
						itemTemp.transform.localScale = new Vector3 (1f, 1f, 1f);
						itemTemp.transform.localPosition = new Vector3 (i * 44f, 0, 0);                
					}


				}
			}


            MyDebug.Log("--GlobalDataScript.surplusTimes ---------1-----" + GlobalDataScript.surplusTimes);
            MyDebug.Log("--GlobalDataScript.surplusTimes ---------2-----" + GlobalDataScript.isOverByPlayer);
            closeButton.transform.gameObject.SetActive (false);
			if (GlobalDataScript.surplusTimes == 0 || GlobalDataScript.isOverByPlayer) {
				//shareSiganlButton.GetComponent<Image> ().color =Color.white;
                shareSiganlButton.gameObject.SetActive(true);
                continueGame.gameObject.SetActive(false);
                MyDebug.Log("--dispalyFlag---------2-----" + dispalyFlag);
            } else {
                shareSiganlButton.gameObject.SetActive(false);
                continueGame.gameObject.SetActive(true);
                MyDebug.Log("--dispalyFlag---------3-----" + dispalyFlag);
                //shareSiganlButton.GetComponent<Image> ().color = new Color32 (200, 200, 200, 128); 
            }
          
            getMas (allMas);
			setSignalContent ();
		} else if (dispalyFlag == 1) {
            MyDebug.Log("--dispalyFlag--------4-----" + dispalyFlag);
			panelJingArray.SetActive (false);
			panelJingArraysh.SetActive (false);

			maObjList.SetActive (false); //马牌

			signalEndPanel.SetActive (false);
			finalEndPanel.SetActive (true);
			shareSiganlButton.SetActive (false);
            continueGame.gameObject.SetActive(false);
            continueGame.SetActive (false);
			shareFinalButton.SetActive (true);
            button_Delete.SetActive(true);
            closeButton.transform.gameObject.SetActive (true);
			setFinalContent ();
		}

		//赣州冲关显示精牌
		if (GlobalDataScript.roomVo.roomType == 5){
			float startPosition = 30f;
			int jingtextlength = 0;
			//是否显示左精和右精文字
			if(1 == GlobalDataScript.roomVo.shangxiaFanType){
				leftJing.text = "";
				rightJing.text = "";
			}

			for(int i=0, len=GlobalDataScript.hupaiResponseVo.otherJing.Length; i<len; i++){
				int jingpai = GlobalDataScript.hupaiResponseVo.otherJing [i];
				if (jingpai > -1) {
					GameObject itemTemp = Instantiate (Resources.Load ("Prefab/ThrowCard/TopAndBottomCard")) as GameObject;
					itemTemp.transform.parent = panelJingArray.transform;
					//itemTemp.transform.localScale = new Vector3(0.8f,0.8f,1f);
					itemTemp.transform.localScale = Vector3.one;
					itemTemp.GetComponent<TopAndBottomCardScript> ().setPoint (jingpai);
					if (i % 2 == 0) {
						jingtextlength += 36;
					}
					itemTemp.transform.localPosition = new Vector3 (startPosition+i*36f+jingtextlength, 0, 0);
				}
			}
			//显示上精
			GameObject Temp = Instantiate (Resources.Load ("Prefab/ThrowCard/TopAndBottomCard")) as GameObject;
			Temp.transform.parent = panelJingArraysh.transform;
			Temp.transform.localScale = Vector3.one;
			Temp.GetComponent<TopAndBottomCardScript> ().setPoint (GlobalDataScript.roomVo.guiPai);
			Temp.transform.localPosition = new Vector3 (0, 0, 0);

			Temp = Instantiate (Resources.Load ("Prefab/ThrowCard/TopAndBottomCard")) as GameObject;
			Temp.transform.parent = panelJingArraysh.transform;
			Temp.transform.localScale = Vector3.one;
			Temp.GetComponent<TopAndBottomCardScript> ().setPoint (GlobalDataScript.roomVo.guiPai2);
			Temp.transform.localPosition = new Vector3 (37, 0, 0);
		}


	}


	/**
	 * 设置面板的显示内容
	 * dispalyFlag:0------>表示单据结束； 1--------->全局结束
	 */ 
	public void setDisplayContent_pdk(int dispalyFlag,List<AvatarVO> personList,string allMas,List<int> validMas,int gametype = 1){

		mAvatarvoList = personList;
		mDispalyFlag = dispalyFlag;
		mValidMas = validMas; 
		initRoomBaseInfo ();
		if (GlobalDataScript.goldType) {
			jushuText.text = "局数：" + GlobalDataScript.playCountForGoldType;
		} else {
			jushuText.text = "局数：" + (GlobalDataScript.totalTimes - GlobalDataScript.surplusTimes) + "/" + GlobalDataScript.totalTimes;
		}

		if (dispalyFlag == 0) {

			signalEndPanel.SetActive (true);
			finalEndPanel.SetActive (false);
			continueGame.SetActive (true);
			shareFinalButton.SetActive (false);
			button_Delete.SetActive(false);

			closeButton.transform.gameObject.SetActive (false);
            print("setDisplayContent_pdk+++++++++ " + GlobalDataScript.isOverByPlayer);
			if (GlobalDataScript.surplusTimes == 0 || GlobalDataScript.isOverByPlayer) {
				//shareSiganlButton.GetComponent<Image> ().color =Color.white;
				shareSiganlButton.gameObject.SetActive(true);
				continueGame.gameObject.SetActive(false);
				MyDebug.Log("--dispalyFlag---------2-----" + dispalyFlag);
			} else {
				shareSiganlButton.gameObject.SetActive(false);
				continueGame.gameObject.SetActive(true);
				MyDebug.Log("--dispalyFlag---------3-----" + dispalyFlag);
				//shareSiganlButton.GetComponent<Image> ().color = new Color32 (200, 200, 200, 128); 
			}
				
			setSignalContent_pdk ();
		} else if (dispalyFlag == 1) {
			MyDebug.Log("--dispalyFlag--------4-----" + dispalyFlag);
			signalEndPanel.SetActive (false);
			finalEndPanel.SetActive (true);
			shareSiganlButton.SetActive (false);
			continueGame.gameObject.SetActive(false);
			continueGame.SetActive (false);
			shareFinalButton.SetActive (true);
			button_Delete.SetActive(true);
			closeButton.transform.gameObject.SetActive (true);
			if (gametype == 1)
				setFinalContent_pdk ();
			else if (gametype == 3)
				setFinalContent_DN ();
			else if (gametype == 4) {
				setFinalContent_DZPK ();
			}
		}

	}


	private void  getMas(string mas){
        //广东麻将
		if(GlobalDataScript.roomVo.roomType == (int)GameType.GameType_MJ_GuangDong)
        {

        }//其他
        else
        {
            List<string> paiList = new List<string>();
            if (mas != null && mas != "")
            {
                string uuid = mas.Split(new char[1] { ':' })[0];
                string[] paiArray = mas.Split(new char[1] { ':' });
                paiList = new List<string>(paiArray);
                paiList.RemoveAt(0);
                int referIndex = getIndex(int.Parse(uuid));
                List<int> temp = new List<int>();

                int resultIndex = 0;
                for (int i = 0; i < paiList.Count; i++)
                {
                    int cardPoint = int.Parse(paiList[i]);
                    int positionIndex = (cardPoint + 1) % 9;
                    if (cardPoint != 31)
                    {
                        switch (positionIndex)
                        {
                            case 1:
                            case 5:
                            case 0:
                                resultIndex = referIndex;
                                break;
                            case 2:
                            case 6:

                                if ((referIndex + 1) == 4)
                                {
                                    resultIndex = 0;

                                }
                                else {
                                    resultIndex = referIndex + 1;
                                }
                                break;
                            case 4:
                            case 8:
                                if ((referIndex - 1) < 0)
                                {
                                    resultIndex = 3;
                                }
                                else {
                                    resultIndex = referIndex - 1;
                                }
                                break;
                            case 3:
                            case 7:
                                if ((referIndex + 2) == 4)
                                {
                                    resultIndex = 0;
                                }
                                else if ((referIndex + 2) > 4)
                                {
                                    resultIndex = 1;
                                }
                                else if ((referIndex + 2) < 4)
                                {
                                    resultIndex = referIndex + 2;
                                }
                                break;
                        }
                    }
                    else {
                        resultIndex = referIndex;
                    }

                    switch (resultIndex)
                    {
                        case 0:
                            mas_0.Add(cardPoint);
                            break;
                        case 1:
                            mas_1.Add(cardPoint);
                            break;
                        case 2:
                            mas_2.Add(cardPoint);
                            break;
                        case 3:
                            mas_3.Add(cardPoint);
                            break;

                    }
                }
                allMasList.Add(mas_0);
                allMasList.Add(mas_1);
                allMasList.Add(mas_2);
                allMasList.Add(mas_3);
            }
        }
		
	}



	private int getIndex(int uuid){
		if (mAvatarvoList != null) {
			for (int i = 0; i < mAvatarvoList.Count; i++) {
				if (mAvatarvoList[i].account.uuid ==uuid) {
						return i;
					}
				}
			}
			return 0;
		}

	private void initRoomBaseInfo(){

        MyDebug.Log("--setUI-------------------initRoomBaseInfo-------设置文字--------------");

        timeText.text=DateTime.Now.ToString("yyyy-MM-dd");
		roomNoText.text = "房间号：" + GlobalDataScript.roomVo.roomId;

		/*
		if (GlobalDataScript.roomVo.roomType == GameConfig.GAME_TYPE_ZHUANZHUAN) {//转转麻将
			title.text = "转转麻将";
		} else if (GlobalDataScript.roomVo.roomType == GameConfig.GAME_TYPE_HUASHUI) {//划水麻将
			title.text = "划水麻将";
		} else if (GlobalDataScript.roomVo.roomType == GameConfig.GAME_TYPE_CHANGSHA) {
			title.text = "长沙麻将";
		} else if (GlobalDataScript.roomVo.roomType == GameConfig.GAME_TYPE_GUANGDONG) {
			title.text = "广东麻将";
		}
		*/


      //  wanfa.text = MyMahjongScript.roomRemark_tv;


        MyDebug.Log("--setUI-------------------initRoomBaseInfo-------设置文字完毕--------------");
        /**
		if (mDispalyFlag == 1) {
			TitleText.text = "棋牌结束";
		}
		*/

    }

	private Account getAcount(int uuid){
		if (mAvatarvoList != null && mAvatarvoList.Count > 0) {
			for (int i = 0; i < mAvatarvoList.Count; i++) {
				if (mAvatarvoList [i].account.uuid == uuid) {
					return mAvatarvoList [i].account;
				}
			}
		}
		return null;
	}

	/*
	private AvatarVO getAvatar(int uuid){
		if (mAvatarvoList != null && mAvatarvoList.Count > 0) {
			for (int i = 0; i < mAvatarvoList.Count; i++) {
				if (mAvatarvoList [i].account.uuid == uuid) {
					return mAvatarvoList [i];
				}
			}
		}
		return null;
	}
*/

	private void setFinalContent(){
		GlobalDataScript.finalGameEndVo.totalInfo [0].setIsWiner (true);
		GlobalDataScript.finalGameEndVo.totalInfo [0].setIsPaoshou (true);
		int topScore = GlobalDataScript.finalGameEndVo.totalInfo [0].scores;
		int topPaoshou =  GlobalDataScript.finalGameEndVo.totalInfo[0].dianpao;

		int uuid0= GlobalDataScript.finalGameEndVo.totalInfo [0].uuid;
		int owerUuid = GlobalDataScript.finalGameEndVo.theowner;

		Account account0 = getAcount (uuid0);

		//AvatarVO avatarVO0 = getAvatar (uuid0);

		string iconstr = account0.headicon;
		string nickName = account0.nickname;
		GlobalDataScript.finalGameEndVo.totalInfo [0].setIcon (iconstr);
		GlobalDataScript.finalGameEndVo.totalInfo [0].setNickname (nickName);
		if (owerUuid == uuid0) {
			GlobalDataScript.finalGameEndVo.totalInfo [0].setIsMain (true);
		} else {
			GlobalDataScript.finalGameEndVo.totalInfo [0].setIsMain (false);
		}
	//	GlobalDataScript.finalGameEndVo.totalInfo [0].setIsMain (avatarVO0.main);
		int lastTopIndex = 0;
		int lastPaoshouIndex = 0;
		if (GlobalDataScript.finalGameEndVo != null && GlobalDataScript.finalGameEndVo.totalInfo.Count > 0) {
			
			for (int i = 1; i < GlobalDataScript.finalGameEndVo.totalInfo.Count; i++) {
				if (topScore < GlobalDataScript.finalGameEndVo.totalInfo [i].scores) {
					GlobalDataScript.finalGameEndVo.totalInfo [lastTopIndex].setIsWiner (false);
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIsWiner (true);
					lastTopIndex = i;
					topScore = GlobalDataScript.finalGameEndVo.totalInfo[i].scores;
				}
				if (topPaoshou < GlobalDataScript.finalGameEndVo.totalInfo [i].dianpao && !GlobalDataScript.finalGameEndVo.totalInfo [i].getIsWiner()) {
					topPaoshou =GlobalDataScript.finalGameEndVo.totalInfo[i].dianpao;
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIsPaoshou (true);
					GlobalDataScript.finalGameEndVo.totalInfo [lastPaoshouIndex].setIsPaoshou (false);
					lastPaoshouIndex = i;
				}
			

				int uuid = GlobalDataScript.finalGameEndVo.totalInfo [i].uuid;
				Account account = getAcount (uuid);
				if (account != null) {
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIcon (account.headicon);
					GlobalDataScript.finalGameEndVo.totalInfo [i].setNickname (account.nickname);

				}
				if (owerUuid == uuid) {
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIsMain (true);
				} else {
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIsMain (false);
				}


			}

			for (int i = 0; i < GlobalDataScript.finalGameEndVo.totalInfo.Count; i++) {
				FinalGameEndItemVo itemdata = GlobalDataScript.finalGameEndVo.totalInfo [i];
				GameObject itemTemp = Instantiate (Resources.Load("Prefab/Panel_Final_Item")) as GameObject;
				itemTemp.transform.parent = finalEndPanel.transform;
				itemTemp.transform.localScale = Vector3.one;
				itemTemp.GetComponent<finalOverItem>().setUI(itemdata);

            }

		}
	}

	private void setFinalContent_pdk(){
		GlobalDataScript.finalGameEndVo.totalInfo [0].setIsWiner (true);
		GlobalDataScript.finalGameEndVo.totalInfo [0].setIsPaoshou (true);
		int topScore = GlobalDataScript.finalGameEndVo.totalInfo [0].scores;
		int topPaoshou =  GlobalDataScript.finalGameEndVo.totalInfo[0].dianpao;

		int uuid0= GlobalDataScript.finalGameEndVo.totalInfo [0].uuid;
		int owerUuid = GlobalDataScript.finalGameEndVo.theowner;

		Account account0 = getAcount (uuid0);

		//AvatarVO avatarVO0 = getAvatar (uuid0);

		string iconstr = account0.headicon;
		string nickName = account0.nickname;
		GlobalDataScript.finalGameEndVo.totalInfo [0].setIcon (iconstr);
		GlobalDataScript.finalGameEndVo.totalInfo [0].setNickname (nickName);
		if (owerUuid == uuid0) {
			GlobalDataScript.finalGameEndVo.totalInfo [0].setIsMain (true);
		} else {
			GlobalDataScript.finalGameEndVo.totalInfo [0].setIsMain (false);
		}
		//	GlobalDataScript.finalGameEndVo.totalInfo [0].setIsMain (avatarVO0.main);
		int lastTopIndex = 0;
		int lastPaoshouIndex = 0;
		if (GlobalDataScript.finalGameEndVo != null && GlobalDataScript.finalGameEndVo.totalInfo.Count > 0) {

			for (int i = 1; i < GlobalDataScript.finalGameEndVo.totalInfo.Count; i++) {
				if (topScore < GlobalDataScript.finalGameEndVo.totalInfo [i].scores) {
					GlobalDataScript.finalGameEndVo.totalInfo [lastTopIndex].setIsWiner (false);
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIsWiner (true);
					lastTopIndex = i;
					topScore = GlobalDataScript.finalGameEndVo.totalInfo[i].scores;
				}
				if (topPaoshou < GlobalDataScript.finalGameEndVo.totalInfo [i].dianpao && !GlobalDataScript.finalGameEndVo.totalInfo [i].getIsWiner()) {
					topPaoshou =GlobalDataScript.finalGameEndVo.totalInfo[i].dianpao;
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIsPaoshou (true);
					GlobalDataScript.finalGameEndVo.totalInfo [lastPaoshouIndex].setIsPaoshou (false);
					lastPaoshouIndex = i;
				}


				int uuid = GlobalDataScript.finalGameEndVo.totalInfo [i].uuid;
				Account account = getAcount (uuid);
				if (account != null) {
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIcon (account.headicon);
					GlobalDataScript.finalGameEndVo.totalInfo [i].setNickname (account.nickname);

				}
				if (owerUuid == uuid) {
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIsMain (true);
				} else {
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIsMain (false);
				}
			}

			for (int i = 0; i < GlobalDataScript.finalGameEndVo.totalInfo.Count; i++) {
				FinalGameEndItemVo itemdata = GlobalDataScript.finalGameEndVo.totalInfo [i];
				GameObject itemTemp = Instantiate (Resources.Load("Prefab/pdk/Panel_Pdk_Final_Item")) as GameObject;
				itemTemp.transform.parent = signalEndPanel_pdk.transform;
				itemTemp.transform.localScale = Vector3.one;
				itemTemp.GetComponent<finalOverItem>().setUI_pdk_final(itemdata);
			}

		}
	}

	private void setFinalContent_DN(){
		GlobalDataScript.finalGameEndVo.totalInfo [0].setIsWiner (true);
		GlobalDataScript.finalGameEndVo.totalInfo [0].setIsPaoshou (true);
		int topScore = GlobalDataScript.finalGameEndVo.totalInfo [0].scores;
		int topPaoshou =  GlobalDataScript.finalGameEndVo.totalInfo[0].dianpao;

		int uuid0= GlobalDataScript.finalGameEndVo.totalInfo [0].uuid;
		int owerUuid = GlobalDataScript.finalGameEndVo.theowner;

		Account account0 = getAcount (uuid0);

		//AvatarVO avatarVO0 = getAvatar (uuid0);

		string iconstr = account0.headicon;
		string nickName = account0.nickname;
		GlobalDataScript.finalGameEndVo.totalInfo [0].setIcon (iconstr);
		GlobalDataScript.finalGameEndVo.totalInfo [0].setNickname (nickName);
		if (owerUuid == uuid0) {
			GlobalDataScript.finalGameEndVo.totalInfo [0].setIsMain (true);
		} else {
			GlobalDataScript.finalGameEndVo.totalInfo [0].setIsMain (false);
		}
		//	GlobalDataScript.finalGameEndVo.totalInfo [0].setIsMain (avatarVO0.main);
		int lastTopIndex = 0;
		int lastPaoshouIndex = 0;
		if (GlobalDataScript.finalGameEndVo != null && GlobalDataScript.finalGameEndVo.totalInfo.Count > 0) {

			for (int i = 1; i < GlobalDataScript.finalGameEndVo.totalInfo.Count; i++) {
				if (topScore < GlobalDataScript.finalGameEndVo.totalInfo [i].scores) {
					GlobalDataScript.finalGameEndVo.totalInfo [lastTopIndex].setIsWiner (false);
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIsWiner (true);
					lastTopIndex = i;
					topScore = GlobalDataScript.finalGameEndVo.totalInfo[i].scores;
				}
				if (topPaoshou < GlobalDataScript.finalGameEndVo.totalInfo [i].dianpao && !GlobalDataScript.finalGameEndVo.totalInfo [i].getIsWiner()) {
					topPaoshou =GlobalDataScript.finalGameEndVo.totalInfo[i].dianpao;
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIsPaoshou (true);
					GlobalDataScript.finalGameEndVo.totalInfo [lastPaoshouIndex].setIsPaoshou (false);
					lastPaoshouIndex = i;
				}


				int uuid = GlobalDataScript.finalGameEndVo.totalInfo [i].uuid;
				Account account = getAcount (uuid);
				if (account != null) {
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIcon (account.headicon);
					GlobalDataScript.finalGameEndVo.totalInfo [i].setNickname (account.nickname);

				}
				if (owerUuid == uuid) {
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIsMain (true);
				} else {
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIsMain (false);
				}
			}

			for (int i = 0; i < GlobalDataScript.finalGameEndVo.totalInfo.Count; i++) {
				FinalGameEndItemVo itemdata = GlobalDataScript.finalGameEndVo.totalInfo [i];
				GameObject itemTemp = Instantiate (Resources.Load("Prefab/Panel_DN_Final_Item")) as GameObject;
				itemTemp.transform.parent = signalEndPanel_dn.transform;
				itemTemp.transform.localScale = Vector3.one;//  new Vector3 (0.8f,0.8f,0.8f)
				itemTemp.GetComponent<DNFinalOverScript>().setUI_DN_final(itemdata);
			}
			if (CommonEvent.getInstance ().closeGamePanel != null) {
				CommonEvent.getInstance ().closeGamePanel ();
			}
		}
	}


	private void setFinalContent_DZPK(){
		GlobalDataScript.finalGameEndVo.totalInfo [0].setIsWiner (true);
		GlobalDataScript.finalGameEndVo.totalInfo [0].setIsPaoshou (true);
		int topScore = GlobalDataScript.finalGameEndVo.totalInfo [0].scores;
		int topPaoshou =  GlobalDataScript.finalGameEndVo.totalInfo[0].dianpao;

		int uuid0= GlobalDataScript.finalGameEndVo.totalInfo [0].uuid;
		int owerUuid = GlobalDataScript.finalGameEndVo.theowner;

		Account account0 = getAcount (uuid0);

		//AvatarVO avatarVO0 = getAvatar (uuid0);

		string iconstr = account0.headicon;
		string nickName = account0.nickname;
		GlobalDataScript.finalGameEndVo.totalInfo [0].setIcon (iconstr);
		GlobalDataScript.finalGameEndVo.totalInfo [0].setNickname (nickName);
		if (owerUuid == uuid0) {
			GlobalDataScript.finalGameEndVo.totalInfo [0].setIsMain (true);
		} else {
			GlobalDataScript.finalGameEndVo.totalInfo [0].setIsMain (false);
		}
		//	GlobalDataScript.finalGameEndVo.totalInfo [0].setIsMain (avatarVO0.main);
		int lastTopIndex = 0;
		int lastPaoshouIndex = 0;
		if (GlobalDataScript.finalGameEndVo != null && GlobalDataScript.finalGameEndVo.totalInfo.Count > 0) {

			for (int i = 1; i < GlobalDataScript.finalGameEndVo.totalInfo.Count; i++) {
				if (topScore < GlobalDataScript.finalGameEndVo.totalInfo [i].scores) {
					GlobalDataScript.finalGameEndVo.totalInfo [lastTopIndex].setIsWiner (false);
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIsWiner (true);
					lastTopIndex = i;
					topScore = GlobalDataScript.finalGameEndVo.totalInfo[i].scores;
				}
				if (topPaoshou < GlobalDataScript.finalGameEndVo.totalInfo [i].dianpao && !GlobalDataScript.finalGameEndVo.totalInfo [i].getIsWiner()) {
					topPaoshou =GlobalDataScript.finalGameEndVo.totalInfo[i].dianpao;
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIsPaoshou (true);
					GlobalDataScript.finalGameEndVo.totalInfo [lastPaoshouIndex].setIsPaoshou (false);
					lastPaoshouIndex = i;
				}


				int uuid = GlobalDataScript.finalGameEndVo.totalInfo [i].uuid;
				Account account = getAcount (uuid);
				if (account != null) {
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIcon (account.headicon);
					GlobalDataScript.finalGameEndVo.totalInfo [i].setNickname (account.nickname);

				}
				if (owerUuid == uuid) {
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIsMain (true);
				} else {
					GlobalDataScript.finalGameEndVo.totalInfo [i].setIsMain (false);
				}
			}

			for (int i = 0; i < GlobalDataScript.finalGameEndVo.totalInfo.Count; i++) {
				FinalGameEndItemVo itemdata = GlobalDataScript.finalGameEndVo.totalInfo [i];
				GameObject itemTemp = Instantiate (Resources.Load("Prefab/Panel_DZPK_Final_Item")) as GameObject;
				itemTemp.transform.parent = signalEndPanel_dzpk.transform;
				itemTemp.transform.localScale = Vector3.one;//  new Vector3 (0.8f,0.8f,0.8f)
				itemTemp.GetComponent<DZPKFinalOverScript>().setUI_DZPK_final(itemdata);
			}
//			if (CommonEvent.getInstance ().closeGamePanel != null) {
//				CommonEvent.getInstance ().closeGamePanel ();
//			}
		}
	}


	private void setSignalContent(){

		if (GlobalDataScript.hupaiResponseVo != null && GlobalDataScript.hupaiResponseVo.avatarList.Count > 0) {
			for (int i = 0; i < GlobalDataScript.hupaiResponseVo.avatarList.Count; i++) {
				HupaiResponseItem itemdata = GlobalDataScript.hupaiResponseVo.avatarList [i];
				if (allMasList != null && allMasList.Count != 0) {
					itemdata.setMaPoints (allMasList[i]);
				}
				GameObject itemTemp = Instantiate (Resources.Load("Prefab/Panel_Current_Item")) as GameObject;
				itemTemp.transform.parent = signalEndPanel.transform;
				itemTemp.transform.localScale = Vector3.one;
				itemTemp.GetComponent<SignalOverItemScript>().setUI(itemdata,mValidMas,getMainuuid());
			}
		} 
	}

	private void setSignalContent_pdk(){

		if (GlobalDataScript.hupaiResponseVo != null && GlobalDataScript.hupaiResponseVo.avatarList.Count > 0) {
			for (int i = 0; i < GlobalDataScript.hupaiResponseVo.avatarList.Count; i++) {
				HupaiResponseItem itemdata = GlobalDataScript.hupaiResponseVo.avatarList [i];
				GameObject itemTemp = Instantiate (Resources.Load("Prefab/pdk/Panel_Pdk_Current_Item")) as GameObject;
				itemTemp.transform.parent = signalEndPanel_pdk.transform;
				itemTemp.transform.localScale = Vector3.one;
				itemTemp.GetComponent<finalOverItem>().setUI_pdk_signal(itemdata);
			}
		} 
	}

	private int getMainuuid (){
		for (int i = 0; i < mAvatarvoList.Count; i++) {
			if (i==GlobalDataScript.hupaiResponseVo.oldBankerIndex) {
				return mAvatarvoList [i].account.uuid;
			}
		}
		return 0;
	}

	public void reStratGame(){


		if (GlobalDataScript.isOverByPlayer) {
			//TipsManagerScript.getInstance ().setTips ("房间已解散，不能重新开始游戏");
			closeDialog ();
			return;
		}

		if (GlobalDataScript.surplusTimes > 0) {
			if (GlobalDataScript.roomVo.gameType == 0) {
				CustomSocket.getInstance ().sendMsg (new GameReadyRequest ());
				CommonEvent.getInstance ().readyGame ();
			}
			closeDialog ();

		} else {
			//TipsManagerScript.getInstance ().setTips ("游戏局数已经用完，无法重新开始游戏");
			closeDialog ();
		}
		SoundCtrl.getInstance().playSoundByActionButton(1);
	}


	public void closeDialog(){
		GameOverScript self = GetComponent<GameOverScript> ();
		Destroy (self.continueGame);
		Destroy (self.finalEndPanel);
		Destroy (self.jushuText);
		Destroy (self.signalEndPanel);
		Destroy (self.finalEndPanel);
		Destroy (self.shareSiganlButton);
		Destroy (self.continueGame);
		Destroy (self.shareFinalButton);
        Destroy(self.button_Delete);

		Destroy (self.panelJingArray);
		Destroy (self.panelJingArraysh);


        if (GlobalDataScript.singalGameOverList!=null && GlobalDataScript.singalGameOverList.Count > 0) {
			for (int i = 0; i < GlobalDataScript.singalGameOverList.Count; i++) {
				//GlobalDataScript.singalGameOverList [i].GetComponent<GameOverScript> ().closeDialog ();
				Destroy(GlobalDataScript.singalGameOverList[i].GetComponent<GameOverScript>());
				Destroy (GlobalDataScript.singalGameOverList [i]);
			}
			int count = GlobalDataScript.singalGameOverList.Count;
			for (int i = 0; i < count; i++) {
				GlobalDataScript.singalGameOverList.RemoveAt (0);
			}
		}

		Destroy (this);
		Destroy (gameObject);
		//Destroy (GlobalDataScript.gamePlayPanel);
		SoundCtrl.getInstance().playSoundByActionButton(1);

	}

	public void doShare(){

		GlobalDataScript.getInstance ().wechatOperate.shareAchievementToWeChat (PlatformType.WeChat);
		SoundCtrl.getInstance().playSoundByActionButton(1);

	}

	public void openFinalOverPanl(){
		SoundCtrl.getInstance().playSoundByActionButton(1);
        MyDebug.Log("------------------0----------------------");

        MyDebug.Log("-----------------GlobalDataScript.finalGameEndVo.totalInfo.Count---------------------" + GlobalDataScript.finalGameEndVo != null);

        MyDebug.Log("-----------------GlobalDataScript.finalGameEndVo.totalInfo.Count---------------------" + GlobalDataScript.finalGameEndVo.totalInfo!= null);

        MyDebug.Log("-----------------GlobalDataScript.finalGameEndVo.totalInfo.Count---------------------" + GlobalDataScript.finalGameEndVo.totalInfo.Count);
        MyDebug.Log("------------------GlobalDataScript.finalGameEndVo.totalInfo !=null----------------------" + GlobalDataScript.finalGameEndVo.totalInfo != null);

        MyDebug.Log("-----------------GlobalDataScript.finalGameEndVo !=null--------------------" + GlobalDataScript.finalGameEndVo != null);
		if ( GlobalDataScript.finalGameEndVo !=null && GlobalDataScript.finalGameEndVo.totalInfo !=null && GlobalDataScript.finalGameEndVo.totalInfo.Count>0) {

            MyDebug.Log("------------------1----------------------");
            GameObject obj = PrefabManage.loadPerfab ("prefab/Panel_Game_Over");

			if (GlobalDataScript.roomVo.gameType == 0) {
				obj.GetComponent<GameOverScript> ().setDisplayContent (1, 
					GlobalDataScript.roomAvatarVoList, null, GlobalDataScript.hupaiResponseVo.validMas);
			} else {
				obj.GetComponent<GameOverScript> ().setDisplayContent_pdk (1, 
					GlobalDataScript.roomAvatarVoList, null, GlobalDataScript.hupaiResponseVo.validMas);
			}
			obj.transform.SetSiblingIndex (2);
            MyDebug.Log("------------------2----------------------");
			if (GlobalDataScript.singalGameOverList.Count > 0) {
                MyDebug.Log("------------------3----------------------");
				for (int i = 0; i < GlobalDataScript.singalGameOverList.Count; i++)
				{
					//GlobalDataScript.singalGameOverList [i].GetComponent<GameOverScript> ().closeDialog ();
					Destroy(GlobalDataScript.singalGameOverList[i].GetComponent<GameOverScript>());
					Destroy(GlobalDataScript.singalGameOverList[i]);
				}
                MyDebug.Log("------------------4----------------------");
				//int count = GlobalDataScript.singalGameOverList.Count;
				//for (int i = 0; i < count; i++) {
				//	GlobalDataScript.singalGameOverList.RemoveAt (0);
				//}
				GlobalDataScript.singalGameOverList.Clear();
			}
            MyDebug.Log("------------------5----------------------");
			if (CommonEvent.getInstance ().closeGamePanel != null) {
				CommonEvent.getInstance ().closeGamePanel ();
			}

		}

	

	}


}
