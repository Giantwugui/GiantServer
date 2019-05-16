using Giant.Log;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Giant.DB.MySQL
{
    public class MySqlQueryPlayer : MySqlQueryTask<Player>
    {
        private readonly int uid;

        public MySqlQueryPlayer(DBService service, int uid) : base(service)
        {
            this.DBService = service;
            this.uid = uid;
        }

        public override async Task Run()
        {
            try
            {
                var command = this.GetCommand();
                try
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT Account,Uid,Level FROM Player WHERE Uid = @uid";
                    command.Parameters.AddWithValue("@uid", this.uid);
                    command.Connection.Open();

                    await base.Run(command);
                }
                catch (Exception ex)
                {
                    SetException(ex);
                }
                finally
                {
                    command.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }

    //public class MySqlQueryBatch<T> : MySQLTask<List<T>> where T : class
    //{
    //    public MySqlQueryBatch(DBService service, string tableName)
    //    {
    //        this.DBService = service;
    //        this.TableName = tableName;
    //    }

    //    public override async Task Run()
    //    {
    //        try
    //        {
    //            var command = this.GetCommand();
    //            command.CommandText = "";
    //            command.CommandType = CommandType.Text;

    //            var reader = await command.ExecuteReaderAsync();

    //            List<T> reasult = new List<T>();
    //            Dictionary<string, object> datas = new Dictionary<string, object>();

    //            while (await reader.ReadAsync())
    //            {
    //                for (int i = 0; i < reader.FieldCount; ++i)
    //                {
    //                    datas[reader.GetName(i)] = reader[i];
    //                }

    //                reasult.Add(this.BuildData<T>(datas));
    //                datas.Clear();
    //            }

    //            SetResult(reasult);
    //        }
    //        catch (Exception ex)
    //        {
    //            SetException(ex);
    //        }
    //    }
    //}
}
