using System;
using System.Linq;
using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Exceptions;

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

        public IEnumerable<ReviewDTO> GetAllReviewsForBook(int bookId) {
            // 
            var book = (from b in _db.Books
                        where b.Id == bookId
                        && b.Deleted == false
                        select b).SingleOrDefault();

            if (book == null) {
                throw new NotFoundException("Book with id: " + bookId + " not found.");
            }

            var reviews = (from r in _db.Reviews
                            join b in _db.Books on r.BookID equals b.Id
                            where b.Id == bookId
                            select new ReviewDTO {
                                UserId = r.UserId,
                                BookTitle = b.Title,
                                BookId = r.BookID,
                                ReviewText = r.ReviewText,
                                Stars = r.Stars
                            } ).ToList();
            
            return reviews;
        }
    }
}