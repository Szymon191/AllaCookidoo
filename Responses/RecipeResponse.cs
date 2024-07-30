namespace AllaCookidoo.Responses
{
    public class RecipeResponse
    {
        public int RecipeId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public int CategoryId { get; set; }
    }
}
