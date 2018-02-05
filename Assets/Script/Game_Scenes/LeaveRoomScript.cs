using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LeaveRoomScript : MonoBehaviour {

    // Use this for initialization


    public Text tip;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setTip(string text) {
        tip.text = text;
    }
}
