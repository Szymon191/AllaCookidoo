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

        public FeedbackService(IFeedbackRepository feedbackRepository, ILogger<FeedbackService> logger)
        {
            _feedbackRepository = feedbackRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<FeedbackResponse>> GetFeedbacks()
        {
            _logger.LogInformation("pobieranie wszystkich opinii");
            try
            {
                var feedbacks = await _feedbackRepository.GetFeedbacks();
                _logger.LogDebug("Znaleziono {Count} opinii", feedbacks.Count());
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
                _logger.LogError(ex, "Błąd podczas pobierania wszystkich opinii");
                throw;
            }
        }

        public async Task<FeedbackResponse> GetFeedbackById(int id)
        {
            _logger.LogInformation("pobieranie opinii o ID: {feedbackId}", id);
            try
            {
                var feedback = await _feedbackRepository.GetFeedbackById(id);
                if (feedback == null || feedback.IsDeleted)
                {
                    _logger.LogWarning("opinia o ID {feedbackId} nie została znaleziona lub została usunięta", id);
                    return null;
                }

                _logger.LogDebug("Znaleziono opinie o ID {feedbackId}", id);
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
                _logger.LogError(ex, "Błąd podczas pobierania opinii o ID {feedbackId}", id);
                throw;
            }
        }

        public async Task AddFeedback(FeedbackRequest feedbackCreation)
        {
            _logger.LogInformation("dodawanie nowej opinii: {feedbackOpinion}", feedbackCreation.Opinion);
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
                _logger.LogDebug("Dodano nową opinie o ID {feedbackId}", feedbackCreation.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas dodawania nowej opinii: {feedbackOpinion}", feedbackCreation.Opinion);
                throw;
            }
        }

        public async Task UpdateFeedback(int id, FeedbackResponse feedbackUpdate)
        {
            _logger.LogInformation("aktualizacja opinii o ID: {feedbackId}", id);
            try
            {
                var feedbackEntity = await _feedbackRepository.GetFeedbackById(id);
                if (feedbackEntity == null)
                {
                    _logger.LogWarning("opinia o ID {feedbackId} nie została znaleziona", id);
                    throw new KeyNotFoundException("Category not found");
                }

                feedbackEntity.Opinion = feedbackUpdate.Opinion;
                feedbackEntity.Evaluation = feedbackUpdate.Evaluation;
                feedbackEntity.UpdatedDate = DateTime.UtcNow;

                await _feedbackRepository.UpdateFeedback(feedbackEntity);
                _logger.LogDebug("Zaktualizowano opinie o ID {feedbackId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas aktualizacji opinii o ID {feedbackId}", id);
                throw;
            }
        }

        public async Task DeleteFeedback(int id)
        {
            _logger.LogInformation("usuwanie opinii o ID: {feedbackId}", id);
            try
            {
                var feedbackEntity = await _feedbackRepository.GetFeedbackById(id);
                if (feedbackEntity == null)
                {
                    _logger.LogWarning("opinia o ID {feedbackId} nie została znaleziona", id);
                    throw new KeyNotFoundException("feedback not found");
                }

                feedbackEntity.IsDeleted = true;
                feedbackEntity.UpdatedDate = DateTime.UtcNow;

                await _feedbackRepository.UpdateFeedback(feedbackEntity);
                _logger.LogDebug("Usunięto opinie o ID {feedbackId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas usuwania opinii o ID {feedbackId}", id);
                throw;
            }
        }
    }
}
