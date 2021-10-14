using PixelArtFrontEnd.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PixelArtFrontEnd.Services
{
    public class NeopixelService: INeopixelService
    {
        private readonly string neopixelAddress;
        private readonly HttpClient httpClient = new HttpClient();
        private readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public NeopixelService(string neopixelAddress)
        {
            this.neopixelAddress = neopixelAddress;
        }

        public async Task SendNeopixelUpdate(SortedDictionary<int, string> colorDict)
        {
            var serializedDict = JsonSerializer.Serialize<SortedDictionary<int, string>>(colorDict, serializerOptions);
            var payload = new StringContent(serializedDict, Encoding.Default, "application/json");
            var response = await httpClient.PostAsync($"{this.neopixelAddress}/Neopixels/ChangePattern", payload);
        }
    }
}
