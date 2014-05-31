using System;
using System.Configuration;
using System.Linq;

using Nancy;
using Nancy.ModelBinding;
using Raspberry.IO.GeneralPurpose;
using GpioApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using GpioApi.DataAccess.Mongo;
using GpioApi.DataAccess;
using System.Collections.Generic;



namespace GpioApi
{
    public class Program : NancyModule
    {
        private static IRepository repo;
        static void Main(string[] args)
        {
            Console.Write("Starting server...");
            var server = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:8282"));
            server.Start();
            repo = new MongoRepository();
            Console.WriteLine("started on port 8282!");
            Console.WriteLine("press any key to exit");
            Console.Read();
        }

        public Program()
        {
            Get["/"] = _ =>
            {
                return "Welcome to the Raspberry Pi Gpio Api";
            };
            Get["/GpioPin/{pinNum}"] = parameters =>
            {
                var pinNum = (int)parameters.pinNum;
                var pin = repo.GetPin(pinNum);
                return pin;
            };
            Get["/GpioPin"] = _ =>
            {
                return repo.GetPins();
            };
            Put["GpioPin/Off"] = _ =>
            {
                var pins = repo.GetPins();
                List<OutputPinConfiguration> outputPins = new List<OutputPinConfiguration>();
                pins.ToList().ForEach(t => outputPins.Add(t.ToConnectorPin().Output()));
                using(var connection = new GpioConnection(outputPins))
                {
                    foreach(var each in pins)
                    {
                        connection[each.ToConnectorPin()] = false;
                        each.Powered = false;                        
                        repo.SavePinState(each);
                    }
                }
                return pins;
            };
            Put["/GpioPin"] = _ =>
            {
                var gpioPin = this.Bind<MongoGpioPin>();
                var dbPin = repo.GetPin(gpioPin.PinNumber);

                OutputPinConfiguration pin = gpioPin.ToConnectorPin().Output();
                if (dbPin == null)
                {
                    dbPin = new MongoGpioPin(gpioPin);
                    repo.AddPin(dbPin);
                }

                dbPin.Powered = gpioPin.Powered;
                Console.WriteLine("/GpioPin called for pin{0}", dbPin.PinNumber);
                using (var connection = new GpioConnection(pin))
                {
                    connection[pin] = gpioPin.Powered;
                    repo.SavePinState(dbPin);
                }
                return dbPin;
            };
        }
    }
}
