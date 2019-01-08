using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Giant.Net
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 内部消息
        /// </summary>
        Inner = 1,

        /// <summary>
        /// 客户端消息
        /// </summary>
        Client = 1<<1,

        /// <summary>
        /// 上线消息
        /// </summary>
        ClientOnline = 1 << 2,

        /// <summary>
        /// 下线消息
        /// </summary>
        ClientOffline = 1 << 3,
    }


    public class Message
    {
        public Message(MessageType msgType, params object[] param)
        {
            Param = param;
            MessageType = msgType;
        }

        /// <summary>
        /// 参数列表
        /// </summary>
        public object[] Param;

        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MessageType;
    }

    public class MessageManager
    {
        /// <summary>
        /// 添加一个消息到消息队列
        /// </summary>
        public static void Add(Message message)
        {
            mMessageQueue.Enqueue(message);
        }

        /// <summary>
        /// 获取当前所有内部消息
        /// </summary>
        /// <returns></returns>
        public static Queue<Message> PopList()
        {
            Queue<Message> tempList = new Queue<Message>();

            while (mMessageQueue.TryDequeue(out Message message))
            {
                tempList.Enqueue(message);
            }

            return tempList;
        }

        /// <summary>
        /// 内部消息队列
        /// </summary>
        private static ConcurrentQueue<Message> mMessageQueue = new ConcurrentQueue<Message>();
    }
}
