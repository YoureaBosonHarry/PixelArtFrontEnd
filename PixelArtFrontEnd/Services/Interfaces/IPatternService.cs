using PixelArtFrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelArtFrontEnd.Services.Interfaces
{
    public interface IPatternService
    {
        Task<IEnumerable<AvailablePatterns>> GetAvailablePatternsAsync();
        Task<IEnumerable<PixelPatternDetails>> GetPatternDetailsByUUIDAsync(Guid patternUUID);
        Task AddPatternDetailsByUUIDAsync(PixelPatternDetailsRow patternDetailsRow);
    }
}
