using System;
using System.Collections;

namespace AssemblyCSharp
{
	public class CreateRoomRequest:ClientRequest
	{
		public CreateRoomRequest (string sendMsg)
		{
			//headCode = APIS.CREATEROOM_REQUEST;          原本创建房间的请求
            headCode = APIS.LANDLORD_CREATE_ROOM_REQUEST;        // 斗地主创建房间的请求
            messageContent = sendMsg;
		}

	}
}

