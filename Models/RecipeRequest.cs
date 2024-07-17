namespace AllaCookidoo.Models
{
    public class RecipeRequest
    {
        public int RecipeId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Instruction { get; set; } = null!;
        public string CookTime { get; set; } = null!;
        public int CategoryId { get; set; }
    }
}
