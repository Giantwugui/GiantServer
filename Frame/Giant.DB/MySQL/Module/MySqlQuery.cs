using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Giant.DB.MySQL
{
    public class MySqlQuery<T> : MySQLTask<T> where T : class
    {
        public MySqlQuery(DBService service, string tableName)
        {
            this.DBService = service;
            this.TableName = tableName;
        }

        public override async Task Run()
        {
            try
            {
                var command = this.GetCommand();
                command.CommandText = "";
                command.CommandType = CommandType.Text;

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

    public class MySqlQueryBatch<T> : MySQLTask<List<T>> where T : class
    {
        public MySqlQueryBatch(DBService service, string tableName)
        {
            this.DBService = service;
            this.TableName = tableName;
        }

        public override async Task Run()
        {
            try
            {
                var command = this.GetCommand();
                command.CommandText = "";
                command.CommandType = CommandType.Text;

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
