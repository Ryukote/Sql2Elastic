using ElasticSQLServer.Utilities.Factory;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using Timer = System.Timers.Timer;

namespace ElasticSQLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

                Hook hook = new Hook(configuration);

                Timer timer = new Timer
                {
                    Interval = 600000,
                    Enabled = true
                };

                hook.StartProcess().GetAwaiter();
                timer.Start();

                timer.Elapsed += async (sender, e) =>
                {
                    await hook.IterationProcess();
                };

                Thread.Sleep(Timeout.Infinite);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ReadKey();
            }
        }
    }
}
