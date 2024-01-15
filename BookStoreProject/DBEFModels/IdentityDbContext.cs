using bookStoreProject.DBEFModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using bookStoreProject.Models;
using BookStoreProject.Models;


namespace BookStoreProject.DBEFModels
{
    public class IdentityDbContext : IdentityDbContext<ApiUser>
    {

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

        public DbSet<ApiUser> ApiUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
