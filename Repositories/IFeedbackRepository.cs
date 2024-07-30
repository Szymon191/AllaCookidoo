using AllaCookidoo.Entities;

namespace AllaCookidoo.Repositories
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<FeedbackEntity>> GetFeedbacks();
        Task<FeedbackEntity> GetFeedbackById(int id);
        //Task<IEnumerable<FeedbackEntity>> GetFeedbacksFromRecipeId(int id);
        Task AddFeedback(FeedbackEntity feedback);
        Task UpdateFeedback(FeedbackEntity feedback);
    }
}
