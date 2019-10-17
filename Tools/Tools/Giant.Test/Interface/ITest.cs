using System;

namespace Giant.Test
{
    public interface ITest
    {
        public void DoTest() { Console.WriteLine("Default DoTest()"); }

        public void DoTest(string content) { Console.WriteLine("Default DoTest(string content)"); }
    }
}
