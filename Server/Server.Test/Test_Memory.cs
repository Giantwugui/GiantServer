using Giant.Share;
using Microsoft.IO;
using System;
using System.IO;
using System.Text;

namespace Server.Test
{
    class Test_Memory
    {
        public static RecyclableMemoryStreamManager memoryStreamManager = new RecyclableMemoryStreamManager();
        public static MemoryStream stream = memoryStreamManager.GetStream("Message");

        private static void TestMemoryStream()
        {


            //数据写入测试
            {
                byte[] opcodeBytes = new byte[2];

                byte[] strintBytes = Encoding.UTF8.GetBytes("testtttttttttttttttttttttttttt");

                //设置写入位置(预留两个字节保存消息的长度)
                stream.Seek(2, SeekOrigin.Begin);

                //设置当前字符长度
                stream.SetLength(2);

                //写入消息
                stream.Write(strintBytes, 0, strintBytes.Length);

                //设置写入位置到最开始
                stream.Seek(0, SeekOrigin.Begin);

                //序列化长度
                opcodeBytes.WriteTo(0, (ushort)strintBytes.Length);

                //写入消息长度信息
                stream.Write(opcodeBytes, 0, opcodeBytes.Length);
            }


            //读取数据
            {
                //设置当前位置
                stream.Seek(2, SeekOrigin.Begin);

                //获取buffer中所有的数据(不受seek影响，会获取所有的数据)
                byte[] opcodeByte_re = stream.GetBuffer();

                //读取前两个字节，并转换成short类型
                int opcode = BitConverter.ToInt16(opcodeByte_re);

                //用于接收消息的缓冲区(不包含长度)
                byte[] messageBytes = new byte[stream.Length - stream.Position];

                //获取消息
                Array.Copy(opcodeByte_re, stream.Position, messageBytes, 0, stream.Length - stream.Position);

                //原始消息
                string message = Encoding.UTF8.GetString(messageBytes);



                //
                stream.SetLength(10);
                byte[] changedBytes = stream.GetBuffer();

                byte[] strintBytes = Encoding.UTF8.GetBytes("hhhh");
            }
        }
    }
}
