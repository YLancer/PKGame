using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class biaoqingScript : MonoBehaviour {

	public Image[] biaoqingList;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Image getBiaoqing(int index){
		return biaoqingList [index];
	}
}
