namespace Giant.Model
{
    public static class Game
    {
        private static ObjectPool objectPool;

        public static ObjectPool ObjectPool
        {
            get
            {
                if (objectPool == null)
                {
                    objectPool = new ObjectPool();
                }

                return objectPool;
            }
        }
    }
}
