using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RankNiu : MonoBehaviour {

	public List<Transform> cardsTransformList;
	public Text txtReuslt;
	public Text txtCard1;
	public Text txtCard2;
	public Text txtCard3;

	private int count = 0;
	private int sum = 0;
	private int niu = 0;
	private int[] index = { -1, -1, -1 };
	// Use this for initialization
	void Start () {
		count = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void setNiu(int _niu){
		niu = _niu;
	}
	public void setCards(int point,int pos){
		/**
		if (type == 1) {
			Transform tr = cardsTransformList [count];
			GameObject gob = Instantiate (Resources.Load ("prefab/pdk/HandCard_ss")) as GameObject;
			gob.transform.SetParent (tr);
			gob.GetComponent<pdkCardScript> ().setPoint (point, 3);
			count++;
			int po = point % 13 + 1;
			po = po > 9 ? 10 : po;
			sum += po;
			if (count == 3) {

				if (sum % 10 == 0) {
					string niuStr = "";
					if (niu == 10) {
						niuStr = "牛牛";
					} else if (niu == 11) {
						niuStr = "五花牛";
					} else if (niu == 12) {
						niuStr = "顺牛";
					} else if (niu != 0) {
						niuStr = "牛" + niu;
					}
					txtReuslt.text = niuStr;
				} else {
					txtReuslt.text = "" + sum;

					for (int i = 0; i < cardsTransformList.Count; i++) {
						Transform tr1 = cardsTransformList [i];
						Destroy (tr1.GetChild (0).gameObject);
					}
					count = 0;
					sum = 0;
					txtReuslt.text = "";
				}
			}
		} else if (type == 2) {
		*/
		/**
		if (ArrayList.IndexOf (pos) != -1) {
		}
		*/
		int po = point % 13 + 1;
		po = po > 9 ? 10 : po;
		sum += po;
		switch (count) {
		case 0:
			if (index [0] == -1) {
				txtCard1.text = "" + po;
				index [0] = pos;
			}
			break;
		case 1:
			if (index [1] == -1) {
				txtCard2.text = "" + po;
				index [1] = pos;
			}
			break;
		case 2:
			if (index [2] == -1) {
				txtCard3.text = "" + po;
				index [2] = pos;
			}
			break;
		default:
			break;
		}

		count++;
	}

}
