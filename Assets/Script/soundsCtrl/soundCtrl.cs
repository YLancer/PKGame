using UnityEngine;
using System.Collections;
/**
 * sound control class
 * 
 * author :kevin
 * 
 * */
public class SoundCtrl : MonoBehaviour
{

    private Hashtable soudHash = new Hashtable();

    private static SoundCtrl _instance;

    private static AudioSource audioS;
    private static AudioSource cardSounPlay;
    private static AudioSource ActionSounPlay;
    private static AudioSource MessageSounPlay;
    public static SoundCtrl getInstance()
    {
        if (_instance == null)
        {
            _instance = new SoundCtrl();
            audioS = GameObject.Find("MyAudio").GetComponent<AudioSource>();
            cardSounPlay = GameObject.Find("cardSoundPlay").GetComponent<AudioSource>();
            ActionSounPlay = GameObject.Find("ActionSound").GetComponent<AudioSource>();
            MessageSounPlay = GameObject.Find("MessageSound").GetComponent<AudioSource>();
		

        }

        return _instance;
    }

    public void playSound(int cardPoint, int sex)
    {
        Debug.Log("----------------------------------------------------playSound");

        if (GlobalDataScript.soundToggle)
        {
            string path = "Sounds/";
			if (GlobalDataScript.isYueYu) {
				if (sex == 1)
				{
					path += "man_rj_rj/" + (cardPoint + 1);
				}
				else {
					path += "woman_rj_rj/" + (cardPoint + 1);
				}
			} else {
				if (sex == 1)
				{
					path += "man_rj_pt/" + (cardPoint + 1);
				}
				else {
					path += "woman_rj_pt/" + (cardPoint + 1);
				}
			}
            
            AudioClip temp = (AudioClip)soudHash[path];
            if (temp == null)
            {
                temp = GameObject.Instantiate(Resources.Load(path)) as AudioClip;
                soudHash.Add(path, temp);
            }
            cardSounPlay.volume = GlobalDataScript.yinxiaoVolume;
            cardSounPlay.clip = temp;
            cardSounPlay.loop = false;
            cardSounPlay.Play();
        }
    }

	public void playSound_DN(bool isSuccess)
	{
		Debug.Log("----------------------------------------------------playSound");

		if (GlobalDataScript.soundToggle)
		{
			string path = "Sounds/dn/";
			if (isSuccess)
				path += "success";
			else path += "fail";
			AudioClip temp = (AudioClip)soudHash[path];
			if (temp == null)
			{
				temp = GameObject.Instantiate(Resources.Load(path)) as AudioClip;
				soudHash.Add(path, temp);
			}
			cardSounPlay.volume = GlobalDataScript.yinxiaoVolume;
			cardSounPlay.clip = temp;
			cardSounPlay.loop = false;
			cardSounPlay.Play();
		}
	}

    public void playMessageBoxSound(int codeIndex, int sex,int soundType =1)
    {
        Debug.Log("----------------------------------------------------playMessageBoxSound");
        if (GlobalDataScript.soundToggle)
        {
            string path = "";
			if (soundType == 1) {
				if (sex == 1)
					path = "Sounds/other/boy/fix_msg_" + codeIndex;
				else
					path = "Sounds/other/girl/fix_msg_" + codeIndex;
			} else if (soundType == 2) {
				if (sex == 1)
					path = "Sounds/other/bqboy_";
				else
					path = "Sounds/other/bqgirl_";
				if (GlobalDataScript.isYueYu)
					path += "rj/";
				else
					path += "pt/";
				path += codeIndex > 9 ? ("" + codeIndex) : "0" + codeIndex;
			} else if (soundType == 3) {
				if (sex == 1) {
//					if (GlobalDataScript.roomVo.gameType == 0)
//						path = "Sounds/other/dyboy_";
//					else if (GlobalDataScript.roomVo.gameType == 3)
//						path = "Sounds/dn/boy/chat";

					if (GlobalDataScript.roomVo.gameType == 3) {
						path = "Sounds/dn/boy/chat";
					} else {
						path = "Sounds/other/dyboy_";
					}
				} else {
//					if (GlobalDataScript.roomVo.gameType == 0)
//						path = "Sounds/other/dygirl_";
//					else if (GlobalDataScript.roomVo.gameType == 3)
//						path = "Sounds/dn/girl/chat";

					if (GlobalDataScript.roomVo.gameType == 3) {
						path = "Sounds/dn/girl/chat";
					} else {
						path = "Sounds/other/dygirl_";
					}
				}
				if (soundType == 3 && GlobalDataScript.roomVo.gameType == 3) {
					path += codeIndex;
				} else {
					if (GlobalDataScript.isYueYu)
						path += "rj/";
					else
						path += "pt/";
					path += codeIndex > 9 ? ("" + codeIndex) : "0" + codeIndex;
				}
			}
            AudioClip temp = (AudioClip)soudHash[path];
            if (temp == null)
            {
                temp = GameObject.Instantiate(Resources.Load(path)) as AudioClip;
                soudHash.Add(path, temp);
            }
            MessageSounPlay.volume = GlobalDataScript.yinxiaoVolume;
            MessageSounPlay.clip = temp;
            MessageSounPlay.Play();
            //if (audioS != null)
            //    audioS.volume = 1;


        }
    }

    public void playBGM(int type)
    {
        string path = "";
        switch (type)
        {
            case 0:
                //audioS.loop = false;
                //audioS.Stop();
				audioS.volume = 0;
                return;
            case 1:
				path = "Sounds/bgm1";
                break;
            case 2:
                path = "Sounds/bgm1";
				audioS.volume = GlobalDataScript.yinyueVolume;
                break;
        }
        AudioClip temp = (AudioClip)soudHash[path];
        if (temp == null)
        {
            temp = GameObject.Instantiate(Resources.Load(path)) as AudioClip;
            soudHash.Add(path, temp);
        }
        //audioS.volume = GlobalDataScript.yinyueVolume;
        audioS.clip = temp;
        audioS.loop = true;
        audioS.Play();
        if (GlobalDataScript.soundToggle)
        {
            audioS.mute = false;
        }
        else {
            audioS.mute = true;
        }
    }



    public void playSoundByAction(string str, int sex)
    {
        Debug.Log("----------------------------------------------------playSoundByAction");

        string path = "Sounds/";
		if (GlobalDataScript.isYueYu) {
			if (sex == 1) {
				path += "man_rj_rj/" + str;
			} else {
				path += "woman_rj_rj/" + str;
			}
		} else {
			if (sex == 1) {
				path += "man_rj_pt/" + str;
			} else {
				path += "woman_rj_pt/" + str;
			}
		}
        AudioClip temp = (AudioClip)soudHash[path];
        if (temp == null)
        {
            temp = GameObject.Instantiate(Resources.Load(path)) as AudioClip;
            soudHash.Add(path, temp);
        }

        ActionSounPlay.volume = GlobalDataScript.yinxiaoVolume;
        ActionSounPlay.clip = temp;
        ActionSounPlay.Play();
        //if (audioS != null)
        //    audioS.volume = 1;
    }

	public void playSoundByNiu(int niu,int sex = 1){
		Debug.Log("----------------------------------------------------playSoundByAction");
		if (niu >= 10 && niu <= 13)
			niu = 10;
		
		string path = "Sounds/dn/";
		if (sex == 1) path += "boy/niu";
		else path += "girl/niu";
		path += niu;

		AudioClip temp = (AudioClip)soudHash[path];
		if (temp == null)
		{
			temp = GameObject.Instantiate(Resources.Load(path)) as AudioClip;
			soudHash.Add(path, temp);
		}

		ActionSounPlay.volume = GlobalDataScript.yinxiaoVolume;
		ActionSounPlay.clip = temp;
		ActionSounPlay.Play();
	}

	public void playSoundByDZPK(string str, int sex = 1)
	{
		Debug.Log("----------------------------------------------------playSoundByAction");

		string path = "Sounds/dzpk/";
		if (sex == 1) {
			path += "boy/" + str;
		} else {
			path += "girl/" + str;
		}
		AudioClip temp = (AudioClip)soudHash[path];
		if (temp == null)
		{
			temp = GameObject.Instantiate(Resources.Load(path)) as AudioClip;
			soudHash.Add(path, temp);
		}

		ActionSounPlay.volume = GlobalDataScript.yinxiaoVolume;
		ActionSounPlay.clip = temp;
		ActionSounPlay.Play();
		//if (audioS != null)
		//    audioS.volume = 1;
	}



	public void playSoundByActionButton(int type)
	{
		Debug.Log("----------------------------------------------------playSoundByAction");

		string path = "Sounds/other/";
		//按钮
		if (type == 1)
		{
			path += "clickbutton";
			//发牌
		}else if (type == 2)
		{
			path += "dice";
			//准备
		}else if (type == 3)
		{
			path += "ready";
			//打牌
		}else if (type == 4)
		{
			//path += "tileout";
			path += "out";
			//摸牌
		} else if (type == 5)
		{
			path += "select";
		} else if (type == 6)
		{
			//path += "tileout";
			path += "tileout";
			//摸牌
		}
        else if (type == 7)
        {
            //path += "tileout";
            path += "touzi";
            //骰子
        }
        AudioClip temp = (AudioClip)soudHash[path];
		if (temp == null)
		{
			temp = GameObject.Instantiate(Resources.Load(path)) as AudioClip;
			soudHash.Add(path, temp);
		}

		ActionSounPlay.volume = GlobalDataScript.yinxiaoVolume;
		ActionSounPlay.clip = temp;
		ActionSounPlay.Play();
		//if (audioS != null)
		//	audioS.volume = 1;
	}

	public void playMfbqSound(int type) {
		string path = "Sounds/other/mfbq/";

		if (type == 1) {
			path += "mf_zhadan";
		}else if (type == 2) {
			path += "mf_zhuoji";
		}else if (type == 3) {
			path += "mf_meigui";
		}else if (type == 4) {
			path += "mf_lengjing";
		} else if (type == 5) {
			path += "mf_xihongshi";
		}

		AudioClip temp = (AudioClip)soudHash[path];
		if (temp == null)
		{
			temp = GameObject.Instantiate(Resources.Load(path)) as AudioClip;
			soudHash.Add(path, temp);
		}

		ActionSounPlay.volume = GlobalDataScript.yinxiaoVolume;
		ActionSounPlay.clip = temp;
		ActionSounPlay.Play();
	}

}
