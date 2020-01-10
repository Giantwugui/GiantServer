using System;

namespace Giant.Core
{
    public abstract class Single<T> where T : class
    {
        public static T Instance { get; } = Activator.CreateInstance<T>();
    }
}
