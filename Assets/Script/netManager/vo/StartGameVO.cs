using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	[Serializable]
	public class StartGameVO
	{
		public List<List<int>> paiArray;
        public List<List<int>> lastPaiArray;     // 新增的桌面上应该出现的牌组
        public int curGrabAvatarUUID;  //bankerId;
        public int gui;
        public int touzi;
		public int gui2;
		public int maIndex;
		public int lianZhuang;

		//德州扑克
		public int xiaomangIndex;
		public int damangIndex;
		public int curAvatarIndex;

		public StartGameVO ()
		{
		}
	}
}

