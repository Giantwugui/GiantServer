namespace Giant.Core
{
    public interface IUpdate
    {
        void Update(double dt);
    }

    [ObjectAttribute]
    public interface IUpdateSystem : IUpdate { }
}
