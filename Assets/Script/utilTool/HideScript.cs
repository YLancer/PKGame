﻿using UnityEngine;
using System.Collections;

public class HideScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void hide(int type = 1){
		
		if (type == 1) {
			Destroy (this);
			Destroy (gameObject);
		} else if (type == 2) {
			gameObject.SetActive (false);
		}

	}
}
