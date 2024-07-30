using AllaCookidoo.Entities;
using AllaCookidoo.Models;
using AllaCookidoo.Responses;

namespace AllaCookidoo.Services
{
    public interface IFeedbackService
    {
        Task<IEnumerable<FeedbackResponse>> GetFeedbacks();
        Task<FeedbackResponse> GetFeedbackById(int id);
        Task AddFeedback(FeedbackRequest feedbackCreation);
        Task UpdateFeedback(int id, FeedbackResponse feedbackUpdate);
        Task DeleteFeedback(int id);
    }
}
