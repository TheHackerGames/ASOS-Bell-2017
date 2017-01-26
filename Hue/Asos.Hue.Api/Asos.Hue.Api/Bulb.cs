using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Asos.Hue.Api
{
    public class Bulb
    {
        public bool IsOn { get; set; }
        public int Id { get; set; }

        //public static List<Bulb> ParseFromJson()
        //{
        //    return null;
        //}

        public string OnOffEndpoint(string userKey)
        {
            return $"/api/{ userKey }/lights/{ Id }/state";
        }
    }
}
