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
                var serializedContent = await response.Content.ReadAsStreamAsync();
                var patterns = await JsonSerializer.DeserializeAsync<IEnumerable<PatternList>>(serializedContent, serializerOptions);
                return patterns;
            }
            return Enumerable.Empty<PatternList>();
        }

        public async Task CreatePatternAsync(PatternList patternList)
        {
            var serializedContent = JsonSerializer.Serialize(patternList, serializerOptions);
            var stringContent = new StringContent(serializedContent, Encoding.Default, mediaType: "application/json");
            var response = await httpClient.PostAsync("/Neopixels/CreatePattern", stringContent);
            if (response.IsSuccessStatusCode)
            {
                ;
            }
        }

        public async Task DeletePatternAsync(Guid patternUUID)
        {
            var queryArgs = new Dictionary<string, string>() { { "patternUUID", patternUUID.ToString() } };
            var encodedContent = new FormUrlEncodedContent(queryArgs);
            var encodedURL = $"/Neopixels/DeletePattern?{await encodedContent.ReadAsStringAsync()}";
            var response = await httpClient.DeleteAsync(encodedURL);
            if (response.IsSuccessStatusCode)
            {
                ;
            }
        }

        public async Task<IEnumerable<PatternDetails>> GetPatternDetailsByUUIDAsync(Guid patternUUID)
        {
            var queryArgs = new Dictionary<string, string>() { { "patternUUID", patternUUID.ToString()} };
            var encodedContent = new FormUrlEncodedContent(queryArgs);
            var encodedURL = $"/Neopixels/GetPatternDetails?{await encodedContent.ReadAsStringAsync()}";
            var response = await httpClient.GetAsync(encodedURL);
            if (response.IsSuccessStatusCode)
            {
                var serializedPatternDetails = await response.Content.ReadAsStreamAsync();
                var patternDetails = await JsonSerializer.DeserializeAsync<IEnumerable<PatternDetails>>(serializedPatternDetails, serializerOptions);
                patternDetails = patternDetails.OrderBy(i => i.SequenceNumber);
                return patternDetails;
            }
            return Enumerable.Empty<PatternDetails>();
        }

        public async Task AddPatternDetailsAsync(PatternDetails patternDetails)
        {
            var serializedContent = JsonSerializer.Serialize(patternDetails, serializerOptions);
            var stringContent = new StringContent(serializedContent, Encoding.Default, mediaType: "application/json");
            var response = await httpClient.PostAsync("/Neopixels/AddPatternDetails", stringContent);
            if (response.IsSuccessStatusCode)
            {
                ;
            }
        }

        public async Task UpdatePatternDetailsAsync(IEnumerable<PatternDetails> patternDetails)
        {
            var serializedContent = JsonSerializer.Serialize(patternDetails, serializerOptions);
            var stringContent = new StringContent(serializedContent, Encoding.Default, mediaType: "application/json");
            var response = await httpClient.PutAsync("/Neopixels/UpdatePatternDetails", stringContent);
            if (response.IsSuccessStatusCode)
            {
                ;
            }
        }

        public async Task ChangePatternAsync(PatternChangeRequest patternChangeRequest)
        {
            var serializedContent = JsonSerializer.Serialize(patternChangeRequest, serializerOptions);
            var stringContent = new StringContent(serializedContent, Encoding.Default, mediaType: "application/json");
            var response = await httpClient.PostAsync("/Neopixels/ChangePattern", stringContent);
            if (response.IsSuccessStatusCode)
            {
                ;
            }
        }

        public async Task DeletePatternAsync()
        {
            ;
        }

        public async Task ClearPattern()
        {
            var response = await httpClient.PostAsync("/Neopixels/ClearPixels", new StringContent(String.Empty));
            if (response.IsSuccessStatusCode)
            {
                ;
            }
        }
    }
}
