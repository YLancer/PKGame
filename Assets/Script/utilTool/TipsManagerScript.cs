﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class TipsManagerScript {

	private static TipsManagerScript _instance;
	public Transform parent;
	public TipsManagerScript(){

	}

	public static TipsManagerScript getInstance(){
		if (_instance == null) {
			_instance = new TipsManagerScript ();
		}
		return _instance;
	}

	public void setTips(string str){
		GameObject temp = GameObject.Instantiate (Resources.Load ("Prefab/TipPanel") as GameObject);
		temp.transform.parent = GlobalDataScript.getInstance ().canvsTransfrom;
        
		temp.transform.localScale = Vector3.one;
		temp.transform.localPosition =new Vector3 (0,0);
		temp.GetComponent<TipPanelScript> ().setText (str);
		//temp.GetComponent<TipPanelScript> ().startAction();

	}

    public void setTipsBig(string str)
    {
		GameObject temp = GameObject.Instantiate (Resources.Load ("Prefab/TipPanel2") as GameObject);
		temp.transform.parent = GlobalDataScript.getInstance ().canvsTransfrom;

		temp.transform.localScale = Vector3.one;
		temp.transform.localPosition =new Vector3 (0,0);
		temp.GetComponent<TipPanelScript> ().setText (str);

    }

	public void loadDialog(string titlestr,string msgstr,DialogPanelScript.ButtonOnClick yesCallBack,DialogPanelScript.ButtonOnClick noCallBack){
		GameObject temp = GameObject.Instantiate (Resources.Load ("Prefab/Image_Base_Dialog") as GameObject);
        
		temp.transform.parent = parent;
		temp.transform.localScale = Vector3.one;
		temp.transform.localPosition = Vector3.zero;
		temp.GetComponent<DialogPanelScript> ().setContent (titlestr,msgstr,true,yesCallBack,noCallBack);
	}

    public void setProgress(string str)
    {
        GameObject temp = GameObject.Instantiate(Resources.Load("Prefab/Panel_waiting") as GameObject);
        temp.transform.parent = parent;

        temp.transform.localScale = Vector3.one;
        temp.transform.localPosition = new Vector3(0, -300);
        temp.GetComponent<TipPanelScript>().setText(str);
    }
    public void delProgress() {
        
    }





}
