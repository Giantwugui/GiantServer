using System;
using System.Text;

namespace SqlGenerator
{
    public class GenerateUpdateCode : BaseGenerator
    {
        public override void GenerateCode(ColumInfo columInfo)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append($"UPDATE `{columInfo.TableName}` SET ");
            foreach (var kv in columInfo.colume2Type)
            {
                builder.Append($"{kv.Key}=@{kv.Key},");
            }
            builder.Append(" WHERE A=B ;\r\n");

            foreach (var kv in columInfo.colume2Type)
            {
                builder.Append($"command.Parameters.AddWithValue(@{kv.Key},)\r\n");
            }

            Console.WriteLine(builder.ToString());
        }
    }
}
