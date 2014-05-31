using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpioApi.Models
{
    public interface IGpioPin
    {
        int PinNumber { get; set; }
        bool Powered { get; set; }
    }
}
