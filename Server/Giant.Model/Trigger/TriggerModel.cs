using Giant.Core;
using Giant.EnumUtil;

namespace Giant.Model
{
    public class TriggerModel : IData
    {
        public int Id { get; private set; }

        public TriggerMessageType TriggerMessageType { get; private set; }
        public TriggerConditionCombine ConditionCombine { get; private set; }
        public TriggerCondition TriggerCondition1 { get; private set; }
        public string TriggerConditionParam1 { get; private set; }
        public TriggerCondition TriggerCondition2 { get; private set; }
        public string TriggerConditionParam2 { get; private set; }
        public TriggerCondition TriggerCondition3 { get; private set; }
        public string TriggerConditionParam3 { get; private set; }
        public TriggerHandler TriggerHandler { get; private set; }
        public string TriggerHandlerParam { get; private set; }

        public void Bind(DataModel data)
        {
            TriggerMessageType = (TriggerMessageType)data.GetInt("TriggerMessageType");
            ConditionCombine = (TriggerConditionCombine)data.GetInt("ConditionCombine");

            TriggerConditionParam1 = data.GetString("TriggerConditionParam1");
            TriggerCondition1 = (TriggerCondition)data.GetInt("TriggerCondition1");
            TriggerConditionParam1 = data.GetString("TriggerConditionParam1");
            TriggerCondition2 = (TriggerCondition)data.GetInt("TriggerCondition2");
            TriggerConditionParam2 = data.GetString("TriggerConditionParam2");
            TriggerCondition3 = (TriggerCondition)data.GetInt("TriggerCondition3");
            TriggerConditionParam2 = data.GetString("TriggerConditionParam3");

            TriggerHandler = (TriggerHandler)data.GetInt("TriggerHandler");
            TriggerHandlerParam = data.GetString("TriggerHandlerParam");
        }
    }
}
