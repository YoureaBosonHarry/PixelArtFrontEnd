using PixelArtFrontEnd.Models;
using PixelArtFrontEnd.Repositories.Interfaces;
using PixelArtFrontEnd.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PixelArtFrontEnd.Services
{
    public class PatternService: IPatternService
    {
        private readonly IPixelPatternRepository patternRepository;
        private readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public PatternService(IPixelPatternRepository patternRepository)
        {
            this.patternRepository = patternRepository;
        }

        public async Task<IEnumerable<AvailablePatterns>> GetAvailablePatternsAsync()
        {
            var availablePatterns = await this.patternRepository.GetAvailablePatternsAsync();
            return availablePatterns;
        }

        public async Task<IEnumerable<PixelPatternDetails>> GetPatternDetailsByUUIDAsync(Guid patternUUID)
        {
            var patternDetails = await patternRepository.GetPatternDetailsAsync(patternUUID);
            return patternDetails;
            
        }

        public async Task AddPatternDetailsByUUIDAsync(PixelPatternDetailsRow patternDetailsRow)
        {
            await this.patternRepository.AddPatternDetails(patternDetailsRow);
        }
    }
}
