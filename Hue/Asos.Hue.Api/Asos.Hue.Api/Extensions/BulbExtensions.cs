using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asos.Hue.Api.Extensions
{
    public static class BulbExtensions
    {
        public static List<Bulb> ParseJsonToBulbs(this string json)
        {
            return new List<Bulb>();
        }
    }
}
