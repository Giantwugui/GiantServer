using Giant.Battle;
using Google.Protobuf;

namespace Server.App
{
    partial class Player : IBattleMsgListener
    {
        public BattleScene BattleScene { get; private set; }

        public void BroadCastBattleMsg(IMessage message)
        {
        }

        public void OnEnterBattleScene(BattleScene scene)
        {
            BattleScene = scene;
        }

        public void OnLeaveBattleScene()
        {
            BattleScene = null;

            //TODO 回到原来的图
        }

        public void OnStopBattle(BattleResult result)
        {
            //战斗奖励结算
        }
    }
}
