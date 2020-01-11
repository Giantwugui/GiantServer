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
            foreach (var kv in columInfo.Colume2Type)
            {
                if (columInfo.IsPrimaryKey(kv.Key)) continue;

                builder.Append($"{kv.Key}=@{kv.Key},");
            }

            builder.Append(" WHERE ");

            int keyCount = columInfo.PrimaryKey.Count;
            int andCount = keyCount - 1;

            for (int i = 0; i < keyCount; ++i)
            { 
                builder.Append($"{columInfo.PrimaryKey[i]}= ");
                if (i < andCount)
                { 
                    builder.Append(" AND ");
                }
            }
            builder.Append(";\r\n");

            foreach (var kv in columInfo.Colume2Type)
            {
                builder.Append($"command.Parameters.AddWithValue(@{kv.Key},)\r\n");
            }

            Console.WriteLine(builder.ToString());
        }
    }
}
