using System;
using System.ComponentModel.DataAnnotations;

namespace DVDMarket.Models
{
    public class CastModel
    {
        [Key]
        public long CastId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime DOB { get; set; }
    }
}
