namespace Giant.Model
{
    public static class Game
    {
        public static ObjectPool ObjectPool => new ObjectPool();

        public static EventSystem EventSystem => new EventSystem();

        public static Screen Screen => new Screen();
    }
}
