namespace AllaCookidoo.Entities
{
    public class RecipeEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public string Instruction { get; set; } = null!;
        public string CookTime { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; } = null!;

    }
}
