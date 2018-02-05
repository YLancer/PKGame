using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using LitJson;
using System.Runtime.InteropServices;

public class HomePanelScript : MonoBehaviour
{
    public Image headIconImg;//头像路径
                             //public Image tipHeadIcon;
    public Text noticeText;  //系统消息
    //public Text tipNameText;
    //	public Text tipIdText;
    //public Text tipIpText;
    public Text nickNameText;//昵称
    public Text cardCountText;//房卡/钻石剩余数量
    public Text IpText;
	public Text goldText;

    public Image jiahao;
    public Image message_view;


    //public GameObject userInfoPanel;
    WWW www;                     //请求
    string filePath;             //保存的文件路径
    Texture2D texture2D;         //下载的图片
    private string headIcon;
    private GameObject panelCreateDialog;//界面上打开的dialog
    private GameObject panelExitDialog;
    /// <summary>
    /// 这个字段是作为消息显示的列表 ，如果要想通过管理后台随时修改通知信息，
    /// 请接收服务器的数据，并重新赋值给这个字段就行了。
    /// </summary>
    private bool startFlag = false;
    public float waiteTime = 1;
    private float TimeNum = 0;
    private int showNum = 0;
    private int i;
    private int a = 0;

    private GameObject creatpanel;

	public GameObject fangkaPanel;
	public GameObject gonggaoPanel;
	public GameObject sharePanel;
	public GameObject fankuiPanel;
	public GameObject lotteryPanel;
	public GameObject zhanjiPanel;

    //-lan  
    public Button createRoomDialog;
    public Button joinRoom;

    void Start()
    {
        createRoomDialog.onClick.AddListener(openCreateRoomDialog);
        joinRoom.onClick.AddListener(openEnterRoomDialog);

        if (GlobalDataScript.configTemp.pay == 0) {
			fangkaPanel.SetActive (true);
			gonggaoPanel.SetActive (true);
			sharePanel.SetActive (false);
			fankuiPanel.SetActive (true);
			lotteryPanel.SetActive (true);
			zhanjiPanel.SetActive (true);
		} else {
			fangkaPanel.SetActive (false);
			gonggaoPanel.SetActive (false);
			sharePanel.SetActive (false);
			fankuiPanel.SetActive (false);
			lotteryPanel.SetActive (false);
			zhanjiPanel.SetActive (false);
		}

		//在这里开启一个异步任务
		//进入loadScene方法。
		StartCoroutine(loadScene());
		//StartCoroutine(LoadGD());
    }
		
		
	/// <summary>
	/// 加载广告
	/// </summary>
	/// <returns>The image.</returns>
	private IEnumerator LoadGD() { 

		if (GlobalDataScript.isGdOk) {
			loadPerfab ("Prefab/Panel_guanggao");
			yield break;
		}

		//开始下载图片
		WWW www = new WWW(APIS.GD_URL);
		yield return www;
		if (www != null) {
			Texture2D texture2D = www.texture;
			byte[] bytes = texture2D.EncodeToPNG ();
			//将图片赋给场景上的Sprite
			Sprite tempSp = Sprite.Create (texture2D, new Rect (0, 0, texture2D.width, texture2D.height), new Vector2 (0, 0));
			GlobalDataScript.gdSprite = tempSp;
			GlobalDataScript.isGdOk = true;

			//添加广告图片
			loadPerfab ("Prefab/Panel_guanggao");

		} else {
			MyDebug.Log ("没有加载到图片");
		}
	}

	public void initValue(){

		//gdImage = GameObject.Find ("Text_version").GetComponent<Image> ();
		//btnActionScript = gameObject.GetComponent<ButtonActionScript> ();
	}
	IEnumerator loadScene()
	{
		yield return new WaitForEndOfFrame();//加上这么一句就可以先显示加载画面然后再进行加载

		initUI();
		GlobalDataScript.isonLoginPage = false;
		//checkEnterInRoom();
		addListener();

        #if !UNITY_EDITOR && UNITY_IPHONE
        #endif

        creatpanel = Resources.Load("Prefab/Panel_Create_Dialog") as GameObject;
	}

	public void cardPay (){
		SoundCtrl.getInstance ().playSoundByActionButton (1);

		if (GlobalDataScript.configTemp.pay == 0) {
			this.showRoomCardPanel ();
		} else {
			this.showIosInPay ();
		}
	}

	public void showIosInPay (){
		string note = "你确定购买一张房卡吗？";
		TipsManagerScript.getInstance().loadDialog("Buy card", note, FangkaPay, onCancle);
	}

	public void FangkaPay(){

	}

	public void onCancle()
	{

	}

    void setNoticeTextMessage()
    {
        if (GlobalDataScript.noticeMegs != null && GlobalDataScript.noticeMegs.Count != 0)
        {
            noticeText.transform.localPosition = new Vector3(500, noticeText.transform.localPosition.y);
            noticeText.text = GlobalDataScript.noticeMegs[showNum];
            float time = noticeText.text.Length * 0.5f + 422f / 56f;

            Tweener tweener = noticeText.transform.DOLocalMove(
                new Vector3(-noticeText.text.Length * 40, noticeText.transform.localPosition.y), (float)(time/1.6))
                .OnComplete(moveCompleted);
            tweener.SetEase(Ease.Linear);
            //tweener.SetLoops(-1);
        }
    }

    void moveCompleted()
    {
        showNum=+1;
        if (showNum == GlobalDataScript.noticeMegs.Count)
        {
            showNum = 0;
        }
        setNoticeTextMessage();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { //Android系统监听返回键，由于只有Android和ios系统所以无需对系统做判断
            MyDebug.Log("Input.GetKey(KeyCode.Escape)");
            if (panelCreateDialog != null)
            {
                Destroy(panelCreateDialog);
                return;
            }
            else if (panelExitDialog == null)
            {
                panelExitDialog = Instantiate(Resources.Load("Prefab/Panel_Exit")) as GameObject;
                panelExitDialog.transform.parent = gameObject.transform;
                panelExitDialog.transform.localScale = Vector3.one;
                //panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
                panelExitDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
                panelExitDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
            }
        }

        //		TimeNum += Time.deltaTime;
        //		if(TimeNum >= waiteTime){
        //			TimeNum = 0;
        //			setNoticeTextMessage ();
        //		}

    }

    //增加服务器返沪数据监听
    public void addListener()
    {
        //SocketEventHandle.getInstance().cardChangeNotice += cardChangeNotice;
        SocketEventHandle.getInstance().contactInfoResponse += contactInfoResponse;


        //	SocketEventHandle.getInstance ().gameBroadcastNotice += gameBroadcastNotice;
        CommonEvent.getInstance().DisplayBroadcast += gameBroadcastNotice;



    }

    public void removeListener()
    {
        //SocketEventHandle.getInstance().cardChangeNotice -= cardChangeNotice;
        CommonEvent.getInstance().DisplayBroadcast -= gameBroadcastNotice;
        SocketEventHandle.getInstance().contactInfoResponse -= contactInfoResponse;
        //	SocketEventHandle.getInstance ().gameBroadcastNotice -= gameBroadcastNotice;
    }



    //房卡变化处理
    private void cardChangeNotice(ClientResponse response)
    {
		string msg = "";
		if (GlobalDataScript.goldType) {
			int oldCout = GlobalDataScript.loginResponseData.account.gold;
			GlobalDataScript.loginResponseData.account.gold = int.Parse(response.message);
			goldText.text = GlobalDataScript.loginResponseData.account.gold + "";
			int count = GlobalDataScript.loginResponseData.account.gold - oldCout;
			msg ="金币"  + (count>=0? "+" + count: count + "");
		} else {
			int oldCout = int.Parse(cardCountText.text);

			cardCountText.text = response.message;
            print(" ++++++++++++++++++++++++++++++++++++ " + int.Parse(response.message));
			GlobalDataScript.loginResponseData.account.roomcard = int.Parse(response.message);

			int count = int.Parse (response.message) - oldCout;
			msg ="元宝"  + (count>=0? "+" + count: count + "")+"个";
		}

		//TipsManagerScript.getInstance ().setTips (msg);
    }

    private void gameBroadcastNotice()
    {
        showNum = 0;
        if (!startFlag)
        {
            startFlag = true;
            setNoticeTextMessage();
        }
    }


    private void contactInfoResponse(ClientResponse response)
    {
		TipsManagerScript.getInstance ().setTips (response.message);
    }
    /***
	 *初始化显示界面 
	 */
	public void initUI()
	{
        if (GlobalDataScript.loginResponseData != null)
        {
            headIcon = GlobalDataScript.loginResponseData.account.headicon;    
            string nickName = GlobalDataScript.loginResponseData.account.nickname;
            int roomCardcount = GlobalDataScript.loginResponseData.account.roomcard;
            cardCountText.text = roomCardcount + "";
			goldText.text = GlobalDataScript.loginResponseData.account.gold+"";
            nickNameText.text = nickName;
            IpText.text = "ID:" + GlobalDataScript.loginResponseData.account.uuid;
            GlobalDataScript.loginResponseData.account.roomcard = roomCardcount;
        }

			
/**#if UNITY_ANDROID
        //显示游客登录按钮  主要用于ios版本
        if (APIS.hide_android.Equals("0" + APIS.version))
        {
            jiahao.gameObject.SetActive(false);
            message_view.gameObject.SetActive(false);
        }

#endif
        **/
        StartCoroutine(LoadImg());
        //	CustomSocket.getInstance ().sendMsg (new GetContactInfoRequest ());

    }

    public void showUserInfoPanel()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        //userInfoPanel.SetActive (true);
        GameObject obj = PrefabManage.loadPerfab("Prefab/userInfo");
        obj.GetComponent<ShowUserInfoScript>().setUIData(GlobalDataScript.loginResponseData);

    }

    /**
	public void closeUserInfoPanel (){
		userInfoPanel.SetActive (false);
	}
*/
    public void showRoomCardPanel()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        CustomSocket.getInstance().sendMsg(new GetContactInfoRequest());

    }
		

    /****
	 * 判断进入房间
	 */
    private void checkEnterInRoom()
    {
        if (GlobalDataScript.roomVo != null && GlobalDataScript.roomVo.roomId != 0)
        {
			//GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_GamePlay");
        }

    }

    public void Button_openWeb()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        
		Application.OpenURL(GlobalDataScript.configTemp.kefu);
      
    }


	private GameObject Panel_message;

    /*
  消息显示隐藏
      */
    public void Button_Mess_Open()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        Panel_message = PrefabManage.loadPerfab("Prefab/Panel_message");




    }

    /***
	 * 打开创建房间的对话框
	 * 
	 */
    public void openCreateRoomDialog()
    {

		SoundCtrl.getInstance().playSoundByActionButton(1);
        if (GlobalDataScript.loginResponseData == null || GlobalDataScript.loginResponseData.roomId == 0)
        {
            //loadPerfab("Prefab/Panel_Create_Dialog");
            loadPerfab_instantiate(creatpanel);
        }
        else {

            TipsManagerScript.getInstance().setTips("当前正在房间状态，无法创建房间");
        }




        //Application.LoadLevel ("Play_Scene");
    }
    public void Button_fankui()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
		Application.OpenURL(GlobalDataScript.configTemp.kefu);

    }
    /***
	 * 打开进入房间的对话框
	 * 
	 */
    public void openEnterRoomDialog()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        if (GlobalDataScript.roomVo == null || GlobalDataScript.roomVo.roomId == 0)
        {
			GlobalDataScript.goldType = false;
			loadPerfab("Prefab/Panel_Enter_Room");
            
        }
        else {
            TipsManagerScript.getInstance().setTips("当前正在房间状态，无法加入新的房间");
        }
    }

	public void OpenGoldTypeRoomDialog(){
		SoundCtrl.getInstance().playSoundByActionButton(1);
		if (GlobalDataScript.roomVo == null || GlobalDataScript.roomVo.roomId == 0)
		{
			loadPerfab("Prefab/Panel_GoldType_Dialog");
			GlobalDataScript.goldType = true;
		}
		else {
			TipsManagerScript.getInstance().setTips("当前正在房间状态，无法加入新的房间");
		}
	}

	/**
	 * 打开分享对话框
	 */
	public void openShareDialog()
	{
		SoundCtrl.getInstance().playSoundByActionButton(1);
		loadPerfab("Prefab/Panel_share");

		SocketEventHandle.getInstance().serviceErrorNotice += serviceErrorNotice;
	}

	public void serviceErrorNotice(ClientResponse response)
	{
		SocketEventHandle.getInstance().serviceErrorNotice -= serviceErrorNotice;
		TipsManagerScript.getInstance().setTips(response.message);
	}
    /**
	 * 打开设置对话框
	 */
    public void openGameSettingDialog()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        loadPerfab("Prefab/Panel_Setting");

        SettingScript ss = panelCreateDialog.GetComponent<SettingScript>();
        ss.jiesanBtn.GetComponentInChildren<Text>().text = "退出游戏";
        ss.type = 1;
    }

    /**
	 * 打开游戏规则对话框
	 */
    public void openGameRuleDialog()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        loadPerfab("Prefab/Panel_Game_Rule_Dialog");
    }

    /**
	 * 打开游戏规则对话框
	 */
    public void openGameRankDialog()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        loadPerfab("Prefab/Panel_Rank_Dialog");
    }


    /**
	 * 打开抽奖对话框
	 * 
	*/
    public void LotteryBtnClick()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        loadPerfab("Prefab/Panel_Lottery");
    }

    public void ZhanjiBtnClick()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        loadPerfab("Prefab/Panel_Report");
    }

    private void loadPerfab(string perfabName)
    {
        panelCreateDialog = Instantiate(Resources.Load(perfabName)) as GameObject;
        panelCreateDialog.transform.parent = gameObject.transform;
        panelCreateDialog.transform.localScale = Vector3.one;
        //panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
        panelCreateDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
        panelCreateDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
    }
    private void loadPerfab_instantiate(GameObject perfabName) {
        panelCreateDialog = Instantiate(perfabName);
        panelCreateDialog.transform.parent = gameObject.transform;
        panelCreateDialog.transform.localScale = Vector3.one;
        //panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
        panelCreateDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
        panelCreateDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
    }


    private IEnumerator LoadImg()
    {
        //开始下载图片
        if (headIcon != null && headIcon != "")
        {
			if (headIcon.IndexOf ("http") == -1) {
				headIconImg.sprite = GlobalDataScript.getInstance ().headSprite;
				yield break;
			}
            if (FileIO.wwwSpriteImage.ContainsKey(headIcon))
            {
                headIconImg.sprite = FileIO.wwwSpriteImage[headIcon];
                yield break;
            }

            WWW www = new WWW(headIcon);
            yield return www;
            //下载完成，保存图片到路径filePath
            try
            {
                texture2D = www.texture;
                byte[] bytes = texture2D.EncodeToPNG();
                //将图片赋给场景上的Sprite
                Sprite tempSp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));
                headIconImg.sprite = tempSp;
                FileIO.wwwSpriteImage.Add(headIcon, tempSp);
            }
            catch (Exception e)
            {

                MyDebug.Log("LoadImg" + e.Message);
            }
        }
    }



    public void exitApp()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
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

}
