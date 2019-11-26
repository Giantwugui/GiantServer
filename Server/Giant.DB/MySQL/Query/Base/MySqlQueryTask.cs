using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Giant.DB.MySQL
{
    public abstract class MySqlQueryTask<TResult> : MySQLTask<TResult>
    {
        public virtual async Task<Dictionary<string, object>> Run(MySqlCommand command)
        {
            Dictionary<string, object> datas = new Dictionary<string, object>();

            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                for (int i = 0; i < reader.FieldCount; ++i)
                {
                    datas[reader.GetName(i)] = reader[i];
                }
            }

            return datas;
        }
    }
}
