using AllaCookidoo.Entities;
using AllaCookidoo.Models;
using AllaCookidoo.Repositories;
using AllaCookidoo.Responses;

namespace AllaCookidoo.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ILogger<FeedbackService> _logger;
        private readonly IRecipeRepository _recipeRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository, ILogger<FeedbackService> logger, IRecipeRepository recipeRepository)
        {
            _feedbackRepository = feedbackRepository;
            _logger = logger;
            _recipeRepository = recipeRepository;
        }

        public async Task<IEnumerable<FeedbackResponse>> GetFeedbacks()
        {
            _logger.LogInformation("Fetching all feedbacks");
            try
            {
                var feedbacks = await _feedbackRepository.GetFeedbacks();
                _logger.LogDebug("Found {Count} feedbacks", feedbacks.Count());
                return feedbacks.Select(feedback => new FeedbackResponse
                {
                    Id = feedback.Id,
                    Opinion = feedback.Opinion,
                    Evaluation = feedback.Evaluation,
                    RecipeId = feedback.RecipeId,
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all feedbacks");
                throw;
            }
        }

        public async Task<FeedbackResponse> GetFeedbackById(int id)
        {
            _logger.LogInformation("Fetching feedback with ID: {feedbackId}", id);
            try
            {
                var feedback = await _feedbackRepository.GetFeedbackById(id);
                if (feedback == null || feedback.IsDeleted)
                {
                    _logger.LogWarning("Feedback with ID {feedbackId} was not found or has been deleted", id);
                    return null;
                }

                _logger.LogDebug("Found feedback with ID {feedbackId}", id);
                return new FeedbackResponse
                {
                    Id=feedback.Id,
                    Evaluation = feedback.Evaluation,
                    Opinion=feedback.Opinion,
                    RecipeId=feedback.RecipeId,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching feedback with ID {feedbackId}", id);
                throw;
            }
        }

        public async Task AddFeedback(FeedbackRequest feedbackCreation)
        {
            var recipeExists = await _recipeRepository.GetRecipeById(feedbackCreation.RecipeId);
            if (recipeExists == null)
            {
                _logger.LogWarning("Recipe with ID {Id} does not exist", feedbackCreation.RecipeId);
                throw new ArgumentException($"Recipe with ID {feedbackCreation.RecipeId} does not exist.");
            }
            _logger.LogInformation("Adding new feedback: {feedbackOpinion}", feedbackCreation.Opinion);
            try
            {
                var feedbackEntity = new FeedbackEntity
                {
                    Id = feedbackCreation.Id,
                    Evaluation=feedbackCreation.Evaluation,
                    Opinion = feedbackCreation.Opinion,
                    RecipeId=feedbackCreation.RecipeId,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                };

                await _feedbackRepository.AddFeedback(feedbackEntity);
                _logger.LogDebug("Added new feedback with ID {feedbackId}", feedbackCreation.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new feedback: {feedbackOpinion}", feedbackCreation.Opinion);
                throw;
            }
        }

        public async Task UpdateFeedback(int id, FeedbackResponse feedbackUpdate)
        {
            _logger.LogInformation("Updating feedback with ID: {feedbackId}", id);
            try
            {
                var feedbackEntity = await _feedbackRepository.GetFeedbackById(id);
                if (feedbackEntity == null)
                {
                    _logger.LogWarning("Feedback with ID {feedbackId} was not found", id);
                    throw new KeyNotFoundException("Feedback not found");
                }

                feedbackEntity.Opinion = feedbackUpdate.Opinion;
                feedbackEntity.Evaluation = feedbackUpdate.Evaluation;
                feedbackEntity.UpdatedDate = DateTime.UtcNow;

                await _feedbackRepository.UpdateFeedback(feedbackEntity);
                _logger.LogDebug("Updated feedback with ID {feedbackId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating feedback with ID {feedbackId}", id);
                throw;
            }
        }

        public async Task DeleteFeedback(int id)
        {
            _logger.LogInformation("Deleting feedback with ID: {feedbackId}", id);
            try
            {
                var feedbackEntity = await _feedbackRepository.GetFeedbackById(id);
                if (feedbackEntity == null)
                {
                    _logger.LogWarning("Feedback with ID {feedbackId} was not found", id);
                    throw new KeyNotFoundException("feedback not found");
                }

                feedbackEntity.IsDeleted = true;
                feedbackEntity.UpdatedDate = DateTime.UtcNow;

                await _feedbackRepository.UpdateFeedback(feedbackEntity);
                _logger.LogDebug("Deleted feedback with ID {feedbackId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting feedback with ID {feedbackId}", id);
                throw;
            }
        }
    }
}
