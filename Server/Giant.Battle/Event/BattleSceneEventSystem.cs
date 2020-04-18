using Giant.Core;
using Giant.Msg;

namespace Giant.Battle
{
    //该层事件监听主要是处理事件回调，
    //譬如：
    // zone需要利用回调向前端同步后端的战斗状态
    // 其他战斗战斗演算，只需要验证结果，则不需要注册该层事件监听

    [Event(EventType.BattleStart)]
    public class BattleStartEventSystem : Event<BattleScene>
    {
        public override void Handle(BattleScene scene)
        {
            Msg_ZGate_Battle_Start msg = new Msg_ZGate_Battle_Start();
            scene.BroadCastMsg(msg);
        }
    }


    [Event(EventType.BattleStop)]
    public class BattleStopEventSystem : Event<BattleScene, BattleResult>
    {
        public override void Handle(BattleScene scene, BattleResult result)
        {
            foreach (var kv in scene.GetComponent<UnitComponent>().UnitList)
            {
                kv.Value.MsgListener.OnBattleStop(result);
            }
        }
    }



}
