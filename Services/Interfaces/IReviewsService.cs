using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;

namespace LibraryAPI.Services
{
    public interface IReviewsService
    {
        IEnumerable<ReviewDTO> GetAllBookReviews();

        IEnumerable<ReviewDTO> GetAllReviewsForBook(int bookId);
    }
}