namespace Giant.Core
{
    public interface IData
    {
        int Id { get; }
    }

    public interface IData<T> : IData
    {
        void Bind(DataModel data);
    }

}
