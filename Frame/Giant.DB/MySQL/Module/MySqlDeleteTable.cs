using Giant.Log;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Giant.DB.MySQL
{
    public class MySqlDeleteTable : MySqlNonQueryTask
    {
        private readonly string tableName;

        public MySqlDeleteTable(string tableName)
        {
            this.tableName = tableName;
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
                    command.CommandText = $"DELETE FROM {this.tableName}";

                    await base.Run(command);
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
}
