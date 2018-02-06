using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	[Serializable]
	public class RoomCreateVo
	{
		public List<AvatarVO> playerList;       

        public  bool hong;
		public int ma;
		public int roomId;
		public int roomType;//1转转；2、划水；3、长沙 ；4、广东
		 /**局数**/
        public int roundNumber;
		public int currentRound;
		public bool sevenDouble;
		public int ziMo;//1：自摸胡；2、抢杠胡
		public int xiaYu;
		public string name;
		public bool addWordCard = true;
		public int magnification;
        public int gui;//鬼牌 0无鬼；1白板；2翻鬼
        public bool gangHu;//可抢杠胡
        public int guiPai=-1;
		public int guiPai2=-1;
		public bool gangHuQuanBao = false;
		public bool wuGuiX2 = false;
		public bool maGenDifen = false;
		public bool maGenGang = false;

		public int shangxiaFanType;
		public int diFen;
		public bool tongZhuang;
		public int pingHu;

		public bool keDianPao;//可点炮胡
		public bool lunZhuang;//轮庄或霸王庄

		//潮汕麻将
		//public bool gengZhuang = false;           //跟庄 //
		public bool genZhuang = false;              //跟庄
		public bool qiDui = false;
		public bool qiangGangHu = false;
		public bool pengpengHu = false;
		public bool qingYiSe = false;
		public bool gangShangKaiHua = false;         //杠上开花
		public bool haoHuaQiDui = false;             //豪华七对
		public bool shiSanYao = false;               //十三幺
		public bool tianDiHu = false;                //天地胡
		public bool shuangHaoHua = false;            //双豪华
		public bool quanFeng = false;                //全风
		public bool huaYaoJiu = false;               //花幺九
		public int fengDing = 9999;                  //封顶  默认100 不封顶
		public bool jiejiegao = false;                //节节高

		//新潮汕麻将
		public bool chongZhuang = false;
		public bool sanHaoHua = false;
		public bool shiBaLuoHan = false;
		public bool xiaoSanYuan = false;
		public bool xiaoSiXi = false;
		public bool daSanYuan = false;
		public bool daSiXi = false;
		public bool zhuaMa = false;
		public bool hunYiSe = false;

		//跑得快 gameType=1 .麻将全部为0
		public int gameType=0;
		public bool zhang16 = true;//16张
		public bool showPai = true;//显示牌
		public bool xian3 = true;//先出黑桃3

        // 斗地主    -lan
        public int bombMultiple;     // 斗地主限制炸弹数目，在结算时用  1、4倍  2、5倍  3、6倍   4、无限制
        public bool isKick = true;   // 踢为加倍

        //斗牛
        public bool qiang = false;
		public bool ming = false;
		public int mengs = 1;
		public bool AA = false;
		public int qiangForNiu = -1;
		public int zhuForNiu = 0;
		public bool niu7fan;

		//德州扑克
		public int initFen_dzpk;

		//添加金币场
		public bool goldType = false;

        // 十三水玩法
        public int typeOfTensanshui;    //35.经典十三水   36.疯狂纯一色   37.七人加三色    38.七人霸王庄

        public RoomCreateVo()
		{

		}
	}
}

