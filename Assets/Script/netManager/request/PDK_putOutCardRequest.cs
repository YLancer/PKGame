using System;
using LitJson;

namespace AssemblyCSharp
{
	public class PDK_PutOutCardRequest:ClientRequest
	{
		public PDK_PutOutCardRequest (pdkCardVO cardvo)
		{
            //headCode = APIS.CHUPAI_PDK_REQUEST;  原游戏出牌请求
            headCode = APIS.LANDLORDS_CHUPAI_REQUEST;   //斗地主出牌请求
            messageContent = JsonMapper.ToJson (cardvo);;
		}
	}
}

