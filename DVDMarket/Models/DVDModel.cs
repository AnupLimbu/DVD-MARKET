using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DVDMarket.Models
{
    public class DVDModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }                // DVD ID
        [Required]
 
        public String Title { get; set; }           // DVD URL ADDRESS
        [Required]
        public String Actor { get; set; }           // DVD ACOTR`S LASTNAME
        [Required]
        public String Producer { get; set; }    // DVD PRODUCER
        public String Url { get; set; }
        public String Cast { get; set;  }          // array ot store all the actor in the dvd
        [Required]
        public String Studio { get; set; }          // DVD STUDIO TYPE
        [Required]
        public bool Master { get; set; }            // DVD`S LOAN OR SHELF STATE ; TRUE : SHELF, FALSE : LOAN
        [Required]
        public long Copies { get; set; }            // DVD COPIES COUNT

    

        public int CopyNo { get; set; } 
        public bool NonpermAge { get; set; }        // DVD NONPERMISSION BY UNDER 18 AGES

        public DateTime Released { get; set; }      // DVD RELEASED DATE
        /// </Detail>
        public virtual ICollection<UserModel> UserModels { get; set; }
        public virtual ICollection<LoanModel> LoanModels { get; set; }
        public virtual ICollection<MemberModel> MemberModels { get; set; }
        public virtual ICollection<CategoryModel> CategoryModels { get; set; }
    }
}