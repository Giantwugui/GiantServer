using System;

namespace Giant.Message
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class BaseAttribute : Attribute
    {
        public Type AttributeType { get; }

        public BaseAttribute()
        {
            this.AttributeType = this.GetType();
        }
    }

    public class MessageAttribute: BaseAttribute
	{
		public ushort Opcode { get; }

		public MessageAttribute(ushort opcode)
		{
			this.Opcode = opcode;
		}
	}
}