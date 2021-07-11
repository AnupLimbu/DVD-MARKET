using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DVDMarket.Models
{
    public class UserAgeModel
    {
        [Key]
        public int Id { get; set; }
        public String UserEmail { get; set; }
        public int UserAge { get; set; }
    }
}
