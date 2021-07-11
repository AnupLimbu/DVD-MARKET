using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DVDMarket.Models
{
    public class UserModel
    {
        [Key]
        public int UserStockId { get; set; }
        public String UserId { get; set; }
        public String UserEmail { get; set; }
        public IdentityUser User { get; set; }
        public long DvdId { get; set; }
        public DVDModel Dvd { get; set; }
    }
}
