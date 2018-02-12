using System;

namespace AssemblyCSharp
{
	public class OutRoomRequest :ClientRequest
	{
		
		public OutRoomRequest (string sendMsg)
		{
            //headCode = APIS.OUT_ROOM_REQUEST;     原游戏退出请求
            headCode =APIS.LANDLORDS_OUTROOM_REQUEST;   // 斗地主剔除请求
            messageContent = sendMsg;
		}
	}
}

