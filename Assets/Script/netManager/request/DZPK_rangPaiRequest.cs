using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class DZPK_rangPaiRequest : ClientRequest {
		public DZPK_rangPaiRequest(){
			headCode = APIS.RANGPAI_DZPK_REQUEST;
			messageContent = "";
		}
	}
}
