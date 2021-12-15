using Microsoft.AspNetCore.Components;
using PixelArtFrontEnd.Models;
using PixelArtFrontEnd.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelArtFrontEnd.Components
{
    public class IndexComponent : ComponentBase
    {
        protected readonly IPatternService patternService;
        protected PatternDetails currentPatternSequenceDetails;
        protected IEnumerable<PatternDetails> patternDetails;
        protected IEnumerable<PatternList> availablePatterns = new List<PatternList>();
        protected PatternList selectedPattern = new PatternList();

        public IndexComponent(IPatternService patternService)
        {
            this.patternService = patternService;
        }

        protected override async Task OnInitializedAsync()
        {
            availablePatterns = await patternService.GetPatternListAsync();
        }

        protected async Task GetPatternDetailsByUUIDAsync()
        {
            patternDetails = await patternService.GetPatternDetailsByUUIDAsync(selectedPattern.PatternUUID);
            currentPatternSequenceDetails = patternDetails.First();
        }

        protected void StartPattern()
        {

        }

        protected void StopSending()
        {

        }
    }
}
