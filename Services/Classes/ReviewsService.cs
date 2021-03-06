using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services {
    public class ReviewsService : IReviewsService {
        public readonly IReviewsRepository _repo;

        public ReviewsService (IReviewsRepository repo) {
            _repo = repo;
        }

        public IEnumerable<ReviewDTO> GetAllBookReviews () {
            return _repo.GetAllBookReviews ();
        }

        public IEnumerable<ReviewDTO> GetAllReviewsForBook (int bookId) {
            return _repo.GetAllReviewsForBook (bookId);
        }

        public ReviewDTO AddReviewToBook (int userId, int bookId, ReviewView review) {
            return _repo.AddReviewToBook (userId, bookId, review);
        }

        public IEnumerable<ReviewDTO> GetReviewsForUser (int userId) {
            return _repo.GetReviewsForUser (userId);
        }

        public ReviewDTO GetBookReviewFromUser (int userId, int bookId) {
            return _repo.GetBookReviewFromUser (userId, bookId);
        }
        public void DeleteUsersBookReview (int userId, int bookId) {
            _repo.DeleteUsersBookReview (userId, bookId);
        }

        public void UpdateBooksUserReview (int bookId, int userId, ReviewView updatedReview) {
            _repo.UpdateBooksUserReview (bookId, userId, updatedReview);
        }
    }
}