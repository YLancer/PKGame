using System;
using LitJson;

namespace AssemblyCSharp
{
	public class PDK_PutOutCardRequest:ClientRequest
	{
		public PDK_PutOutCardRequest (pdkCardVO cardvo)
		{
			headCode = APIS.CHUPAI_PDK_REQUEST;
			messageContent = JsonMapper.ToJson (cardvo);;
		}
	}
}

