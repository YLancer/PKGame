using System;

namespace AssemblyCSharp
{
	public class GameReadyRequest:ClientRequest
	{
		public GameReadyRequest ()
		{

            // headCode = APIS.PrepareGame_MSG_REQUEST;   原本游戏的准备请求
            headCode = APIS.LANDLORDS_READY_REQUEST;          //斗地主游戏的准备请求
            messageContent = "ss";
		}
	}
}

