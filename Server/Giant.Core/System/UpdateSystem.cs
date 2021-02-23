namespace Giant.Core
{
    public interface IUpdate
    {
        void Update(double dt);
    }

    public interface IUpdateSystem : IUpdate { }
}
