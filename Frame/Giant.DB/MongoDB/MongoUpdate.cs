using System.Threading.Tasks;

namespace Giant.DB.MongoDB
{
    public abstract class MongoUpdate
    {
        public abstract Task UpdateTask();
    }
}
