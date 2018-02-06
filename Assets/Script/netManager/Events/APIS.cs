using System;

namespace AssemblyCSharp
{

    public class APIS
    {
        public APIS()
        {
        }
        //public static string baseUrl = "http://mj.wapvc.cn";
        //public static string baseUrl = "xw.y6pay.com";
        //public static string baseUrl = "192.168.0.100";
        public static string baseUrl = "103.26.79.31";

		public static string UPDATE_INFO_JSON_URL = baseUrl + "/download/update.xml";//服务器上最新的软件版本信息存储文件
		public static string GD_URL = baseUrl + "/xxadmin/images/guanggao.png";//广告图片



        //		public static string chatSocketUrl = "www.zzcf518.com"; //www.y6pay.com    192.168.0.109
        //		public static string socketUrl = "www.zzcf518.com";//119.23.172.226   192.168.1.4

        //		public static string chatSocketUrl = "mj.wapvc.cn"; 
        //		public static string socketUrl = "mj.wapvc.cn";

        //public static string chatSocketUrl = "192.168.0.100";
        //public static string socketUrl = "192.168.0.100";

        public static string chatSocketUrl = "103.26.79.31";
        public static string socketUrl = "103.26.79.31";

        //public static string chatSocketUrl = "skyl.shenkongjian.com";4
        //public static string socketUrl = "skyl.shenkongjian.com";

        //		public static string chatSocketUrl = "xw.y6pay.com"; 
        //		public static string socketUrl = "xw.y6pay.com";

        public const int socketPort = 10122;
        public const int chatPort = 10112;

		public static string PIC_PATH = baseUrl + "/download/";
        //	public const string apkDownLoadurl="192.168.0.111/aaa/weipai.apk";
		public static string ImgUrl = baseUrl + "/weiPaiImage/";
        public const int head = 0x000030;
        public const int headRESPONSE = 0x000031;

        //游戏关闭返回
        public const int CLOSE_RESPONSE = 0x000000;
        public const int LOGIN_REQUEST = 0x000001; //登陆请求码
        public const int LOGIN_RESPONSE = 0x000002;//登陆返回码

		public const int RETURN_ONLINE_RESPONSE = 0x001003;//断线重连返回最后一次打牌数据
		public const int REQUEST_CURRENT_DATA = 0x001004;//申请最后打牌数据数据

        public const int JOIN_ROOM_REQUEST = 0x000003;//加入房间请求码
        public const int JOIN_ROOM_RESPONSE = 0x000004;//加入房间返回码
        public const int JOIN_ROOM_NOICE = 0x10a004;//其它 人加入房间通知
		public const int JOIN_ROOM_IP_RESPONSE = 0x000005;//加入房间相同ip返回码

        public const int CREATEROOM_REQUEST = 0x00009;//创建房间请求码
        public const int CREATEROOM_RESPONSE = 0x00010;//创建房间返回吗

        public const int STARTGAME_RESPONSE_NOTICE = 0x00012;//开始游戏
        public const int PICKCARD_RESPONSE = 0x100004;//自己摸牌 
        public const int OTHER_PICKCARD_RESPONSE_NOTICE = 0x100014;//别人摸牌通知

        public const int RETURN_INFO_RESPONSE = 0x100000;//胡、碰、杠等通知
        public const int CHUPAI_REQUEST = 0x100001;//出牌请求
        public const int CHUPAI_RESPONSE = 0x100002;//出牌通知

        public const int PENGPAI_REQUEST = 0x100005;//碰牌请求
        public const int PENGPAI_RESPONSE = 0x100006;//碰牌通知
        public const int GANGPAI_REQUEST = 0x100007;//杠牌请求
        public const int GANGPAI_RESPONSE = 0x100008;//杠牌返回
        public const int OTHER_GANGPAI_NOICE = 0x10a008;//杠牌通知
        public const int HUPAI_REQUEST = 0x100009;//胡牌请求
        public const int HUPAI_RESPONSE = 0x100010;//胡牌通知
		public const int HUPAIBACKDZPK_RESPONSE = 0x100011;//德州扑克断线重连，胡牌特殊处理

		public const int CHI_REQUEST = 0x100011;//吃牌请求
		public const int CHI_RESPONSE = 0x100012;//吃牌通知
        public const int HUPAIALL_RESPONSE = 0x100110;//全局结束通知
        public const int GAVEUP_REQUEST = 0x100015;//放弃（胡，杠，碰，吃）

        public const int BACK_LOGIN_REQUEST = 0x001001;//掉线后重新登录查询当前牌桌情况请求
        public const int BACK_LOGIN_RESPONSE = 0x001002;//掉线后重新登录查询当前牌桌情况返回

        public const int OUT_ROOM_REQUEST = 0x000013;//退出房间请求
        public const int OUT_ROOM_RESPONSE = 0x000014;//退出房间返回数7

        public const int DISSOLIVE_ROOM_REQUEST = 0X000113;//申请解散房间
        public const int DISSOLIVE_ROOM_RESPONSE = 0X000114;//解散房间回调

        public const int PrepareGame_MSG_REQUEST = 0x333333;  //准备
        public const int PrepareGame_MSG_RESPONSE = 0x444444;//

		public const int ShangHuo_MSG_REQUEST = 0x444445;
		public const int ShangHuo_MSG_RESPONSE = 0x444446;

        public const int ERROR_RESPONSE = 0xffff09;//错误回调

        public const int MicInput_Request = 200;
        public const int MicInput_Response = 201;

        public const int LoginChat_Request = 202;

        public const int MessageBox_Request = 203;
        public const int MessageBox_Notice = 204;

        public const int QUITE_LOGIN = 0x555555;//退出登录调用，仅限于正常登录
        public const int CARD_CHANGE = 0x777777;

        //public const int OUT_ROOM_RESPONSE = 0x001002;//离线通知
        public const int OFFLINE_NOTICE = 0x000015;
        public const int ONLINE_NOTICE = 0x001111;

        public const int PRIZE_RESPONSE = 0x999999;//抽奖接口
        public const int GET_PRIZE = 0x888888;//抽奖请求接口

        public const int Game_FollowBander_Notice = 0x100016;//跟庄


		public const int GAME_CONFIG = 0x157778;
        public const int GAME_BROADCAST = 0x157777;//游戏公告
        public const int CONTACT_INFO_REQUEST = 0x156666;//添加房卡请求数据
        public const int CONTACT_INFO_RESPONSE = 0x155555;//添加房卡返回数据
        public const int HOST_UPDATEDRAW_RESPONSE = 0x010111;//抽奖信息变化
        public const int ZHANJI_REPOTER_REQUEST = 0x002001;//战绩请求
        public const int ZHANJI_REPORTER_REPONSE = 0x002002;//房间战绩返回数据
        public const int ZHANJI_DETAIL_REPORTER_REPONSE = 0x002003;//某个房间详细每局战绩
        public const int ZHANJI_SEARCH_REQUEST = 0x002004;//搜索房间对应战绩

        public const int GAME_BACK_PLAY_REQUEST = 0x003001;//回放请求
        public const int GAME_BACK_PLAY_RESPONSE = 0x003002;//回放返回数据
        public const int TIP_MESSAGE = 0x160016;

        public const int OTHER_TELE_LOGIN = 0x211211;//其他设备登录

		public const int WECHAT_SHARE = 0x301;//微信分享送房卡请求


		//跑得快
		public const int CHUPAI_PDK_REQUEST = 0xa1;//出牌请求_跑得快
		public const int CHUPAI_PDK_RESPONSE = 0xa2;//出牌通知_跑得快
		public const int CHIBUQI_PDK_RESPONSE = 0xa3;//吃不起_跑得快

        //  -lan    新增斗地主协议
        public const int QIANG_DDZ_REQUEST = 0xa98;   //斗地主抢庄请求
        public const int QIANG_DDZ_RESPONE = 0xa99;   //斗地主抢庄回调
        public const int ZHUANG_DDZ_RESPONE = 0xa100;   //斗地主庄确定回调

        public const int DDZ_TI_REQUEST = 0x110;    // 踢牌的请求
        public const int DDZ_TI_RESPONE = 0x111;    // 踢牌回调

        //斗牛
        public const int QIANG_DN_REQUEST = 0xb1;
		public const int QIANG_DN_RESPONSE = 0xb2;
		public const int ZHUANG_DN_RESPONSE = 0xb3;
		public const int ZHU_DN_REQUEST = 0xb4;
		public const int ZHU_DN_RESPONSE = 0xb5;
		public const int NIU_DN_RESPONSE = 0xb6;
		public const int NIUNOTICE_DN_REQUEST = 0xb7;
		public const int NIUNOTICE_DN_RESPONSE = 0xb8;
		public const int NIUSETTLE_DN_RESPONSE = 0xb9;

		//德州扑克
		public const int GENZHU_DZPK_REQUEST = 0xd1;
		public const int JIAZHU_DZPK_REQUEST = 0xd2;
		public const int QIPAI_DZPK_REQUEST = 0xd3;
		public const int RANGPAI_DZPK_REQUEST = 0xd4;

		public const int GENZHU_DZPK_RESPONSE = 0xd5;
		public const int JIAZHU_DZPK_RESPONSE = 0xd6;
		public const int QIPAI_DZPK_RESPONSE = 0xd7;
		public const int RANGPAI_DZPK_RESPONSE = 0xd8;
		public const int PUTOFF_DZPK_RESPONSE = 0xd9;
		public const int FAPAI_DZPK_RESPONSE = 0xda;


		//cheat
		public const int CHEAT_DN_Request = 0xaa1;
		public const int CHEAT_DN_Response = 0xaa2;

		public const int CHEAT_Request = 0xaa3;
		public const int CHEAT_Response = 0xaa4;
    }



}

