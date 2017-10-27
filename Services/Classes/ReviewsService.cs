using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services
{
    public class ReviewsService : IReviewsService
    {
        public readonly IReviewsRepository _repo;

        public ReviewsService(IReviewsRepository repo) {
            _repo = repo;
        }

        public IEnumerable<ReviewDTO> GetAllBookReviews() {
            var reviews = _repo.GetAllBookReviews();
            return reviews;
        }

        public IEnumerable<ReviewDTO> GetAllReviewsForBook(int bookId) {
            var reviews = _repo.GetAllReviewsForBook(bookId);
            return reviews;
        }
    }
}