using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;

namespace LibraryAPI.Repositories
{
    public interface IReviewsRepository
    {
        IEnumerable<ReviewDTO> GetAllBookReviews();

        IEnumerable<ReviewDTO> GetAllReviewsForBook(int bookId);
    }
}