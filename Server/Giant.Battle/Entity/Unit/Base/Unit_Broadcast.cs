namespace Giant.Battle
{
    partial class Unit
    {
        /// <summary>
        /// 用于构造广播消息
        /// </summary>
        public IBattleMsgSource MsgSource { get; protected set; }

        /// <summary>
        /// 向listener 广播消息
        /// </summary>
        public IBattleMsgListener MsgListener { get; protected set; }

        public void Broadcast(Google.Protobuf.IMessage message)
        {
            MsgListener?.BroadCastBattleMsg(message);
        }
    }
}
