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
        Task CreatePatternAsync(PatternList patternList);
        Task<IEnumerable<PatternDetails>> GetPatternDetailsByUUIDAsync(Guid patternUUID);
        Task AddPatternDetailsAsync(PatternDetails patternDetails);
        Task UpdatePatternDetailsAsync(IEnumerable<PatternDetails> patternDetails);
        Task ChangePatternAsync(PatternChangeRequest patternChangeRequest);
        Task ClearPattern();
    }
}
