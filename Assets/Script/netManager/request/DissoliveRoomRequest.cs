using System;

namespace AssemblyCSharp
{
	public class DissoliveRoomRequest:ClientRequest
	{
		public DissoliveRoomRequest (string msg)
		{
            //headCode = APIS.DISSOLIVE_ROOM_REQUEST;     原游戏申请解散房间
            headCode = APIS.LANDLORDS_APPLYLEAVE_ROOM_REQUEST;   // 斗地主申请解散房间
            messageContent = msg;
		}
	}
}

