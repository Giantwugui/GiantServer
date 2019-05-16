using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Giant.DB.MySQL
{
    public abstract class MySqlQueryBatchTask<T> : MySQLTask<List<T>> where T : class
    {
        public MySqlQueryBatchTask(DBService service)
        {
            this.DBService = service;
        }

        public override async Task Run(MySqlCommand command)
        {
            try
            {
                var reader = await command.ExecuteReaderAsync();

                List<T> reasult = new List<T>();
                Dictionary<string, object> datas = new Dictionary<string, object>();

                while (await reader.ReadAsync())
                {
                    for (int i = 0; i < reader.FieldCount; ++i)
                    {
                        datas[reader.GetName(i)] = reader[i];
                    }

                    reasult.Add(this.BuildData<T>(datas));
                    datas.Clear();
                }

                SetResult(reasult);
            }
            catch (Exception ex)
            {
                SetException(ex);
            }
        }
    }
}
