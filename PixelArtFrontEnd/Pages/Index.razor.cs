using Microsoft.AspNetCore.Components;
using PixelArtFrontEnd.Models;
using PixelArtFrontEnd.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PixelArtFrontEnd.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        protected IPatternService patternService { get; set; }
        protected static PatternDetails currentPatternSequenceDetails;
        protected Dictionary<int, string> copyPatternSequence = new Dictionary<int, string>();
        protected static IEnumerable<PatternDetails> patternDetails;
        protected IEnumerable<PatternList> availablePatterns = new List<PatternList>();
        protected PatternList selectedPattern = new PatternList();
        JsonSerializerOptions serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        protected static bool isOpened = false;
        protected static string color = "#F1F7E9";
        protected static int currentI = 0;
        protected static int currentY = 0;
        protected PatternList newPattern = new PatternList();
        protected PatternList deletePattern = new PatternList();
        protected bool showPatternModal = false;
        protected bool showDeleteButton = false;
        protected bool showDeleteModal = false;
        protected string deleteTextColor = "black";
        protected override async Task OnInitializedAsync()
        {
            availablePatterns = await patternService.GetPatternListAsync();
            for (int i = 0; i < 256; ++i)
            {
                copyPatternSequence.Add(i, "#000000");
            }
        }

        protected async Task GetPatternDetailsByUUIDAsync()
        {
            patternDetails = await patternService.GetPatternDetailsByUUIDAsync(selectedPattern.PatternUUID);
            showDeleteButton = true;
            if (patternDetails.Any())
            {
                currentPatternSequenceDetails = patternDetails.First();
                copyPatternSequence = new Dictionary<int, string>(currentPatternSequenceDetails.SequenceDictionary);
            }
            else
            {
                currentPatternSequenceDetails = new PatternDetails() { PatternUUID = selectedPattern.PatternUUID, PatternName = availablePatterns.Where(i => i.PatternUUID == selectedPattern.PatternUUID).Single().PatternName, SequenceDescription = JsonSerializer.Serialize(copyPatternSequence), SequenceNumber = 1 };
                patternDetails = new List<PatternDetails>() { currentPatternSequenceDetails };
                await this.patternService.AddPatternDetailsAsync(currentPatternSequenceDetails);
            }
        }

        protected void OnSequenceChange(int sequenceNumber)
        {
            currentPatternSequenceDetails = patternDetails.Where(i => i.SequenceNumber == sequenceNumber).FirstOrDefault();
            copyPatternSequence = new Dictionary<int, string>(currentPatternSequenceDetails.SequenceDictionary);
        }

        protected void AddSequence()
        {
            var newPatternDetails = new PatternDetails()
            { 
                PatternUUID = selectedPattern.PatternUUID, 
                PatternName = availablePatterns.Where(i => i.PatternUUID == selectedPattern.PatternUUID).Single().PatternName, 
                SequenceDescription = JsonSerializer.Serialize(copyPatternSequence), 
                SequenceNumber = currentPatternSequenceDetails.SequenceNumber + 1
            };
            var tempPatternDetails = patternDetails.ToList();
            tempPatternDetails.Add(newPatternDetails);
            patternDetails = tempPatternDetails;
            this.patternService.AddPatternDetailsAsync(newPatternDetails);

        }

        protected async Task StartPattern()
        {
            var patternRequest = new PatternChangeRequest() { PatternUUID = currentPatternSequenceDetails.PatternUUID };
            await patternService.ChangePatternAsync(patternRequest);
        }

        protected async Task StopPattern()
        {
            await patternService.ClearPattern();
        }

        protected async Task UpdatePattern()
        {
            await patternService.UpdatePatternDetailsAsync(patternDetails);
        }
        protected void OpenColorPickerModal(int i, int j)
        {
            currentI = i;
            currentY = j;
            isOpened = true;
        }

        protected async Task ClosedEvent(string value)
        {
            color = value;
            var trueIndex = GetMatrixPosition(currentI, currentY);
            if (currentPatternSequenceDetails.SequenceDictionary.TryGetValue(trueIndex, out var hexColor))
            {
                copyPatternSequence[trueIndex] = color;
                var newStringSequence = JsonSerializer.Serialize(copyPatternSequence);
                currentPatternSequenceDetails.SequenceDescription = newStringSequence;
                patternDetails.Where(i => i.SequenceNumber == currentPatternSequenceDetails.SequenceNumber).First().SequenceDescription = newStringSequence;
            }
            await Task.Run(() => patternService.UpdatePatternDetailsAsync(patternDetails));
            isOpened = false;
        }

        protected string GetButtonColor(PatternDetails patternDetails, int i, int j)
        {
            var trueIndex = GetMatrixPosition(i, j);
            patternDetails.SequenceDictionary.TryGetValue(trueIndex, out var hexColor);
            return hexColor;
        }

        protected static int GetMatrixPosition(int i, int j)
        {
            if (i % 2 == 0)
            {
                return Math.Abs((j + 1) - (16 * (i + 1)));
            }
            return (16 * i) + j;
        }

        protected string MapDictionaryToString(SortedDictionary<int, string> colorDict)
        {
            var serialized = JsonSerializer.Serialize<SortedDictionary<int, string>>(colorDict, serializerOptions);
            return serialized;
        }

        protected void OpenNewPatternModal()
        {
            showPatternModal = true;
        }

        protected void OpenDeletePatternModal()
        {
            deletePattern = selectedPattern;
            showDeleteModal = true;
        }

        protected void CloseModal()
        {
            showPatternModal = false;
        }

        protected void CloseDeleteModal()
        {
            showDeleteModal = false;
        }

        protected async Task HandleValidCreate()
        {
            CloseModal();
            newPattern.PatternUUID = Guid.NewGuid();
            await Task.Run(() => this.patternService.CreatePatternAsync(newPattern));
            newPattern = new PatternList();
            availablePatterns = await Task.Run(() => patternService.GetPatternListAsync());

        }

        protected async Task HandleValidDelete()
        {
            if (deletePattern.PatternName.ToLower() == currentPatternSequenceDetails.PatternName.ToLower())
            {
                await Task.Run(() => patternService.DeletePatternAsync(deletePattern.PatternUUID));
                availablePatterns = await Task.Run(() => patternService.GetPatternListAsync());
                selectedPattern = availablePatterns.Last();
                await Task.Run(() => GetPatternDetailsByUUIDAsync());
                CloseDeleteModal();
                deletePattern = new PatternList();
            }
            else
            {
                deleteTextColor = "red";
            }
        }
    }
}
