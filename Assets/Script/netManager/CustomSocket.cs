using System;
using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using AssemblyCSharp;
using System.IO;

public class CustomSocket{
    //reset对象
    //public ManualResetEvent connectDone = new ManualResetEvent(false);
    //tcp客户端,即是与服务端通讯的组件
	int waitLen = 0;
	bool isWait = false;
    TcpClient tcpclient = new TcpClient();
    //网络流
    NetworkStream stream;

    //接受到的数据
	public byte[] databuffer = new byte[StateObject.BufferSize];//数据缓冲区;
    public int offset = 0;//处理的位置
    public int end = 0;//数据缓冲区的长度
	byte[] sources;
	byte[] headBytes;

	private int disConnectCount = 0; 

	public static bool hasStartTimer = false;
	public bool isConnected = false;
    public GameObject watingPanel;

    private static CustomSocket _instance;
	public static CustomSocket getInstance(){
		if (_instance == null) {
			_instance = new CustomSocket ();
			//_instance.Connect ();
		}
        
        return _instance;
	}

	public CustomSocket(){
		HeadRequest head = new HeadRequest ();
		headBytes = head.ToBytes();
	}
		

    /// <summary>
    /// 连接到服务器
    /// </summary>
    /// <param name="ip">服务器IP</param>
    /// <returns></returns>
    public void Connect()
    {
		AddressFamily family = AddressFamily.InterNetwork;
		//ipv4转ipv6 - 苹果ios
		#if !UNITY_EDITOR && UNITY_IPHONE
		APIS.socketUrl = IOSIpv6.GetIPv6Str(APIS.socketUrl, APIS.socketPort.ToString(), out family);
		#endif

        try
        {
			//closeSocket();//有可能是从主界面返回登陆页面，要先断开tcpclient
			tcpclient = new TcpClient(family);
            //防止延迟,即时发送!
            tcpclient.NoDelay = true;
			tcpclient.BeginConnect(APIS.socketUrl, APIS.socketPort, new AsyncCallback(ConnectCallback), tcpclient);
			//tcpclient.BeginConnect(IPAddress.Parse(APIS.socketUrl), APIS.socketPort, new AsyncCallback(ConnectCallback), tcpclient);
            MyDebug.Log(" APIS.socketUrl:" + APIS.socketUrl);
        }
        catch(Exception ex)
        {
            //设置标志,连接服务端失败!
			showMessageTip("服务器断开连接，请重新运行程序或稍后再试");
            MyDebug.Log("----------------Connect------------------------Exception----------------");
		//	ReConnectScript.getInstance().ReConnectToServer(); 
			Debug.Log(ex.ToString());   
			isConnected = false;
        }
    }

    /// <summary>
    /// 关闭网络流
    /// </summary>
    private void DisConnect()
    {
        if (tcpclient != null)
        {
            tcpclient.Close();
            tcpclient = null;
        }
        if (stream != null)
        {
            stream.Close();
            stream = null;
        }
    }

	public void sendMsg(ClientRequest client){
        MyDebug.Log("---------------sendMsg----------------messageContent--" + client.messageContent);
		SendData (client.ToBytes());
	}

	 /// <summary>
    /// 发送数据
    /// </summary>
    private void SendData(byte[] data)
    {
        MyDebug.Log("----------------SendData-----------------");
		//MyDebug.Log ("send data"+data.ToString ());
        try
        {
            if (stream != null)
            {
                MyDebug.Log("----------------SendData----2-------------");
                stream.Write(data, 0, data.Length);
            }
            else
            {
			//	showMessageTip("服务器断开连接，请重新运行程序或稍后再试");
				MyDebug.Log("22222222222222222222222222222");
				isConnected = false;
				SocketEventHandle.getInstance ().noticeDisConnect ();
				//ReConnectScript.getInstance().ReConnectToServer(); 
				//Connect();
            }
        }
        catch(Exception ex)
        {
			MyDebug.Log(ex.ToString());
            MyDebug.Log("----------------SendData------------------------Exception----------------");
		//	showMessageTip("服务器断开连接，请重新运行程序或稍后再试");
			isConnected = false;
			SocketEventHandle.getInstance ().noticeDisConnect ();
			//ReConnectScript.getInstance().ReConnectToServer(); 
			//Connect();
        }
      
    }
	/// <summary>
	/// 发送心跳包
	/// </summary>
	/// *

	public bool sendHeadData(){
		//MyDebug.Log("send head data");
		try{
			if(stream != null && tcpclient.Connected){
				stream.Write(headBytes,0,headBytes.Length);
                return true;
			}else{
				return false;
			}

		}catch(Exception ex){
			MyDebug.Log (ex.ToString());
			return false;
		}
	}

    public bool sendHeadData02()
    {
        try
        {
			//wifi和4g切换 自动重连
			if(Application.internetReachability == NetworkReachability.NotReachable){
				GlobalDataScript.network = NetworkReachability.NotReachable;
				isConnected = false;
				SocketEventHandle.getInstance ().disConnetNotice ();
				return false;
			}else {
				if(GlobalDataScript.network != NetworkReachability.NotReachable
					&& GlobalDataScript.network != Application.internetReachability){
					GlobalDataScript.network = Application.internetReachability;
					isConnected = false;
					SocketEventHandle.getInstance ().disConnetNotice ();
					return false;
				}
			}

			GlobalDataScript.network = Application.internetReachability;




			//60秒没有收到服务端消息自动断开重连
			long time = GlobalDataScript.GetTimeStamp();
			if(time - GlobalDataScript.headTime>=600*1000){
				isConnected = false;
				SocketEventHandle.getInstance ().noticeDisConnect ();
				return false;
			}

			//判断socket是否还有连接，没有了则断开
            if (stream != null && tcpclient.Connected)
            {
				return true;
            }
            else {
				isConnected = false;
				SocketEventHandle.getInstance ().noticeDisConnect ();
				return false;
            }

        }
        catch (Exception ex)
        {
            MyDebug.Log(ex.ToString());
            isConnected = false;
			SocketEventHandle.getInstance().noticeDisConnect();
            return false;
        }
    }


    private void showMessageTip(string message){
		ClientResponse temp = new ClientResponse ();
		temp.headCode = APIS.TIP_MESSAGE;
		temp.message = message;
		SocketEventHandle.getInstance ().addResponse (temp);
	}

    /// <summary>
    /// 异步连接的回调函数
    /// </summary>
    /// <param name="ar"></param>
    private void ConnectCallback(IAsyncResult ar)
    {
        //connectDone.Set();
        if ((tcpclient != null) && (tcpclient.Connected))
        {
            stream = tcpclient.GetStream();
            asyncread(tcpclient);
            isConnected = true;
            Debug.Log("服务器已经连接!");
            MyDebug.Log("---------------ConnectCallback----------------ConnectCallback--");
        }
        else
        {
			TipsManagerScript.getInstance ().setTips ("连接服务器失败");
            //tcpclient.BeginConnect(IPAddress.Parse(APIS.socketUrl), APIS.socketPort, new AsyncCallback(ConnectCallback), tcpclient);
        }
        
        TcpClient t = (TcpClient)ar.AsyncState;
        try
        {
            t.EndConnect(ar);
        }
        catch(Exception ex)
        {
            MyDebug.Log("---------------ConnectCallback----------------Exception--");
            //设置标志,连接服务端失败!
            Debug.Log(ex.ToString());
            //tcpclient.BeginConnect(IPAddress.Parse(APIS.socketUrl), APIS.socketPort, new AsyncCallback(ConnectCallback), tcpclient);
        }
    }
     /// <summary>
    /// 异步读TCP数据
    /// </summary>
    /// <param name="sock"></param>
    private void asyncread(TcpClient sock)
    {
        StateObject state = new StateObject();
        state.client = sock;
        NetworkStream stream;
        try
        {
            stream = sock.GetStream();
            if (stream.CanRead)
            {
                try
                {
					IAsyncResult ar = stream.BeginRead(state.buffer, 0, StateObject.BufferSize,
                            new AsyncCallback(TCPReadCallBack), state);
                  
                }
                catch(Exception ex)
                {
                    //设置标志,连接服务端失败!
                    MyDebug.Log("---------------连接异常-------------1-----");
                    Debug.Log(ex.ToString());
                }
            }
        }
        catch(Exception ex)
        {
            //设置标志,连接服务端失败!
           // NetManaged.isConnectServer = false;
           // NetManaged.surcessstate = 0;
            Debug.Log(ex.ToString());
            MyDebug.Log("---------------连接异常-------------2-----");
        }

    }

    /// <summary>
    /// TCP读数据的回调函数
    /// </summary>
    /// <param name="ar"></param>
    private void TCPReadCallBack(IAsyncResult ar)
    {
        StateObject state = (StateObject)ar.AsyncState;
        //主动断开时
        if ((state.client == null) || (!state.client.Connected))
        {
            MyDebug.Log("---------------TCPReadCallBack----------------closeSocket--"  );
			closeSocket ();
            return;
        }
        int numberOfBytesRead;
        NetworkStream mas = state.client.GetStream();
        numberOfBytesRead = mas.EndRead(ar);
        state.totalBytesRead += numberOfBytesRead;
        if (numberOfBytesRead > 0)
        {
            byte[] dd = new byte[numberOfBytesRead];
            Array.Copy(state.buffer,0,dd,0,numberOfBytesRead);
			if (isWait) {
				byte[] temp = new byte[sources.Length+dd.Length];
				sources.CopyTo (temp,0);
				dd.CopyTo (temp,sources.Length);
				sources = temp;
				if (sources.Length >= waitLen) {
					ReceiveCallBack (sources.Clone() as byte[]);
					isWait = false;
					waitLen = 0;
				}
			} else {
				sources = null;
				ReceiveCallBack (dd);
			}
            mas.BeginRead(state.buffer, 0, StateObject.BufferSize,
                    new AsyncCallback(TCPReadCallBack), state);
        }
        else
        {
            //被动断开时 
            mas.Close();
            state.client.Close();
            mas = null;
            state = null;
            //设置标志,连接服务端失败!
            MyDebug.Log("---------------连接异常-------------3-----");
            Debug.Log("客户端被动断开");
        }
    }

	/// <summary>
	/// 读取大端序的int
	/// </summary>
	/// <param name="value"></param>
	public int ReadInt(byte[] intbytes)
	{
		Array.Reverse(intbytes);
		return BitConverter.ToInt32(intbytes, 0);
	}

	public short ReadShort(byte[] intbytes){
		Array.Reverse(intbytes);
		return BitConverter.ToInt16(intbytes, 0);
	}

	private void ReceiveCallBack(byte[] m_receiveBuffer)
	{
		//通知调用端接收完毕
		try
		{
		//	MyDebug.Log("m_receiveBuffer======"+m_receiveBuffer.Length);
				MemoryStream ms = new MemoryStream(m_receiveBuffer);
				BinaryReader buffers = new BinaryReader(ms, UTF8Encoding.Default);
				readBuffer(buffers);
		}
		catch (Exception ex)
		{
            MyDebug.Log("---------------连接异常-------------4-----");
			MyDebug.Log ("socket exception:"+ex.Message);
			throw new Exception(ex.Message);
		}
	}

	private void readBuffer(BinaryReader buffers){
		byte flag = buffers.ReadByte();
		int lens = ReadInt(buffers.ReadBytes(4));
		disConnectCount = 0;
		if (!hasStartTimer && lens == 16) {
			//startTimer ();
			hasStartTimer = true;
		}
		//MyDebug.Log ("lengs ====>>  "+lens);

		if (lens > buffers.BaseStream.Length) {
			waitLen = lens;
			isWait = true;
			buffers.BaseStream.Position = 0;
			byte[] dd = new byte[buffers.BaseStream.Length];
			byte[] temp =  buffers.ReadBytes ((int)buffers.BaseStream.Length);
			Array.Copy (temp, 0, dd, 0, (int)buffers.BaseStream.Length);
			if (sources == null) {
				sources = dd;
			} 
			return;
		}
		int headcode = ReadInt(buffers.ReadBytes(4));
		int status = ReadInt(buffers.ReadBytes(4));
		short messageLen = ReadShort(buffers.ReadBytes(2));
		if(flag == 1){
			string message = Encoding.UTF8.GetString(buffers.ReadBytes(messageLen));
			ClientResponse response = new ClientResponse();
			response.status = status;
			response.message = message;
			response.headCode = headcode;
			MyDebug.Log("response.headCode = "+response.headCode+"  response.message =   "+message);
			SocketEventHandle.getInstance().addResponse(response);
		}
		if (buffers.BaseStream.Position < buffers.BaseStream.Length) {
			readBuffer (buffers);
		}
	}

	public void closeSocket(){
		DisConnect ();
	}
  
	System.Timers.Timer t;
	private void startTimer(){
		if (t == null) {
			t = new System.Timers.Timer(20000);   //实例化Timer类，设置间隔时间为10000毫秒；   
			t.Elapsed += new System.Timers.ElapsedEventHandler(timeout); //到达时间的时候执行事件；   
			t.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
			t.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；   
		}else{
			
			t.Start ();
		}

	}


	public void timeout(object source, System.Timers.ElapsedEventArgs e)   
	{  
		//MyDebug.Log ("disConnectCount+" + disConnectCount);
		disConnectCount += 1;
		if (disConnectCount >= 3) {
			t.Stop ();
		
			disConnectCount = 0;
		//	closeSocket ();
			isConnected = false;
			SocketEventHandle.getInstance ().noticeDisConnect ();
			return;

		}
	}  



}
