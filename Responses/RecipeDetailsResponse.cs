using AllaCookidoo.Entities;

namespace AllaCookidoo.Responses
{
    public class RecipeDetailsResponse
    {
        public int RecipeId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Instruction { get; set; } = null!;
        public string CookTime { get; set; } = null!;
        public int CategoryId { get; set; }

        public List<FeedbackResponse> Feedbacks { get; set; }
        public List<RecipeIngredientResponse> RecipeIngredients { get; set; }
    }
}
