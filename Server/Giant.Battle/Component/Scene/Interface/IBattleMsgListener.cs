using Giant.Model;
using Google.Protobuf;

namespace Giant.Battle
{
    /*
     * 用于监听战斗场景中的消息
     * 重要用于战斗同步前端，战斗录像等需要感知战斗过程的场景
     */
    public interface IBattleMsgListener
    {
        void OnBattleStop(MapModel model, BattleResult result);

        void BroadCastBattleMsg(IMessage message);
    }
}
