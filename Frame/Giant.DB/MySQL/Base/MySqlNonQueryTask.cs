using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace Giant.DB.MySQL
{
    //执行Insert，Delete，Update 需要继承该类
    public abstract class MySqlNonQueryTask : MySQLTask<int>
    {
        public MySqlNonQueryTask(DBService service)
        {
            this.DBService = service;
        }

        public override async Task Run(MySqlCommand command)
        {
            try
            {
                var result = await command.ExecuteNonQueryAsync();

                SetResult(result);
            }
            catch (Exception ex)
            {
                SetException(ex);
            }
        }
    }
}
