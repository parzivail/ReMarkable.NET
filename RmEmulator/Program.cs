using System;
using System.Linq;

namespace RmEmulator
{
    class Program
    {
        static void Main(string[] args)
        {
            string assemblyName = (args.Count() > 0) ? args[0] : null;
            if (!string.IsNullOrWhiteSpace(assemblyName))
            {
                new EmulatorWindow(assemblyName).Run();
            }
            else
            {
                Console.Out.WriteLine($@"RmEmulator.exe <emulated_app.dll>
  - emulated_app.dll - dll assembly compiled in AnyCPU to be loaded and executed by emulator

");
                Console.Out.WriteLine("Press ENTER to continue...");
                Console.In.ReadLine();
            }
        }
    }
}
