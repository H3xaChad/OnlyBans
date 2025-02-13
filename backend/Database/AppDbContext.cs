using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Models;
using OnlyBans.Backend.Models.Comments;
using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Rules;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, UserRole, Guid>(options) {
    
    public DbSet<Post> Posts { get; init; }
    public DbSet<Comment> Comments { get; init; }
    public DbSet<UserPostLike> UserPostLikes { get; init; }
    public DbSet<UserFollow> UserFollows { get; init; }
    public DbSet<Rule> Rules { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.HasPostgresExtension("uuid-ossp"); // Guid support in postgres
        base.OnModelCreating(modelBuilder); // Required for Identity setup

        ConfigureUserEntity(modelBuilder);
        ConfigurePostEntity(modelBuilder);
        ConfigureCommentEntity(modelBuilder);
        ConfigureRuleEntity(modelBuilder);
        ConfigureUserPostLikeEntity(modelBuilder);
        ConfigureUserFollowEntity(modelBuilder);
        ConfigureIdentityRelations(modelBuilder);
    }

    private static void ConfigureUserEntity(ModelBuilder modelBuilder) {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasMany(u => u.Posts)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasMany(u => u.LikedPosts)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigurePostEntity(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Post>()
            .Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(255);

        modelBuilder.Entity<Post>()
            .Property(p => p.Description)
            .IsRequired();

        modelBuilder.Entity<Post>()
            .HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureCommentEntity(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Comment>()
            .Property(c => c.Content)
            .IsRequired();

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureRuleEntity(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Rule>()
            .Property(r => r.Text)
            .IsRequired();

        modelBuilder.Entity<Rule>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureUserPostLikeEntity(ModelBuilder modelBuilder) {
        modelBuilder.Entity<UserPostLike>()
            .HasKey(l => new { l.UserId, l.PostId });

        modelBuilder.Entity<UserPostLike>()
            .HasOne(l => l.User)
            .WithMany(u => u.LikedPosts)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserPostLike>()
            .HasOne(l => l.Post)
            .WithMany(p => p.LikedByUsers)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureUserFollowEntity(ModelBuilder modelBuilder) {
        modelBuilder.Entity<UserFollow>()
            .HasKey(f => new { f.FollowerId, f.FollowedId });

        modelBuilder.Entity<UserFollow>()
            .HasOne(f => f.Follower)
            .WithMany()
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserFollow>()
            .HasOne(f => f.Followed)
            .WithMany()
            .HasForeignKey(f => f.FollowedId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureIdentityRelations(ModelBuilder modelBuilder) {
        modelBuilder.Entity<IdentityUserLogin<Guid>>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });

        modelBuilder.Entity<IdentityUserRole<Guid>>()
            .HasKey(r => new { r.UserId, r.RoleId });

        modelBuilder.Entity<IdentityUserToken<Guid>>()
            .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
    }
}