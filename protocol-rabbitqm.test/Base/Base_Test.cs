using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace protocol.rabbitqm.test.Base
{
    public class Base_Test
    {
        private readonly ITestOutputHelper outputWriter;

        public Base_Test(ITestOutputHelper outputWriter)
        {
            this.outputWriter = outputWriter;
        }

        public void Log(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            outputWriter.WriteLine(message);
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }

    }
}
