using System;
using System.Threading.Tasks;

namespace Giant.DB.MongoDB
{
    public class MongoDBSaveTask : MongoDBTask<bool>
    {
        private Player player;

        public MongoDBSaveTask(DBService service, Player player)
        {
            this.CollectionName = "Player";
            this.Service = service;
            this.player = player;
        }

        public override async Task Run()
        {
            try
            {
                var collection = (this.Service.Service as MongoDBService).GetCollection<Player>(this.CollectionName);

                await collection.InsertOneAsync(this.player);

                SetResult(true);
            }
            catch (Exception ex)
            {
                SetException(ex);
            }
        }
    }
}
