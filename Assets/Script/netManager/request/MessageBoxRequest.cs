using System;

namespace AssemblyCSharp
{
	public class MessageBoxRequest : ClientRequest
	{
		public MessageBoxRequest (int type, string codeIndex, int uuid)
		{
            //headCode = APIS.MessageBox_Request;  发送聊天消息的请求
            headCode = APIS.LANDLORDS_CHATMESSAGE_REQUEST;
			messageContent = type + "|" + codeIndex + "|"+uuid;
		}

		public MessageBoxRequest (int type, string codeIndex, int srcuuid, int destuuid)
		{
            //headCode = APIS.MessageBox_Request;
            headCode = APIS.LANDLORDS_CHATMESSAGE_REQUEST;
            messageContent = type + "|" + codeIndex + "|"+srcuuid + "|" + destuuid;
		}
	}
}

