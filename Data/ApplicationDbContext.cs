using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Areas.Identity.Pages.Application;
using WebApplication5.Areas.Identity.Pages.Application.SQL;

namespace WebApplication5.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserData>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {


        }
        public DbSet<Lore> myLore { get; set; }
        public DbSet<TaskData> myTasks { get; set; }
        public DbSet<EventData> myGlobalEvents { get; set; }
        public DbSet<Email> myEmails { get; set; }
        public DbSet<Poll> myPolls { get; set; }
        public DbSet<BlogItem> myBlogs { get; set; }
        public DbSet<ScreenShots> myScreenShots { get; set; }
        public DbSet<Request> myRequests { get; set; }
    }
}
