using AllaCookidoo.Entities;
using AllaCookidoo.Models;
using Microsoft.EntityFrameworkCore;


namespace AllaCookidoo.Database
{
    public class AllaCookidoDatabaseContext : DbContext
    {
        public virtual DbSet<RecipeEntity> Recipes { get; set; }
        public virtual DbSet<CategoryEntity> Categories { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }

        public AllaCookidoDatabaseContext(DbContextOptions<AllaCookidoDatabaseContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeEntity>().HasKey(e => new {e.Id});
            modelBuilder.Entity<CategoryEntity>().HasKey(e => new {e.Id});
            
            modelBuilder.Entity<RecipeEntity>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Recipe)
                .HasForeignKey(x => x.CategoryId)
                .IsRequired();
        }
    }
}
