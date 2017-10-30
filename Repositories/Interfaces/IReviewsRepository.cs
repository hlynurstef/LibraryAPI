using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Repositories
{
    public interface IReviewsRepository
    {
        IEnumerable<ReviewDTO> GetAllBookReviews();

        IEnumerable<ReviewDTO> GetAllReviewsForBook(int bookId);

        ReviewDTO AddReviewToBook(int userId, int bookId, ReviewView review);

        IEnumerable<ReviewDTO> GetReviewsForUser(int userId);

        ReviewDTO GetBookReviewFromUser(int userId, int bookId);

        void DeleteUsersBookReview(int userId, int bookId);

        void UpdateBooksUserReview(int bookId, int userId, ReviewView updatedReview);
    }
}