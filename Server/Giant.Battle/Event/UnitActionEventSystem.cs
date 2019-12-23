using Giant.Core;
using UnityEngine;

namespace Giant.Battle
{
    //该层事件监听主要是处理事件回调，
    //譬如：
    // zone需要利用回调向前端同步后端的战斗状态
    // 其他战斗战斗演算，只需要验证结果，则不需要注册该层事件监听

    [Event(EventType.UnitEnterScene)]
    public class UnitEnterSceneEventSystem : Event<BattleScene, Unit>
    {
        public override void Handle(BattleScene scene, Unit unit)
        {
            unit.MsgListener.OnEnterBattleScene(scene);
        }
    }

    [Event(EventType.UnitLeaveScene)]
    public class UnitLeaveSceneEventSystem : Event<BattleScene, Unit>
    {
        public override void Handle(BattleScene scene, Unit unit)
        {
            unit.MsgListener.OnLeaveBattleScene();
        }
    }

    [Event(EventType.PosChange)]
    public class UnitPosChangeEventSystem : Event<BattleScene, Unit, Vector2>
    {
        public override void Handle(BattleScene scene, Unit unit, Vector2 vector)
        {
        }
    }

    [Event(EventType.UnitCastSkill)]
    public class UnitCastSkillEventSystem : Event<BattleScene, int, int>
    {
        public override void Handle(BattleScene scene, int id, int skillId)
        {

        }
    }

    [Event(EventType.UnitAddBuff)]
    public class UnitAddBuffEventSystem : Event<BattleScene, Unit>
    {
        public override void Handle(BattleScene scene, Unit unit)
        {
        }
    }

    [Event(EventType.UnitRemoveBuff)]
    public class UnitRemoveBuffEventSystem : Event<BattleScene, Unit>
    {
        public override void Handle(BattleScene scene, Unit unit)
        {
        }
    }
}
