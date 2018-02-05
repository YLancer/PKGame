using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;

public class GangpaiObj{
	public int cardPiont;//出牌的下标
	public string uuid;//出牌的玩家
	public string type;
}

//public class PengpaiObj{
	//public string cardPoints;//出牌的玩家
//}

public class HuipaiObj{
	public int cardPiont;//出牌的下标
	public string uuid;
	public string type;
}

public class ChipaiObj{
	public string[] cardPionts;//出牌的下标

}
	


/**
 * 
 * 
 */ 
public class SignalOverItemScript : MonoBehaviour {

	public Text nickName;
	public Text resultDes;
	public GameObject huFlagImg;
	public Text totalScroe;
	public Text fanCount;
	public Text gangScore;
	public Text biJingScore;
	public GameObject paiArrayPanel;
	public Image zhongMaFlag;//中码标记
	public GameObject GenzhuangFlag;
	public GameObject zhuangFlagImg;

	//public GameObject subContaner ;
	//public GameObject chiContaner;
	//public GameObject pengContaner;
	//public GameObject gangContaner;	
	//public GameObject huContaner;

	private List<GangpaiObj> gangPaiList = new List<GangpaiObj>();//杠牌列表
	private string[] pengpaiList ;//碰牌列表
	private List<ChipaiObj> chipaiList = new List<ChipaiObj>();//吃牌列表
	private List<int> maPais;//码牌数组

	private HuipaiObj hupaiObj = new HuipaiObj();//胡牌列表


	private string mdesCribe = "";//对结果展示字符串
	private int[] paiArray;//牌列表

	private List<int> validMas;
	private HupaiResponseItem mHupaiResponseItemData;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setUI(HupaiResponseItem itemData,List<int> validMasParm ,int mainuuid){
		mHupaiResponseItemData = itemData;
		validMas = validMasParm;
		nickName.text = itemData.nickname;
		totalScroe.text = itemData.totalScore+"";
		gangScore.text = itemData.gangScore+"";
		biJingScore.text = itemData.jingScore+"";
        //fanCount.text = (itemData.totalScore- itemData.gangScore)+"";
		fanCount.text = (itemData.totalScore- itemData.gangScore - itemData.jingScore)+"";//总 - 杠 ?

		if (itemData.uuid == mainuuid) {
			zhuangFlagImg.SetActive (true);
		} else {
			zhuangFlagImg.SetActive (false);
		}

		paiArray = itemData.paiArray;
		huFlagImg.SetActive (false);
		if (itemData.totalInfo.genzhuang == "1" && itemData.uuid == GlobalDataScript.mainUuid) {
			GenzhuangFlag.SetActive (true);
		} else {
			GenzhuangFlag.SetActive (false);
		}
			
		/*
		if (GlobalDataScript.isGenzhuang && mainuuid == itemData.uuid) {
			GenzhuangFlag.SetActive(true);
		} else {
			GenzhuangFlag.SetActive(false);
		}
*/
		analysisPaiInfo (itemData);

	}

	TotalInfo itemData;
	private void analysisPaiInfo(HupaiResponseItem parms){
		itemData = parms.totalInfo;
		string gangpaiStr = itemData.gang;
		if (gangpaiStr != null && gangpaiStr.Length > 0) {
			string[] gangtemps = gangpaiStr.Split (new char[1]{','});
			for (int i = 0; i < gangtemps.Length; i++) {
				string item = gangtemps [i];
				GangpaiObj gangpaiObj = new GangpaiObj ();
				gangpaiObj.uuid  =item.Split (new char[1]{':'})[0];
				gangpaiObj.cardPiont =int.Parse( item.Split (new char[1]{':'})[1]);
				gangpaiObj.type = item.Split (new char[1]{':'})[2];
				//增加判断是否为自己的杠牌的操作

				paiArray [gangpaiObj.cardPiont] -= 4;
				gangPaiList.Add (gangpaiObj);

				if (gangpaiObj.type == "an") {
					mdesCribe += "暗杠  ";
				} else {
					mdesCribe += "明杠  ";
				}
			}
		}


		//20171102
		if(GlobalDataScript.roomVo.roomType == 11){
			if (parms.shanghuo) {
				mdesCribe += "上火  ";
			} else {
				mdesCribe += "不上火  ";
			}
			mdesCribe += "飘数:" + parms.piao + "  ";
		}


		string pengpaiStr = itemData.peng;
		if (pengpaiStr != null && pengpaiStr.Length > 0) {
			
			pengpaiList = pengpaiStr.Split (new char[1]{','});


			//string[] pengpaiListTTT = pengpaiList;
			List<string> pengpaiListTTT = new List<string>();
			for (int i = 0; i <pengpaiList.Length; i++) {
				string[] temp = pengpaiList [i].Split (':');
				if (paiArray [int.Parse (temp [0])] >= 3) {
					paiArray [int.Parse (temp [0])] -= 3;
					pengpaiListTTT.Add (temp [0]);
				}

			}
			pengpaiList = pengpaiListTTT.ToArray ();
		}


		string chipaiStr = itemData.chi;
		if (chipaiStr != null && chipaiStr.Length > 0) {
			string[] chitemps = chipaiStr.Split (new char[1]{','});
			for (int i = 0; i < chitemps.Length; i++) {
				string item = chitemps[i];
				ChipaiObj chipaiObj = new ChipaiObj ();
				string[] pointStr = item.Split (new char[1]{ ':' }); 
				chipaiObj.cardPionts = pointStr;
				chipaiList.Add (chipaiObj);
				paiArray [int.Parse(chipaiObj.cardPionts[0])] -= 1;
				paiArray [int.Parse(chipaiObj.cardPionts[1])] -= 1;
				paiArray [int.Parse(chipaiObj.cardPionts[2])] -= 1;
			}

		}
			
		string hupaiStr = itemData.hu;
		if(hupaiStr!=null && hupaiStr.Length>0){
			bool isJian = false;
			string[] hupai = hupaiStr.Split (new char[1]{','});
			foreach (string hu in hupai) {
				if (hu == null || hu.Length == 0)
					continue;
				hupaiObj.uuid = hu.Split (new char[1]{ ':' }) [0];
				hupaiObj.cardPiont = int.Parse (hu.Split (new char[1]{ ':' }) [1]);
				hupaiObj.type = hu.Split (new char[1]{ ':' }) [2];
				//增加判断是否是自己胡牌的判断

				if (hupaiStr.Contains ("d_other")) {//排除一炮多响的情况
					mdesCribe += "点炮";
				} else if (hupaiStr.Contains ("other_common")) {//排除一炮多响的情况

				}else {
					if (hupaiObj.type == "menqing") {
						mdesCribe += "门清";
						huFlagImg.SetActive (true);
					}
					else if (hupaiObj.type == "zi_common") {
						mdesCribe += "自摸";
						huFlagImg.SetActive (true);

					} else if (hupaiObj.type == "d_self") {
						mdesCribe += "接炮";
						huFlagImg.SetActive (true);
					} else if (hupaiObj.type == "qiyise") {
						mdesCribe += "清一色";
						huFlagImg.SetActive (true);
					} else if (hupaiObj.type == "zi_qingyise") {
						mdesCribe += "自摸清一色";
						huFlagImg.SetActive (true);
					} else if (hupaiObj.type == "qixiaodui") {
						mdesCribe += "七小对";
						huFlagImg.SetActive (true);
					} else if (hupaiObj.type == "self_qixiaodui") {
						mdesCribe += "七小对";
						huFlagImg.SetActive (true);
					} else if (hupaiObj.type == "gangshangpao") {
						mdesCribe += "杠上炮";
					} else if (hupaiObj.type == "gangshanghua") {
						mdesCribe += "杠上花";
						huFlagImg.SetActive (true);
					}
					//红中宝
					else if (hupaiObj.type == "tian_hu") {
						mdesCribe += "天胡";
						huFlagImg.SetActive (true);
					}else if (hupaiObj.type == "di_hu") {
						mdesCribe += "地胡";
						huFlagImg.SetActive (true);
					} else if (hupaiObj.type == "chaochaoqidui") {
						mdesCribe += "超超豪华七对";
						huFlagImg.SetActive (true);
					} else if (hupaiObj.type == "chaoqidui") {
						mdesCribe += "超豪华七对";
						huFlagImg.SetActive (true);
					} else if (hupaiObj.type == "haohuaqidui") {
						mdesCribe += "豪华七对";
						huFlagImg.SetActive (true);
					} else if (hupaiObj.type == "quanfengtou") {
						mdesCribe += "全风头";
						huFlagImg.SetActive (true);
					} else if (hupaiObj.type == "qingyise") {
						mdesCribe += "清一色";
						huFlagImg.SetActive (true);
					} else if (hupaiObj.type == "hunyise") {
						mdesCribe += "混一色";
						huFlagImg.SetActive (true);
					} else if (hupaiObj.type == "quanyaojiu") {
						mdesCribe += "全幺九";
						huFlagImg.SetActive (true);
					} else if (hupaiObj.type == "hunyaojiu") {
						mdesCribe += "混幺九";
						huFlagImg.SetActive (true);
					} else if (hupaiObj.type == "shisanyao") {
						mdesCribe += "十三幺";
						huFlagImg.SetActive (true);
					} else if (hupaiObj.type == "shibaluohan") {
						mdesCribe += "十八罗汉";
						huFlagImg.SetActive (true);
					} else if (hupaiObj.type == "kankanhu") {
						mdesCribe += "坎坎胡";
						huFlagImg.SetActive (true);
					} else if (hupaiObj.type == "pengpenghu") {
						mdesCribe += "碰碰胡";
						huFlagImg.SetActive (true);
					} else if (hupaiObj.type == "haidilao") {
						mdesCribe += "海底捞X2";
						huFlagImg.SetActive (true);
					}else if (hupaiObj.type == "dasixi") {
						mdesCribe += "大四喜";
						huFlagImg.SetActive (true);
					}else if (hupaiObj.type == "xiaosixi") {
						mdesCribe += "小四喜";
						huFlagImg.SetActive (true);
					}
					else if (hupaiObj.type == "dasanyuan") {
						mdesCribe += "大三元";
						huFlagImg.SetActive (true);
					}
					else if (hupaiObj.type == "xiaosanyuan") {
						mdesCribe += "小三元";
						huFlagImg.SetActive (true);
					}
					//瑞金
					else if (hupaiObj.type == "tian_hu") {
						mdesCribe += "天胡";
						huFlagImg.SetActive (true);
					}else if (hupaiObj.type == "di_hu") {
						mdesCribe += "地胡";
						huFlagImg.SetActive (true);
					}else if (hupaiObj.type == "jing_diao") {
						mdesCribe += "飞牌";
						huFlagImg.SetActive (true);
					}

					if (!isJian) {
						paiArray [hupaiObj.cardPiont] -= 1;
						isJian = true;
					}
				}

				mdesCribe += " ";
			}
		}

		if (mHupaiResponseItemData.huType != null) {
			mdesCribe += mHupaiResponseItemData.huType;
		}
			
		resultDes.text = mdesCribe;
		maPais = parms.getMaPoints ();
		arrangePai (hupaiStr);
	}


	/**整理牌**/
	private void arrangePai(string hu){
		
		float startPosition = 30f;
		GameObject itemTemp;

		int subPaiConut = 0;
		if(gangPaiList!=null){
			for (int i = 0; i < gangPaiList.Count; i++) {
				GangpaiObj itemgangData = gangPaiList [i];
				for (int j = 0; j < 4; j++) {

					itemTemp = Instantiate (Resources.Load ("Prefab/ThrowCard/TopAndBottomCard")) as GameObject;
					itemTemp.transform.parent = paiArrayPanel.transform;
				//	itemTemp.transform.localScale = new Vector3(0.8f,0.8f,1f);
					itemTemp.transform.localScale = Vector3.one;
					itemTemp.GetComponent<TopAndBottomCardScript> ().setPoint (itemgangData.cardPiont);
					itemTemp.transform.localPosition = new Vector3 (startPosition+((i*4)+j)*36f, 0, 0);

				}
			}
			startPosition = startPosition + (gangPaiList.Count > 0 ? (gangPaiList.Count * 4 * 36f +8f) : 0f);
		}



		if (pengpaiList != null) {
			for (int i = 0; i < pengpaiList.Length; i++) {
				string cardPoint = pengpaiList [i];
				for (int j = 0; j < 3; j++) {

					itemTemp = Instantiate (Resources.Load ("Prefab/ThrowCard/TopAndBottomCard")) as GameObject;
					itemTemp.transform.parent = paiArrayPanel.transform;
					//itemTemp.transform.localScale = new Vector3(0.8f,0.8f,1f);
					itemTemp.transform.localScale = Vector3.one;
					itemTemp.GetComponent<TopAndBottomCardScript> ().setPoint (int.Parse(cardPoint));

					itemTemp.transform.localPosition = new Vector3 (startPosition+((i*3)+j)*36f, 0, 0);


				}
			}
			startPosition =startPosition+ (pengpaiList.Length > 0 ? (pengpaiList.Length * 3 * 36f + 8f) : 0f);

		}



		if (chipaiList != null) {
			for (int i = 0; i < chipaiList.Count; i++) {
				ChipaiObj itemgangData = chipaiList [i];
				for (int j = 0; j < 3; j++) {

					itemTemp = Instantiate (Resources.Load ("Prefab/ThrowCard/TopAndBottomCard")) as GameObject;
					itemTemp.transform.parent = paiArrayPanel.transform;
					//itemTemp.transform.localScale = new Vector3(0.8f,0.8f,1f);
					itemTemp.transform.localScale = Vector3.one;
					itemTemp.GetComponent<TopAndBottomCardScript> ().setPoint (int.Parse(itemgangData.cardPionts[j]));

					itemTemp.transform.localPosition = new Vector3 (startPosition+((i*3)+j)*36f, 0, 0);


				}
			}

			startPosition =startPosition+  (chipaiList.Count > 0 ? (chipaiList.Count * 3 * 36f + 8f) : 0f);
		}


		for(int i=0 ; i<paiArray.Length ;i++){
			if (paiArray [i] > 0) {

				for (int j = 0; j < paiArray [i]; j++) {
					itemTemp = Instantiate (Resources.Load ("Prefab/ThrowCard/TopAndBottomCard")) as GameObject;
					itemTemp.transform.parent = paiArrayPanel.transform;
					//itemTemp.transform.localScale = new Vector3(0.8f,0.8f,1f);
					itemTemp.transform.localScale = Vector3.one;
					itemTemp.GetComponent<TopAndBottomCardScript> ().setPoint (i);
					if (i == GlobalDataScript.roomVo.guiPai || i == GlobalDataScript.roomVo.guiPai2)
						itemTemp.GetComponent<TopAndBottomCardScript> ().showGuiIcon ();

					itemTemp.transform.localPosition = new Vector3 (startPosition+subPaiConut*36f, 0, 0);

					subPaiConut += 1;
				}

			}
		}
		MyDebug.Log ("subPaiConut:"+subPaiConut);

		startPosition =startPosition +  (subPaiConut * 36f + 8f);
		if (hu != null) {
			if (hu.Contains("_self") || hu.Contains("zi_common")) {
				itemTemp = Instantiate (Resources.Load ("Prefab/ThrowCard/TopAndBottomCard")) as GameObject;
				itemTemp.transform.parent = paiArrayPanel.transform;
				//itemTemp.transform.localScale = new Vector3 (0.8f, 0.8f, 1f);
				itemTemp.transform.localScale = Vector3.one;
				itemTemp.GetComponent<TopAndBottomCardScript> ().setPoint (hupaiObj.cardPiont);
				if (hupaiObj.cardPiont == GlobalDataScript.roomVo.guiPai || hupaiObj.cardPiont == GlobalDataScript.roomVo.guiPai2)
					itemTemp.GetComponent<TopAndBottomCardScript> ().showGuiIcon ();
				itemTemp.transform.localPosition = new Vector3 (startPosition, 0, 0);
			}
			startPosition = startPosition + 36f + 52f;
		} else {
			startPosition = startPosition  + 52f;
		}


        //显示中码牌信息==>广东麻将单独处理
		if (GlobalDataScript.roomVo.roomType == 4
			|| GlobalDataScript.roomVo.roomType == 8
			|| GlobalDataScript.roomVo.roomType == 10)
        {
			if (hu != null && hu!="" && (hu.IndexOf ("zi_") != -1 || hu.IndexOf ("_self") != -1)) {
				for (int i = 0; i < validMas.Count; i++) {
					itemTemp = Instantiate (Resources.Load ("Prefab/ThrowCard/ZhongMa")) as GameObject;
					zhongMaFlag.transform.gameObject.SetActive (true);
					itemTemp.transform.parent = paiArrayPanel.transform;
					itemTemp.GetComponent<TopAndBottomCardScript> ().setPoint (validMas [i]);
					itemTemp.transform.localScale = new Vector3 (0.8f, 0.8f, 1f);
					itemTemp.transform.localPosition = new Vector3 ((20 + i) * 36f, 0, 0);                
				}
			}
        }
        else
        {
            if (maPais != null && maPais.Count > 0)
            {
                for (int i = 0; i < maPais.Count; i++)
                {
                    itemTemp = Instantiate(Resources.Load("Prefab/ThrowCard/ZhongMa")) as GameObject;
                    if (isMaValid(maPais[i]))
                    {
                        zhongMaFlag.transform.gameObject.SetActive(true);
                    }
                    else {
                        zhongMaFlag.transform.gameObject.SetActive(false);
                    }

                    itemTemp.transform.parent = paiArrayPanel.transform;
                    itemTemp.GetComponent<TopAndBottomCardScript>().setPoint(maPais[i]);
                    itemTemp.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
                    itemTemp.transform.localPosition = new Vector3((20 + i) * 36f, 0, 0);
                }
            }
		}

		//划水
		if (GlobalDataScript.roomVo.roomType == (int)GameType.GameType_MJ_HuaShui) {
			itemTemp = Instantiate (Resources.Load ("Prefab/Image_yu")) as GameObject;
			itemTemp.transform.parent = paiArrayPanel.transform;
			itemTemp.GetComponent<yuSetScript> ().setCount (GlobalDataScript.roomVo.xiaYu);
			itemTemp.transform.localScale =  Vector3.one;
			itemTemp.transform.localPosition = new Vector3 (20*36f, 0, 0);
		}




	}




	private bool isMaValid(int cardPonit){
		for (int i = 0; i < validMas.Count; i++) {
			if (cardPonit == validMas [i]) {
				return true;		
			}
		}
		return false;
	}




}
