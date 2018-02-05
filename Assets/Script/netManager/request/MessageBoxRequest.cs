using System;

namespace AssemblyCSharp
{
	public class MessageBoxRequest : ClientRequest
	{
		public MessageBoxRequest (int type, string codeIndex, int uuid)
		{
			headCode = APIS.MessageBox_Request;
			messageContent = type + "|" + codeIndex + "|"+uuid;
		}

		public MessageBoxRequest (int type, string codeIndex, int srcuuid, int destuuid)
		{
			headCode = APIS.MessageBox_Request;
			messageContent = type + "|" + codeIndex + "|"+srcuuid + "|" + destuuid;
		}
	}
}

