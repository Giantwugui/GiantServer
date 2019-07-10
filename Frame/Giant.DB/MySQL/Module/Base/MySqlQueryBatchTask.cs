using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Giant.DB.MySQL
{
    public abstract class MySqlQueryBatchTask<T> : MySQLTask<List<T>>
    {
        public async Task<List<Dictionary<string, object>>> Run(MySqlCommand command)
        {
            Dictionary<string, object> datas;
            List<Dictionary<string, object>> reasult = new List<Dictionary<string, object>>();

            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                datas = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; ++i)
                {
                    datas[reader.GetName(i)] = reader[i];
                }
                reasult.Add(datas);
            }

            return reasult;
        }
    }
}
