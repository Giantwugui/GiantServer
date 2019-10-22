using System.Threading.Tasks;

namespace Giant.DB.MongoDB
{
    public abstract class MongoUpdateTask
    {
        public abstract Task UpdateTask();
    }
}
