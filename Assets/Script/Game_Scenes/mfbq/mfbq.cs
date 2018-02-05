using UnityEngine;
using System.Collections;
using System;

public class mfbq : MonoBehaviour {


	// Use this for initialization
	void Start () {
		//StartCoroutine(MoveToPosition(new Vector3(0, 0, 0)));
	}

	public void setSrcDest(int srcIndex, int destIndex){
		switch(srcIndex){
		case 0:
			{
				switch (destIndex) {
				case 1:
					setSrcPosition (new Vector3(-1193, -196, 0));
					break;
				case 2:
					setSrcPosition (new Vector3(-231, -426, 0));
					break;
				case 3:
					setSrcPosition (new Vector3(-2, -186, 0));
					break;
				default:
					Debug.LogError ("mfbq.cs->setSrcDest() error");
					break;
				}
			}
			break;
		case 1:
			switch (destIndex) {
			case 0:
				setSrcPosition (new Vector3(1187, 195, 0));
				break;
			case 2:
				setSrcPosition (new Vector3(965, -231, 0));
				break;
			case 3:
				setSrcPosition (new Vector3(1180, 10, 0));
				break;
			default:
				Debug.LogError ("mfbq.cs->setSrcDest() error");
				break;
			}
			break;
		case 2:
			switch (destIndex) {
			case 0:
				setSrcPosition (new Vector3(223, 422, 0));
				break;
			case 1:
				setSrcPosition (new Vector3(-968, 229, 0));
				break;
			case 3:
				setSrcPosition (new Vector3(224, 239, 0));
				break;
			default:
				Debug.LogError ("mfbq.cs->setSrcDest() error");
				break;
			}
			break;
		case 3:
			switch (destIndex) {
			case 0:
				setSrcPosition (new Vector3(-2, 186, 0));
				break;
			case 1:
				setSrcPosition (new Vector3(-1193, -12, 0));
				break;
			case 2:
				setSrcPosition (new Vector3(-227, -239, 0));
				break;
			default:
				Debug.LogError ("mfbq.cs->setSrcDest() error");
				break;
			}
			break;
		}
	}

//	private void setDest(int destIndex){
//		switch (destIndex) {
//		case 0:
//			
//			break;
//		case 1:
//			break;
//		case 2:
//			break;
//		case 3:
//			break;
//		}
//	}

	private void setSrcPosition(Vector3 src){
		gameObject.transform.localPosition = src;
		StartCoroutine(MoveToPosition(new Vector3(0,0,0)));
	}

	public void setPosition(){
		
		//gameObject.transform.position = new Vector2 (-100, 10);
		transform.position = new Vector2(540,360);
		Debug.LogError ("position.x:"+transform.position.x+",position.y:"+transform.position.y);
	}

	public void setPosition2(){

		//gameObject.transform.position = new Vector2 (-100, 10);
		transform.position = new Vector2(568,385);
		Debug.LogError ("position.x:"+transform.position.x+",position.y:"+transform.position.y);
	}


	public void destroy(){
		Destroy (gameObject);
	}


	IEnumerator MoveToPosition(Vector3 dest) {
		//while (transform.position != dest) {
		while (Math.Abs(0 - transform.localPosition.x)>1 || Math.Abs(0 - transform.localPosition.y)>1){
			//transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(10, 193, 0), 200 * Time.deltaTime); 
			transform.Translate((0-transform.localPosition.x)*Time.deltaTime*4, (0-transform.localPosition.y)*Time.deltaTime*4, 0);
			yield return 0;  
		}
		GetComponent<Animator> ().enabled = true;
	}

	public void OnDestroy(){
		Destroy (gameObject);
	}



}
