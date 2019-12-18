using Google.Protobuf;

namespace Giant.Battle
{
    public interface IBattleMsgListener
    {
        void OnEnterBattleScene(BattleScene scene);
        void OnLeaveBattleScene();
        void OnStopBattle(BattleResult result);
        void BroadCastBattleMsg(IMessage message);
    }
}
