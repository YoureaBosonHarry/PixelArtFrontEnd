using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelArtFrontEnd.Models
{
    public class PatternList
    {
        public Guid PatternUUID { get; set; } = Guid.Empty;
        public string PatternName { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
