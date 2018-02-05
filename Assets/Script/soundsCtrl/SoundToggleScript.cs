using UnityEngine;
using System.Collections;

public class SoundToggleScript : MonoBehaviour {
	public GameObject openBtn;
	public GameObject closeBtn;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void openClick(){
		
		openBtn.SetActive(false);
		closeBtn.SetActive(true);
		GlobalDataScript.soundToggle = true;
        if (GlobalDataScript.soundToggle)
        {
            SoundCtrl.getInstance().playBGM(2);
        }
		SoundCtrl.getInstance().playSoundByActionButton(1);
    }

	public void closeClick(){
		openBtn.SetActive(true);
		closeBtn.SetActive(false);
		GlobalDataScript.soundToggle = false;
        if (!GlobalDataScript.soundToggle) {
            SoundCtrl.getInstance().playBGM(0);
        }
		SoundCtrl.getInstance().playSoundByActionButton(1);
	}
}
