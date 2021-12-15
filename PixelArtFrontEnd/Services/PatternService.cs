using PixelArtFrontEnd.Models;
using PixelArtFrontEnd.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace PixelArtFrontEnd.Services
{
    public class PatternService: IPatternService
    {

        private readonly string neopixelBackendURL;
        private readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private readonly HttpClient httpClient = new HttpClient();

        public PatternService(string neopixelBackendURL)
        {
            this.neopixelBackendURL = neopixelBackendURL;
            httpClient.BaseAddress = new Uri(neopixelBackendURL);
        }

        public async Task<IEnumerable<PatternList>> GetPatternListAsync()
        {
            var response = await httpClient.GetAsync("/Neopixels/GetPatternList");
            if (response.IsSuccessStatusCode)
            {
                var serializedPatterns = await response.Content.ReadAsStreamAsync();
                var patterns = await JsonSerializer.DeserializeAsync<IEnumerable<PatternList>>(serializedPatterns, serializerOptions);
                return patterns;
            }
            return Enumerable.Empty<PatternList>();
        }

        public async Task<IEnumerable<PatternDetails>> GetPatternDetailsByUUIDAsync(Guid patternUUID)
        {
            var queryArgs = new Dictionary<string, string>() { { "patternUUID", patternUUID.ToString()} };
            var encodedContent = new FormUrlEncodedContent(queryArgs);
            var encodedURL = $"/Neopixels/GetPatternDetails?{await encodedContent.ReadAsStringAsync()}";
            //var encodedURL = HttpUtility.UrlEncode(unformattedURL);
            var response = await httpClient.GetAsync(encodedURL);
            if (response.IsSuccessStatusCode)
            {
                var serializedPatternDetails = await response.Content.ReadAsStreamAsync();
                var patternDetails = await JsonSerializer.DeserializeAsync<IEnumerable<PatternDetails>>(serializedPatternDetails, serializerOptions);
                return patternDetails;
            }
            return Enumerable.Empty<PatternDetails>();
        }
    }
}
