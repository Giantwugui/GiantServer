using System;
using Grpc.Core;
using MathMethod;
using Math = MathMethod.Math;

namespace Grpc.Test.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var channel = new Channel("127.0.0.1", 23456, ChannelCredentials.Insecure);

            Math.MathClient client = new Math.MathClient(channel);
            MathExamples.DivExample(client);

            MathExamples.DivAsyncExample(client).Wait();

            MathExamples.FibExample(client).Wait();

            MathExamples.SumExample(client).Wait();

            MathExamples.DivManyExample(client).Wait();

            MathExamples.DependendRequestsExample(client).Wait();

            channel.ShutdownAsync().Wait();
        }
    }
}
