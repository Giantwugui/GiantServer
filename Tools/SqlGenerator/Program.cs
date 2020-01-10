using Giant.Core;
using System;
using System.Threading;

namespace SqlGenerator
{
    class Program
    {

        static void Main(string[] args)
        {
            DataManager.Instance.Init();
            DBConfig.Init();

            CodeGenerator.Instance.Init();

            Console.WriteLine("Operation：\r\n");
            Console.WriteLine("1.生成表对应的Class");
            Console.WriteLine("2.生成出入数据语句");
            Console.WriteLine("3.生成更新数据语句(where条件需要生成后手动替换)");

            Console.WriteLine("请输入表名：");

            while (true)
            {
                Thread.Sleep(100);

                string[] param = Console.ReadLine().Split(" ");
                if (param.Length != 2)
                {
                    Console.WriteLine("输入 type tablename, eg: 1 player");
                    continue;
                }

                if (int.TryParse(param[0], out var type))
                { 
                    CodeGenerator.Instance.GenerateCode((GeneratorType)type, param[1]);
                }
            }
        }
    }
}
