using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System;//Math.Abs

public class PlayerItemScript : MonoBehaviour
{

    public Image headerIcon;
    public Image bankerImg;
    public Text nameText;
    public Image readyImg;
    public Text scoreText;
	public Image scoreImg;
    public string dir;
    public GameObject chatAction;
    //public Text offline;//离线字样
    public Image offlineImage;//离线图片
    public Text chatMessage;
    public GameObject chatPaoPao;
    public Image chatBiaoQing;
    public GameObject HuFlag;

	public Text text_dzpk;
	public GameObject qipaiImg_dzpk;
	public Text avaZhu_dzpk;
	public Text type_dzpk;
	private int avaCurZhu_dzpk = 0;//本轮的注


    private AvatarVO avatarvo;
    private int showTime;
    private int showChatTime;
    private int biaoQingTime;
	public Image fangzhu;
	private Vector3 vec3;

	private bool isFirst = true;

	public Image paiCountImage;
	public Text paiCountText;
	public Image yaobuqiImage;

	public Image clockImage;
	public Text clockText;
	public Image zhuImg;
	public Image niuImg;

	public GameObject shanghuo;

	public GameObject mfbq;

    // Use this for initialization
    void Start()
    {
		avaCurZhu_dzpk = 0;
		if (avaZhu_dzpk != null) {
			avaZhu_dzpk.gameObject.SetActive (false);
		}
        //bankerImg.enabled = false;
        //readyImg.enabled = false;
        //	    scoreText.text = 1000 + "";
    }

    // Update is called once per frame
    void Update()
    {
        if (showTime > 0)
        {
            showTime--;
            if (showTime == 0)
            {
                chatPaoPao.SetActive(false);
            }
        }

        if (showChatTime > 0)
        {
            showChatTime--;
            if (showChatTime == 0)
            {
                chatAction.SetActive(false);
            }
        }

        if (biaoQingTime > 0)
        {
            biaoQingTime--;
            if (biaoQingTime == 0)
            {
                chatBiaoQing.gameObject.SetActive(false);
            }
        }
    }
	public void setZhuImg(int zhu){
		zhuImg.gameObject.SetActive (true);
		zhuImg.sprite = Resources.Load ("dn/zhu" + zhu, typeof(Sprite)) as Sprite;
	}
	public void setNiuImg(int niu){
		
		if (niu >= 10 && niu <= 13)
			niu = 10;
		niuImg.sprite = Resources.Load ("dn/niu" + niu, typeof(Sprite)) as Sprite;
		niuImg.gameObject.SetActive (true);//0907
	}

    public void setAvatarVo(AvatarVO value)
    {
        if (value != null)
        {
            avatarvo = value;
            readyImg.enabled = avatarvo.isReady;
			bankerImg.enabled = avatarvo.main;
            nameText.text = avatarvo.account.nickname;
            scoreText.text = avatarvo.scores + "";
			if(scoreImg!=null)
				scoreImg.gameObject.SetActive (true);
            offlineImage.transform.gameObject.SetActive(!avatarvo.isOnLine);
            StartCoroutine(LoadImg());

        }
        else {
            nameText.text = "";
            readyImg.enabled = false;
            bankerImg.enabled = false;
            scoreText.text = "";
			if(scoreImg!=null)
				scoreImg.gameObject.SetActive (false);
            readyImg.enabled = false;
			avatarvo = null;
            //			SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer> ();
            //			Texture2D texture =(Texture2D)Resources.Load ("Image/gift");
            //			Sprite sp = Sprite.Create (texture, spr.sprite.textureRect, new Vector2 (0.5f, 0.5f));
            headerIcon.sprite = Resources.Load("Image/morentouxiang", typeof(Sprite)) as Sprite;
        }
    }
    /// <summary>
    /// 加载头像
    /// </summary>
    /// <returns>The image.</returns>
    private IEnumerator LoadImg()
    {

		if (avatarvo.account.headicon.IndexOf ("http") == -1) {
			headerIcon.sprite = GlobalDataScript.getInstance().headSprite;
			yield break;
		}

        if (FileIO.wwwSpriteImage.ContainsKey(avatarvo.account.headicon))
        {
            headerIcon.sprite = FileIO.wwwSpriteImage[avatarvo.account.headicon];
            yield break;
        }

        //开始下载图片
        WWW www = new WWW(avatarvo.account.headicon);
        yield return www;
        //下载完成，保存图片到路径filePath
        if (www != null)
        {
            Texture2D texture2D = www.texture;
            byte[] bytes = texture2D.EncodeToPNG();

            //将图片赋给场景上的Sprite
            Sprite tempSp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));
            headerIcon.sprite = tempSp;
            FileIO.wwwSpriteImage.Add(avatarvo.account.headicon, tempSp);
        }
        else {
            MyDebug.Log("没有加载到图片");
        }
    }



    public void setbankImgEnable(bool flag)
    {
        bankerImg.enabled = flag;

    }

    public void showChatAction()
    {
        showChatTime = 120;
        chatAction.SetActive(true);
    }

    public int getUuid()
    {
        int result = -1;
        if (avatarvo != null)
        {
            result = avatarvo.account.uuid;
        }
        return result;
    }

    public void clean()
    {
        Destroy(headerIcon.gameObject);
        Destroy(bankerImg.gameObject);
        Destroy(nameText.gameObject);
        Destroy(readyImg.gameObject);
    }

    /**设置游戏玩家离线**/
    public void setPlayerOffline()
    {
        offlineImage.transform.gameObject.SetActive(true);
    }

    /**设置游戏玩家上线**/
    public void setPlayerOnline()
    {
        offlineImage.transform.gameObject.SetActive(false);
    }

    public void showChatMessage(int index)
    {
        showTime = 200;
        index = index - 1;
		if (GlobalDataScript.roomVo.gameType == 3) {
			chatMessage.text = GlobalDataScript.messageBoxContents_DN[index];
		} else {
			chatMessage.text = GlobalDataScript.messageBoxContents[index];
		}
        
        chatBiaoQing.gameObject.SetActive(false);
        chatPaoPao.SetActive(true);
    }


    public void showChatMessage(string msg)
    {
        showTime = 200;
        chatMessage.text = msg;
        chatBiaoQing.gameObject.SetActive(false);
        chatPaoPao.SetActive(true);
    }

    public void showBiaoQing(Image img) {
        if (chatBiaoQing.isActiveAndEnabled) {
            chatBiaoQing.gameObject.SetActive(false);
        }
        biaoQingTime = 200;
		if (isFirst) {
			vec3 = chatBiaoQing.transform.position;
			isFirst = false;
		}
        chatBiaoQing = img;
        chatBiaoQing.transform.position = vec3;
        chatPaoPao.SetActive(false);
        chatBiaoQing.gameObject.SetActive(true);
    }

	//System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
	public void showMfbq(int srcIndex, int destIndex, string bqindex){
		bool right = false;
		int index = int.Parse (bqindex);
		if (GlobalDataScript.roomVo.gameType == 1) {//pdk
			if(1 == destIndex && 4 == index){
				right = true;
			}
		} else if (GlobalDataScript.roomVo.gameType == 3) {//dn
			if ((1 == destIndex || 2 == destIndex) && 4 == index) {
				right = true;
			}
		} else {//majiang
			if ((1 == destIndex || 2 == destIndex) && 4 == index) {
				right = true;
			}
		}

		//GameObject obj = PrefabManage.loadPerfab ("Prefab/mfbq/mfbq" + index);
		GameObject obj = Instantiate (Resources.Load("Prefab/mfbq/mfbq" + bqindex)) as GameObject;
		obj.transform.parent = mfbq.transform;
		if (right) {
			obj.transform.localScale = new Vector3 (-1, 1, 1);
		} else {
			//obj.transform.localScale = Vector3.one;
			obj.transform.localScale = new Vector3 (1, 1, 1);
		}
		//obj.transform.localPosition = new Vector3 (0, 0, 0);
		//obj.GetComponent<mfbq> ().setSrcDest (srcIndex, destIndex);

		obj.transform.localPosition = getSrcPosition (srcIndex, destIndex, right);
		float distance = Vector3.Distance (obj.transform.localPosition, new Vector3 (0, 0, 0));
		float step = distance * Time.deltaTime;
		GameObject zhuojiobj = null;
		if (2 == index) {
			zhuojiobj = Instantiate (Resources.Load("Prefab/mfbq/mfbq21")) as GameObject;
			zhuojiobj.transform.parent = mfbq.transform;
			zhuojiobj.transform.localPosition = new Vector3 (0, 0, 0);
			zhuojiobj.transform.localScale = new Vector3 (1, 1, 1);
		}


		//sw.Start ();
		StartCoroutine(MoveToPosition(obj, step, index, zhuojiobj));


	}

	private Vector3 getSrcPosition(int srcIndex, int destIndex, bool right){
		Vector3 res = new Vector3(0,0,0);
		if (GlobalDataScript.roomVo.gameType == 0) {
			switch (srcIndex) {
			case 0:
				{
					switch (destIndex) {
					case 1:
						res = right ? new Vector3 (-1274, -285, 0) : new Vector3 (-1192, -224, 0);
						break;
					case 2:
						res = right ? new Vector3 (-1121, -516, 0) : new Vector3 (-1039, -461, 0);
						break;
					case 3:
						res = right ? new Vector3 (-79, -298, 0) : new Vector3 (2, -234, 0);
						break;
					default:
						Debug.LogError ("getSrcPosition() error");
						break;
					}
				}
				break;
			case 1:
				switch (destIndex) {
				case 0:
					res = right ? new Vector3 (1090, 147, 0) : new Vector3 (1174, 210, 0);
					break;
				case 2:
					res = right ? new Vector3 (73, -314, 0) : new Vector3 (153, -257, 0);
					break;
				case 3:
					res = right ? new Vector3 (1097, -76, 0) : new Vector3 (1184, -12, 0);
					break;
				default:
					Debug.LogError ("getSrcPosition() error");
					break;
				}
				break;
			case 2:
				switch (destIndex) {
				case 0:
					res = right ? new Vector3 (939, 379, 0) : new Vector3 (1025, 440, 0);
					break;
				case 1:
					res = right ? new Vector3 (-253, 171, 0) : new Vector3 (-171, 239, 0);
					break;
				case 3:
					res = right ? new Vector3 (944, 160, 0) : new Vector3 (1025, 232, 0);
					break;
				default:
					Debug.LogError ("getSrcPosition() error");
					break;
				}
				break;
			case 3:
				switch (destIndex) {
				case 0:
					res = right ? new Vector3 (-98, 147, 0) : new Vector3 (-6, 228, 0);
					break;
				case 1:
					res = right ? new Vector3 (-1280, -67, 0) : new Vector3 (-1196, -5, 0);
					break;
				case 2:
					res = right ? new Vector3 (-1125, -302, 0) : new Vector3 (-1039, -243, 0);
					break;
				default:
					Debug.LogError ("getSrcPosition() error");
					break;
				}
				break;
			}
		} else if (GlobalDataScript.roomVo.gameType == 1) {
			switch (srcIndex) {
			case 0:
				{
					switch (destIndex) {
					case 1:
						res = right ? new Vector3 (-1083, -387, 0) : new Vector3 (-995, -316, 0);
						break;
					case 2:
						res = right ? new Vector3 (-92, -387, 0) : new Vector3 (-6, -321, 0);
						break;
					default:
						Debug.LogError ("getSrcPosition() error");
						break;
					}
				}
				break;
			case 1:
				switch (destIndex) {
				case 0:
					res = right ? new Vector3 (896, 244, 0) : new Vector3 (985, 311, 0);
					break;
				case 2:
					res = right ? new Vector3 (897, -71, 0) : new Vector3 (984, -7, 0);
					break;
				default:
					Debug.LogError ("getSrcPosition() error");
					break;
				}
				break;
			case 2:
				switch (destIndex) {
				case 0:
					res = right ? new Vector3 (-93, 252, 0) : new Vector3 (1, 317, 0);
					break;
				case 1:
					res = right ? new Vector3 (-1081, -70, 0) : new Vector3 (-992, 1, 0);
					break;
				default:
					Debug.LogError ("getSrcPosition() error");
					break;
				}
				break;
			}
		} else if (GlobalDataScript.roomVo.gameType == 3) {
			switch (srcIndex) {
			case 0:
				{
					switch (destIndex) {
					case 1:
						res = right ? new Vector3 (-937, -270, 0) : new Vector3 (-851, -198, 0);
						break;
					case 2:
						res = right ? new Vector3 (-874, -432, 0) : new Vector3 (-791, -367, 0);
						break;
					case 3:
						res = right ? new Vector3 (-66, -462, 0) : new Vector3 (22, -389, 0);
						break;
					case 4:
						res = right ? new Vector3 (-66, -235, 0) : new Vector3 (24, -158, 0);
						break;
					default:
						Debug.LogError ("getSrcPosition() error");
						break;
					}
				}
				break;
			case 1:
				switch (destIndex) {
				case 0:
					res = right ? new Vector3 (753, 118, 0) : new Vector3 (839, 184, 0);
					break;
				case 2:
					res = right ? new Vector3 (155, -249, 0) : new Vector3 (58, -176, 0);
					break;
				case 3:
					res = right ? new Vector3 (778, -274, 0) : new Vector3 (866, -197, 0);
					break;
				case 4:
					res = right ? new Vector3 (779, -32, 0) : new Vector3 (869, 37, 0);
					break;
				default:
					Debug.LogError ("getSrcPosition() error");
					break;
				}
				break;
			case 2:
				switch (destIndex) {
				case 0:
					res = right ? new Vector3 (690, 294, 0) : new Vector3 (779, 360, 0);
					break;
				case 1:
					res = right ? new Vector3 (27, 99, 0) : new Vector3 (-69, 171, 0);
					break;
				case 3:
					res = right ? new Vector3 (715, -99, 0) : new Vector3 (806, -25, 0);
					break;
				case 4:
					res = right ? new Vector3 (718, 143, 0) : new Vector3 (807, 211, 0);
					break;
				default:
					Debug.LogError ("getSrcPosition() error");
					break;
				}
				break;
			case 3:
				switch (destIndex) {
				case 0:
					res = right ? new Vector3 (-116, 316, 0) : new Vector3 (-30, 385, 0);
					break;
				case 1:
					res = right ? new Vector3 (-964, 124, 0) : new Vector3 (-877, 195, 0);
					break;
				case 2:
					res = right ? new Vector3 (-898, -50, 0) : new Vector3 (-814, 16, 0);
					break;
				case 4:
					res = right ? new Vector3 (-93, 160, 0) : new Vector3 (-2, 235, 0);
					break;
				default:
					Debug.LogError ("getSrcPosition() error");
					break;
				}
				break;
			case 4:
				switch (destIndex) {
				case 0:
					res = right ? new Vector3 (-116, 85, 0) : new Vector3 (-33, 158, 0);
					break;
				case 1:
					res = right ? new Vector3 (-961, -108, 0) : new Vector3 (-877, -40, 0);
					break;
				case 2:
					res = right ? new Vector3 (-898, -282, 0) : new Vector3 (-814, -219, 0);
					break;
				case 3:
					res = right ? new Vector3 (-90, -304, 0) : new Vector3 (-2, -242, 0);
					break;
				default:
					Debug.LogError ("getSrcPosition() error");
					break;
				}
				break;
			}



		}
		return res;
	}

	IEnumerator MoveToPosition(GameObject obj, float speed, int index, GameObject zhuojiobj) {
		//while (transform.position != dest) {
		while (Math.Abs(0 - obj.transform.localPosition.x)>1 || Math.Abs(0 - obj.transform.localPosition.y)>1){

			//obj.transform.Translate((0-obj.transform.localPosition.x)*Time.deltaTime*2, (0-obj.transform.localPosition.y)*Time.deltaTime*2, 0);//mfbq4 error
			//obj.transform.localPosition = Vector3.MoveTowards(obj.transform.localPosition, new Vector3(0, 0, 0), speed);
			obj.transform.localPosition = Vector3.Lerp(obj.transform.localPosition, new Vector3(0, 0, 0), 0.2f);
			yield return 0;  
		}
		//sw.Stop ();
		//Debug.LogError ("using time:"+sw.ElapsedMilliseconds);
		obj.GetComponent<Animator> ().enabled = true;
		if (zhuojiobj != null) {
			Destroy (zhuojiobj);
		}
		SoundCtrl.getInstance ().playMfbqSound (index);
	}

    public void displayAvatorIp()
    {
        //userInfoPanel.SetActive (true);
        if (avatarvo != null)
        {
            GameObject obj = PrefabManage.loadPerfab("Prefab/userInfo");
            obj.GetComponent<ShowUserInfoScript>().setUIData(avatarvo);
			SoundCtrl.getInstance().playSoundByActionButton(1);

//			if (avatarvo.account.uuid == GlobalDataScript.loginResponseData.account.uuid) {
//				GameObject obj = PrefabManage.loadPerfab ("Prefab/userInfo");
//				obj.GetComponent<ShowUserInfoScript> ().setUIData (avatarvo);
//				SoundCtrl.getInstance ().playSoundByActionButton (1);
//			} else {
//				GameObject obj = PrefabManage.loadPerfab ("Prefab/userInfo2");
//				obj.GetComponent<ShowUserInfoScript> ().setUIData (avatarvo);
//				SoundCtrl.getInstance ().playSoundByActionButton (1);
//			}
        }
    }

    public void setHuFlagDisplay()
    {
        HuFlag.SetActive(true);
    }

    public void setHuFlagHidde()
    {
        HuFlag.SetActive(false);
    }

	public void setDNHide(){
		zhuImg.gameObject.SetActive (false);
		niuImg.gameObject.SetActive (false);
		niuImg.sprite = null;//0907
	}
	public void showFangZhu(){
		fangzhu.gameObject.SetActive (true);
	}

	public void showPaiCountImage(bool isShow){
		paiCountImage.gameObject.SetActive (isShow);
	}

	public void showPaiCountText(int count){
		if (GlobalDataScript.roomVo.showPai) {
			showPaiCountImage (true);
			paiCountText.text = count.ToString ();
		}
	}

	public void showYaoBuQi(bool isShow){
		yaobuqiImage.gameObject.SetActive (isShow);
	}

	public void showClock(bool isShow){
		clockImage.gameObject.SetActive (isShow);
	}

	public void showClockText(string count){
		showClock (true);
		clockText.text = count;
	}


	public void setScoreText(int score){
		avatarvo.scores = score;
		scoreText.text = avatarvo.scores + "";
	}

	public void updateScoreText(int score){
		avatarvo.scores += score;
		scoreText.text = avatarvo.scores + "";
//		Debug.LogError ("scoreText.text:" + int.Parse("scoreText.text"));
//		scoreText.text = int.Parse("scoreText.text") + score + "";
	}

	//德州扑克
	public void showDZPKdaxiaoMang(int genzhu, int seat){
		avaZhu_dzpk.gameObject.SetActive (true);
		avatarvo.scores -= genzhu;
		if (avatarvo.scores <= 0) {
			text_dzpk.text = "全下";
		}

		avaCurZhu_dzpk += genzhu; 

		//移动
		GameObject obj = Instantiate (Resources.Load ("prefab/dzpk/zhuCoinMove")) as GameObject;
		if (obj != null) {
			obj.transform.SetParent (gameObject.transform);
			obj.transform.localScale = new Vector3 (1.3f, 1.3f, 1.3f);
			obj.transform.localPosition = new Vector3 (0,0,0);
			Vector3 vec = getZhuCoinPosition (seat);

			StartCoroutine(ZhuCoinMoveToPosition(obj, vec, genzhu));
		}
		//avaZhu_dzpk.text = avaCurZhu_dzpk + "";
	}

	public void showDZPKGenZhu(int genzhu, int seat){
		avaZhu_dzpk.gameObject.SetActive (true);
		avatarvo.scores -= genzhu;
		if (avatarvo.scores <= 0) {
			text_dzpk.text = "全下";
			SoundCtrl.getInstance ().playSoundByDZPK ("allin", avatarvo.account.sex);
		} else {
			text_dzpk.text = "跟注";
			SoundCtrl.getInstance ().playSoundByDZPK ("genzhu", avatarvo.account.sex);
		}

		avaCurZhu_dzpk += genzhu; 

		//移动
		GameObject obj = Instantiate (Resources.Load ("prefab/dzpk/zhuCoinMove")) as GameObject;
		if (obj != null) {
			obj.transform.SetParent (gameObject.transform);
			obj.transform.localScale = new Vector3 (1.3f, 1.3f, 1.3f);
			obj.transform.localPosition = new Vector3 (0,0,0);
			Vector3 vec = getZhuCoinPosition (seat);

			StartCoroutine(ZhuCoinMoveToPosition(obj, vec, genzhu));
		}

		//avaZhu_dzpk.text = avaCurZhu_dzpk + "";
	}

	private Vector3 getZhuCoinPosition(int i){
		Vector3 vec = new Vector3 (0,0,0);
		switch (i) {
		case 0:
			vec = new Vector3 (55, 67, 0);  // 第一位
			break;
		case 1:
			vec = new Vector3 (60, 65, 0);  //第二位
            break;
		case 2:
			vec = new Vector3 (-75, 20, 0);  //第三位
			break;
		case 3:
			vec = new Vector3 (-125,30, 0);  //第四位
			break;
		case 4:
			vec = new Vector3 (10, -85, 0);  //第五位
			break;
		case 5:
			vec = new Vector3 (50, -110, 0);
			break;
                 //-lan 新增至10人桌    位置
        case 6:
            vec = new Vector3(20,-100, 0);
            break;
        case 7:
            vec = new Vector3(110,20, 0);
            break;
        case 8:
            vec = new Vector3(220,65, 0);
            break;
        case 9:
            vec = new Vector3(145,80, 0);
            break;
            default:
			break;
		}
		return vec;
	}

	IEnumerator ZhuCoinMoveToPosition(GameObject obj, Vector3 dest, int zhu) {
		while(obj.transform.localPosition != dest){
			obj.transform.localPosition = Vector3.MoveTowards (obj.transform.localPosition, dest, 400*Time.deltaTime);
			yield return 0;
		}
		Destroy (obj);

		scoreText.text = avatarvo.scores + "";//20171026
		avaZhu_dzpk.text = avaCurZhu_dzpk + "";

	}



	public void showDZPKRangPai(){
		avaZhu_dzpk.gameObject.SetActive (false);
		text_dzpk.text = "让牌";
		SoundCtrl.getInstance ().playSoundByDZPK ("guopai", avatarvo.account.sex);
	}
	public void showDZPKQiPai(){
		//avaZhu_dzpk.gameObject.SetActive (false);
		cleanDZPKtext();
		qipaiImg_dzpk.SetActive(true);
		text_dzpk.text = "弃牌";
		SoundCtrl.getInstance ().playSoundByDZPK ("qipai", avatarvo.account.sex);
	}
	public void showDZPKJiaZhu(int zhu, int seat){
		avaZhu_dzpk.gameObject.SetActive (true);
		avatarvo.scores -= zhu;
		if (avatarvo.scores <= 0) {
			text_dzpk.text = "全下";
			SoundCtrl.getInstance ().playSoundByDZPK ("allin", avatarvo.account.sex);
		} else {
			text_dzpk.text = "加注";
			SoundCtrl.getInstance ().playSoundByDZPK ("jiazhu", avatarvo.account.sex);
		}

		avaCurZhu_dzpk += zhu;

		//移动
		GameObject obj = Instantiate (Resources.Load ("prefab/dzpk/zhuCoinMove")) as GameObject;
		if (obj != null) {
			obj.transform.SetParent (gameObject.transform);
			obj.transform.localScale = new Vector3 (1.3f, 1.3f, 1.3f);
			obj.transform.localPosition = new Vector3 (0,0,0);
			Vector3 vec = getZhuCoinPosition (seat);

			StartCoroutine(ZhuCoinMoveToPosition(obj, vec, zhu));
		}


		//avaZhu_dzpk.text = avaCurZhu_dzpk + "";
	}

	public void showDZPKSuccess(){
		//text_dzpk.text = "胜利";
		setHuFlagDisplay();
		avaCurZhu_dzpk = 0;
	}

	public void cleanDZPKtext(){
		avaZhu_dzpk.gameObject.SetActive (false);
		//text_dzpk.text = "";
		avaCurZhu_dzpk = 0;
		avaZhu_dzpk.text = "";
	}

	public void cleanDZPKAlltext(){
		text_dzpk.text = "";
		avaCurZhu_dzpk = 0;
		avaZhu_dzpk.text = "";
		qipaiImg_dzpk.SetActive(false);
		setHuFlagHidde ();
		type_dzpk.text = "";
	}

	public void showDZPKType(int type){
		type_dzpk.text = getType (type);
	}

	private string getType(int para){
		string res = "";
		switch(para){
		case 9:
			res = "皇家同花顺";
			break;
		case 8:
			res = "同花顺";
			break;
		case 7:
			res = "四条";
			break;
		case 6:
			res = "葫芦";
			break;
		case 5:
			res = "同花";
			break;
		case 4:
			res = "顺子";
			break;
		case 3:
			res = "三条";
			break;
		case 2:
			res = "两对";
			break;
		case 1:
			res = "一对";
			break;
		case 0:
			res = "高牌";
			break;
		default:
			res = "";
			break;
		}
		return res;	
	}


	public int getCurZhu(){
		return avaCurZhu_dzpk;
	}

	public void SetShanghuo(bool flag){
		if (shanghuo != null) {
			shanghuo.SetActive (flag);
		}
	}

}
