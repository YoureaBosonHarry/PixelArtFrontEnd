using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PixelArtFrontEnd.Models
{
    public class PatternList
    {
        public Guid PatternUUID { get; set; } = Guid.Empty;
        [Required]
        [Display(Name = "Pattern Name")]
        [StringLength(64, ErrorMessage = "Pattern name length can't be more than 64.")]
        public string PatternName { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
