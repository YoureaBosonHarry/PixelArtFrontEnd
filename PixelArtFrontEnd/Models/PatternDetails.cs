using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PixelArtFrontEnd.Models
{
    public class PatternDetails
    {
        public Guid PatternUUID { get; set; }
        public string PatternName { get; set; }
        public int SequenceNumber { get; set; }
        public string SequenceDescription { get; set; }
        public string SequenceMetadata { get; set; }
        private Dictionary<int, string> _value;
        public Dictionary<int, string> SequenceDictionary { get => JsonSerializer.Deserialize<Dictionary<int, string>>(SequenceDescription); set => _value = value; }
    }
}
