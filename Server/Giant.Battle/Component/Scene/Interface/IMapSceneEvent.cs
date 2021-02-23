namespace Giant.Battle
{
    public interface IMapSceneEvent
    {
        public void OnUnitEnter(Unit unit);
        public void OnUnitLeave(Unit unit);
    }
}
