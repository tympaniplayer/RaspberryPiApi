using Nancy;
using System;
using Raspberry.IO.GeneralPurpose;


namespace Example
{
        public class Program:Nancy.NancyModule
        {
                static void Main(string[] args) {
                        Console.Write("Starting server...");
                        var server = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:8282"));
                        server.Start();
                        Console.WriteLine("started on port 8282!");
                        Console.WriteLine("press any key to exit");
                        Console.Read();
                }

                public Program()
                {
                        Get["/"] = _ => { 
                            OutputPinConfiguration red = ConnectorPin.P1Pin03.Output();
                            var connection = new GpioConnection(red);
                            connection[red] = false;

                            return "Nancy says hello!"; 
                        };
                }
        }
}
