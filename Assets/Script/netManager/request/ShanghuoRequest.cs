using System;
using LitJson;

namespace AssemblyCSharp
{
	public class ShanghuoRequest : ClientRequest
	{
		public ShanghuoRequest (bool shanghuo, int piao)
		{
			headCode = APIS.ShangHuo_MSG_REQUEST;
			ShanghuoRequestVo vo = new ShanghuoRequestVo();
			vo.shanghuo = shanghuo;
			vo.piao = piao;
			messageContent = JsonMapper.ToJson (vo);
		}
	}
}
