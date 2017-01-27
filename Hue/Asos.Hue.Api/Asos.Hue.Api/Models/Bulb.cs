using System.Collections.Generic;
using System.Security.Permissions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Asos.Hue.Api.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Bulb
    {
        public string Name { get; set; }
        [JsonProperty("on")]
        public bool IsOn { get; set; }
        [JsonProperty("Reachable")]

        public bool Reachable { get; set; }
        public int Id { get; set; }

        //public static List<Bulb> ParseFromJson()
        //{
        //    return null;
        //}

        public string OnOffEndpoint(string userKey)
        {
            return $"/api/{ userKey }/lights/{ Id }/state";
        }

        public static Bulb ParseFromJson(KeyValuePair<string, JToken> json)
        {
            var bulb = json.Value["state"].ToObject<Bulb>();
            bulb.Id = int.Parse(json.Key);
            bulb.Name = json.Value["name"].ToString();
            return bulb;
        } 
    }
}
