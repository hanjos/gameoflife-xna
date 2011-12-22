using System;
using System.Reflection;

namespace Tests
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            NUnit.Gui.AppEntry.Main(new string[] 
            {
              Assembly.GetExecutingAssembly().Location, 
              "/runselected"
            });
        }
    }
}
