using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Giant.DB.MySQL
{
    public abstract class MySqlQueryTask<T> : MySQLTask<T> where T : class
    {
        public MySqlQueryTask(DBService service)
        {
            this.DBService = service;
        }

        public override async Task Run(MySqlCommand command)
        {
            try
            {
                var reader = await command.ExecuteReaderAsync();

                Dictionary<string, object> datas = new Dictionary<string, object>();

                while (await reader.ReadAsync())
                {
                    for (int i = 0; i < reader.FieldCount; ++i)
                    {
                        datas[reader.GetName(i)] = reader[i];
                    }
                }

                SetResult(this.BuildData<T>(datas));
            }
            catch (Exception ex)
            {
                SetException(ex);
            }
        }
    }
}
