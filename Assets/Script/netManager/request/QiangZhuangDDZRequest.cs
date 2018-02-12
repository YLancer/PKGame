using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AssemblyCSharp
{
    public class QiangZhuangDDZRequest : ClientRequest
    {
        public QiangZhuangDDZRequest(int QiangDDZ)
        {
            headCode = APIS.QIANG_LANDLORDS_REQUEST;     // 斗地主抢庄的请求
            if (GlobalDataScript.loginResponseData != null)
            {
                messageContent = "" + QiangDDZ;           
            }
        }
    }
}
