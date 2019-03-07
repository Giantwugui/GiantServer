using System;

namespace Giant.Model
{
    public static class Opcode
    {
        public const ushort C2G_login = 101;
    }


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    class MessageAttribute : Attribute
    {
        public ushort Opcode { get; private set; }

        public MessageAttribute(ushort opcode)
        {
            Opcode = opcode;
        }

    }
}
