using System;
using LitJson;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class CheatDnRequest : ClientRequest {
		public CheatDnRequest(List<int> cardList){
			headCode = APIS.CHEAT_DN_Request;
			DnCheatMsgVo vo = new DnCheatMsgVo ();
			vo.card0 = cardList [0];
			vo.card1 = cardList [1];
			vo.card2 = cardList [2];
			vo.card3 = cardList [3];
			vo.card4 = cardList [4];

			messageContent = JsonMapper.ToJson (vo);
		}
	}
}
