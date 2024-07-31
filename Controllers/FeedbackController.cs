using AllaCookidoo.Models;
using AllaCookidoo.Responses;
using AllaCookidoo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AllaCookidoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private readonly ILogger<FeedbackController> _logger;

        public FeedbackController(IFeedbackService feedbackService, ILogger<FeedbackController> logger)
        {
            _feedbackService = feedbackService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackResponse>>> GetFeedbacks()
        {
            _logger.LogInformation("Fetching all feedbacks");
            var feedbacks = await _feedbackService.GetFeedbacks();
            return Ok(feedbacks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackResponse>> GetFeedbackById(int id)
        {
            _logger.LogInformation("Fetching feedback with ID: {feedbackId}", id);
            var feedback = await _feedbackService.GetFeedbackById(id);
            if (feedback == null)
            {
                _logger.LogWarning("Opinia o ID {FeedbackId} nie została znaleziona", id);
                return NotFound();
            }
            return Ok(feedback);
        }

        [HttpPost]
        public async Task<ActionResult> PostFeedback(FeedbackRequest feedbackRequest)
        {
            _logger.LogInformation("Adding new feedback:");
            await _feedbackService.AddFeedback(feedbackRequest);
            return CreatedAtAction(nameof(GetFeedbackById), new { id = feedbackRequest.RecipeId }, feedbackRequest);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, FeedbackResponse feedbackUpdate)
        {
            _logger.LogInformation("Updating feedback with ID: {feedbackId}", id);
            if (id != feedbackUpdate.RecipeId)
            {
                _logger.LogWarning("ID opinii w URL ({UrlId}) nie zgadza się z ID opinii w treści ({ContentId})", id, feedbackUpdate.RecipeId);
                _logger.LogWarning("Feedback ID in URL: {UrlId} does not match with feedback ID in content: {ContentId}", id, recipeUpdate.RecipeId);

                return BadRequest();
            }

            await _feedbackService.UpdateFeedback(id, feedbackUpdate);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            _logger.LogInformation("Deleting feedback with ID: {feedbackId}", id);
            await _feedbackService.DeleteFeedback(id);
            return NoContent();
        }
    }
}
