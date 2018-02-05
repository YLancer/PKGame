using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	[Serializable]
	public class CheatResponseVO {
		public List<int> paiArray;
		public int state;//-1:显示所有牌，-2:状态不对不是摸牌状态，其他:牌的值

		public CheatResponseVO(){
		}
	}
}
