using AllaCookidoo.Entities;
using AllaCookidoo.Models;
using Microsoft.EntityFrameworkCore;


namespace AllaCookidoo.Database
{
    public class AllaCookidoDatabaseContext : DbContext
    {
        public virtual DbSet<RecipeEntity> Recipes { get; set; }
        public virtual DbSet<CategoryEntity> Categories { get; set; }
        public virtual DbSet<FeedbackEntity> Feedbacks { get; set; }
        public virtual DbSet<IngredientEntity> Ingredients { get; set; }
        public virtual DbSet<RecipeIngredientEntity> RecipeIngredients { get; set; }

        public AllaCookidoDatabaseContext(DbContextOptions<AllaCookidoDatabaseContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeEntity>().HasKey(e => new {e.Id});
            modelBuilder.Entity<CategoryEntity>().HasKey(e => new {e.Id});
            modelBuilder.Entity<FeedbackEntity>().HasKey(e => new { e.Id });
            modelBuilder.Entity<IngredientEntity>().HasKey(e => new { e.Id });
            modelBuilder.Entity<RecipeIngredientEntity>().HasKey(e => e.RecipeIngredientId);
            
            
            modelBuilder.Entity<RecipeEntity>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Recipe)
                .HasForeignKey(x => x.CategoryId)
                .IsRequired();

            modelBuilder.Entity<FeedbackEntity>()
                .HasOne(x => x.Recipe)
                .WithMany(x => x.Feedbacks)
                .HasForeignKey(x => x.RecipeId)
                .IsRequired();

            modelBuilder.Entity<RecipeIngredientEntity>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId);

            modelBuilder.Entity<RecipeIngredientEntity>()
                .HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientId);

        }
    }
}
