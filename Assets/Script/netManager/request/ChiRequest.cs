using System;
using LitJson;

namespace AssemblyCSharp
{
	public class ChiRequest: ClientRequest
	{
		public ChiRequest (CardVO cardvo)
		{
			headCode = APIS.CHI_REQUEST;
			messageContent = JsonMapper.ToJson (cardvo);
		}
	}
}

