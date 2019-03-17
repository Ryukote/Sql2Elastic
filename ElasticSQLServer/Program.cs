using ElasticSQLServer.Utilities.Factory;
using System;
using System.Timers;

namespace ElasticSQLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Hook hook = new Hook();

                Timer timer = new Timer
                {
                    Interval = 10000,
                    Enabled = true
                };

                hook.StartProcess().GetAwaiter();

                timer.Start();

                timer.Elapsed += async (sender, e) =>
                {
                    await hook.IterationProcess();
                };

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}
