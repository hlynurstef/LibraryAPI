using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Exceptions;

namespace LibraryAPI.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewsService _reviewsService;

        public ReviewsController(IReviewsService reviewsService) {
            _reviewsService = reviewsService;
        }

        // TODO: GET /books/reviews - Fá alla dóma fyrir allar bækur
        // GET /books/reviews
        /// <summary>
        /// Gets all reviews for all books.
        /// </summary>
        /// <returns>List of all reviews for all books.</returns>
        [HttpGet("books/reviews")]
        public IActionResult GetAllBookReviews()
        {
            var reviews = _reviewsService.GetAllBookReviews();
            return Ok(reviews);
        }

        // TODO: GET    /books/{bookId}/reviews          - Fá alla dóma fyrir bók
        [HttpGet("books/{bookId}/reviews")]
        public IActionResult GetAllReviewsForBook(int bookId) 
        {
            try {
                var reviews = _reviewsService.GetAllReviewsForBook(bookId);
                return Ok(reviews);
            } catch (NotFoundException e) {
                return NotFound("{\"error\": \"" + e.Message + " \"}");
            }
            
        }
        // TODO: GET    /books/{bookId}/reviews/{userId} - Fá dóm frá notanda fyrir bók

        // TODO: PUT    /books/{bookId}/reviews/{userId} - Breyta dómi notanda um bók

        // TODO: DELETE /books/{bookId}/reviews/{userId} - Eyða dómi notanda um bók


        // TODO: GET    /users/{userId}/reviews          - Sækja dóma fyrir notanda

        // TODO: GET    /users/{userId}/reviews/{bookId} - Sækja dóma fyrir bók

        // TODO: POST   /users/{userId}/reviews/{bookId} - Skrá dóma fyrir bók

        // TODO: DELETE /users/{userId}/reviews/{bookId} - Fjarlæga dóma

        // TODO: PUT    /users/{userId}/reviews/{bookId} - Uppfæra dóma um bók



    }
}
