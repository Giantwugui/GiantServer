namespace Giant.Battle
{
    public partial class BattleScene
    {
        public virtual void OnStart()
        {
            MonsterStartFighting();
            PlayerStartFighting();
            HeroStartFighting();

            OnBattleStart();
        }

        public virtual void OnStop(BattleResult result)
        {
            MonsterStopFighting();
            PlayerStopFighting();
            HeroStopFighting();

            OnBattleStop(MapModel, result);
        }

        public virtual void Close()
        {
            OnBattleEnd();
        }
    }
}
