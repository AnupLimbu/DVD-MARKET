using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DVDMarket.Models
{
    public class CategoryModel
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Limit { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public int blank{ get; set; }
    }
}