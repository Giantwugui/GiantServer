using System;

namespace Giant.Model
{
    public class OpcodeType
    {
        public const ushort Login = 1;
    }


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    class MessageAttribute : Attribute
    {
        public MessageAttribute(ushort opcode)
        {
            Opcode = opcode;
        }

        public ushort Opcode { get; private set; }
    }
}
