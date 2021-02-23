using Giant.Model;

namespace Giant.Battle
{
    public interface IBattleSceneEvent : IMapSceneEvent
    {
        void OnBattleStart();
        void OnBattleStop(MapModel model, BattleResult result);
        void OnBattleEnd();
    }
}
