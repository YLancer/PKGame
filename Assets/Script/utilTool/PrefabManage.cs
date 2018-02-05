using System;
using UnityEngine;

/**
 * prefab 管理器
 */ 
public class PrefabManage : MonoBehaviour
{
	public PrefabManage ()
	{
	}
	public static GameObject loadPerfab(string perfabName, int vec = 1){

//		GameObject panelCreateDialog = Instantiate (Resources.Load(perfabName)) as GameObject;
//		panelCreateDialog.transform.parent = GlobalDataScript.getInstance ().canvsTransfrom;
//		if(vec == 1)
//			panelCreateDialog.transform.localScale = Vector3.one;
//		else if(vec == 2)
//			panelCreateDialog.transform.localScale = new Vector3(2,2,1);
//		panelCreateDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
//		panelCreateDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
//		return panelCreateDialog;

		//0828 @@#
		GameObject panelCreateDialog = Instantiate (Resources.Load(perfabName)) as GameObject;
		panelCreateDialog.transform.parent = GlobalDataScript.getInstance ().canvsTransfrom;
		if (vec == 1) {
			panelCreateDialog.transform.localScale = Vector3.one;
			panelCreateDialog.GetComponent<RectTransform> ().offsetMax = new Vector2 (0f, 0f);
			panelCreateDialog.GetComponent<RectTransform> ().offsetMin = new Vector2 (0f, 0f);
		} else if (vec == 2) {
			panelCreateDialog.transform.localScale = new Vector3 (2, 2, 1);
			panelCreateDialog.GetComponent<RectTransform> ().offsetMax = new Vector2 (0f, 0f);
			panelCreateDialog.GetComponent<RectTransform> ().offsetMin = new Vector2 (0f, 0f);
		} else if (vec == 3) {
			panelCreateDialog.transform.localScale = Vector3.one;
			panelCreateDialog.transform.localPosition = new Vector3 (0, 0);
		}
		return panelCreateDialog;
	}

	public static GameObject loadPerfab(GameObject perfabName){

		GameObject panelCreateDialog = Instantiate (perfabName) as GameObject;
		panelCreateDialog.transform.parent = GlobalDataScript.getInstance ().canvsTransfrom;;
		panelCreateDialog.transform.localScale = Vector3.one;
		panelCreateDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
		panelCreateDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
		return panelCreateDialog;
	}

	public static void showPerfab(GameObject perfabName){

		perfabName.transform.parent = GlobalDataScript.getInstance ().canvsTransfrom;;
		perfabName.transform.localScale = Vector3.one;
		perfabName.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
		perfabName.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
	}
}
