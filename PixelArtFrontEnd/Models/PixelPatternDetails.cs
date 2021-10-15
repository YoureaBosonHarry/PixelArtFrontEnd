using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelArtFrontEnd.Models
{
    public class PixelPatternDetails
    {
        public Guid PatternUUID { get; set; }
        public string PatternName { get; set; }
        public int PatternSequenceNumber { get; set; }
        public SortedDictionary<int, string> PatternDetails { get; set; }

        public PixelPatternDetails()
        {
            PatternDetails = new SortedDictionary<int, string>() { };
        }
    }
}
