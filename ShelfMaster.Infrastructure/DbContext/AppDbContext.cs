using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShelfMaster.Domain.Entities;
using ShelfMaster.Infrastructure.Entities;

namespace ShelfMaster.Infrastructure.DbContext;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Loan> Loans { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<IdentityUserRole<int>>().HasKey(x => new { x.UserId, x.RoleId });
        builder.Entity<IdentityUserToken<int>>().HasKey(x => x.UserId);
        builder.Entity<IdentityUserLogin<int>>().HasKey(x => x.UserId);

        builder.Entity<Loan>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
