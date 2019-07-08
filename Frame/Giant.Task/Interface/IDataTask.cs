using System.Threading.Tasks;

namespace Giant.DataTask
{
    public interface IDataTask
    {
        long TaskId { get; set; }

        IDataService DataService { get; }

        Task Run();
    }
}
