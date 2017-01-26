using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asos.Hue.Api.Interfaces
{
    public interface IHue
    {
        //Task<List<Bulb>>  GetAllBulbs();
        Task TurnOn(Bulb lightNumber);
        Task TurnOff(Bulb lightNumber);
        Task Flash(Bulb lightNumber, int duration = 10);
    }
}
