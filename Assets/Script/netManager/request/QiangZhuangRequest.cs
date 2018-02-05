using System;

namespace AssemblyCSharp
{
	public class QiangZhuangRequest :ClientRequest
	{
		public QiangZhuangRequest (int isQiang)
		{
			headCode = APIS.QIANG_DN_REQUEST;
			if (GlobalDataScript.loginResponseData != null) {
				messageContent = ""+isQiang;//GlobalDataScript.loginResponseData.account.uuid +
			}
		}
	}
}

