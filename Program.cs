using ClinicAppointmentSystemApp.Context;
using ClinicAppointmentSystemApp.Models;
using ClinicAppointmentSystemApp.Repository;
using ClinicAppointmentSystemApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointmentSystemApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
                options.Cookie.HttpOnly = true; // Make the session cookie HTTP-only
                options.Cookie.IsEssential = true; // Mark the session cookie as essential
            });

            // Add database connection 
            string connection = builder.Configuration.GetConnectionString("LocalDatabaseConnection");
            builder.Services.AddDbContext<ClinicDbContext>(option => option.UseSqlServer(connection));

            // Add Identity Service

            builder.Services.AddIdentity<User, IdentityRole>(option =>
            {
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequiredLength = 8;
                option.Password.RequireUppercase = false;
                option.Password.RequireLowercase = false;
                option.User.RequireUniqueEmail = true;
                option.SignIn.RequireConfirmedAccount = false;
                option.SignIn.RequireConfirmedEmail = false;
                option.SignIn.RequireConfirmedPhoneNumber = false;

            })
                .AddEntityFrameworkStores<ClinicDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<IAppointmentRepository , AppointmentRepository>();
            builder.Services.AddScoped<IAppointmentService , AppointmentService>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();

            //// for idetity first create this services
            //builder.Services.AddAuthorization();
            //builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);

            // builder.Services.AddIdentityCore < here we have to pass the identity user from DbContext>()
            //builder.Services.AddIdentityCore<>();



            var app = builder.Build();


            // Seed roles and admin user
            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                await SeedRolesAndAdmin(serviceProvider); // Await the async method
            }


             
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // use middleware
            app.UseAuthentication();

            app.UseSession(); 

            app.UseRouting();

            app.UseAuthorization();
           

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        public static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            string[] roleNames = { "Patient", "Doctor", "Admin" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 🔹 Create Default Admin User
            string adminEmail = "admin@example.com";
            string adminPassword = "Admin@123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, adminPassword);
                //Console.WriteLine("Admin user created and role assigned.");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                    Console.WriteLine("**********************************Admin user created and role assigned.");
                }
                else
                {
                    Console.WriteLine("Admin user already exists.");
                }
            }
        }

    }
}