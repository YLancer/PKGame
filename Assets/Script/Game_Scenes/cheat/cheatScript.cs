using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class cheatScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{

	private int cardPoint;

	//==================================================
	public Image image;
	public Image guiIcon;
	//
	public delegate void EventHandler(GameObject obj);
	public event EventHandler onSendMessage;

	public void OnPointerClick(PointerEventData eventData){
		if (GlobalDataScript.isDrag) {
			sendObjectToCallBack ();
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{

	}


	public void OnPointerUp(PointerEventData eventData)
	{


	}

	public void setPoint(int _cardPoint, int guiPai=-1)
	{
		cardPoint = _cardPoint;//设置所有牌指针
		image.sprite = Resources.Load("Cards/Big/b"+cardPoint,typeof(Sprite)) as Sprite;

		if (guiPai != -1)
		{ //显示鬼牌icon
			if (_cardPoint == guiPai)
				guiIcon.gameObject.SetActive(true);
		}

	}

	private void sendObjectToCallBack(){
		if (onSendMessage != null)     //发送消息
		{
			onSendMessage(gameObject);//发送当前游戏物体消息
		}
	}

	public void setGuiPoint(int _cardPoint)
	{
		cardPoint = _cardPoint;//设置所有牌指针
		image.sprite = Resources.Load("Cards/Small/s" + cardPoint, typeof(Sprite)) as Sprite;
		guiIcon.gameObject.SetActive(true);
	}

	public void setGuiShow() {
		guiIcon.gameObject.SetActive(true);
	}

	public int getPoint()
	{
		return cardPoint;
	}

	private void destroy()
	{
		// Destroy(this.gameObject);
	}

}
