namespace Giant.Net
{
	public static class ErrorCode
	{
		public const int Success = 0;
		public const int Fail = 1;

        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000 以上，避免跟SocketError冲突
        public const int MyErrorCode = 100000;
		
		public const int ActorNoMailBoxComponent = 100003;
		public const int ActorRemove = 100004;
		public const int PacketParserError = 100005;
		
		public const int KcpCantConnect = 102005;
		public const int KcpChannelTimeout = 102006;
		public const int KcpRemoteDisconnect = 102007;
		public const int PeerDisconnect = 102008;
		public const int SocketCantSend = 102009;
		public const int SocketError = 102010;
		public const int KcpWaitSendSizeTooLarge = 102011;

		public const int WebsocketPeerReset = 103001;
		public const int WebsocketMessageTooBig = 103002;
		public const int WebsocketError = 103003;
		public const int WebsocketConnectError = 103004;
		public const int WebsocketSendError = 103005;
		public const int WebsocketRecvError = 103006;
		
		public const int RpcFail = 102001;
		public const int ReloadFail = 102003;
		public const int ConnectGateKeyError = 100105;
		public const int ActorLocationNotFound = 102004;


		//-----------------------------------
		// 小于这个Rpc会抛异常，大于这个异常的error需要自己判断处理，也就是说需要处理的错误应该要大于该值
		public const int Exception = 200000;
		
		public const int NotFoundActor = 200002;
		
		public const int AccountOrPasswordError = 200102;
		public const int HaveNotFindAccount = 200103;
		public const int HaveNotFindServer = 200104;
        public const int HaveNotFindCharacer = 200105;

        //-----------------------------------
        public static bool IsRpcNeedThrowException(int error)
		{
			if (error == 0)
			{
				return false;
			}

			if (error > Exception)
			{
				return false;
			}

			return true;
		}
	}
}