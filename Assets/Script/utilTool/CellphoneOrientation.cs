using System;
using UnityEngine;
namespace AssemblyCSharp
{
	public class CellphoneOrientation : MonoBehaviour
	{
		public CellphoneOrientation ()
		{
		}
		void Awake()  
		{  
			/// 如果在发布游戏包的时候，在playerSetting中设置了禁止屏幕翻转，但是代码中设置屏幕是可自动翻转，则游戏发布出来后，任然是可翻转的。  
			Screen.orientation = ScreenOrientation.AutoRotation;  
			/// 下面几个bool值设置了是否可以翻转到某个方向。false代表是禁止  
			Screen.autorotateToLandscapeLeft = true;  
			Screen.autorotateToLandscapeRight = true;  
			Screen.autorotateToPortrait = false;  
			Screen.autorotateToPortraitUpsideDown = false;  
		}  
	}
}

