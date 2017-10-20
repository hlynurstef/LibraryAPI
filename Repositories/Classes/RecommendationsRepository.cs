using System.Collections.Generic;
using System.Linq;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Repositories;

namespace LibraryAPI.Repositories
{
    public class RecommendationsRepository : IRecommendationsRepository
    {
        private readonly AppDataContext _db;

        public RecommendationsRepository(AppDataContext db) {
            _db = db;
        }

        public IEnumerable<BookDTO> GetRecommendationsForUser(int userId)
        {
            // TODO: implement
            // Should not include books that the user has already taken out on a loan
            List<BookDTO> recommendations = new List<BookDTO>();
            
            return recommendations;
        }
    }
}