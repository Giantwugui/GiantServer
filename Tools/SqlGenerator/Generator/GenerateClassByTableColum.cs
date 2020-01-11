using System;
using System.IO;
using System.Text;

namespace SqlGenerator
{
    public class GenerateClassByTableColum : BaseGenerator
    {
        public override void GenerateCode(ColumInfo columInfo)
        {
            string className = SerializeText(columInfo.TableName);

            StringBuilder builder = new StringBuilder();
            builder.Append($"public class {className}\r\n");
            builder.Append("{\r\n");

            foreach (var cloume in columInfo.Colume2Type)
            {
                builder.Append($"\tpublic {cloume.Value.Name} {SerializeText(cloume.Key)} ");
                builder.Append("{ get; set; }\r\n");
            }

            builder.Append("}\r\n");

            Write2File(className, builder);

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
