namespace Giant.Core
{
    public interface IData
    {
        int Id { get; }

        void Bind(DataModel data);
    }
}
