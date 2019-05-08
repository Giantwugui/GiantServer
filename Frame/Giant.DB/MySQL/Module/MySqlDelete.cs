using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Giant.DB.MySQL
{
    public class MySqlDelete<T> : MySQLTask<bool>
    {
        private readonly Expression<Func<T, bool>> filter;

        public MySqlDelete(DBService service, string tableName, Expression<Func<T, bool>> filter)
        {
            this.DBService = service;
            this.filter = filter;
            this.TableName = tableName;
        }

        public override async Task Run()
        {
            try
            {
                SetResult(true);
            }
            catch (Exception ex)
            {
                SetException(ex);
            }
        }
    }

    public class MongoDBDeleteBatch<T> : MySQLTask<bool>
    {
        private readonly Expression<Func<T, bool>> filter;

        public MongoDBDeleteBatch(DBService service, string tableName, Expression<Func<T, bool>> filter)
        {
            this.DBService = service;
            this.filter = filter;
            this.TableName = tableName;
        }

        public override async Task Run()
        {
            try
            {

                SetResult(false);
            }
            catch (Exception ex)
            {
                SetException(ex);
            }
        }
    }
}
