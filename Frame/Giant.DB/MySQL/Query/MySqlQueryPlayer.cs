using Dapper;
using Giant.Log;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Giant.DB.MySQL
{
    public class MySqlQueryPlayer : MySqlQueryTask<PlayerInfo>
    {
        private readonly int uid;

        public MySqlQueryPlayer(int uid)
        {
            this.uid = uid;
        }

        public override async Task Run()
        {
            var connection = this.GetConnection();
            try
            {
                connection.Open();
                string sql = "SELECT Account,Uid,Level FROM player WHERE Uid = @uid";
                var result = await connection.QueryAsync<PlayerInfo>(sql, new { this.uid });
                SetResult(result.FirstOrDefault());
            }
            catch (Exception ex)
            {
                SetException(ex);
                Logger.Error(ex);
            }
            finally
            {
                connection.Dispose();
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
            var connection = this.GetConnection();
            try
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT Max(Uid) AS maxUid FROM player";

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
                connection.Dispose();
            }
        }
    }
}
