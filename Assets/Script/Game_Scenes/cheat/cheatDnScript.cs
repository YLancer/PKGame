using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using AssemblyCSharp;

public class cheatDnScript : MonoBehaviour, IPointerClickHandler
{
	public int cardPoint = -1;
	public Image pointImage;
	public Image kingPointImage;
	public Image typeImage;
	public Image centerImage;
	public Image kingCenterImage;
	public GameObject select_frame;

	private bool isSelect = false;

	public delegate void EventHandler(GameObject obj);
	public event EventHandler onCardSelect;


	public void OnPointerClick(PointerEventData eventData){
		if (!isSelect) {
			isSelect = true;
			setSelectFrame (true);
			if (onCardSelect != null)     //发送消息
			{
				onCardSelect(gameObject);//发送当前游戏物体消息
			}
		}

	}



	public void setPoint(int _cardPoint,int gameType = 1)
	{

		if (gameType == 1) {
			int point = _cardPoint % 13;
			int type = _cardPoint / 13;

			cardPoint = _cardPoint;//设置所有牌指针
			typeImage.sprite = Resources.Load ("pdk/card/type" + type, typeof(Sprite)) as Sprite;

			point = (point + 3) > 13 ? (point - 10) : (point + 3);
			if (type == 0 || type == 2)
				pointImage.sprite = Resources.Load ("pdk/card/b_" + point, typeof(Sprite)) as Sprite;
			else
				pointImage.sprite = Resources.Load ("pdk/card/r_" + point, typeof(Sprite)) as Sprite;
		} else if (gameType == 3) {
			GlobalDataScript.isDrag = false;
			if (_cardPoint == 52) {
				cardPoint = _cardPoint;

				typeImage.gameObject.SetActive (false);
				pointImage.gameObject.SetActive (false);
				centerImage.gameObject.SetActive (false);
				kingPointImage.gameObject.SetActive (true);
				kingCenterImage.gameObject.SetActive (true);

				kingCenterImage.sprite = Resources.Load ("pdk/card/" + "20_1", typeof(Sprite)) as Sprite;
				kingPointImage.sprite = Resources.Load ("pdk/card/" + "20_2", typeof(Sprite)) as Sprite;

			} else if (_cardPoint == 53) {
				cardPoint = _cardPoint;

				typeImage.gameObject.SetActive (false);
				pointImage.gameObject.SetActive (false);
				centerImage.gameObject.SetActive (false);
				kingPointImage.gameObject.SetActive (true);
				kingCenterImage.gameObject.SetActive (true);

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
		}
	}

	public int getPoint(){
		return cardPoint;
	}


	public void setSelectFrame(bool flag){
		select_frame.SetActive (flag);
	}
}


