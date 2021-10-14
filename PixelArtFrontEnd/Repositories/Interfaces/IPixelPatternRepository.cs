using PixelArtFrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelArtFrontEnd.Repositories.Interfaces
{
    public interface IPixelPatternRepository
    {
        Task<IEnumerable<AvailablePatterns>> GetAvailablePatternsAsync();
        Task AddPatternDetails(PixelPatternDetailsRow patternDetails);
        Task<IEnumerable<PixelPatternDetails>> GetPatternDetailsAsync(Guid patternUUID);
    }
}
