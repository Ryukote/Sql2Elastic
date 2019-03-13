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
                Timer timer = new Timer
                {
                    Interval = 3600000,
                    Enabled = true
                };

                timer.Start();

                timer.Elapsed += async (sender, e) =>
                {
                    await new Hook().IterationProcess();
                };
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}
