using Giant.Model;
using Giant.Util;

namespace Giant.Battle
{
    /*
     * 统一构造IMessage，用于通知前端
     */
    partial class BattleScene : IBattleSceneEvent
    {
        #region 广播

        public void OnBattleStart()
        {
        }

        public void OnBattleStop(MapModel model, BattleResult result)
        {
            PlayerList.ForEach(x => x.Value.MsgListener.OnBattleStop(model, result));
        }

        public void OnBattleEnd()
        {
        }

        #endregion
    }
}
