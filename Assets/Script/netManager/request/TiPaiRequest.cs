using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
    public class TiPaiRequest : ClientRequest
    {

        public TiPaiRequest(int ti)
        {
            headCode = APIS.LANDLORDS_TI_REQUEST;          //踢牌的请求
            if (GlobalDataScript.loginResponseData != null)
            {
                messageContent = "" + ti;
            }
        }
    }
}
