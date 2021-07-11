using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DVDMarket.Models
{
    public class LoanModel
    {
        [Key]
        public long Id { get; set; }
        public long LoanId { get; set; }
        [Required]
        public bool BorrowType { get; set; }
        [Required]
        public DateTime BorrowDate { get; set; }
        [Required]
        public DateTime ExceptDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public long DvdId { get; set; }
        public DVDModel Dvd { get; set; }
        [Required]
        public string MemberEmail { get; set; }      
        public int Fine { get; set; }
        public MemberModel Memb { get; set; }

        public virtual ICollection<UserModel> UserModels { get; set; }
    }
}
