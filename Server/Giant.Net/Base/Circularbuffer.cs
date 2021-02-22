using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Giant.Net
{
    public class CircularBuffer : Stream
    {
        public int ChunkSize = 8192;
        private readonly Queue<byte[]> bufferQueue = new Queue<byte[]>();
        private readonly Queue<byte[]> bufferCache = new Queue<byte[]>();

        public int FirstIndex { get; set; }
        public int LastIndex { get; set; }

        private byte[] lastBuffer;
        public byte[] Last
        {
            get
            {
                if (bufferQueue.Count == 0)
                {
                    AddLast();
                }
                return lastBuffer;
            }
        }

        public byte[] First
        {
            get
            {
                if (bufferQueue.Count == 0)
                {
                    AddLast();
                }
                return bufferQueue.Peek();
            }
        }

        public override long Length
        {
            get
            {
                int count;
                if (bufferQueue.Count == 0)
                {
                    count = 0;
                }
                else
                {
                    count = (bufferQueue.Count - 1) * ChunkSize + LastIndex - FirstIndex;
                }

                return count;
            }
        }

        public CircularBuffer()
        {
            AddLast();
        }

        /// <summary>
        /// 构建正在使用的byte[]
        /// </summary>
        public void AddLast()
        {
            if (!bufferCache.TryDequeue(out byte[] buffer))
            {
                buffer = new byte[ChunkSize];
            }

            bufferQueue.Enqueue(buffer);
            lastBuffer = buffer;
        }


        /// <summary>
        /// bufferqueue中的byte[]使用完成 放回到待使用队列中
        /// </summary>
        public void RemoveFirst()
        {
            bufferCache.Enqueue(bufferQueue.Dequeue());
        }

        // 把CircularBuffer中数据写入buffer
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer.Length < offset + count)
            {
                throw new Exception($"bufferList length < coutn, buffer length: {buffer.Length} {offset} {count}");
            }

            long length = Length;
            if (length < count)
            {
                count = (int)length;
            }

            int readedSize = 0;
            while (readedSize < count)
            {
                int needReadSize = count - readedSize;
                int bufferSize = ChunkSize - FirstIndex;//当前缓冲区剩余空间

                if (bufferSize > needReadSize)
                {
                    Array.Copy(First, FirstIndex, buffer, offset + readedSize, needReadSize);
                    readedSize += needReadSize;
                    FirstIndex += needReadSize;
                }
                else
                {
                    Array.Copy(First, FirstIndex, buffer, offset + readedSize, bufferSize);
                    readedSize += bufferSize;

                    FirstIndex = 0;
                    RemoveFirst();
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
            long buffLength = Length;
            int sendSize = ChunkSize - FirstIndex;
            if (sendSize > buffLength)
            {
                sendSize = (int)buffLength;
            }

            await stream.WriteAsync(First, FirstIndex, sendSize);

            FirstIndex += sendSize;
            if (FirstIndex == ChunkSize)
            {
                FirstIndex = 0;
                RemoveFirst();
            }
        }

        /// <summary>
        /// 从CircularBuffer读到stream 读取所有缓存的消息 需要拷贝 count长度的数据到stream中
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="count"></param>
        public void Read(Stream stream, int count)
        {
            if (count > Length)
            {
                throw new Exception($"bufferList length < count, {Length} {count}");
            }

            //已经拷贝了的长度
            int readedSize = 0;
            while (readedSize < count)
            {
                //还需要读取的字节数
                int needReadSize = count - readedSize;
                int bufferSize = ChunkSize - FirstIndex;//当前缓冲区剩余空间

                //当前字节已经包含了所有需要读取的字节
                if (bufferSize > needReadSize)
                {
                    stream.Write(First, FirstIndex, needReadSize);
                    FirstIndex += needReadSize;
                    readedSize += needReadSize; //已经读了所有需要读取的字节数目，推出循环
                }
                else
                {
                    //获取第一个byte[]
                    //当第一个byte[]数据不够的时候，先读取所有剩余的字节
                    stream.Write(First, FirstIndex, bufferSize);
                    readedSize += bufferSize;
                    FirstIndex = 0;
                    RemoveFirst();
                }
            }
        }

        // 把buffer写入CircularBuffer中
        public override void Write(byte[] buffer, int offset, int count)
        {
            int writedSize = 0;
            while (writedSize < count)
            {
                int bufferSize = ChunkSize - LastIndex;//当前缓冲区剩余空间
                int needWriteSize = count - writedSize;

                if (bufferSize > needWriteSize)
                {
                    Array.Copy(buffer, offset + writedSize, lastBuffer, LastIndex, needWriteSize);
                    writedSize += needWriteSize;
                    LastIndex += needWriteSize;
                }
                else
                {
                    Array.Copy(buffer, offset + writedSize, lastBuffer, LastIndex, bufferSize);

                    writedSize += bufferSize;
                    LastIndex = 0;
                    AddLast();
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
                if (LastIndex == ChunkSize)
                {
                    AddLast();
                    LastIndex = 0;
                }

                //当前bye[]足够使用
                int needWriteSize = count - writedSize;
                int bufferSize = ChunkSize - LastIndex;//当前缓冲区剩余空间

                if (bufferSize > needWriteSize)
                {
                    stream.Read(lastBuffer, LastIndex, needWriteSize);
                    LastIndex += count - writedSize;
                    writedSize += needWriteSize;
                }
                else
                {
                    stream.Read(lastBuffer, LastIndex, bufferSize);
                    writedSize += bufferSize;
                    LastIndex = ChunkSize;
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
            int size = ChunkSize - LastIndex;

            int n = await stream.ReadAsync(Last, LastIndex, size);

            if (n == 0)
            {
                return 0;
            }

            LastIndex += n;

            if (LastIndex == ChunkSize)
            {
                AddLast();
                LastIndex = 0;
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