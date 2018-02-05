using System;

namespace AssemblyCSharp
{
	public class XiaZhuRequest :ClientRequest
	{
		public XiaZhuRequest (int zhu)
		{
			headCode = APIS.ZHU_DN_REQUEST;
			if (GlobalDataScript.loginResponseData != null) {
				messageContent = ""+zhu;//GlobalDataScript.loginResponseData.account.uuid +
			}
		}
	}
}

