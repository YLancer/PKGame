using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	[Serializable]

	public class HupaiResponseItem
	{
		public int[]paiArray;
		public TotalInfo totalInfo;
		public string huType;
		public string nickname;
		public int gangScore;
		public int jingScore;
		public int totalScore;
		public int uuid;
		public bool shanghuo;
		public int piao;
		private List<int> maPonits;
		public HupaiResponseItem ()
		{
		}

		public void setMaPoints(List<int> mas){
			maPonits = mas;
		}
		public List<int> getMaPoints(){
			return  maPonits;
		}
	}
}

