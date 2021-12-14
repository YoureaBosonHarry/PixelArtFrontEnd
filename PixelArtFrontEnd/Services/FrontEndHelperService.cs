using PixelArtFrontEnd.Models;
using PixelArtFrontEnd.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PixelArtFrontEnd.Services
{
    public class FrontEndHelperService: IFrontEndHelperService
    {

        JsonSerializerOptions serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        public FrontEndHelperService()
        {

        }

        public string GetButtonColor(PixelPatternDetails patternDetails, int i, int j)
        {
            var trueIndex = GetMatrixPosition(i, j);
            patternDetails.PatternDetails.TryGetValue(trueIndex, out var hexColor);
            return hexColor;
        }

        public int GetMatrixPosition(int i, int j)
        {
            if (i % 2 == 0)
            {
                return Math.Abs((j + 1) - (16 * (i + 1)));
            }
            return (16 * i) + j;
        }

        public string MapDictionaryToString(SortedDictionary<int, string> colorDict)
        {
            var serialized = JsonSerializer.Serialize<SortedDictionary<int, string>>(colorDict, serializerOptions);
            return serialized;
        }
    }
}
