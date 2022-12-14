
using BulkyBook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess;
public class ApplicationDbContext :IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Comment> Comments { get; set; }


    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public DbSet<Video> videos { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Gallery> Galleries { get; set; }
    public DbSet<Upcomming> Upcommings { get; set; }
}
