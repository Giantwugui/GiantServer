namespace Giant.Core
{
    public static class Scene
    {
        public static AppConfig AppConfig { get; set; }

        private static ComponentPool pool;
        public static ComponentPool Pool
        {
            get { return pool ?? (pool = new ComponentPool()); }
        }

        private static EventSystem eventSystem;
        public static EventSystem EventSystem
        {
            get { return eventSystem ?? (eventSystem = new EventSystem()); }
        }
    }
}
