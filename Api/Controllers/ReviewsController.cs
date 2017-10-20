using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet]
        [Route("books/reviews")]
        public IActionResult GetAllBookReviews()
        {
            var reviews = _reviewsService.GetAllBookReviews();
            return Ok(reviews);
        }

        // TODO: GET /users/{user_id}/reviews - Sækja dóma fyrir notanda

        // TODO: POST /users/{user_id}/reviews/{book_id} - Skrá dóma fyrir bók

        // TODO: DELETE /users/{user_id}/reviews/{book_id} - Fjarlæga dóma

        // TODO: PUT /users/{user_id}/reviews/{book_id} - Uppfæra dóma um bók

        // TODO: GET /books/{book_id}/reviews - Fá alla dóma fyrir bók

        // TODO: GET /books/{book_id}/reviews/{user_id} - Fá dóm frá notanda fyrir bók

        // TODO: PUT /books/{book_id}/reviews/{user_id} - Breyta dómi notanda um bók

        // TODO: DELETE /books/{book_id}/reviews/{user_id} - Eyða dómi notanda um bók
    }
}
