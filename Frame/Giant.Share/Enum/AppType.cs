namespace Giant.Share
{
    public enum AppType
    {
        Global = 1,
        Gate = 1<<1,
        Manager = 1<<2,
        Map = 1<<3,
        Relation = 1<<4,
        AllServer = Gate | Manager | Map | Relation
    }
}
