namespace Giant.Core
{
    public interface ILoadSystem
    {
        void Load();
    }

    [ObjectAttribute]
    public abstract class LoadSystem : Component, ILoadSystem
    { 
        public abstract void Load();
    }
}
