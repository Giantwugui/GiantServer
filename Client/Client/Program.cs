using Giant.Data;
using Giant.Share;
using System;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //DataManager.Instance.Init();
                NET.Init();

                ConsoleReader.Instance.Start(DoCmd);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            while (true)
            {
                try
                {
                    PlayerManager.Instance.Update();
                    Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

        }

        private static void DoCmd(string content)
        {
            string[] param = content.Split(' ');
            if (param.Length <= 0)
            {
                Console.WriteLine("Param error !");
            }

            switch (param[0])
            {
                case "Login":
                    {
                        NET.DoLogin(param[1], "");
                    }
                    break;
                default:
                    Console.WriteLine("Not suport cmd !");
                    break;
            }
        }

    }
}