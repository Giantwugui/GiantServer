namespace Giant.Battle
{
    partial class Unit
    {
        /// <summary>
        /// 用于构造广播消息
        /// </summary>
        private IBattleMsgSource msgSource;

        /// <summary>
        /// 向listener 广播消息
        /// </summary>
        private IBattleMsgListener msgListener;
        public IBattleMsgListener MsgListener => msgListener;

        public void BroadCast(Google.Protobuf.IMessage message)
        {
            msgListener?.BroadCastBattleMsg(message);
        }
    }
}
