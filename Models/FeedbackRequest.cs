using AllaCookidoo.Entities;

namespace AllaCookidoo.Models
{
    public class FeedbackRequest
    {
        public int Id { get; set; }
        public Evaluation Evaluation { get; set; }
        
        public string Opinion { get; set; }

        public int RecipeId { get; set; }
    }
}
