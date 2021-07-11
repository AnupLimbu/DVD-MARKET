using System;
using System.ComponentModel.DataAnnotations;

namespace DVDMarket.Models
{
    public class MemberModel
    {
        [Key]
        public int MemberStockId { get; set; }
        public String MemberName { get; set; }
        public String MemberEmail { get; set; }
        public String Permission { get; set; }
        public String Category { get; set; }
    }
}
