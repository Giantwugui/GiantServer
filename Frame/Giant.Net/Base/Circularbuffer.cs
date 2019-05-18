using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Giant.Share
{
    public class CircularBuffer: Stream
    {
        public int ChunkSize = 8192;
        private readonly Queue<byte[]> bufferQueue = new Queue<byte[]>();
        private readonly Queue<byte[]> bufferCache = new Queue<byte[]>();

        public int LastIndex { get; set; }

        public int FirstIndex { get; set; }
		
        private byte[] lastBuffer;
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

        public byte[] First
        {
            get
            {
                if (this.bufferQueue.Count == 0)
                {
                    this.AddLast();
                }
                return this.bufferQueue.Peek();
            }
        }

        public override long Length
        {
            get
            {
                int count;
                if (this.bufferQueue.Count == 0)
                {
                    count = 0;
                }
                else
                {
                    count = (this.bufferQueue.Count - 1) * ChunkSize + this.LastIndex - this.FirstIndex;
                }

                return count;
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
            if (!this.bufferCache.TryDequeue(out byte[] buffer))
            {
                buffer = new byte[ChunkSize];
            }

            this.bufferQueue.Enqueue(buffer);
            this.lastBuffer = buffer;
        }


        /// <summary>
        /// bufferqueue中的byte[]使用完成 放回到待使用队列中
        /// </summary>
        public void RemoveFirst()
        {
            this.bufferCache.Enqueue(bufferQueue.Dequeue());
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

            int readedSize = 0;
            while (readedSize < count)
            {
                int needReadSize = count - readedSize;
                int bufferSize = ChunkSize - this.FirstIndex;//当前缓冲区剩余空间

                if (bufferSize > needReadSize)
                {
                    Array.Copy(this.First, this.FirstIndex, buffer, offset + readedSize, needReadSize);
                    readedSize += needReadSize;
                    this.FirstIndex += needReadSize;
                }
                else
                {
                    Array.Copy(this.First, this.FirstIndex, buffer, offset + readedSize, bufferSize);
                    readedSize += bufferSize;

                    this.FirstIndex = 0;
                    this.RemoveFirst();
                }
            }

            return count;
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
		    int readedSize = 0;
		    while (readedSize < count)
		    {
                //还需要读取的字节数
			    int needReadSize = count - readedSize;
                int bufferSize = this.ChunkSize - this.FirstIndex;//当前缓冲区剩余空间

                //当前字节已经包含了所有需要读取的字节
                if (bufferSize > needReadSize)
			    {
				    stream.Write(this.First, this.FirstIndex, needReadSize);
				    this.FirstIndex += needReadSize;
				    readedSize += needReadSize; //已经读了所有需要读取的字节数目，推出循环
			    }
			    else
			    {
                    //获取第一个byte[]
                    //当第一个byte[]数据不够的时候，先读取所有剩余的字节
				    stream.Write(this.First, this.FirstIndex, bufferSize);
				    readedSize += bufferSize;
				    this.FirstIndex = 0;
				    this.RemoveFirst();
			    }
		    }
	    }

        // 把buffer写入CircularBuffer中
        public override void Write(byte[] buffer, int offset, int count)
        {
            int writedSize = 0;
            while (writedSize < count)
            {
                int bufferSize = this.ChunkSize - this.LastIndex;//当前缓冲区剩余空间
                int needWriteSize = count - writedSize;

                if (bufferSize > needWriteSize)
                {
                    Array.Copy(buffer, offset + writedSize, this.lastBuffer, this.LastIndex, needWriteSize);
                    writedSize += needWriteSize;
                    this.LastIndex += needWriteSize;
                }
                else
                {
                    Array.Copy(buffer, offset + writedSize, this.lastBuffer, this.LastIndex, bufferSize);

                    writedSize += bufferSize;
                    this.LastIndex = 0;
                    this.AddLast();
                }
            }
        }


        // 从stream写入CircularBuffer
        public void Write(Stream stream)
		{
			int count = (int)(stream.Length - stream.Position);
			
			int writedSize = 0;
			while (writedSize < count)
			{
                //最后一个byte[]已经写满数据，则新建一个byte[],并设置成当前正在使用的byte[]
				if (this.LastIndex == ChunkSize)
				{
					this.AddLast();
					this.LastIndex = 0;
				}

                //当前bye[]足够使用
				int needWriteSize = count - writedSize;
                int bufferSize = this.ChunkSize - this.LastIndex;//当前缓冲区剩余空间

				if (bufferSize > needWriteSize)
                {
					stream.Read(this.lastBuffer, this.LastIndex, needWriteSize);
					this.LastIndex += count - writedSize;
					writedSize += needWriteSize;
				}
				else
				{
					stream.Read(this.lastBuffer, this.LastIndex, bufferSize);
					writedSize += bufferSize;
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

        #region 基类方法

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

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

	    public override long Position { get; set; }

        #endregion
    }
}