using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DVDMarket.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DVDMarket.Models;

namespace DVDMarket
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateRolesandUsers();
        }

        // In this method we will create default User roles and Admin user for login  
        private void CreateRolesandUsers()
        {
            //DefaultConnection
            var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            ApplicationDbContext context = new ApplicationDbContext(optionBuilder.Options);
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context), null, null, null, null);
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
            var userHash = new PasswordHasher<ApplicationUser>();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore, null, userHash, null, null, null, null, null, null);

            // In Startup I am creating first Admin Role and creating a default Admin User   
            Task<bool> isExist = roleManager.RoleExistsAsync("Admin");

            if (!isExist.Result)
            {
                // first we create Admin role   
                var role = new IdentityRole
                {
                    Name = "Admin"
                };
                roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website                 

                var user = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com"
                };

                string userPWD = "adminADMIN123!@#";

                userManager.CreateAsync(user, userPWD);
                //var newPasswordHash = userManager.PasswordHasher.HashPassword(user, userPWD);

                //userStore.SetPasswordHashAsync(user, newPasswordHash);
                //Add default User to Role Admin  
                userManager.AddToRoleAsync(user, "Admin");
            }

            isExist = roleManager.RoleExistsAsync("Assis");
            // creating Creating Employee role   
            if (!isExist.Result)
            {
                var role = new IdentityRole
                {
                    Name = "Assis"
                };

                roleManager.CreateAsync(role);
            }

            isExist = roleManager.RoleExistsAsync("User");
            // creating Creating Employee role   
            if (!isExist.Result)
            {
                var role = new IdentityRole
                {
                    Name = "User"
                };

                roleManager.CreateAsync(role);
            }

            InitializeContextDB(context);
        }

        private void InitializeContextDB(ApplicationDbContext context)
        {
            if (!context.CategoryModel.Select(s=>s.Name == "Basic").Any())
            {
                CategoryModel categoryModel = new CategoryModel();
                categoryModel.Name = "Basic";
                categoryModel.Description = "The most basic package";
                categoryModel.Limit = 3;
                categoryModel.Duration = 7;
                context.Add(categoryModel);

                context.SaveChangesAsync();
            }
        }
    }

    internal class ApplicationUser : IdentityUser
    {
    }
}
