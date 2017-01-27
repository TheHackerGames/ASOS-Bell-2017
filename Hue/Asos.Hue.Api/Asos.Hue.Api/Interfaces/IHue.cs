using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asos.Hue.Api.Enums;
using Asos.Hue.Api.Models;

namespace Asos.Hue.Api.Interfaces
{
    public interface IHue
    {
        Task<List<Bulb>> GetAllBulbs();
        Task TurnOn(Bulb lightNumber, HueColor color = HueColor.Default);
        Task TurnOff(Bulb lightNumber, HueColor color = HueColor.Default);
        Task Toggle(Bulb lightNumber, HueColor color = HueColor.Default);
        Task Flash(Bulb lightNumber, HueColor color = HueColor.Default, int cycles= 3);
    }
}
