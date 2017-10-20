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
            // TODO: Get information from database
            return new List<BookReviewsDTO>();
        }
    }
}