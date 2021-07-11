using System;
using System.ComponentModel.DataAnnotations;

namespace DVDMarket.Models
{
    public class ProducerModel
    {
        [Key]
        public long ProducerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime DOB { get; set; }
    }
}
