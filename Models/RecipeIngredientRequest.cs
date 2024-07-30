namespace AllaCookidoo.Models
{
    public class RecipeIngredientRequest
    {
        public int RecipeIngredientId { get; set; }
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public string Amount { get; set; }
    }
}
