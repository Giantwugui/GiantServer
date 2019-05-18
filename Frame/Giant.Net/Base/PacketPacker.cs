using Giant.Share;
using System;
using System.IO;

namespace Giant.Net
{
	public enum PackerState
	{
		PacketSize,
		PacketBody
	}
	
	public static class Packet
	{
		public const int PacketSizeLength2 = 2;
		public const int PacketSizeLength4 = 4;
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
        private readonly int packetSizeLength;
        private MemoryStream memoryStream;

        public PacketPacker(int packetSizeLength, CircularBuffer buffer, MemoryStream memoryStream)
		{
			this.buffer = buffer;
            this.memoryStream = memoryStream;
			this.packetSizeLength = packetSizeLength;
        }

		public bool Parse()
		{
			if (this.isOK)
			{
				return true;
			}

			bool finish = false;
			while (!finish)
			{
				switch (this.state)
				{
					case PackerState.PacketSize:
						if (this.buffer.Length < this.packetSizeLength)
						{
							finish = true;
						}
						else
						{
                            byte[] bytes = this.memoryStream.GetBuffer();

                            //获取消息长度放入到memorystream
                            this.buffer.Read(bytes, 0, this.packetSizeLength);
							
							switch (this.packetSizeLength)
							{
								case Packet.PacketSizeLength4:
									this.packetSize = BitConverter.ToInt32(bytes, 0);
									if (this.packetSize > ushort.MaxValue * 16 || this.packetSize < Packet.MinPacketSize)
									{
										throw new Exception($"recv packet size error: {this.packetSize}");
									}
									break;
								case Packet.PacketSizeLength2:
									this.packetSize = BitConverter.ToUInt16(bytes, 0);
									if (this.packetSize > ushort.MaxValue || this.packetSize < Packet.MinPacketSize)
									{
										throw new Exception($"recv packet size error: {this.packetSize}");
									}
									break;
								default:
									throw new Exception("packet size byte count must be 2 or 4!");
							}
							this.state = PackerState.PacketBody;
						}
						break;
					case PackerState.PacketBody:
						if (this.buffer.Length < this.packetSize)
						{
							finish = true;
						}
						else
						{
							this.memoryStream.Seek(0, SeekOrigin.Begin);
							this.memoryStream.SetLength(this.packetSize);
							byte[] bytes = this.memoryStream.GetBuffer();

                            //读取消息体到memorystream 会覆盖之前的长度标识
							this.buffer.Read(bytes, 0, this.packetSize);
							this.state = PackerState.PacketSize;
							this.isOK = true;
							finish = true;
                        }
						break;
				}
			}
			return this.isOK;
		}

		public MemoryStream GetPacket()
		{
			this.isOK = false;
			return this.memoryStream;
		}
	}
}