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
    }
}