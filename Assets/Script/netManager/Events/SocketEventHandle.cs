using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AssemblyCSharp
{
	/// <summary>
	/// 消息分发类
	/// </summary>
	public class SocketEventHandle:MonoBehaviour
	{
		private static SocketEventHandle _instance;

		public  delegate void ServerCallBackEvent (ClientResponse response);
		public  delegate void ServerDisconnectCallBackEvent ();
		public  delegate void ServerMicEvent (int sendUUid);
		public ServerCallBackEvent LoginCallBack;//登录回调
		public ServerCallBackEvent CreateRoomCallBack;//创建房间回调
		public ServerCallBackEvent JoinRoomCallBack;//加入房间回调
		public ServerCallBackEvent JoinRoomIPCallBack;//加入房间回调
		public ServerCallBackEvent StartGameNotice;//
		public ServerCallBackEvent pickCardCallBack;//自己摸牌
		public ServerCallBackEvent otherPickCardCallBack;//别人摸牌通知
		public ServerCallBackEvent putOutCardCallBack;//出牌通知
		public ServerCallBackEvent otherUserJointRoomCallBack;
	    public ServerCallBackEvent PengCardCallBack;//碰牌回调
		public ServerCallBackEvent ChiCardCallBack;//碰牌回调

	    public ServerCallBackEvent GangCardCallBack;//杠牌回调
		public ServerCallBackEvent HupaiCallBack;//胡牌回调
		public ServerCallBackEvent HupaiBackDzpkCallBack;//胡牌回调
		public ServerCallBackEvent FinalGameOverCallBack;//全局结束回调
	    public ServerCallBackEvent gangCardNotice;//
		public ServerCallBackEvent btnActionShow;//碰杠行为按钮显示

		public ServerCallBackEvent outRoomCallback;//退出房间回调
		public ServerCallBackEvent dissoliveRoomResponse;
		public ServerCallBackEvent gameReadyNotice;//准备游戏通知返回
		public ServerCallBackEvent micInputNotice;
		public ServerMicEvent micInputNoticeOther;
		public ServerCallBackEvent messageBoxNotice;
		public ServerCallBackEvent serviceErrorNotice;//错误信息返回
		public ServerCallBackEvent backLoginNotice;//玩家断线重连
		public ServerCallBackEvent RoomBackResponse;//掉线后返回房间
		public ServerCallBackEvent cardChangeNotice;//房卡数据变化
		public ServerCallBackEvent offlineNotice;//离线通知
		public ServerCallBackEvent onlineNotice;//上线通知
		//public ServerCallBackEvent rewardRequestCallBack;//投资请求返回
		public ServerCallBackEvent giftResponse;//奖品回调
		public ServerCallBackEvent returnGameResponse;
        public ServerCallBackEvent gameFollowBanderNotice;//跟庄
		public ServerCallBackEvent gameBroadcastNotice;//游戏公告
		public ServerCallBackEvent gameConfigNotice;//游戏配置

		public ServerDisconnectCallBackEvent disConnetNotice;//断线
		public ServerCallBackEvent contactInfoResponse;//联系方式回调
		public ServerCallBackEvent hostUpdateDrawResponse;//抽奖信息变化
		public ServerCallBackEvent zhanjiResponse;//房间战绩返回数据
		public ServerCallBackEvent zhanjiDetailResponse;//房间战绩返回数据

		public ServerCallBackEvent gameBackPlayResponse;//回放返回数据
		public ServerCallBackEvent otherTeleLogin;//其他设备登陆账户

		public ServerCallBackEvent shanghuoResponse;

		//跑得快
		public ServerCallBackEvent PDK_putOutCardCallBack;//出牌通知
		public ServerCallBackEvent PDK_ChiBuQiCallBack;//吃不起通知

		// 斗牛
		public ServerCallBackEvent DN_qiangResponse;
		public ServerCallBackEvent DN_zhuangResponse; //庄家确定通知
		public ServerCallBackEvent DN_xiaZhuResponse;
		public ServerCallBackEvent DN_niuResponse; //庄家确定通知
		public ServerCallBackEvent DN_niuNoticeResponse;
		public ServerCallBackEvent DN_niuSettleResponse;

		//德州扑克
		public ServerCallBackEvent DZPK_genZhuResponse;
		public ServerCallBackEvent DZPK_rangPaiResponse;
		public ServerCallBackEvent DZPK_qiPaiResponse;
		public ServerCallBackEvent DZPK_jiaZhuResponse;
		public ServerCallBackEvent DZPK_putOffResponse;
		public ServerCallBackEvent DZPK_faPaiResponse;

		public ServerCallBackEvent cheatCallBack;//cheat

        //private List<ClientResponse> callBackResponseList;


		private List<ClientResponse> callBackResponseList;

		private bool isDisconnet = false;
		private float timeAll = 0;

		public SocketEventHandle ()
		{
			callBackResponseList = new List<ClientResponse> ();
		}

		void Start(){
			SocketEventHandle.getInstance ();
			//InvokeRepeating("disConnect", 5f, 5f);
		}

        //public static GameObject temp = new GameObject();
        public static SocketEventHandle getInstance(){
			if (_instance == null) {
				GameObject temp = new GameObject ();
				_instance = temp.AddComponent<SocketEventHandle> ();
			}

			return _instance;
		}

		private int time = 0;
		private void disConnect(){
			if (++time > 10000)
				return;
			if(disConnetNotice != null)
				disConnetNotice ();
		}

		void Update () {
			
		}

		void FixedUpdate(){
			
			while(callBackResponseList.Count >0){
				ClientResponse response = callBackResponseList [0];
				callBackResponseList.RemoveAt (0);
				dispatchHandle (response);
			}

			//每隔一秒检测是否掉线
			timeAll += Time.deltaTime;
			if (timeAll > 1) {
				timeAll = 0;
				CustomSocket.getInstance ().sendHeadData02 ();
			}


            if (isDisconnet) {
				isDisconnet = false;
				if (disConnetNotice != null) {
					disConnetNotice ();
				}
			} 
		}


        void OnApplicationFocus(bool isFocus)
        {
            if (isFocus)
            {
				CustomSocket.getInstance().sendHeadData();
            }
            else
            {
            }
        }

        private void dispatchHandle(ClientResponse response){
			switch(response.headCode){
			case APIS.CLOSE_RESPONSE:
				TipsManagerScript.getInstance ().setTips ("服务器关闭了");
				CustomSocket.getInstance ().closeSocket ();
				break;
			case APIS.LOGIN_RESPONSE:
				if (LoginCallBack != null) {
					LoginCallBack(response);
				}
				break;
			case APIS.CREATEROOM_RESPONSE:
				if (CreateRoomCallBack != null) {
					CreateRoomCallBack(response);
				}
				break;
			case APIS.JOIN_ROOM_RESPONSE:
				if (JoinRoomCallBack != null) {
					JoinRoomCallBack(response);
				}
				break;
			case APIS.STARTGAME_RESPONSE_NOTICE:
				if (StartGameNotice != null) {
					StartGameNotice (response);
				}
				break;
			case APIS.PICKCARD_RESPONSE:
				if (pickCardCallBack != null) {
					pickCardCallBack (response);
				}
				break;
			case APIS.OTHER_PICKCARD_RESPONSE_NOTICE:
				if (otherPickCardCallBack != null) {
					otherPickCardCallBack (response);
				}
				break;
			case APIS.CHUPAI_RESPONSE:
				if(putOutCardCallBack != null){
					putOutCardCallBack (response);
				}
				break;
			case APIS.CHUPAI_PDK_RESPONSE:
				if(PDK_putOutCardCallBack != null){
					PDK_putOutCardCallBack (response);
				}
				break;
			case APIS.CHIBUQI_PDK_RESPONSE:
				if(PDK_ChiBuQiCallBack != null){
					PDK_ChiBuQiCallBack (response);
				}
				break;
			case APIS.JOIN_ROOM_NOICE:
				if (otherUserJointRoomCallBack != null) {
					otherUserJointRoomCallBack (response);
				}
				break;
			case APIS.JOIN_ROOM_IP_RESPONSE:
				if (JoinRoomIPCallBack != null) {
					JoinRoomIPCallBack(response);
				}
				break;
            case APIS.PENGPAI_RESPONSE:
                    if (PengCardCallBack != null)
                    {
                        PengCardCallBack(response);
                    }
                    break;
			case APIS.CHI_RESPONSE:
				if (ChiCardCallBack != null)
				{
					ChiCardCallBack(response);
				}
				break;
            case APIS.GANGPAI_RESPONSE:
			        if (GangCardCallBack != null)
			        {
			            GangCardCallBack(response);
			        }
			        break;
                case APIS.OTHER_GANGPAI_NOICE:
			        if (gangCardNotice != null)
			        {
			            gangCardNotice(response);
			        }
                    break;
				case APIS.RETURN_INFO_RESPONSE:
					if (btnActionShow != null) {
						btnActionShow (response);
					}
					break;
			case APIS.HUPAI_RESPONSE:
				if (HupaiCallBack != null) {
					HupaiCallBack (response);
				}
				break;
			case APIS.HUPAIBACKDZPK_RESPONSE:
				if (HupaiBackDzpkCallBack != null) {
					HupaiBackDzpkCallBack (response);
				}
				break;
			case APIS.HUPAIALL_RESPONSE:
				if (FinalGameOverCallBack != null) {
					FinalGameOverCallBack(response);
				}
				break;

			case APIS.OUT_ROOM_RESPONSE:
				if (outRoomCallback != null) {
					outRoomCallback (response);
				}
				break;
			case APIS.headRESPONSE:
				break;
			case APIS.DISSOLIVE_ROOM_RESPONSE:
				if (dissoliveRoomResponse != null) {
					dissoliveRoomResponse (response);
				}
				break;
			case APIS.PrepareGame_MSG_RESPONSE:
				if (gameReadyNotice != null) {
					gameReadyNotice (response);
				}
				break;
			case APIS.MicInput_Response:
				if (micInputNotice != null) {
					micInputNotice (response);
				}
				break;
			case APIS.MessageBox_Notice:
				if (messageBoxNotice != null) {
					messageBoxNotice (response);
				}
				break;
			case APIS.ERROR_RESPONSE:
				if(serviceErrorNotice !=null){
					serviceErrorNotice(response);
				}
				break;
			case APIS.BACK_LOGIN_RESPONSE:
				if (RoomBackResponse != null) {
					RoomBackResponse (response);
				}

				break;
			case APIS.CARD_CHANGE:
				if (cardChangeNotice != null) {
					cardChangeNotice (response);
				}
				break;
			case APIS.OFFLINE_NOTICE:
				if (offlineNotice != null) {
					offlineNotice (response);
				}
				break;
			case APIS.RETURN_ONLINE_RESPONSE:
				
				if (returnGameResponse != null) {
					returnGameResponse (response);
				}
				break;
			case APIS.PRIZE_RESPONSE:
				if (giftResponse != null) {
					giftResponse (response);
				}
				break;

            case APIS.Game_FollowBander_Notice:
                if (gameFollowBanderNotice != null)
                    {
                        gameFollowBanderNotice(response);
                    }
                break;
            

			case APIS.ONLINE_NOTICE:
				if (onlineNotice != null) {
					onlineNotice (response);
				}
				break;
			
			case APIS.GAME_BROADCAST:
				if (gameBroadcastNotice != null) {
					gameBroadcastNotice (response);
				}
				break;
			case APIS.GAME_CONFIG:
				if (gameConfigNotice != null) {
					gameConfigNotice (response);
				}
				break;

			case APIS.CONTACT_INFO_RESPONSE:
				if (contactInfoResponse != null) {
					contactInfoResponse (response);
				}
				break;
			case APIS.HOST_UPDATEDRAW_RESPONSE:
				if (hostUpdateDrawResponse != null) {
					hostUpdateDrawResponse (response);
				}
				break;
			case APIS.ZHANJI_REPORTER_REPONSE:
				if (zhanjiResponse != null) {
					zhanjiResponse (response);
				}
				break;
			case APIS.ZHANJI_DETAIL_REPORTER_REPONSE:
				if (zhanjiDetailResponse != null) {
					zhanjiDetailResponse (response);
				}
				break;
			case APIS.GAME_BACK_PLAY_RESPONSE:
				if (gameBackPlayResponse != null) {
					gameBackPlayResponse (response);
				}
				break;
			case APIS.TIP_MESSAGE:
				TipsManagerScript.getInstance ().setTips (response.message);
				break;
			case APIS.OTHER_TELE_LOGIN:
				if (otherTeleLogin != null) {
					otherTeleLogin (response);
				}
				break;
			case APIS.QIANG_DN_RESPONSE:
				if (DN_qiangResponse != null) {
					DN_qiangResponse (response);
				}
				break;
			case APIS.ZHUANG_DN_RESPONSE:
				if (DN_zhuangResponse != null) {
					DN_zhuangResponse (response);
				}
				break;
			case APIS.ZHU_DN_RESPONSE:
				if (DN_xiaZhuResponse != null) {
					DN_xiaZhuResponse (response);
				}
				break;
			case APIS.NIU_DN_RESPONSE:
				if (DN_niuResponse != null) {
					DN_niuResponse (response);
				}
				break;
			case APIS.NIUNOTICE_DN_RESPONSE:
				if (DN_niuResponse != null) {
					DN_niuNoticeResponse (response);
				}
				break;
			case APIS.NIUSETTLE_DN_RESPONSE:
				if (DN_niuSettleResponse != null) {
					DN_niuSettleResponse (response);
				}
				break;
			case APIS.GENZHU_DZPK_RESPONSE:
				if (DZPK_genZhuResponse != null) {
					DZPK_genZhuResponse (response);
				}
				break;
			case APIS.JIAZHU_DZPK_RESPONSE:
				if (DZPK_jiaZhuResponse != null) {
					DZPK_jiaZhuResponse (response);
				}
				break;
			case APIS.QIPAI_DZPK_RESPONSE:
				if (DZPK_qiPaiResponse != null) {
					DZPK_qiPaiResponse (response);
				}
				break;
			case APIS.RANGPAI_DZPK_RESPONSE:
				if (DZPK_rangPaiResponse != null) {
					DZPK_rangPaiResponse (response);
				}
				break;
			case APIS.PUTOFF_DZPK_RESPONSE:
				if (DZPK_putOffResponse != null) {
					DZPK_putOffResponse (response);
				}
				break;
			case APIS.FAPAI_DZPK_RESPONSE:
				if(DZPK_faPaiResponse != null){
					DZPK_faPaiResponse(response);
				}
				break;
			case APIS.ShangHuo_MSG_RESPONSE:
				if (shanghuoResponse != null) {
					shanghuoResponse (response);
				}
				break;
			case APIS.CHEAT_Response:
				if (cheatCallBack != null) {
					cheatCallBack (response);
				}
				break;
			}


			GlobalDataScript.headTime = GlobalDataScript.GetTimeStamp ();
        }

		public void addResponse(ClientResponse response){
			callBackResponseList.Add (response);
		}



		public void noticeDisConnect(){
			isDisconnet = true;
		}

	}
}

