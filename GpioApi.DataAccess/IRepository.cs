using GpioApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpioApi.DataAccess
{
    public interface IRepository
    {
        /// <summary>
        /// Gets the GpioPin
        /// </summary>
        /// <param name="pinNumber">The pin number</param>
        /// <returns>The GpioPin</returns>
        IGpioPin GetPin(int pinNumber);
        /// <summary>
        /// Gets all the GpioPins in the database
        /// </summary>
        /// <returns>All the GpioPins</returns>
        IEnumerable<IGpioPin> GetPins();
        /// <summary>
        /// Saves the pin back to the database
        /// </summary>
        /// <param name="pin">The pin to save</param>
        void SavePinState(IGpioPin pin);
        /// <summary>
        /// Adds a pin to the database.
        /// </summary>
        /// <param name="pin"></param>
        void AddPin(IGpioPin pin);
    }
}
