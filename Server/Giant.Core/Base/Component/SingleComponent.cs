namespace Giant.Core
{
    public abstract class SingleComponent<T> : Component, IInitSystem where T : Component, new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Scene.Pool.GetComponent<T>();
                    if (instance == null)
                    {
                        instance = Scene.Pool.AddComponent<T>();
                    }
                }
                return instance;
            }
        }

        public abstract void Init();
    }

    public abstract class SingleEntity<T> : Entity, IInitSystem where T : Entity, new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Scene.Pool.GetComponent<T>();
                    if (instance == null)
                    {
                        instance = Scene.Pool.AddComponent<T>();
                    }
                }
                return instance;
            }
        }
    }

}
