using System;
using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;


public class GlobalDataScript
{

    public static bool isDrag = false;
	public static bool isCanChu = true;//是否可以出牌
    /**登陆返回数据**/
    public static AvatarVO loginResponseData;
    /**加入房间返回数据**/
	public static RoomCreateVo roomJoinResponseData;
    /**房间游戏规则信息**/
    public static RoomCreateVo roomVo = new RoomCreateVo();
    /**单局游戏结束服务器返回数据**/
    public static HupaiResponseVo hupaiResponseVo;
    /**全局游戏结束服务器返回数据**/
    public static FinalGameEndVo finalGameEndVo;

    public static int mainUuid;
    /**房间成员信息**/
    public static List<AvatarVO> roomAvatarVoList;

    //	public static Dictionary<int, Account > palyerBaseInfo = new Dictionary<int, Account> (); 

    public static GameObject homePanel;//主界面
    public static GameObject gamePlayPanel;//游戏界面

    /**麻将剩余局数**/
    public static int surplusTimes;
    /**总局数**/
    public static int totalTimes;
    /**默认面板**/

    /// <summary>
    /// 最顶层的容器
    /// </summary>
    public Transform canvsTransfrom;
    /**重新加入房间的数据**/
	public static RoomCreateVo reEnterRoomData;

    public WechatOperateScript wechatOperate;
    /// <summary>
    /// 声音开关
    /// </summary>
    public static bool soundToggle = true;
    public static float yinyueVolume = 1f;
    public static float yinxiaoVolume = 1f;

    public static List<String> messageBoxContents = new List<string>();
	public static List<String> messageBoxContents_DN = new List<string>();
    /// <summary>
    /// 单局结算面板
    /// </summary>
    public static List<GameObject> singalGameOverList = new List<GameObject>();


    public static bool isonLoginPage = true;//是否在登陆页面

    //public SocketEventHandle socketEventHandle;
    /// <summary>
    /// 抽奖数据
    /// </summary>
    public static List<LotteryData> lotteryDatas;
    public static bool isonApplayExitRoomstatus = false;//是否处于申请解散房间状态
    public static bool isOverByPlayer = false;//是否由用用户选择退出而退出的游戏
    public static LoginVo loginVo;//登录数据
    public static List<String> noticeMegs = new List<string>();

	public static ConfigVo configTemp;
	public static bool isAutoLogin = true;
	public static bool isYueYu = false;
	public static Sprite gdSprite;
	public static bool isGdOk = false;

	public Sprite headSprite = new Sprite();

	public static long loadTime = 0;

	public static bool isShare = false;

	public static bool isPdkDrag = false;

	public static int count_Players_DN = 0;

	public static GameType userGameType = GameType.GameType_MJ_RuiJin;

	public static GamePlayResponseVo aa;
	public static bool goldType = false;
	public static int playCountForGoldType = 0;

	public static long headTime;
	public static NetworkReachability network;
    /**
     * 重新初始化数据
    */
    public static void reinitData()
    {
        isDrag = false;
        loginResponseData = null;
        roomJoinResponseData = null;
        roomVo = new RoomCreateVo();
        hupaiResponseVo = null;
        finalGameEndVo = null;
        roomAvatarVoList = null;
        surplusTimes = 0;
        totalTimes = 0;
		userGameType = 0;
        reEnterRoomData = null;
        singalGameOverList = new List<GameObject>();
        lotteryDatas = null;
        isonApplayExitRoomstatus = false;
        isOverByPlayer = false;
        loginVo = null;
    }


    public void init()
    {
        //socketEventHandle = GameObject.Find ("Canvas").transform.GetComponent<SocketEventHandle> ();
        canvsTransfrom = GameObject.Find("container").transform;
        TipsManagerScript.getInstance().parent = GameObject.Find("Canvas").transform;
        wechatOperate = GameObject.Find("Canvas").GetComponent<WechatOperateScript>();
        initMessageBox();

		headSprite = Resources.Load ("xianlai/public_ui/head_img_male", typeof(Sprite)) as Sprite;
    }

    void initMessageBox()
    {
		
        messageBoxContents.Add("你太牛了");
        messageBoxContents.Add("哈哈，手气真好");
        messageBoxContents.Add("快点出牌哦");
        messageBoxContents.Add("今天真高兴");
        messageBoxContents.Add("这个吃的好");
        messageBoxContents.Add("你放炮我不胡");
        messageBoxContents.Add("你家里是开银行的吧");
        messageBoxContents.Add("不好意思，我有事先走一步了");
        messageBoxContents.Add("你的牌打得太好啦");
        messageBoxContents.Add("大家好，很高兴见到各位");
        messageBoxContents.Add("怎么又断线了啊，网络怎么这么差啊");
        
//		messageBoxContents.Add("你们慢慢打，我老公(婆)叫我早点回家");
//		messageBoxContents.Add("赌一把，赢就赢大点");
//		messageBoxContents.Add("让我飞一把吧，待会请大伙吃大餐");
//		messageBoxContents.Add("你是男生还是女生啊");
//		messageBoxContents.Add("一把都不让我胡，你们想打劫啊");
//		messageBoxContents.Add("你们在荒山野岭约会吗？信号这么差");
//		messageBoxContents.Add("快点出牌，我等到花儿也谢了");
//		messageBoxContents.Add("你们好厉害，我裤头都要输掉了");
//		messageBoxContents.Add("不要走，决战到天亮");
//		messageBoxContents.Add("今天运气真好，想什么就有什么");
//		messageBoxContents.Add("又有碰又有吃，不飞一把你都对不住我");
//		messageBoxContents.Add("再不让我胡一把，我可不认账哦");
//		messageBoxContents.Add("不好意思，我刚才在接电话");
//		messageBoxContents.Add("对不住，你们玩，我有点事先走了");

		messageBoxContents_DN.Add("怎么又断线了啊，你那毛线上网呀");
		messageBoxContents_DN.Add("一个都不要跑决战到天亮");
		messageBoxContents_DN.Add("牛牛在哪里，牛牛在这里");
		messageBoxContents_DN.Add("你太牛了");
		messageBoxContents_DN.Add("你牛什么牛");
		messageBoxContents_DN.Add("牛牛去哪了");
		messageBoxContents_DN.Add("你家养牛的呀");
		messageBoxContents_DN.Add("再见了，俺会想念大家的");
		messageBoxContents_DN.Add("下次我们再玩吧，我要走了");
		messageBoxContents_DN.Add("不要走，决战到天亮");
		messageBoxContents_DN.Add("你是妹妹还是哥哥");
		messageBoxContents_DN.Add("交个朋友吧，能不能告诉我你的联系方法");


    }

    private static GlobalDataScript _instance;
    public static GlobalDataScript getInstance()
    {
        if (_instance == null)
        {
            _instance = new GlobalDataScript();
        }
        return _instance;
    }

    public GlobalDataScript()
    {
        init();
    }

    public string getIpAddress()
    {
		return "127.0.0.1";

        string tempip = "";
        try
        {
            WebRequest wr = WebRequest.Create("http://1212.ip138.com/ic.asp");
            Stream s = wr.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.Default);
            string all = sr.ReadToEnd(); //读取网站的数据

            int start = all.IndexOf("[") + 1;
            int end = all.IndexOf("]");
            int count = end - start;
            tempip = all.Substring(start, count);
            sr.Close();
            s.Close();
        }
        catch
        {
        }
        return tempip;
    }

	/// <summary>  
	/// 获取时间戳  
	/// </summary>  
	/// <returns></returns>  
	public static long GetTimeStamp()
	{
		TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
		return Convert.ToInt64(ts.TotalMilliseconds);
	}

}

