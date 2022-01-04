using PixelArtFrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelArtFrontEnd.Services.Interfaces
{
    public interface IPatternService
    {
        Task<IEnumerable<PatternList>> GetPatternListAsync();
        Task<IEnumerable<PatternDetails>> GetPatternDetailsByUUIDAsync(Guid patternUUID);
        Task UpdatePatternDetailsAsync(IEnumerable<PatternDetails> patternDetails);
        Task ChangePatternAsync(PatternChangeRequest patternChangeRequest);
        Task ClearPattern();
    }
}
