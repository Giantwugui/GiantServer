using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Server.Frame
{
	public class OneThreadSynchronizationContext : SynchronizationContext
	{
        public static OneThreadSynchronizationContext Instance { get; } = new OneThreadSynchronizationContext();

        private readonly int mainThreadId = Thread.CurrentThread.ManagedThreadId;

        // 线程同步队列,发送接收socket回调都放到该队列,由poll线程统一执行
        private readonly ConcurrentQueue<Action> queue = new ConcurrentQueue<Action>();

        private Action acction;

        private OneThreadSynchronizationContext()
        {
        }


        public void Update()
		{
			while (true)
			{
				if (!this.queue.TryDequeue(out acction))
				{
					return;
				}
				acction();
			}
		}

        /// <summary>
        /// 将异步消息分派到同步上下文
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
		public override void Post(SendOrPostCallback callback, object state)
		{
			if (Thread.CurrentThread.ManagedThreadId == this.mainThreadId)
			{
				callback(state);
				return;
			}
			
			this.queue.Enqueue(() => { callback(state); });
		}

    }
}
