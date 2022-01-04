﻿using Microsoft.AspNetCore.Components;
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
        protected bool showPatternModal = false;
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
            currentPatternSequenceDetails = patternDetails.First();
            copyPatternSequence = new Dictionary<int, string>(currentPatternSequenceDetails.SequenceDictionary);
        }

        protected void OnSequenceChange(int sequenceNumber)
        {
            currentPatternSequenceDetails = patternDetails.Where(i => i.SequenceNumber == sequenceNumber).FirstOrDefault();
            copyPatternSequence = new Dictionary<int, string>(currentPatternSequenceDetails.SequenceDictionary);
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

        protected async Task UpdatePattern()
        {
            await patternService.UpdatePatternDetailsAsync(patternDetails);
        }
        protected void OpenModal(int i, int j)
        {
            currentI = i;
            currentY = j;
            isOpened = true;
        }

        protected void ClosedEvent(string value)
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

        protected void OpenModal()
        {
            showPatternModal = true;
        }

        protected void CloseModal()
        {
            showPatternModal = false;
        }

        protected void HandleValidCreate()
        {
            ;
        }
    }
}
