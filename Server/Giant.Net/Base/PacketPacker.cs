using System;
using System.IO;

namespace Giant.Net
{
    enum PackerState
    {
        PacketSize,
        PacketBody
    }

    public static class Packet
    {
        public const int PacketSizeLength = 4;
        public const int MinPacketSize = 2;
        public const int OpcodeIndex = 0;
        public const int MessageIndex = 2;
    }

    public class PacketPacker
    {
        private bool isOK;
        private int packetSize;
        private PackerState state;
        private readonly CircularBuffer buffer;
        private MemoryStream memoryStream;

        public PacketPacker(CircularBuffer buffer, MemoryStream memoryStream)
        {
            this.buffer = buffer;
            this.memoryStream = memoryStream;
        }

        public bool Parse()
        {
            if (isOK)
            {
                return true;
            }

            bool finish = false;
            while (!finish)
            {
                switch (state)
                {
                    case PackerState.PacketSize:
                        if (buffer.Length < Packet.PacketSizeLength)
                        {
                            finish = true;
                        }
                        else
                        {
                            byte[] bytes = memoryStream.GetBuffer();

                            //获取消息长度放入到memorystream
                            buffer.Read(bytes, 0, Packet.PacketSizeLength);

                            packetSize = BitConverter.ToInt32(bytes, 0);
                            if (packetSize > ushort.MaxValue * 16 || packetSize < Packet.MinPacketSize)
                            {
                                throw new Exception($"recv packet size error: {packetSize}");
                            }

                            state = PackerState.PacketBody;
                        }
                        break;
                    case PackerState.PacketBody:
                        if (buffer.Length < packetSize)
                        {
                            finish = true;
                        }
                        else
                        {
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            memoryStream.SetLength(packetSize);
                            byte[] bytes = memoryStream.GetBuffer();

                            //读取消息体到memorystream 会覆盖之前的长度标识
                            buffer.Read(bytes, 0, packetSize);
                            state = PackerState.PacketSize;
                            isOK = true;
                            finish = true;
                        }
                        break;
                }
            }
            return isOK;
        }

        public MemoryStream GetPacket()
        {
            isOK = false;
            return memoryStream;
        }
    }
}