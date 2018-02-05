using System;

namespace AssemblyCSharp
{
	public class NiuNoticeRequest : ClientRequest
	{
		public NiuNoticeRequest() 
		{
			headCode = APIS.NIUNOTICE_DN_REQUEST;
			if (GlobalDataScript.loginResponseData != null) {
				messageContent = GlobalDataScript.loginResponseData.account.uuid + "";//
			}
		}
	}
}

