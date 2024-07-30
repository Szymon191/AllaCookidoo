namespace AllaCookidoo.Entities
{
    public class FeedbackEntity
    {
        public int Id { get; set; }
        public Evaluation Evaluation { get; set; }
        public string Opinion { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public int RecipeId { get; set; }
        public RecipeEntity Recipe { get; set; }

    }
    public enum Evaluation
    {
        veryBad = 1,
        Bad = 2,
        average = 3,
        good = 4,
        veryGood = 5,
    }
}
