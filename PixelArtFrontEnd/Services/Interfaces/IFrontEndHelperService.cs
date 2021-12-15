using PixelArtFrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelArtFrontEnd.Services.Interfaces
{
    public interface IFrontEndHelperService
    {
        string GetButtonColor(PatternDetails patternDetails, int i, int j);
        int GetMatrixPosition(int i, int j);
        string MapDictionaryToString(SortedDictionary<int, string> colorDict);
    }
}
