using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Asos.Hue.Api.Enums;
using Asos.Hue.Api.Interfaces;
using Asos.Hue.Api.Models;
using Asos.Hue.Api.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Asos.Hue.Api
{
    public class HueHub : IHue
    {
        private readonly HueHubOptions _options;
        private readonly HttpClient _httpClient;

        public HueHub(HueHubOptions options)
        {
            _options = options;
            _httpClient = ConfigureHttpClient(options);
        }

        public async Task<List<Bulb>> GetAllBulbs()
        {
            List<Bulb> bulbs = new List<Bulb>();
            var uri = $"api/{_options.UserKey}/lights";
            HttpResponseMessage response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                try
                {
                    foreach (var json in JObject.Parse(content))
                    {
                        bulbs.Add(Bulb.ParseFromJson(json));
                    }
                }
                catch (Exception)
                {
                    bulbs = null;
                }
            }
            return bulbs;
        }

        public async Task TurnOn(Bulb bulb, HueColor color = HueColor.Default)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(
                bulb.OnOffEndpoint(_options.UserKey),
                new PostJsonModel(isOn: true, color: color));

            response.EnsureSuccessStatusCode();
        }
        public async Task TurnOff(Bulb bulb, HueColor color = HueColor.Default)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(
                bulb.OnOffEndpoint(_options.UserKey),
                new PostJsonModel(isOn: false, color: color));

            response.EnsureSuccessStatusCode();
        }
        public async Task Toggle(Bulb bulb, HueColor color = HueColor.Default)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(
                bulb.OnOffEndpoint(_options.UserKey),
                new PostJsonModel(!bulb.IsOn, color));

            response.EnsureSuccessStatusCode();
        }
        public async Task Flash(Bulb bulb, HueColor color = HueColor.Default, int cycles = 3)
        {
            //TODO: A bit broken
            while (cycles > 0)
            {
                await TurnOn(bulb, color);
                Thread.Sleep(1000);
                await TurnOff(bulb, color);
                Thread.Sleep(1000);
                cycles--;
            }
            //TODO: IsOn not the correct property to use and TurnOn and TurnOff should change state of bulb
            if (bulb.IsOn) //reset
            {
                await TurnOn(bulb);
            }
            else
            {
                await TurnOff(bulb);
            }
        }
        public static HueHub Create(HueHubOptions options)
        {
            return new HueHub(options);
        }
        private HttpClient ConfigureHttpClient(HueHubOptions options)
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(options.Uri)
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/JSON"));
            return httpClient;
        }
    }
}
