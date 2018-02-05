using UnityEngine;
using System.Collections;

public class pdkButtonActionScript : MonoBehaviour
{
	public GameObject buchuBtn;
	public GameObject tishiBtn;
	public GameObject chupaiBtn;
	public GameObject chupai2Btn;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void showBtn (int type = 2)
	{
		if (type == 2) {
			buchuBtn.SetActive (false);
			tishiBtn.SetActive (true);
			chupaiBtn.SetActive (false);
			chupai2Btn.SetActive (true);
		} else if (type == 3){
			buchuBtn.SetActive (false);
			tishiBtn.SetActive (true);
			chupaiBtn.SetActive (true);
			chupai2Btn.SetActive (false);
		}
	}

	public void cleanBtnShow ()
	{
		buchuBtn.SetActive (false);
		tishiBtn.SetActive (false);
		chupaiBtn.SetActive (false);
		chupai2Btn.SetActive (false);
	}
}

