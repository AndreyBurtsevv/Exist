using Exist.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Exist.DB
{
    public class MyDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        public DbSet<Company> Company { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Detail> Detail { get; set; }
        public DbSet<Group> Group { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>()
                    .HasMany(c => c.Groups)
                    .WithMany(s => s.Companys)
                    .UsingEntity(j => j.ToTable("GroupsCompanys"));

            modelBuilder.Entity<Company>()
                    .HasMany(x => x.Details)
                    .WithOne(x => x.Company)
                    .HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
////dotnet ef migrations add Create --project "C:\Users\Backend\source\repos\Exist" --context MyDbContext   
////dotnet ef migrations update Create --project "C:\Users\Backend\source\repos\Exist" --context MyDbContext
////dotnet ef database update