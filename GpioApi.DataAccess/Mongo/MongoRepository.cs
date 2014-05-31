using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using GpioApi.Models;

namespace GpioApi.DataAccess.Mongo
{
    public class MongoRepository : IRepository
    {
        private MongoDatabase database;
        private MongoServer server;
        private MongoCollection gpioPins;
        public MongoRepository()
        {
            var client = new MongoClient(ConfigurationManager.AppSettings["MongoDB"]);
            server = client.GetServer();
            database = server.GetDatabase("gpio");
            gpioPins = database.GetCollection("gpiopins");
        }
        public Models.IGpioPin GetPin(int pinNumber)
        {
            return Pins().FirstOrDefault(t => t.PinNumber == pinNumber);
        }

        public IEnumerable<Models.IGpioPin> GetPins()
        {
            return Pins().ToList();
        }

        public void SavePinState(Models.IGpioPin pin)
        {
            if (!(pin is MongoGpioPin))
            {
                throw new InvalidOperationException("Must be mongo pin type");
            }
            gpioPins.Save<MongoGpioPin>(pin as MongoGpioPin);
        }

        public void AddPin(Models.IGpioPin pin)
        {
            var checkPin = Pins().FirstOrDefault(t => t.PinNumber == pin.PinNumber);
            if (checkPin != null)
            {
                throw new InvalidOperationException("Pin already exists!");
            }
        }

        private IQueryable<MongoGpioPin> Pins()
        {
            return gpioPins.AsQueryable<MongoGpioPin>();
        }
    }
}
