using AllaCookidoo.Entities;

namespace AllaCookidoo.Responses
{
    public class FeedbackResponse
    {
        public int Id { get; set; }

        public Evaluation Evaluation { get; set; }

        public string Opinion { get; set; } 

        public int RecipeId { get; set; }
    }
}
