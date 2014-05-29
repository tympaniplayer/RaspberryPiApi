using MongoDB.Bson;
using Raspberry.IO.GeneralPurpose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NancyBlink.Models
{
    public class GpioPin
    {
        public GpioPin() { }
        public GpioPin(GpioPin other)
        {
            Powered = other.Powered;
            PinNumber = other.PinNumber;
        }
        public ObjectId Id { get; set; }
        public bool Powered { get; set; }
        public int PinNumber { get; set; }
    }

    public static class PinExtensions
    {
        public static ConnectorPin ToConnectorPin (this GpioPin pin)
        {
            switch (pin.PinNumber)
            {
                case 3:
                    return ConnectorPin.P1Pin03;
                case 5:
                    return ConnectorPin.P1Pin05;
                case 7:
                    return ConnectorPin.P1Pin07;
                case 8: 
                    return ConnectorPin.P1Pin08;
                case 10:
                    return ConnectorPin.P1Pin10;
                case 11: 
                    return ConnectorPin.P1Pin11;
                case 12:
                    return ConnectorPin.P1Pin12;
                case 13:
                    return ConnectorPin.P1Pin13;
                case 15:
                    return ConnectorPin.P1Pin15;
                case 16:
                    return ConnectorPin.P1Pin16;
                default:
                    throw new InvalidOperationException("Pin not implemented");
            }
        }
    }
}
