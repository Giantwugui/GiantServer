using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GiantNode
{
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
        /// <param name="message"></param>
        public static void Enqueue(Message message)
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
