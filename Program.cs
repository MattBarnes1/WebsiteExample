using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApplication5.Data;

namespace WebApplication5
{
    public class Program
    {
        static UserManager<UserData> userManager;
        static RoleManager<IdentityRole> RoleManager;
        public static void Main(string[] args)
        {
            var MyBuilder = CreateWebHostBuilder(args).Build();
            var serviceProvider = MyBuilder.Services.CreateScope().ServiceProvider;
            ApplicationDbContext myContext = (ApplicationDbContext)serviceProvider.GetService<ApplicationDbContext>();
            myContext.Database.Migrate();
            RoleManager = (RoleManager<IdentityRole>)serviceProvider.GetService(typeof(RoleManager<IdentityRole>));
            userManager = (UserManager<UserData>)serviceProvider.GetService(typeof(UserManager<UserData>));
            var FileReadWriteManager = (IHostingEnvironment)serviceProvider.GetService(typeof(IHostingEnvironment));
            CheckCreateRole("Modeler", RoleManager);
            CheckCreateRole("HR", RoleManager);
            CheckCreateRole("Writer", RoleManager);
            CheckCreateRole("LeadWriter", RoleManager);
            CheckCreateRole("LeadLorewriter", RoleManager);
            CheckCreateRole("Loremaster", RoleManager);
            CheckCreateRole("LeadModeler", RoleManager);
            CheckCreateRole("TaskMaster", RoleManager);
            CheckCreateRole("VotingPermissionRevoked", RoleManager);
            CheckCreateRole("EmailPermissionRevoked", RoleManager);
           // CheckAddRole(AdminID, "TaskMaster");
           // CheckAddRole(AdminID, "LeadModeler");
           // CheckAddRole(AdminID, "LeadWriter");
           // CheckAddRole(AdminID, "Loremaster");
          //  CheckAddRole(AdminID, "HR");
           // SyncPermissions(AdminID);
            myContext.SaveChanges();
            userManager = (UserManager<UserData>)serviceProvider.GetService(typeof(UserManager<UserData>));    
            HardDriveFiles myFiles = new HardDriveFiles(FileReadWriteManager);
            FileEditingService.InitializeLookupService(myFiles, userManager);
            
            MyBuilder.Run();
        }

        private static void SyncPermissions(UserData adminID)
        {
           var Value = userManager.UpdateAsync(adminID);
            Value.Wait();
            Debug.Assert(Value.Result.Succeeded);
        }

        private static void CheckAddRole(UserData AdminID, string role)
        {
            var IsIn = userManager.IsInRoleAsync(AdminID, role);
            IsIn.Wait();
            if (IsIn.Result) return;
            var Value = userManager.AddToRoleAsync(AdminID, role);
            Value.Wait();
            Debug.Assert(Value.Result.Succeeded);
        }

        private static UserData CheckCreateAdmin(string AdminAccountName, string AdminPassword)
        {
            var Task = userManager.FindByNameAsync(AdminAccountName);
            Task.Wait();
            if (Task.Result != null)
            {
                return Task.Result;
            }
            else
            {
                return MakeAdminAccount(AdminAccountName, AdminPassword);
            }
        }

        private static UserData MakeAdminAccount(string AdminAccountName, string AdminPassword)
        {
            UserData AdminID = new UserData { Email = "NONE", UserName = AdminAccountName, SecurityStamp = Guid.NewGuid().ToString() };
            var myTask = userManager.CreateAsync(AdminID, AdminPassword);
            myTask.Wait();
            Debug.Assert(myTask.Result.Succeeded);
            return AdminID;
        }

        public static void CheckCreateRole(String RoleName, RoleManager<IdentityRole> RoleManager)
        {
            var What = RoleManager.RoleExistsAsync(RoleName);
            What.Wait();
            if (!What.Result)
            {
               var Result = RoleManager.CreateAsync(new IdentityRole(RoleName));
                Result.Wait();
                Debug.Assert(Result.Result.Succeeded);
            }
        }



        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
