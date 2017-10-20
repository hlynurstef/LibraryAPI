using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Repositories;
using LibraryAPI.Services;

namespace LibraryAPI.Services
{
    public class RecommendationsService : IRecommendationsService
    {
        public readonly IRecommendationsRepository _repo;

        public RecommendationsService(IRecommendationsRepository repo) {
            _repo = repo;
        }

        public IEnumerable<BookDTO> GetRecommendationsForUser(int userId)
        {
            return _repo.GetRecommendationsForUser(userId);
        }
    }
}