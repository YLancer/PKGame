using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	[Serializable]
	public class RoomJoinResponseVo
	{
		public bool addWordCard;
		public bool hong;
		public int ma;
		public string name;
		public int roomId;
		public int roomType;
		public int roundNumber;
		public bool sevenDouble;
		public int xiaYu;
		public int ziMo;
		public int magnification;
        public int gui;//鬼牌 0无鬼；1白板；2翻鬼
        public bool gangHu;//可抢杠胡
        public List<AvatarVO> playerList;
        public int guiPai;
		public int guiPai2;
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
		public bool gengZhuang = false;              //跟庄
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

		//斗牛
		public bool qiang = false;
		public bool ming = false;
		public int mengs = 0;
		public bool AA = false;
		public int qiangForNiu = -1;
		public int zhuForNiu = 0;
		public bool noticedForNiu = false;
		public int stateForNiu = 0;

		//添加金币场
		public bool goldType = false;


		//public LastOperationVo lastOperationVo;
		public RoomJoinResponseVo ()
		{
		}
	}
}

