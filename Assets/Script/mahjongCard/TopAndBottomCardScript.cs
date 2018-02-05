using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class TopAndBottomCardScript : MonoBehaviour {
    private int cardPoint;

    //=========================================
	public Image cardImg;
	public Image zhongmaIcon;
	public Image guiIcon;
	public Image baoIcon;
	public Image indexIcon;
    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update () {
	
	}

	public void setPoint(int _cardPoint,int gui = -1)
    {
    
        cardPoint = _cardPoint;//设置所有牌指针
        MyDebug.Log("put out cardPoint" + cardPoint + "----------------------4.2-111-------------");
		cardImg.sprite = Resources.Load("Cards/Small/s"+cardPoint,typeof(Sprite)) as Sprite;
        MyDebug.Log("put out cardPoint" + cardPoint + "----------------------4.2--------------");
		if (gui != -1) {
			if (cardPoint == gui) {
				guiIcon = baoIcon;
				guiIcon.gameObject.SetActive (true);
			}
		}
    }

	public void setLefAndRightPoint(int _cardPoint, int gui=-1){
		cardPoint = _cardPoint;//设置所有牌指针
        MyDebug.Log("put out cardPoint" + cardPoint + "----------------------4.3--------------");
		cardImg.sprite = Resources.Load("Cards/Left&Right/lr"+cardPoint,typeof(Sprite)) as Sprite;
		if (gui != -1) {
			if (cardPoint == gui) {
				guiIcon = baoIcon;
				guiIcon.gameObject.SetActive (true);
			}
		}
	}

    public int getPoint()
    {
        return cardPoint;
    }

	public void showZhongMaIcon(){
		zhongmaIcon.gameObject.SetActive (true);
	}

	public void showGuiIcon(){
		if (GlobalDataScript.roomVo.roomType == 6)
			guiIcon = baoIcon;
		guiIcon.gameObject.SetActive (true);
	}
	public void ShowIndexIcon(int index = -1){
		if (index != -1) {
			indexIcon.sprite = Resources.Load("xianlai/index/"+index,typeof(Sprite)) as Sprite;
			indexIcon.gameObject.SetActive (true);
		}
	}
}
