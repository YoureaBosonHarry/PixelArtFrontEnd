using Microsoft.AspNetCore.Components;
using PixelArtFrontEnd.Models;
using PixelArtFrontEnd.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelArtFrontEnd.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        protected IPatternService patternService { get; set; }
        protected PatternDetails currentPatternSequenceDetails;
        protected IEnumerable<PatternDetails> patternDetails;
        protected IEnumerable<PatternList> availablePatterns = new List<PatternList>();
        protected PatternList selectedPattern = new PatternList();

        public Index()
        {
            
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

        protected void OnSequenceChange(int sequenceNumber)
        {
            currentPatternSequenceDetails = patternDetails.Where(i => i.SequenceNumber == sequenceNumber).FirstOrDefault();
        }

        protected async Task StartPattern()
        {
            var patternRequest = new PatternChangeRequest() { PatternUUID = currentPatternSequenceDetails.PatternUUID };
            await patternService.ChangePatternAsync(patternRequest);
        }

        protected async void StopPattern()
        {
            await patternService.ClearPattern();
        }
    }
}
