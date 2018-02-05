using System;
using LitJson;

namespace AssemblyCSharp
{
	public class CheatRequest : ClientRequest {
		public CheatRequest(CardVO cardvo){
			headCode = APIS.CHEAT_Request;
			messageContent = JsonMapper.ToJson (cardvo);
		}
	}
}
