using Library.Contracts.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Domain.Database;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }

    public LibraryDbContext() : base()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "LibraryDb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(etb =>
        {
            etb.Property(e => e.Id);
            etb.HasKey(e => e.Id);
        });
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Review> Reviews { get; set; }
}