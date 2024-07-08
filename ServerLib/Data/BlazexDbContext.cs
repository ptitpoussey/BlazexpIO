using BaseLib.Entities;
using Microsoft.EntityFrameworkCore;

namespace ServerLib.Data
{
    public class BlazexDbContext(DbContextOptions<BlazexDbContext> options) : DbContext(options)
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<CategoryEntity> CategoryEntities { get; set; }
        public DbSet<ExpenseEntity> ExpenseEntities { get; set; }
        public DbSet<IncomeEntity> IncomeEntities { get; set; }
        public DbSet<RefreshTokenInfos> RefreshTokenInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring TPT mapping strategy to have separate table
            modelBuilder.Entity<CategoryEntity>()
                .ToTable("Categories");

            modelBuilder.Entity<ExpenseEntity>()
                .ToTable("Expenses") // Table for ExpenseEntity
                .HasBaseType<CategoryEntity>(); // Specify the base type
            
            modelBuilder.Entity<RefreshTokenInfos>()
                .ToTable("RefreshTokens");

            modelBuilder.Entity<IncomeEntity>()
                .ToTable("Incomes") // Table for IncomeEntity
                .HasBaseType<CategoryEntity>(); // Specify the base type

        }
    }
}
