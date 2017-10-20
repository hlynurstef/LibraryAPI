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

        public IEnumerable<BookReviewsDTO> GetAllBookReviews() {
            var bookReviews = (from b in _db.Books
                                select new BookReviewsDTO {
                                    BookId = b.Id,
                                    BookTitle = b.Title,
                                    BookAuthor = b.Author,
                                    ISBN = b.ISBN,
                                    Stars = (from r in _db.Reviews
                                            where r.BookID == b.Id
                                            select r.Stars).ToList()
                                }).ToList();
            return bookReviews;
        }
    }
}