using System;
using LitJson;

namespace AssemblyCSharp
{
	public class PDK_NOputCardRequest : ClientRequest
	{
		public PDK_NOputCardRequest(int NoCard)
		{
            //headCode = APIS.CHUPAI_PDK_REQUEST;  原游戏出牌请求
            headCode = APIS.LANDLORDS_YAOBUQI_REQUEST;   //斗地主出牌请求
            messageContent = JsonMapper.ToJson (NoCard);;
		}
	}
}

