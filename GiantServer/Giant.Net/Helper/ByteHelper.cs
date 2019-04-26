using System;
using System.Collections.Generic;
using System.Text;

namespace Giant.Net
{
    public static class ByteHelper
    {
        public static string ToUtf8String(this byte[] content, int offset, int count)
        {
            return Encoding.UTF8.GetString(content, offset, count);
        }

        public static byte[] ToUtf8Bytes(this string content)
        {
            return Encoding.UTF8.GetBytes(content);
        }


        public static byte[] Read(this byte[] content, int offset, int length)
        {
            byte[] message = new byte[length];

            Array.Copy(content, offset, message, 0, length);

            return message;
        }

        public static void WriteTo(this byte[] content, int offset, byte num)
        {
            content[offset] = num;
        }

        public static void WriteTo(this byte[] content, int offset, byte[] message)
        {
            Array.Copy(message, 0, content, offset, message.Length);
        }

        public static void WriteTo(this byte[] content, ushort num)
        {
            content[0] = (byte)((num & 0xff));
            content[1] = (byte)((num & 0xff00) >> 8);
        }

        public static void WriteTo(this byte[] content, short num)
        {
            content[0] = (byte)((num & 0xff));
            content[1] = (byte)((num & 0xff00) >> 8);
        }

        public static void WriteTo(this byte[] content, uint num)
        {
            content[0] = (byte)((num & 0xff));
            content[1] = (byte)((num & 0xff00) >> 8);
            content[2] = (byte)((num & 0xff0000) >> 16);
            content[3] = (byte)((num & 0xff000000) >> 24);
        }

        public static void WriteTo(this byte[] content, int num)
        {
            content[0] = (byte)((num & 0xff));
            content[1] = (byte)((num & 0xff00) >> 8);
            content[2] = (byte)((num & 0xff0000) >> 16);
            content[3] = (byte)((num & 0xff000000) >> 24);
        }

        public static void WriteTo(this byte[] content, int offset, uint num)
        {
            content[offset] = (byte)((num & 0xff));
            content[offset + 1] = (byte)((num & 0xff00) >> 8);
            content[offset + 2] = (byte)((num & 0xff0000) >> 16);
            content[offset + 3] = (byte)((num & 0xff000000) >> 24);
        }

        public static void WriteTo(this byte[] content, int offset, int num)
        {
            content[offset] = (byte)((num & 0xff));
            content[offset + 1] = (byte)((num & 0xff00) >> 8);
            content[offset + 2] = (byte)((num & 0xff0000) >> 16);
            content[offset + 3] = (byte)((num & 0xff000000) >> 24);
        }



    }
}
