using Google.Protobuf;

namespace Giant.Battle
{
    public interface IBattleMsgListener
    {
        void OnEnterBattleScene(BattleScene scene);
        void OnLeaveBattleScene();

        void OnBattleStart();
        void OnBattleStop(BattleResult result);
        void OnBattleEnd();


        void BroadCastBattleMsg(IMessage message);
    }
}
