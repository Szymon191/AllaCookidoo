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
            _logger.LogInformation("Pobieranie wszystkich opinii z bazy danych");
            return await _context.Feedbacks.Where(x => !x.IsDeleted).ToListAsync();
        }
        
        public async Task<FeedbackEntity> GetFeedbackById(int id)
        {
            _logger.LogInformation($"Pobieranie opinii o ID: {id} z bazy danych");
            return await _context.Feedbacks.FindAsync(id);
        }
        
        public async Task AddFeedback(FeedbackEntity feedback)
        {
            _logger.LogInformation("Dodawanie nowej opinii do bazy danych");
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFeedback(FeedbackEntity feedback)
        {
            _logger.LogInformation($"Aktualizacja opinii o ID: {feedback.Id} w bazie danych");
            _context.Entry(feedback).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
