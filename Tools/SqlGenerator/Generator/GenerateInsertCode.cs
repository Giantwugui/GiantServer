using System;
using System.Text;

namespace SqlGenerator
{
    public class GenerateInsertCode : BaseGenerator
    {
        public override void GenerateCode(ColumInfo columInfo)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append($"INSERT INTO `{columInfo.TableName}` ");
            builder.Append("(");
            foreach (var kv in columInfo.colume2Type)
            {
                builder.Append($"`{kv.Key}`,");
            }
            builder.Append(")");
            builder.Append("VALUES");
            builder.Append("(");
            foreach (var kv in columInfo.colume2Type)
            {
                builder.Append($"@{kv.Key},");
            }
            builder.Append(");\r\n");
            foreach (var kv in columInfo.colume2Type)
            {
                builder.Append($"command.Parameters.AddWithValue(@{kv.Key},)\r\n");
            }


            Console.WriteLine(builder.ToString());
        }
    }
}
