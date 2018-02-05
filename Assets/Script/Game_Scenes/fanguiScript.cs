using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class fanguiScript : MonoBehaviour {

	public Image fanPoint;
	public Image gui1;
	public Image gui2;
	public Image gui11;
	public Image gui22;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setFanPoint(int iGui){
		Sprite sp = Resources.Load ("Cards/Big/b" + iGui.ToString (), typeof(Sprite)) as Sprite;
		fanPoint.sprite = sp;
	}

	public void setGui1(int iGui){
		Sprite sp = Resources.Load ("Cards/Big/b" + iGui.ToString (), typeof(Sprite)) as Sprite;
		gui1.sprite = sp;
		gui11.sprite = sp;
	}

	public void setGui2(int iGui){
		Sprite sp = Resources.Load ("Cards/Big/b" + iGui.ToString (), typeof(Sprite)) as Sprite;
		gui2.sprite = sp;
		gui22.sprite = sp;
	}

}
