using Giant.Log;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace Giant.DB.MySQL
{
    //执行Insert，Delete，Update 需要继承该类
    public abstract class MySqlNonQueryTask : MySQLTask<int>
    {
        public async Task Run(MySqlCommand command)
        {
            var result = await command.ExecuteNonQueryAsync();

            SetResult(result);
        }
    }
}
