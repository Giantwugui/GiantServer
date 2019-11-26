using System.Threading.Tasks;

namespace Giant.Core
{
    public interface IDataTask
    {
        long TaskId { get; set; }

        IDataService DataService { get; }

        Task Run();
    }
}
