using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Giant.Log;

//using ETModel

namespace Giant.Net
{
    public class CircularBuffer: Stream
    {
        public int ChunkSize = 8192;

        //buffer队列 正在使用的 把所接收到的消息分成段缓存在多个byte[]当中
        private readonly Queue<byte[]> bufferQueue = new Queue<byte[]>();

        //已经不再使用了的
        private readonly Queue<byte[]> bufferCache = new Queue<byte[]>();

        /// <summary>
        /// 正在使用的字符数组的当前位置
        /// </summary>
        public int LastIndex { get; set; }

        /// <summary>
        /// 所有消息的其实位置
        /// </summary>
        public int FirstIndex { get; set; }
		

        /// <summary>
        /// 正在使用的byte[]
        /// </summary>
        private byte[] lastBuffer;

        /// <summary>
        /// 正在使用的byte[]
        /// </summary>
        public byte[] Last
        {
            get
            {
                if (this.bufferQueue.Count == 0)
                {
                    this.AddLast();
                }
                return this.lastBuffer;
            }
        }

        /// <summary>
        /// 返回填满了的yte[]或者正在使用的byte[]
        /// </summary>
        public byte[] First
        {
            get
            {
                //如果当前没有正在使用的byte[]
                if (this.bufferQueue.Count == 0)
                {
                    this.AddLast();
                }
                return this.bufferQueue.Peek();
            }
        }

        public CircularBuffer()
        {
            this.AddLast();
        }

        /// <summary>
        /// 构建正在使用的byte[]
        /// </summary>
        public void AddLast()
        {
            byte[] buffer;
            if (this.bufferCache.Count > 0)
            {
                buffer = this.bufferCache.Dequeue();
            }
            else
            {
                //首次使用的时候 新构建一个 byte[] 
                buffer = new byte[ChunkSize];
            }

            //新建的byte[] 加入队列
            this.bufferQueue.Enqueue(buffer);
            this.lastBuffer = buffer;
        }

        /// <summary>
        /// 缓存的所有byte[]长度
        /// </summary>
        public override long Length
        {
            get
            {
                int c = 0;
                if (this.bufferQueue.Count == 0)
                {
                    c = 0;
                }
                else
                {
                    //除了最后一个byte[] 其他的byte[]全部满
                    c = (this.bufferQueue.Count - 1) * ChunkSize + this.LastIndex - this.FirstIndex;
                }
                if (c < 0)
                {
                    Logger.Error($"CircularBuffer count < 0: {this.bufferQueue.Count}, {this.LastIndex}, {this.FirstIndex}");
                }
                return c;
            }
        }


        /// <summary>
        /// bufferqueue中的byte[]使用完成 放回到待使用队列中
        /// </summary>
        public void RemoveFirst()
        {
            this.bufferCache.Enqueue(bufferQueue.Dequeue());
        }

		/// <summary>
		/// 从CircularBuffer读到stream中
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public async Task ReadAsync(Stream stream)
	    {
		    long buffLength = this.Length;
			int sendSize = this.ChunkSize - this.FirstIndex;
		    if (sendSize > buffLength)
		    {
			    sendSize = (int)buffLength;
		    }
			
		    await stream.WriteAsync(this.First, this.FirstIndex, sendSize);
		    
		    this.FirstIndex += sendSize;
		    if (this.FirstIndex == this.ChunkSize)
		    {
			    this.FirstIndex = 0;
			    this.RemoveFirst();
		    }
		}

        /// <summary>
        /// 从CircularBuffer读到stream 读取所有缓存的消息 需要拷贝 count长度的数据到stream中
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="count"></param>
        public void Read(Stream stream, int count)
	    {
		    if (count > this.Length)
		    {
			    throw new Exception($"bufferList length < count, {Length} {count}");
		    }

            //已经拷贝了的长度
		    int alreadyCopyCount = 0;
		    while (alreadyCopyCount < count)
		    {
                //还需要读取的字节数
			    int n = count - alreadyCopyCount;

                //当前字节已经包含了所有需要读取的字节
			    if (ChunkSize - this.FirstIndex > n)
			    {
				    stream.Write(this.First, this.FirstIndex, n);
				    this.FirstIndex += n;
				    alreadyCopyCount += n; //已经读了所有需要读取的字节数目，推出循环
			    }
			    else
			    {
                    //获取第一个byte[]
                    //当第一个byte[]数据不够的时候，先读取所有剩余的字节
				    stream.Write(this.First, this.FirstIndex, ChunkSize - this.FirstIndex);
				    alreadyCopyCount += ChunkSize - this.FirstIndex;
				    this.FirstIndex = 0;
				    this.RemoveFirst();
			    }
		    }
	    }
	    
	    // 从stream写入CircularBuffer
	    public void Write(Stream stream)
		{
			int count = (int)(stream.Length - stream.Position);
			
			int alreadyCopyCount = 0;
			while (alreadyCopyCount < count)
			{
                //最后一个byte[]已经写满数据，则新建一个byte[],并设置成当前正在使用的byte[]
				if (this.LastIndex == ChunkSize)
				{
					this.AddLast();
					this.LastIndex = 0;
				}

                //当前bye[]足够使用
				int n = count - alreadyCopyCount;
				if (ChunkSize - this.LastIndex > n)
				{
					stream.Read(this.lastBuffer, this.LastIndex, n);
					this.LastIndex += count - alreadyCopyCount;
					alreadyCopyCount += n;
				}
				else
				{
					stream.Read(this.lastBuffer, this.LastIndex, ChunkSize - this.LastIndex);
					alreadyCopyCount += ChunkSize - this.LastIndex;
					this.LastIndex = ChunkSize;
				}
			}
		}
	    

	    /// <summary>
		///  从stream写入CircularBuffer
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public async Task<int> WriteAsync(Stream stream)
	    {
		    int size = this.ChunkSize - this.LastIndex;
		    
		    int n = await stream.ReadAsync(this.Last, this.LastIndex, size);

		    if (n == 0)
		    {
			    return 0;
		    }

		    this.LastIndex += n;

		    if (this.LastIndex == this.ChunkSize)
		    {
			    this.AddLast();
			    this.LastIndex = 0;
		    }

		    return n;
	    }

	    // 把CircularBuffer中数据写入buffer
        public override int Read(byte[] buffer, int offset, int count)
        {
	        if (buffer.Length < offset + count)
	        {
		        throw new Exception($"bufferList length < coutn, buffer length: {buffer.Length} {offset} {count}");
	        }

	        long length = this.Length;
			if (length < count)
            {
	            count = (int)length;
            }

            int alreadyCopyCount = 0;
            while (alreadyCopyCount < count)
            {
                int n = count - alreadyCopyCount;
				if (ChunkSize - this.FirstIndex > n)
                {
                    Array.Copy(this.First, this.FirstIndex, buffer, alreadyCopyCount + offset, n);
                    this.FirstIndex += n;
                    alreadyCopyCount += n;
                }
                else
                {
                    Array.Copy(this.First, this.FirstIndex, buffer, alreadyCopyCount + offset, ChunkSize - this.FirstIndex);
                    alreadyCopyCount += ChunkSize - this.FirstIndex;
                    this.FirstIndex = 0;
                    this.RemoveFirst();
                }
            }

	        return count;
        }

	    // 把buffer写入CircularBuffer中
        public override void Write(byte[] buffer, int offset, int count)
        {
	        int alreadyCopyCount = 0;
            while (alreadyCopyCount < count)
            {
                if (this.LastIndex == ChunkSize)
                {
                    this.AddLast();
                    this.LastIndex = 0;
                }

                int n = count - alreadyCopyCount;
                if (ChunkSize - this.LastIndex > n)
                {
                    Array.Copy(buffer, alreadyCopyCount + offset, this.lastBuffer, this.LastIndex, n);
                    this.LastIndex += count - alreadyCopyCount;
                    alreadyCopyCount += n;
                }
                else
                {
                    Array.Copy(buffer, alreadyCopyCount + offset, this.lastBuffer, this.LastIndex, ChunkSize - this.LastIndex);
                    alreadyCopyCount += ChunkSize - this.LastIndex;
                    this.LastIndex = ChunkSize;
                }
            }
        }

	    public override void Flush()
	    {
		    throw new NotImplementedException();
		}

	    public override long Seek(long offset, SeekOrigin origin)
	    {
			throw new NotImplementedException();
	    }

	    public override void SetLength(long value)
	    {
		    throw new NotImplementedException();
		}

	    public override bool CanRead
	    {
		    get
		    {
			    return true;
		    }
	    }

	    public override bool CanSeek
	    {
		    get
		    {
			    return false;
		    }
	    }

	    public override bool CanWrite
	    {
		    get
		    {
			    return true;
		    }
	    }

	    public override long Position { get; set; }
    }
}