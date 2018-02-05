using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using LitJson;
using System.Collections.Generic;

public class SettingScript : MonoBehaviour {

    // Use this for initialization
    public Slider yinyueSlider;
    public Slider yinxiaoSlider;
    public GameObject panelExitDialog = null;
    private static AudioSource audioS;
    public Button jiesanBtn;

    public bool canClickButtonFlag = false;
    public int type = 0;
    public List<Toggle> yuyinList;

	public Image dialog_fanhui;

    void Awake() {
        
        
    }

    void Start () {
        if (type == 1)
        {
            jiesanBtn.onClick.AddListener(delegate ()
            {
                this.toExit();
            });
        }
        else if (type == 2)
        {
            jiesanBtn.onClick.AddListener(delegate ()
            {
                this.toJieSan();
            });
        }else if(type ==3)
        {
            jiesanBtn.onClick.AddListener(delegate ()
            {
                this.toLeaveRoom();
            });
        }

        audioS = GameObject.Find("MyAudio").GetComponent<AudioSource>();
        yinyueSlider.value = GlobalDataScript.yinyueVolume;
        yinxiaoSlider.value = GlobalDataScript.yinxiaoVolume;

        if (GlobalDataScript.isYueYu)
        {
            yuyinList[0].isOn = false;
            yuyinList[1].isOn = true;
        }
        else {
            yuyinList[0].isOn = true;
            yuyinList[1].isOn = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    string dissoliveRoomType = "";
    public void toJieSan() {
        if (canClickButtonFlag)
        {
            dissoliveRoomType = "0";
            TipsManagerScript.getInstance().loadDialog("申请解散房间", "你确定要申请解散房间？", doDissoliveRoomRequest, cancle);
        }
        else {
            TipsManagerScript.getInstance().setTips("还没有开始游戏，不能申请退出房间");
        }

        closeDialog();
    }

    public void toExit() {        
        if (panelExitDialog == null)
        {
            panelExitDialog = Instantiate(Resources.Load("Prefab/Panel_Exit")) as GameObject;
            panelExitDialog.transform.parent = gameObject.transform;
            panelExitDialog.transform.localScale = Vector3.one;
            //panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
            panelExitDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
            panelExitDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);

        }
    }
    public void toLeaveRoom()
    {
		dialog_fanhui.gameObject.SetActive (true);
        closeDialog();
    }
    public void tuichu()
    {
        OutRoomRequestVo vo = new OutRoomRequestVo();
        vo.roomId = GlobalDataScript.roomVo.roomId;
        string sendMsg = JsonMapper.ToJson(vo);
        CustomSocket.getInstance().sendMsg(new OutRoomRequest(sendMsg));
    }

    private void cancle()
    {

    }
    public void quxiao()
    {
		dialog_fanhui.gameObject.SetActive (false);
    }

    public void doDissoliveRoomRequest()
    {
        DissoliveRoomRequestVo dissoliveRoomRequestVo = new DissoliveRoomRequestVo();
        dissoliveRoomRequestVo.roomId = GlobalDataScript.loginResponseData.roomId;
        dissoliveRoomRequestVo.type = dissoliveRoomType;
        string sendMsg = JsonMapper.ToJson(dissoliveRoomRequestVo);
        CustomSocket.getInstance().sendMsg(new DissoliveRoomRequest(sendMsg));
        GlobalDataScript.isonApplayExitRoomstatus = true;
    }

    public void silderChange()
    {
        GlobalDataScript.yinyueVolume = yinyueSlider.value;
		PlayerPrefs.SetFloat ("yinyueVolume", GlobalDataScript.yinyueVolume);
        audioS.volume = yinyueSlider.value;
    }

    public void silder2Change()
    {
        GlobalDataScript.yinxiaoVolume = yinxiaoSlider.value;
		PlayerPrefs.SetFloat ("yinxiaoVolume", GlobalDataScript.yinxiaoVolume);
    }

    public void closeDialog()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        Destroy(this);
        Destroy(gameObject);
    }

    public void setJingyin() {
        yinyueSlider.value = 0;
        GlobalDataScript.yinyueVolume = yinyueSlider.value;
    }

    public void setMax()
    {
        yinyueSlider.value = 1;
        GlobalDataScript.yinyueVolume = yinyueSlider.value;
        SoundCtrl.getInstance().playSoundByActionButton(1);
    }

    public void setJingyin2()
    {
        yinxiaoSlider.value = 0;
        GlobalDataScript.yinxiaoVolume = yinxiaoSlider.value;
    }
    public void setMax2()
    {
        yinxiaoSlider.value = 1;
        GlobalDataScript.yinxiaoVolume = yinxiaoSlider.value;
        SoundCtrl.getInstance().playSoundByActionButton(1);
    }

    public void yuyinChange() {
        if (yuyinList[0].isOn)
        {
            GlobalDataScript.isYueYu = false;
            PlayerPrefs.SetInt("isYueYu", 0);
        }
        else {
            GlobalDataScript.isYueYu = true;
            PlayerPrefs.SetInt("isYueYu", 1);
        }
    }
}
