using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace AWING.TreasureHuntAPI.Models;

public partial class TreasureHuntDbContext : DbContext
{
    public TreasureHuntDbContext()
    {
    }

    public TreasureHuntDbContext(DbContextOptions<TreasureHuntDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TreasureCell> TreasureCells { get; set; }

    public virtual DbSet<TreasureMap> TreasureMaps { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseMySql("server=145.14.158.43;user id=awing;port=3306;password=Qwer123@123;database=treasure_hunt_db", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.39-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("PRIMARY");

            entity.ToTable("results");

            entity.HasIndex(e => e.MapId, "map_id");

            entity.Property(e => e.ResultId).HasColumnName("result_id");
            entity.Property(e => e.CalculatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("calculated_at");
            entity.Property(e => e.FuelUsed)
                .HasPrecision(10, 5)
                .HasColumnName("fuel_used");
            entity.Property(e => e.MapId).HasColumnName("map_id");

            entity.HasOne(d => d.TreasureMap).WithMany(p => p.Results)
                .HasForeignKey(d => d.MapId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("results_ibfk_1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.HasIndex(e => e.RoleName, "role_name").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<TreasureCell>(entity =>
        {
            entity.HasKey(e => e.CellId).HasName("PRIMARY");

            entity.ToTable("treasure_cells");

            entity.HasIndex(e => e.MapId, "map_id");

            entity.Property(e => e.CellId).HasColumnName("cell_id");
            entity.Property(e => e.ChestNumber).HasColumnName("chest_number");
            entity.Property(e => e.ColNum).HasColumnName("col_num");
            entity.Property(e => e.MapId).HasColumnName("map_id");
            entity.Property(e => e.RowNum).HasColumnName("row_num");

            entity.HasOne(d => d.TreasureMap).WithMany(p => p.TreasureCells)
                .HasForeignKey(d => d.MapId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("treasure_cells_ibfk_1");
        });

        modelBuilder.Entity<TreasureMap>(entity =>
        {
            entity.HasKey(e => e.MapId).HasName("PRIMARY");

            entity.ToTable("treasure_maps");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.MapId).HasColumnName("map_id");
            entity.Property(e => e.ColsCount).HasColumnName("cols_count");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.P).HasColumnName("p");
            entity.Property(e => e.RowsCount).HasColumnName("rows_count");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.TreasureMaps)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("treasure_maps_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Username).HasColumnName("username");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("user_roles_ibfk_2"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("user_roles_ibfk_1"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("user_roles");
                        j.HasIndex(new[] { "RoleId" }, "role_id");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("RoleId").HasColumnName("role_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
