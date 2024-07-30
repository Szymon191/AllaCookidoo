namespace AllaCookidoo.Entities
{
    public class RecipeIngredientEntity
    {
        public int RecipeIngredientId { get; set; }
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public string Amount { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public RecipeEntity Recipe { get; set; }
        public IngredientEntity Ingredient { get; set; }
    }
}
