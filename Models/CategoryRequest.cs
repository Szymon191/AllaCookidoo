namespace AllaCookidoo.Models
{
    public class CategoryRequest
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
