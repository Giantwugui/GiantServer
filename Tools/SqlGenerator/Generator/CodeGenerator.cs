using Giant.Core;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace SqlGenerator
{
    public class CodeGenerator : Single<CodeGenerator>
    {
        MySqlConnection connection;
        private string codePath;

        private List<string> tables = new List<string>();
        private Dictionary<GeneratorType, BaseGenerator> generators = new Dictionary<GeneratorType, BaseGenerator>();

        private Dictionary<string, ColumInfo> columInfoes = new Dictionary<string, ColumInfo>();

        public string CodePath => codePath;

        public void Init()
        {
            connection = DBConnector.GetConnection();

            codePath = Path.Combine(Directory.GetCurrentDirectory(), "Code");

            GetAllTables();
            ShowTables();
            GenerateColumInfo();//生成表结构信息

            //注册生成器
            RetistGenerator(GeneratorType.Colum2Class);
            RetistGenerator(GeneratorType.Insert);
            RetistGenerator(GeneratorType.Update);
        }

        public void GenerateCode(GeneratorType type, string table)
        {
            BaseGenerator gene = GetGenerator(type);
            ColumInfo columInfo = GetColumInfo(table);

            if (gene == null || columInfo == null)
            {
                Console.WriteLine("没有该表！");
                return;
            }

            gene.GenerateCode(columInfo);
        }

        private BaseGenerator GetGenerator(GeneratorType type)
        {
            generators.TryGetValue(type, out var generator);
            return generator;
        }

        private void RetistGenerator(GeneratorType type)
        {
            BaseGenerator generator = BuildGenerator(type);
            generators.Add(type, generator);
        }

        private BaseGenerator BuildGenerator(GeneratorType type)
        {
            switch (type)
            {
                case GeneratorType.Colum2Class: return new GenerateClassByTableColum();
                case GeneratorType.Insert: return new GenerateInsertCode();
                case GeneratorType.Update: return new GenerateUpdateCode();
                default: return null;
            }
        }

        private void GetAllTables()
        {
            string sql = "SHOW TABLES;";

            try
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tables.Add(reader.GetString(0));
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
                Console.WriteLine(ex);
            }
        }

        private void ShowTables()
        {
            Console.WriteLine("tables:\r\n");

            for (int i = 0; i < tables.Count; ++i)
            {
                Console.WriteLine($"{i + 1}：{tables[i]}");
            }
            Console.WriteLine("\r\n");
        }

        private void GenerateColumInfo()
        {
            try
            {
                connection.Open();
                foreach (var table in tables)
                {
                    MySqlCommand command = connection.CreateCommand();

                    string sql = $"show fields from {table};";

                    ColumInfo columInfo = new ColumInfo(table);

                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;

                    using var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string name = reader.GetString("Field");
                        string type = reader.GetString("Type");

                        string key = reader.GetString("Key");
                        if ("PRI".Equals(key))
                        {
                            columInfo.AddPrimaryKey(name);
                        }

                        columInfo.AddColume(name, DBType2ClassType(type));
                    }

                    columInfoes[table] = columInfo;
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
                Console.WriteLine(ex);
            }
        }

        public ColumInfo GetColumInfo(string table)
        {
            columInfoes.TryGetValue(table, out var info);
            return info;
        }

        public static Type DBType2ClassType(string typeName)
        {
            if (typeName.StartsWith("int")) return typeof(int);
            else if (typeName.StartsWith("tinyint")) return typeof(int);
            else if (typeName.StartsWith("smallint")) return typeof(int);
            else if (typeName.StartsWith("double")) return typeof(double);
            else if (typeName.StartsWith("varchar")) return typeof(string);
            else if (typeName.StartsWith("datetime")) return typeof(DateTime);

            return typeof(string);
        }
    }
}
