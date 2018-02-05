using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class DNFinalOverScript : MonoBehaviour {

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

	public Text niuText;//牛牛数量
	public Text peiText;//通赔数量
	public Text shaText;//通杀数量
	public Text shengliText;//通杀数量

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void setUI_DN_final(FinalGameEndItemVo itemData)
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


		niuText.text = itemData.nius.ToString ();
		peiText.text = itemData.peis.ToString ();
		shaText.text = itemData.shas.ToString ();
		shengliText.text = itemData.shengju.ToString () + "胜" + itemData.fuju.ToString () + "负";
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
