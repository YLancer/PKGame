using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AssemblyCSharp
{
    public class finalOverItem : MonoBehaviour
    {
        public Text nickName;//昵称
        public Text ID;//id
        public Image icon;//头像
        public Image fangzhu;//房主
        public GameObject winer;//打赢家
        public GameObject paoshou;//点炮手
		public Text feiCount;//自摸次数
        public Text zimoCount;//自摸次数
        public Text jiepaoCount;//接炮次数
        public Text dianpaoCount;//点炮次数
        public Text angangCount;//暗杠次数
        public Text minggangCount;//明杠次数
        public Text finalScore;//总成绩
        private string headIcon;
        private Texture2D texture2D;         //下载的图片

		public Text shengpaiText;//剩牌数量
		public Text zhadanText;//炸弹数量
		public Text scoreText;//积分

		public Text highScoreText;//最高分
		public Text zhadanTimesText;//炸弹次数
		public Text shengfuText;//胜负局数
        public finalOverItem()
        {
        }

        public void setUI(FinalGameEndItemVo itemData)
        {

            MyDebug.Log("--setUI--------------------Panel_Final_Item--------进入-----------------");


            if (itemData.getIsMain())
                fangzhu.gameObject.SetActive(true);
            else
                fangzhu.gameObject.SetActive(false);
            MyDebug.Log("--setUI--------------------Panel_Final_Item-------显示----------------");
            nickName.text = itemData.getNickname();
            ID.text = "ID:" + itemData.uuid + "";
            if (itemData.getIsWiner() && itemData.scores > 0)
            {
                winer.SetActive(true);
            }
            if (itemData.getIsPaoshou() && itemData.dianpao > 0)
            {
                paoshou.SetActive(true);
            }

			feiCount.text = itemData.fei + "";
            zimoCount.text = itemData.zimo + "";
            jiepaoCount.text = itemData.jiepao + "";
            dianpaoCount.text = itemData.dianpao + "";
            angangCount.text = itemData.angang + "";
            minggangCount.text = itemData.minggang + "";
            finalScore.text = itemData.scores + "";
            headIcon = itemData.getIcon();
            StartCoroutine(LoadImg());
            MyDebug.Log("--setUI--------------------Panel_Final_Item-------赋值完毕----------------");

        }
			

		public void setUI_pdk_signal(HupaiResponseItem itemData)
		{
			MyDebug.Log("--setUI--------------------Panel_Final_Item--------进入-----------------");

			if (itemData.uuid == GlobalDataScript.mainUuid)
				fangzhu.gameObject.SetActive(true);
			else
				fangzhu.gameObject.SetActive(false);
			MyDebug.Log("--setUI--------------------Panel_Final_Item-------显示----------------");
			nickName.text = itemData.nickname;
			ID.text = "ID:" + itemData.uuid + "";
			int[] paiList = itemData.paiArray;
			int paiCount = 0;

			foreach (int pai in paiList) {
				if (pai == 1)
					paiCount++;
			}
			if (paiCount == 0)
			{
				winer.SetActive(true);
			}

			/**
			if (itemData.totalScore > 0)
				winer.SetActive (true);
			*/

			shengpaiText.text = paiCount.ToString ();
			zhadanText.text = itemData.gangScore.ToString ();
			scoreText.text = itemData.totalScore.ToString ();
			headIcon = GlobalDataScript.loginResponseData.account.headicon;
			StartCoroutine(LoadImg());
			MyDebug.Log("--setUI--------------------Panel_Final_Item-------赋值完毕----------------");

		}

		public void setUI_pdk_final(FinalGameEndItemVo itemData)
		{
			if (itemData.getIsMain())
				fangzhu.gameObject.SetActive(true);
			else
				fangzhu.gameObject.SetActive(false);
			MyDebug.Log("--setUI--------------------Panel_Final_Item-------显示----------------");
			nickName.text = itemData.getNickname();
			ID.text = "ID:" + itemData.uuid + "";
			if (itemData.getIsWiner() && itemData.scores > 0)
			{
				winer.SetActive(true);
			}


			highScoreText.text = itemData.highScore.ToString ();
			zhadanTimesText.text = itemData.zhadan.ToString ();
			shengfuText.text = itemData.shengju.ToString () + "胜" + itemData.fuju.ToString () + "负";
			finalScore.text = itemData.scores + "";
			headIcon = itemData.getIcon();
			StartCoroutine(LoadImg());
		}

        private IEnumerator LoadImg()
        {

			if (headIcon.IndexOf ("http") == -1) {
				icon.sprite = GlobalDataScript.getInstance().headSprite;
				yield break;
			}

            if (FileIO.wwwSpriteImage.ContainsKey(headIcon))
            {
                icon.sprite = FileIO.wwwSpriteImage[headIcon];
                yield break;
            }
            else {


                //开始下载图片
                WWW www = new WWW(headIcon);
                yield return www;
                //下载完成，保存图片到路径filePath
                texture2D = www.texture;
                byte[] bytes = texture2D.EncodeToPNG();

                //将图片赋给场景上的Sprite
                Sprite tempSp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));
                icon.sprite = tempSp;
                FileIO.wwwSpriteImage.Add(headIcon, tempSp);
            }
        }

    }
}

