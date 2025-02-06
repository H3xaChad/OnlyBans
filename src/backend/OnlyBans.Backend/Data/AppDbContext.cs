using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Models;
using OnlyBans.Backend.Models.Comments;
using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Rules;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, UserRole, Guid>(options) {
    
    public DbSet<Post> Posts { get; init; }
    public DbSet<Comment> Comments { get; init; }
    public DbSet<UserPostLike> UserPostLikes { get; init; }
    
    public DbSet<UserFollow> UserFollows { get; init; }
    public DbSet<Rule> Rules { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.HasPostgresExtension("uuid-ossp");
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<UserPostLike>()
            .HasKey(l => new { l.UserId, l.PostId });
        
        modelBuilder.Entity<IdentityUserLogin<Guid>>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });
        
        modelBuilder.Entity<IdentityUserRole<Guid>>()
            .HasKey(r => new { r.UserId, r.RoleId });
        
        modelBuilder.Entity<IdentityUserToken<Guid>>()
            .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
    }
}