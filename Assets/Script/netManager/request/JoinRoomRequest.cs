using System;

namespace AssemblyCSharp
{
	public class JoinRoomRequest : ClientRequest
	{
		public JoinRoomRequest (string sendMsg)
		{
            //headCode = APIS.JOIN_ROOM_REQUEST;   原本加入房间的请求
            headCode = APIS.LANDLORD_JOIN_ROOM_REQUEST;       // 斗地主加入房间的请求
			messageContent = sendMsg;
		}
	}
}

