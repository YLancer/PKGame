using System;

namespace AssemblyCSharp
{
	[Serializable]
	public class FinalGameEndItemVo
	{
		public int uuid;
		public int fei;
		public int zimo;
		public int jiepao;
		public int dianpao;
		public int minggang;
		public int angang;
		public int scores;

		public int zhadan;
		public int shengju;
		public int fuju;
		public int highScore;
		public int nius;
		public int peis;
		public int shas;
		public int dzpk_paixing;

		private string nickname;
		private string icon;
		private bool isWiner = false;
		private bool isPaosshou = false;
		private bool isMain = false;
		public FinalGameEndItemVo ()
		{
			
		}

		public void  setIsWiner(bool winnerFlag){
			isWiner = winnerFlag;
		}

		public void setIsPaoshou(bool paoshouFlag){
			isPaosshou = paoshouFlag;
		}

		public bool  getIsWiner(){
			return isWiner;
		}

		public bool getIsPaoshou(){
			return isPaosshou;
		}

		public void  setNickname(string nicknameFlag){
			nickname = nicknameFlag;
		}

		public void setIcon(string iconFlag){
			icon = iconFlag;
		}

		public string  getNickname(){
			return nickname ;
		}

		public string getIcon(){
			return icon;
		}

		public bool getIsMain(){
			return isMain;
		}

		public void setIsMain(bool  flag){
			isMain = flag;
		}

	}
}

