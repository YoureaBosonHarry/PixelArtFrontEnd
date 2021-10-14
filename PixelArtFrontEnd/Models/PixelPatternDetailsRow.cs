using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelArtFrontEnd.Models
{
    public class PixelPatternDetailsRow
    {
        public Guid PatternUUID { get; set; }
        public string PatternName { get; set; }
        public int PatternSequenceNumber { get; set; }
        public string PatternDetails { get; set; }
    }
}
