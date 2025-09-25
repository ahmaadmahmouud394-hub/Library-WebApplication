using Library_WebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library_WebApplication.Models
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Pubblisher> Publishers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Tipology> Tipologys { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Tipology)
                .WithMany(t => t.Books)
                .HasForeignKey(b => b.IdTipology)
                 ; // Optional    // FK in Employee table

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(t => t.Books)
                .HasForeignKey(e => e.IdAuthor)
                ;

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Pubblisher)
                .WithMany(t => t.Books)
                .HasForeignKey(e => e.IdPubblisher)
                ;

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.User)
                .WithMany(u => u.Invoices)
                .HasForeignKey(e => e.IdUser)
                ;

            modelBuilder.Entity<Invoice>()
                .HasOne(b => b.Book)
                .WithMany(t => t.Invoices)
                .HasForeignKey(e => e.IdBook)
               ;


            modelBuilder.Entity<User>()
                .HasOne(b => b.Role)
                .WithMany(t => t.Users)
                .HasForeignKey(b => b.IdRole)
                ;
            base.OnModelCreating(modelBuilder);
        }
    }
}
