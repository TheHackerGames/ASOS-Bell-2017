using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asos.Hue.Api.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Asos.Hue.Api.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PostJsonModel
    {
        [JsonProperty("on")]
        public bool IsOn { get; set; }
        [JsonProperty("sat")]
        public int Saturation { get; set; }
        [JsonProperty("bri")]
        public int Brightness { get; set; }
        [JsonProperty("hue")]
        public int Hue { get; set; }

        public PostJsonModel(bool isOn)
        {
            IsOn = isOn;
            SetColour(HueColor.Default);
        }
        public PostJsonModel(bool isOn, HueColor color)
        {
            IsOn = isOn;
            SetColour(color);
        }

        public void SetColour(HueColor color)
        {
            switch (color)
            {
                case HueColor.Green:
                    Saturation = 254;
                    Brightness = 107;
                    Hue = 25500;
                    break;
                case HueColor.Orange:
                    Saturation = 240;
                    Brightness = 100;
                    Hue = 65500;
                    break;
                case HueColor.White:
                    Saturation = 100;
                    Brightness = 100;
                    Hue = 40000;
                    break;
                case HueColor.Default:
                    Saturation = 0;
                    Brightness = 0;
                    Hue = 0;
                    break;
                default:
                    throw new InvalidOperationException("Incorrect Color");
            }
        }
    }
}
