using System;
using Grpc.Core;
using MathMethod;
using Math = MathMethod.Math;

namespace Grpc.Test.Server
{
    //身份验证 https://grpc.io/docs/guides/auth/
    class Program
    {
        const string Host = "0.0.0.0";
        const int Port = 23456;

        public static void Main(string[] args)
        {
            Grpc.Core.Server server = new Core.Server
            {
                Services = { Math.BindService(new MathMethodServiceImpl()) },
                Ports = { { Host, Port, ServerCredentials.Insecure } }
            };

            server.Start();

            Console.WriteLine("MathServer listening on port " + Port);

            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
