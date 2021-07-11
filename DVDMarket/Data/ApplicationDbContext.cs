using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DVDMarket.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DVDMarket.Models.DVDModel> DVDModel { get; set; }
        public DbSet<DVDMarket.Models.UserModel> UserModel { get; set; }
        public DbSet<DVDMarket.Models.UserAgeModel> UserAgeModel { get; set; }
        public DbSet<DVDMarket.Models.LoanModel> LoanModel { get; set; }
        public DbSet<DVDMarket.Models.MemberModel> MemberModel { get; set; }
        public DbSet<DVDMarket.Models.CategoryModel> CategoryModel { get; set; }
        public DbSet<DVDMarket.Models.CastModel> CastModel { get; set; }
        public DbSet<DVDMarket.Models.ProducerModel> ProducerModel { get; set; }
    }
}
