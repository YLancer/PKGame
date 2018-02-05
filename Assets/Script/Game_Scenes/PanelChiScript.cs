using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;

public class PanelChiScript : MonoBehaviour {

	public GameObject cardPoint;
	public GameObject onePoint;
	public GameObject twoPoint;

	public int card;
	public int one;
	public int two;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setChiPoint(int cardNum, int oneNum, int twoNum)
	{
		card = cardNum;
		one = oneNum;
		two = twoNum;

		int[] result = paixu (cardNum, oneNum, twoNum);
		twoPoint.GetComponent<bottomScript> ().setPoint (result[0]);
		if (result [0] == card)
			twoPoint.GetComponent<bottomScript> ().ShowChi ();
		onePoint.GetComponent<bottomScript> ().setPoint (result[1]);
		if (result [1] == card)
			onePoint.GetComponent<bottomScript> ().ShowChi ();
		cardPoint.GetComponent<bottomScript> ().setPoint (result[2]);
		if (result [2] == card)
			cardPoint.GetComponent<bottomScript> ().ShowChi ();
	}

	public void click()
	{
		CardVO cardvo = new CardVO ();
		cardvo.cardPoint = card;
		cardvo.onePoint = one;
		cardvo.twoPoint = two;
		CustomSocket.getInstance ().sendMsg (new ChiRequest (cardvo));

		GameObject passButton = GameObject.Find("Canvas/container/Panel_GamePlay(Clone)/PassButton2");
		if(passButton!=null)
			passButton.SetActive (false);
		for(int i = 0;i<this.transform.parent.childCount;i++)
		{
			GameObject go = this.transform.parent.GetChild(i).gameObject;
			Destroy(go);
		}
	}

	public int[] paixu (int cardPoint, int onePoint, int twoPoint)
	{
		int[] result = new int[3];
		if (cardPoint < onePoint) {
			if (cardPoint < twoPoint) {
				result [0] = cardPoint;
				if (onePoint < twoPoint) {
					result [1] = onePoint;
					result [2] = twoPoint;
				} else {
					result [1] = twoPoint;
					result [2] = onePoint;
				}
			} else {
				result [0] = twoPoint;
				result [1] = cardPoint;
				result [2] = onePoint;
			}
		} else {
			if (cardPoint > twoPoint) {
				result [2] = cardPoint;
				if (onePoint < twoPoint) {
					result [0] = onePoint;
					result [1] = twoPoint;
				} else {
					result [0] = twoPoint;
					result [1] = onePoint;
				}
			} else {
				result [0] = onePoint;
				result [1] = cardPoint;
				result [2] = twoPoint;
			}
		}

		return result;
	}
}
