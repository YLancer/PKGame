using System;
using UnityEngine;
using UnityEngine.UI;
/**
 * 服务器返回错误
 */
namespace AssemblyCSharp
{
	public class ServiceErrorListener :  MonoBehaviour
	{
        public Image watingPanel;

        public ServiceErrorListener ()
		{
			SocketEventHandle.getInstance().serviceErrorNotice += serviceErrorNotice;
		}

		public void serviceErrorNotice(ClientResponse response){
            watingPanel.gameObject.SetActive(false);
            TipsManagerScript.getInstance().setTips(response.message);
		}
	}
}

