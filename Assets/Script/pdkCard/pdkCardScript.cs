using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using AssemblyCSharp;

public class pdkCardScript : MonoBehaviour, IPointerDownHandler, 
			IPointerUpHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler
{
	public int cardPoint = -1;
	public Image pointImage;       //点数
	public Image kingPointImage;   //大王
	public Image typeImage;        //花色小
	public Image centerImage;      //中间位置花色大
	public Image kingCenterImage;
	public bool hasShow = false;
	public bool isSmall = false;

	public delegate void EventHandler(GameObject obj);
	public event EventHandler onDragSelect;
	public event EventHandler onCardSelect;

	public bool selected = false;
	public bool dragFlag = false;

	private Vector3 RawPosition;
	private Vector3 oldPosition;

	// Use this for initialization
	void Start ()
	{
		GlobalDataScript.isDrag = true;

		stopPlay ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void stopPlay(){
		if (gameObject.GetComponent<Animator> () != null) {
			gameObject.GetComponent<Animator> ().StopPlayback ();
			gameObject.GetComponent<Animator> ().enabled = false;
		}
	}
	public void startPlay(){
		gameObject.GetComponent<Animator> ().enabled = false;
		gameObject.GetComponent<Animator> ().enabled = true;
		string state = isSmall? "FanPaiS" : "FanPai";
		gameObject.GetComponent<Animator> ().Play (state);
	}

	public void OnDrag(PointerEventData eventData)
	{
		GlobalDataScript.isPdkDrag = true;
		MyDebug.Log ("OnDrag");
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		GlobalDataScript.isPdkDrag = false;
		MyDebug.Log ("OnEndDrag");
		cardDragCallBack ();
	}
		
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (GlobalDataScript.isPdkDrag) {
			dragFlag = true;
			MyDebug.Log ("OnPointerEnter");
		}

	}

	public void OnPointerDown(PointerEventData eventData)
	{
		/*
		if (GlobalDataScript.isDrag) {
			if (selected == false) {
				selected = true;
				oldPosition = transform.localPosition;
			} else {
				cardOutCallBack ();
			}
		}*/

	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (GlobalDataScript.isDrag) {
			cardSelectCallBack ();
		}
	}

	private void cardDragCallBack(){
		if (onDragSelect != null) {
			onDragSelect (gameObject);
		}
	}

	private void cardSelectCallBack(){
		if (onCardSelect != null) {
			onCardSelect (gameObject);
		}
	}

	public void setPoint(int _cardPoint,int gameType = 1)
	{
		
		if (gameType == 1) {
			int point = _cardPoint % 13;
			int type = _cardPoint / 13;

			cardPoint = _cardPoint;//设置所有牌指针
			typeImage.sprite = Resources.Load ("pdk/card/type" + type, typeof(Sprite)) as Sprite;
			centerImage.sprite = Resources.Load ("pdk/card/type" + type, typeof(Sprite)) as Sprite;

			point = (point + 3) > 13 ? (point - 10) : (point + 3);
			if (type == 0 || type == 2)
				pointImage.sprite = Resources.Load ("pdk/card/b_" + point, typeof(Sprite)) as Sprite;
			else
				pointImage.sprite = Resources.Load ("pdk/card/r_" + point, typeof(Sprite)) as Sprite;
            //-lan   48张牌的值变成54张牌的值
            if(_cardPoint ==52)
            {
                cardPoint = _cardPoint;
                kingCenterImage.sprite = Resources.Load("pdk/card/" + "20_1", typeof(Sprite)) as Sprite;
                kingPointImage.sprite = Resources.Load("pdk/card/" + "20_2", typeof(Sprite)) as Sprite;
            }
            else if(_cardPoint ==53)
            {
                cardPoint = _cardPoint;
                kingCenterImage.sprite = Resources.Load("pdk/card/" + "21_1", typeof(Sprite)) as Sprite;
                kingPointImage.sprite = Resources.Load("pdk/card/" + "21_2", typeof(Sprite)) as Sprite;
            }

		} else if (gameType == 3) {
			GlobalDataScript.isDrag = false;
			if (_cardPoint == 52) {
				cardPoint = _cardPoint;
				/**
				typeImage.gameObject.SetActive (false);
				pointImage.gameObject.SetActive (false);
				centerImage.gameObject.SetActive (false);
				kingPointImage.gameObject.SetActive (false);
				kingCenterImage.gameObject.SetActive (false);
				*/
				kingCenterImage.sprite = Resources.Load ("pdk/card/" + "20_1", typeof(Sprite)) as Sprite;
				kingPointImage.sprite = Resources.Load ("pdk/card/" + "20_2", typeof(Sprite)) as Sprite;

			} else if (_cardPoint == 53) {
				cardPoint = _cardPoint;
				/**
				typeImage.gameObject.SetActive (false);
				pointImage.gameObject.SetActive (false);
				centerImage.gameObject.SetActive (false);
				kingPointImage.gameObject.SetActive (false);
				kingCenterImage.gameObject.SetActive (false);
				*/
				kingCenterImage.sprite = Resources.Load ("pdk/card/" + "21_1", typeof(Sprite)) as Sprite;
				kingPointImage.sprite = Resources.Load ("pdk/card/" + "21_2", typeof(Sprite)) as Sprite;
			} else if (_cardPoint >= 0) {
				int point = _cardPoint % 13 + 1;
				int type = _cardPoint / 13;

				cardPoint = _cardPoint;//设置所有牌指针
				typeImage.sprite = Resources.Load ("pdk/card/type" + (3 - type), typeof(Sprite)) as Sprite;
				centerImage.sprite = Resources.Load ("pdk/card/type" + (3 - type), typeof(Sprite)) as Sprite;

				if (type == 1 || type == 3)
					pointImage.sprite = Resources.Load ("pdk/card/b_" + point, typeof(Sprite)) as Sprite;
				else
					pointImage.sprite = Resources.Load ("pdk/card/r_" + point, typeof(Sprite)) as Sprite;
			} else if (_cardPoint == -1) {
				cardPoint = _cardPoint;
				return;
			}
		} else if (gameType == 4) {//德州扑克
			int point = _cardPoint % 13;      //取余运算符
            int type = _cardPoint / 13;       //整除运算符  

            cardPoint = _cardPoint;//设置所有牌指针
			typeImage.sprite = Resources.Load ("pdk/card/type" + type, typeof(Sprite)) as Sprite;
			centerImage.sprite = Resources.Load ("pdk/card/type" + type, typeof(Sprite)) as Sprite;

			point = (point + 2) > 13 ? (point - 11) : (point + 2);
			if (type == 0 || type == 2)
				pointImage.sprite = Resources.Load ("pdk/card/b_" + point, typeof(Sprite)) as Sprite;
			else
				pointImage.sprite = Resources.Load ("pdk/card/r_" + point, typeof(Sprite)) as Sprite;
		}
	}

	public int getPoint(){
		return cardPoint;
	}
	public bool hasAnimation = false;
	public void show (){
		if (!hasAnimation) {
			hasAnimation = true;
			//setPoint (52, 3);
			gameObject.GetComponent<Image> ().sprite = Resources.Load ("pdk/card/bg_big", typeof(Sprite)) as Sprite;
			if (cardPoint > 51) {
				kingPointImage.gameObject.SetActive (true);
				kingCenterImage.gameObject.SetActive (true);
				return;
			}
			typeImage.gameObject.SetActive (true);
			pointImage.gameObject.SetActive (true);
			centerImage.gameObject.SetActive (true);
		} else {
			hasAnimation = false;
			gameObject.GetComponent<Image> ().sprite = Resources.Load ("pdk/card/p", typeof(Sprite)) as Sprite;
			kingPointImage.gameObject.SetActive (false);
			kingCenterImage.gameObject.SetActive (false);
			typeImage.gameObject.SetActive (false);
			pointImage.gameObject.SetActive (false);
			centerImage.gameObject.SetActive (false);
		}
	}
	public void ShowUp(){
		/**
		gameObject.GetComponent<Image> ().sprite = Resources.Load ("pdk/card/bg_big", typeof(Sprite)) as Sprite;

		pointImage.gameObject.transform.rotation = Quaternion.Euler(0f,0f,0f);
		kingPointImage.gameObject.transform.rotation = Quaternion.Euler(0f,0f,0f);
		kingCenterImage.gameObject.transform.rotation = Quaternion.Euler(0f,0f,0f);
		pointImage.gameObject.transform.position = new Vector3(0-pointImage.gameObject.transform.position.x,pointImage.gameObject.transform.position.y,pointImage.gameObject.transform.position.z);
		kingPointImage.gameObject.transform.position = new Vector3(0-pointImage.gameObject.transform.position.x,pointImage.gameObject.transform.position.y,pointImage.gameObject.transform.position.z);
		if (cardPoint > 51) {
			kingPointImage.gameObject.SetActive (true);
			kingCenterImage.gameObject.SetActive (true);
			return;
		}
		typeImage.gameObject.SetActive (true);
		pointImage.gameObject.SetActive (true);
		centerImage.gameObject.SetActive (true);
		*/
	}
}

