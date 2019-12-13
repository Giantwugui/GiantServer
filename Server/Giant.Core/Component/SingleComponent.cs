namespace Giant.Core
{
    public abstract class SingleComponent<T> : InitSystem where T : Component, new()
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
