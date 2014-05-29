using System;
using System.Configuration;
using System.Linq;

using Nancy;
using Nancy.ModelBinding;
using Raspberry.IO.GeneralPurpose;
using NancyBlink.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;



namespace Example
{
    public class Program : NancyModule
    {
        private static MongoClient _client;
        private static MongoServer _server;
        private static MongoDatabase _database;
        static void Main(string[] args)
        {
            Console.Write("Starting server...");
            var server = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:8282"));
            server.Start();
            _client = new MongoClient(ConfigurationManager.AppSettings["MongoDB"]);
            _server = _client.GetServer();
            _database = _server.GetDatabase("gpio");
            Console.WriteLine("started on port 8282!");
            Console.WriteLine("press any key to exit");
            Console.Read();
        }

        public Program()
        {
            Get["/"] = _ =>
            {
                var collection = _database.GetCollection<GpioPin>("gpiopins");

                return collection.AsQueryable<GpioPin>().First(t => t.PinNumber == 16);
            };
            Put["/GpioPin"] = _ =>
            {
                var collection = _database.GetCollection<GpioPin>("gpiopins");
                var gpioPin = this.Bind<GpioPin>();
                var dbPin = collection.AsQueryable<GpioPin>().FirstOrDefault(t => t.PinNumber == gpioPin.PinNumber);
                if (dbPin == null)
                {
                    dbPin = new GpioPin(gpioPin);
                    collection.Insert<GpioPin>(dbPin);
                }
                dbPin.Powered = gpioPin.Powered;
                OutputPinConfiguration pin = gpioPin.ToConnectorPin().Output();
                Console.WriteLine("/GpioPin called for pin{0}", dbPin.PinNumber);
                //var connection = new GpioConnection(pin);
                //connection[pin] = gpioPin.Powered;
                collection.Save(dbPin);
                return gpioPin;
            };            
        }
    }
}
