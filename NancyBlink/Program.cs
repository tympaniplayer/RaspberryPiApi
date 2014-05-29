using System;

using Nancy;
using Nancy.ModelBinding;
using Raspberry.IO.GeneralPurpose;
using NancyBlink.Models;


namespace Example
{
    public class Program : NancyModule
    {
        static void Main(string[] args)
        {
            Console.Write("Starting server...");
            var server = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:8282"));
            server.Start();
            Console.WriteLine("started on port 8282!");
            Console.WriteLine("press any key to exit");
            Console.Read();
        }

        public Program()
        {
            Get["/"] = _ =>
            {
                return "Use this api to turn on various lights connected to the Pi's GPIO Pins";
            };
            Post["/GpioPin"] = _ =>
            {
                var gpioPin = this.Bind<GpioPin>();
                OutputPinConfiguration pin = gpioPin.ToConnectorPin().Output();
                //var connection = new GpioConnection(pin);
                //connection[pin] = gpioPin.Powered;
                return gpioPin;
            };
        }
    }
}
