using System;
using System.Linq;
using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;

namespace LibraryAPI.Repositories
{
    public class ReviewsRepository : IReviewsRepository
    {
        private readonly AppDataContext _db;

        public ReviewsRepository(AppDataContext db) {
            _db = db;
        }

        public IEnumerable<ReviewDTO> GetAllBookReviews() {
            var reviews = (from r in _db.Reviews
                            join b in _db.Books on r.BookID equals b.Id
                            select new ReviewDTO {
                                UserId = r.UserId,
                                BookTitle = b.Title,
                                BookId = r.BookID,
                                ReviewText = r.ReviewText,
                                Stars = r.Stars
                            }).ToList();

            return reviews;
        }
    }
}