using System;
using System.IO;
using System.Text;

namespace SqlGenerator
{
    public class GenerateClassByTableColum : BaseGenerator
    {
        public override void GenerateCode(ColumInfo columInfo)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"public class {SerializeText(columInfo.TableName)}\r\n");
            builder.Append("{\r\n");

            foreach (var cloume in columInfo.colume2Type)
            {
                builder.Append($"\tpublic {SerializeText(cloume.Value.Name)} {cloume.Key} ");
                builder.Append("{ get; set; }\r\n");
            }

            builder.Append("}\r\n");

            Write2File(columInfo.TableName, builder);

            Console.WriteLine(builder.ToString());
        }

        private void Write2File(string name, StringBuilder builder)
        {
            string path = CodeGenerator.Instance.CodePath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, name + ".cs");

            StreamWriter writer = File.CreateText(path);
            writer.Write(builder);
            writer.Close();
        }

        private string SerializeText(string text)
        {
            text = text.Replace("_", "");
            return text.Substring(0, 1).ToUpper() + text[1..];
        }
    }

}
