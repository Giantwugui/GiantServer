using Giant.Msg;

namespace Giant.Net
{
    public class InnerMessageDispatcher : MessageDispatcher
    {
        public InnerMessageDispatcher()
        {
            opcodeTypes.AddRange(InnerOpcode.Opcode2Types);
        }
    }
}
