namespace Giant.Share
{
    public enum AppType
    {
        Gate = 1,
        Manager = 1<<1,
        Map = 1<<2,
        Relation = 1<<3,
        AllServer = Gate | Manager | Map | Relation
    }
}
