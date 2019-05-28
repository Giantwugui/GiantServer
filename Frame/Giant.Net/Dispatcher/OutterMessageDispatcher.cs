using Giant.Msg;

namespace Giant.Net
{
    public class OutterMessageDispatcher : MessageDispatcher
    {
        public OutterMessageDispatcher()
        {
            opcodeTypes.AddRange(OuterOpcode.Opcode2Types);
        }
    }
}
