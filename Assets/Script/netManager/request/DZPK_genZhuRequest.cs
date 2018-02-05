using UnityEngine;

namespace AssemblyCSharp
{
	public class DZPK_genZhuRequest : ClientRequest {
		public DZPK_genZhuRequest(){
			headCode = APIS.GENZHU_DZPK_REQUEST;
			messageContent = "";
		}

	}
}

