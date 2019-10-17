using System;
using System.Text;

namespace Giant.Core
{
    public static class ByteEx
    {
        public static string ToUTF8String(this byte[] content)
        {
            return Encoding.UTF8.GetString(content);
        }

        public static string ToBase64String(this byte[] content)
        {
            return Convert.ToBase64String(content);
        }

        public static byte[] FromUTF8String(this string content)
        {
            return Encoding.UTF8.GetBytes(content);
        }

        public static byte[] FromBase64String(this string content)
        {
            return Convert.FromBase64String(content);
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

        public static void WriteTo(this byte[] content, int offset, ushort num)
        {
            content[offset] = (byte)((num & 0xff));
            content[offset + 1] = (byte)((num & 0xff00) >> 8);
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

        public static void WriteTo(this byte[] content, int offset, long num)
        {
            content[offset] = (byte)((num & 0xff));
            content[offset + 1] = (byte)((num & 0xff00) >> 8);
            content[offset + 2] = (byte)((num & 0xff0000) >> 16);
            content[offset + 3] = (byte)((num & 0xff000000) >> 24);
            content[offset + 4] = (byte)((num & 0xff000000) >> 32);
            content[offset + 5] = (byte)((num & 0xff000000) >> 40);
            content[offset + 6] = (byte)((num & 0xff000000) >> 48);
            content[offset + 7] = (byte)((num & 0xff000000) >> 56);
        }



    }
}
