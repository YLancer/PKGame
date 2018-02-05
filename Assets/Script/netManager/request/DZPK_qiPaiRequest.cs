using UnityEngine;


namespace AssemblyCSharp
{
	public class DZPK_qiPaiRequest : ClientRequest {
		public DZPK_qiPaiRequest(){
			headCode = APIS.QIPAI_DZPK_REQUEST;
			messageContent = "";
		}

	}
}