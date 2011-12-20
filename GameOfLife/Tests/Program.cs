using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Tests
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //string[] my_args = { Assembly.GetExecutingAssembly().Location };

            //int returnCode = NUnit.ConsoleRunner.Runner.Main(my_args);

            //if (returnCode != 0)
            //    Console.Beep();

            AppDomain.CurrentDomain.ExecuteAssembly(
                @"C:\Arquivos de programas\NUnit 2.5.10\bin\net-2.0\NUnit-console.exe",
                null,
                new string[] { Assembly.GetExecutingAssembly().Location });
        }
    }
}
