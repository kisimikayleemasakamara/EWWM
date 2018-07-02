using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using WasteMangement.Models;

[assembly: OwinStartupAttribute(typeof(WasteMangement.Startup))]
namespace WasteMangement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                // In Startup  creating first Super Admin Role and creating a default Super Admin User    
                if (!roleManager.RoleExists("SuperAdmin"))
                {

                    // first we create Super Admin rool   
                    var role = new IdentityRole();
                    role.Name = "SuperAdmin";
                    roleManager.Create(role);

                }
            // Then creating  System Admin Role and creating a default System Admin User    
            if (!roleManager.RoleExists("SystemAdmin"))
            {

                // first we create Super Admin rool   
                var role = new IdentityRole();
                role.Name = "SystemAdmin";
                roleManager.Create(role);

                //Here we create a Super Admin super user who will maintain the website                  

                var SystemAdmin = new ApplicationUser();
                SystemAdmin.UserName = "KisimiKayleemasakamara";
                SystemAdmin.Email = "kisimikayleemasa@hotmail.com";
                SystemAdmin.FirstName = "Kisimi";
                SystemAdmin.LastName = "Kayleemasa";
                SystemAdmin.PhoneNo = "03087654657";
                SystemAdmin.YouthGroup = "";
                SystemAdmin.FacilityType = "";
                SystemAdmin.Address = "nnjkdskjcndjuchjdc";
                SystemAdmin.isDeleted = 0;
                string userPWD = "P@ssw0rd";

                var chkUser = UserManager.Create(SystemAdmin, userPWD);
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(SystemAdmin.Id, "SystemAdmin");

                }
            }

            // creating Creating DistrictAdmin role    
            if (!roleManager.RoleExists("DistrictAdmin"))
            {
                var role = new IdentityRole();
                role.Name = "DistrictAdmin";
                roleManager.Create(role);

            }

            // creating Creating Staff role    
            if (!roleManager.RoleExists("Staff"))
            {
                var role = new IdentityRole();
                role.Name = "Staff";
                roleManager.Create(role);

            }
            // creating Creating Staff role    
            if (!roleManager.RoleExists("Client"))
            {
                var role = new IdentityRole();
                role.Name = "Client";
                roleManager.Create(role);

            }
        }
    }
}
