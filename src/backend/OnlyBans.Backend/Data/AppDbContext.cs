using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Models;
using OnlyBans.Backend.Models.Comments;
using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options) {
    
    public DbSet<User> Users { get; init; }
    public DbSet<Post> Posts { get; init; }
    public DbSet<Comment> Comments { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.HasPostgresExtension("uuid-ossp");
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<UserPostLike>()
            .HasKey(l => new { l.UserId, l.PostId });
    }
}