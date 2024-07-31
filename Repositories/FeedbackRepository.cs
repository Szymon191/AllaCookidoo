using AllaCookidoo.Database;
using AllaCookidoo.Entities;
using Microsoft.EntityFrameworkCore;

namespace AllaCookidoo.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly AllaCookidoDatabaseContext _context;
        private readonly ILogger<FeedbackRepository> _logger;

        public FeedbackRepository(AllaCookidoDatabaseContext context, ILogger<FeedbackRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<FeedbackEntity>> GetFeedbacks()
        {
            _logger.LogInformation("Fetching all feedbacks from database");
            return await _context.Feedbacks.Where(x => !x.IsDeleted).ToListAsync();
        }
        
        public async Task<FeedbackEntity> GetFeedbackById(int id)
        {
            _logger.LogInformation($"Fetching feedback with ID: {id} from database");
            return await _context.Feedbacks.FindAsync(id);
        }
        
        public async Task AddFeedback(FeedbackEntity feedback)
        {
            _logger.LogInformation("Adding new feedback to database");
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFeedback(FeedbackEntity feedback)
        {
            _logger.LogInformation($"Updating feedback with ID: {feedback.Id} in database");
            _context.Entry(feedback).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
