using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class guanggaoScript : MonoBehaviour {

	public Image content;
	// Use this for initialization
	void Start () {
		setContent ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setContent(){
		content.sprite = GlobalDataScript.gdSprite;
	}

	public void closeGD()
	{
		Destroy (this);
		Destroy (gameObject);
	}
}
