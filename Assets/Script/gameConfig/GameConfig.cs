using System;

namespace AssemblyCSharp
{
	public enum GameType : int{
		GameType_NULL            = -1,
		GameType_MJ_Developing   = 0,
		GameType_MJ_ZhuangZhuang = 1,
		GameType_MJ_HuaShui      = 2,
		GameType_MJ_ChangSha     = 3,
		GameType_MJ_GuangDong    = 4,
		GameType_MJ_GanZhou      = 5,
		GameType_MJ_RuiJin       = 6,
		GameType_MJ_HongZhong    = 7,
		GameType_MJ_ChaoShan     = 10,
		GameType_MJ_YiChun       = 11,//
		GameType_PK_PDK          = 31,
		GameType_PK_DN           = 32,
		GameType_PK_DZPK         = 33,
        GameType_PK_AMH          = 34,

        GameType_Tensanshui  =35,
        GameType_Purecolor = 36,
        GameType_Sevenandthree = 37,
        GameType_SevenmenOverlord = 38,
        GameType_PK_DDZ = 40,
    }

    public enum GameTypePK //TODO 临时的统一方案
    {
        NULL = 0,
        PDK = 1,
        DN = 3,
        DZPK = 4,
        AMH = 5,  // 未用到    奥马哈和德扑用AA 来区分

        Tensanshui =6,

        DDZ=7,
    }


	public class GameConfig
	{
		/**
		public const int GAME_TYPE_ZHUANZHUAN = 1;
		public const int GAME_TYPE_HUASHUI = 2;
		public const int GAME_TYPE_CHANGSHA = 3;
		public const int GAME_TYPE_GUANGDONG = 4;
		public const int GAME_TYPE_GANZHOU = 5;
		public const int GAME_TYPE_RUIJIN = 6;
		*/

        public const float  GAME_DEFALUT_AGREE_TIME = 100.0f;

		public GameConfig ()
		{
		}
	}
}

