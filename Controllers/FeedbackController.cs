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
            _logger.LogInformation("Pobieranie wszystkich opinii");
            var feedbacks = await _feedbackService.GetFeedbacks();
            return Ok(feedbacks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackResponse>> GetFeedbackById(int id)
        {
            _logger.LogInformation("Pobieranie opinii o ID: {FeedbackId}", id);
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
            _logger.LogInformation("Dodawanie nowej opinii");
            await _feedbackService.AddFeedback(feedbackRequest);
            return CreatedAtAction(nameof(GetFeedbackById), new { id = feedbackRequest.RecipeId }, feedbackRequest);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, FeedbackResponse feedbackUpdate)
        {
            _logger.LogInformation("Aktualizacja opinii o ID: {FeedbackId}", id);
            if (id != feedbackUpdate.RecipeId)
            {
                _logger.LogWarning("ID opinii w URL ({UrlId}) nie zgadza się z ID opinii w treści ({ContentId})", id, feedbackUpdate.RecipeId);
                return BadRequest();
            }

            await _feedbackService.UpdateFeedback(id, feedbackUpdate);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            _logger.LogInformation("Usuwanie opinii o ID: {FeedbackId}", id);
            await _feedbackService.DeleteFeedback(id);
            return NoContent();
        }
    }
}
