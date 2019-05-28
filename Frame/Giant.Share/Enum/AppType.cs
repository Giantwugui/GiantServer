namespace Giant.Share
{
    public enum AppType
    {
        Gate = 1,
        Manager = 1<<1,
        Zone = 1<<2,
        Social = 1<<3,
        AllServer = Gate | Manager | Zone | Social
    }
}
