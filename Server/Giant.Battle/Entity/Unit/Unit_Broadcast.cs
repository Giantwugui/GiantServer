namespace Giant.Battle
{
    partial class Unit
    {
        /// <summary>
        /// 向listener 广播消息
        /// </summary>
        private readonly IBattleMsgListener msgListener;
        public IBattleMsgListener MsgListener => msgListener;

        public void BroadCast(Google.Protobuf.IMessage message)
        {
            msgListener?.BroadCastBattleMsg(message);
        }
    }
}
