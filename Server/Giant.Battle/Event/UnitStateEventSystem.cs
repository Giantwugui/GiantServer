using Giant.Core;

namespace Giant.Battle.Event
{
    //该层事件监听主要是处理事件回调，
    //譬如：
    // zone需要利用回调向前端同步后端的战斗状态
    // 其他战斗战斗演算，只需要验证结果，则不需要注册该层事件监听

    [Event(EventType.UnitDead)]
    public class UnitDeadEventSystem : Event<BattleScene, Unit>
    {
        public override void Handle(BattleScene scene, Unit unit)
        {
        }
    }

    [Event(EventType.UnitRelive)]
    public class UnitReliveEventSystem : Event<BattleScene, Unit>
    {
        public override void Handle(BattleScene scene, Unit unit)
        {
        }
    }

    [Event(EventType.NumbercalChange)]
    public class NumeercalChangeeEventSystem : Event<BattleScene, Unit, NumericalType, int>
    {
        public override void Handle(BattleScene self, Unit a, NumericalType type, int value)
        {
        }
    }
}
