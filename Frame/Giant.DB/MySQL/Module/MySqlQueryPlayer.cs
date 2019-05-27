using Giant.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Giant.DB.MySQL
{
    public class MySqlQueryPlayer : MySqlQueryTask<Player>
    {
        private readonly int uid;

        public MySqlQueryPlayer(int uid)
        {
            this.uid = uid;
        }

        public override async Task Run()
        {
            try
            {
                var connection = this.GetConnection();
                try
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT Account,Uid,Level FROM Player WHERE Uid = @uid";
                    command.Parameters.AddWithValue("@uid", this.uid);

                    Dictionary<string, object> datas = await base.Run(command);
                    Player player = MySqlHelper.BuildInstance<Player>(datas);

                    //其他属性的初始化

                    SetResult(player);
                }
                catch (Exception ex)
                {
                    SetException(ex);
                    Logger.Error(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }


    public class MySqlQueryMaxPlayerUid : MySqlQueryTask<int>
    {
        public MySqlQueryMaxPlayerUid()
        {
        }

        public override async Task Run()
        {
            try
            {
                var connection = this.GetConnection();
                try
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT Max(Uid) AS maxUid FROM Player";

                    var datas = await base.Run(command);

                    var max = datas["maxUid"];

                    SetResult(max == System.DBNull.Value ? 0 : (int)max);
                }
                catch (Exception ex)
                {
                    SetException(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}
