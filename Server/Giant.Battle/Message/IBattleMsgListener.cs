using Giant.Model;
using Google.Protobuf;

namespace Giant.Battle
{
    public interface IBattleMsgListener
    {
        void OnBattleStart();
        void OnBattleEnd();
        void OnBattleStop(MapModel model, BattleResult result);


        void BroadCastBattleMsg(IMessage message);
    }
}
