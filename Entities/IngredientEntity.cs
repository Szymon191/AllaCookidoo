namespace AllaCookidoo.Entities
{
    public class IngredientEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public ICollection<RecipeIngredientEntity> RecipeIngredients { get; set; }
    }
}
