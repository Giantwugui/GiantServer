using Giant.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Giant.DB.MySQL
{
    public class MySqlInsertPlayer : MySqlNonQueryTask
    {
        private readonly Player player;

        public MySqlInsertPlayer(DBService service, Player player)
        {
            this.DBService = service;
            this.player = player;
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
                    command.CommandText = "INSERT INTO Player (Account,Uid,Level) VALUES(@account,@uid,@level)";
                    command.Parameters.AddWithValue("@account", this.player.Account);
                    command.Parameters.AddWithValue("@uid", this.player.Uid);
                    command.Parameters.AddWithValue("@level", this.player.Level);

                    await base.Run(command);
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

    public class MySqlInsertPlayerBatch : MySqlNonQueryTask
    {
        private readonly List<Player> players;

        public MySqlInsertPlayerBatch(DBService service, List<Player> players)
        {
            this.DBService = service;
            this.players = players;
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

                    List<string> playerStrList = players.ConvertAll<string>(player => $"('{player.Account}','{player.Uid}','{player.Level}')");
                    string valueStr = string.Join(",", playerStrList);

                    command.CommandText = $"INSERT INTO Player (Account,Uid,Level) VALUES {valueStr};";

                    await base.Run(command);
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
