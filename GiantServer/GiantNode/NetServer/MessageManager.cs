using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GiantNode
{
    public class MessageManager
    {
        /// <summary>
        /// 添加一个消息到消息队列
        /// </summary>
        /// <param name="message"></param>
        public static void Enqueue(IMessage message)
        {
            mMessageQueue.Enqueue(message);
        }

        /// <summary>
        /// 获取当前所有内部消息
        /// </summary>
        /// <returns></returns>
        public static Queue<IMessage> PopList()
        {
            Queue<IMessage> tempList = new Queue<IMessage>();

            while (mMessageQueue.TryDequeue(out IMessage message))
            {
                tempList.Enqueue(message);
            }
            return tempList;
        }

        /// <summary>
        /// 内部消息队列
        /// </summary>
        private static ConcurrentQueue<IMessage> mMessageQueue = new ConcurrentQueue<IMessage>();
    }
}
