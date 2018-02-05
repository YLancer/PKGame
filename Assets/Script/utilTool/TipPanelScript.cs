using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class TipPanelScript : MonoBehaviour {

	public Text label;

	// Use this for initialization
	void Start () {
	
	}

	public void setText(string str){
		label.text = str;
	}

	public void startAction(){
		Invoke ("TipsMoveCompleted",1f);
	}
    public void startAction1()
    {
        Invoke("TipsMoveCompleted", 0f);
    }

	private void move(){
		gameObject.transform.DOLocalMove (new Vector3(0,-100),0.7f).OnComplete(TipsMoveCompleted);
	}

	public void TipsMoveCompleted(){

		Destroy (gameObject);
	}

    public void close() {
        Destroy(gameObject);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
