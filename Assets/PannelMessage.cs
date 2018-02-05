using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PannelMessage : MonoBehaviour {

	public Text message;
	// Use this for initialization
	void Start () {
		if (GlobalDataScript.noticeMegs != null && GlobalDataScript.noticeMegs.Count > 0)
			message.text = GlobalDataScript.noticeMegs [GlobalDataScript.noticeMegs.Count - 1];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void close(){
		Destroy (gameObject);
	}
}
