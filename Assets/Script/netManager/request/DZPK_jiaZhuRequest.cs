using UnityEngine;
using System.Collections;
namespace AssemblyCSharp
{
	public class DZPK_jiaZhuRequest : ClientRequest {
		public DZPK_jiaZhuRequest(string zhu){
			headCode = APIS.JIAZHU_DZPK_REQUEST;
			messageContent = zhu;
		}
	}
}