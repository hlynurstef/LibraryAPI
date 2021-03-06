using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Exceptions;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers {
    /// <summary>
    /// Review controller class, handles requests related to reviews
    /// </summary>
    public class ReviewsController : Controller {
        private readonly IReviewsService _reviewsService;

        /// <summary>
        /// Generates a new Review service to be used.
        /// </summary>
        /// <param name="reviewsService">reviewService</param>
        public ReviewsController (IReviewsService reviewsService) {
            _reviewsService = reviewsService;
        }

        // GET /books/reviews
        /// <summary>
        /// Gets all reviews for all books.
        /// </summary>
        /// <returns>List of all reviews for all books.</returns>
        [HttpGet ("books/reviews")]
        public IActionResult GetAllBookReviews () {
            var reviews = _reviewsService.GetAllBookReviews ();
            return Ok (reviews);
        }

        /// <summary>
        /// Get all reviews for a book
        /// </summary>
        /// <param name="bookId">Id of the book</param>
        /// <returns>JSON list of reviews for a book</returns>
        [HttpGet ("books/{bookId}/reviews")]
        public IActionResult GetAllReviewsForBook (int bookId) {
            try {
                var reviews = _reviewsService.GetAllReviewsForBook (bookId);
                return Ok (reviews);
            } catch (NotFoundException e) {
                return NotFound (e.Message);
            }
        }
        /// <summary>
        /// User gets his review for the specified book
        /// </summary>
        /// <param name="bookId">The id of the book</param>
        /// <param name="userId">The id of the user </param>
        /// <returns>200 ok if succesful </returns>
        [HttpGet ("books/{bookId}/reviews/{userId}")]
        public IActionResult GetBookReviewFromUser (int bookId, int userId) {
            try {
                var review = _reviewsService.GetBookReviewFromUser (bookId, userId);
                return Ok (review);
            } catch (NotFoundException e) {
                return NotFound (e.Message);
            }
        }

        /// <summary>
        /// Deletes the review of book with bookId and 
        /// that the user with userId made
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpDelete ("books/{bookId}/reviews/{userId}")]
        public IActionResult DeleteBookReview (int bookId, int userId) {
            try {
                _reviewsService.DeleteUsersBookReview (userId, bookId);
                return NoContent ();
            } catch (NotFoundException e) {
                return NotFound (e.Message);
            }
        }

        /// <summary>
        /// Returns all the reviews that a user had given
        /// </summary>
        /// <param name="userId">Id of the user.</param>
        /// <returns>JSON list of reviews</returns>
        [HttpGet ("users/{userId}/reviews")]
        public IActionResult GetReviewsForUser (int userId) {
            try {
                var reviews = _reviewsService.GetReviewsForUser (userId);
                return Ok (reviews);
            } catch (NotFoundException e) {
                return NotFound (e.Message);
            }
        }

        /// <summary>
        /// Adds a new review from a user to a book
        /// </summary>
        /// <param name="userId">Id of the user giving the review</param>
        /// <param name="bookId">Id of the book being reviewed</param>
        /// <param name="review">ReviewDTO</param>
        /// <returns></returns>
        [HttpPost ("users/{userId}/reviews/{bookId}")]
        public IActionResult AddReviewToBook (int userId, int bookId, [FromBody] ReviewView review) {
            if (!ModelState.IsValid) {
                return StatusCode (412, "modelstate is not valid");
            }
            try {
                var newReview = _reviewsService.AddReviewToBook (userId, bookId, review);
                //FIXME: Create a createdAt path and return that instead.
                return StatusCode (201);
            } catch (NotFoundException e) {
                return NotFound (e.Message);
            }
        }

        /// <summary>
        /// Deletes the review of book with bookId and 
        /// that the user with userId made
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpDelete ("users/{userId}/reviews/{bookId}")]
        public IActionResult DeleteUsersBookReview (int userId, int bookId) {
            try {
                _reviewsService.DeleteUsersBookReview (userId, bookId);
                return NoContent ();
            } catch (NotFoundException e) {
                return NotFound (e.Message);
            }
        }

        /// <summary>
        /// Updates the users review for the given book
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="bookId">The id of the book </param>
        /// <param name="updatedReview">The updated review</param>
        /// <returns>StatusCode 204 if everything went as planned</returns>
        [HttpPut ("books/{bookId}/reviews/{userId}")]
        public IActionResult UpdateBooksUserReview (int bookId, int userId, [FromBody] ReviewView updatedReview) {
            try {
                _reviewsService.UpdateBooksUserReview (bookId, userId, updatedReview);
                return NoContent ();
            } catch (NotFoundException e) {
                return NotFound (e.Message);
            }
        }

        /// <summary>
        /// Updates the users review for the given book
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <param name="updatedReview">The updated review</param>
        /// <returns>StatusCode 204 if everything went as planned</returns>
        [HttpPut ("users/{userId}/reviews/{bookId}")]
        public IActionResult UpdateUsersBookReview (int userId, int bookId, [FromBody] ReviewView updatedReview) {
            try {
                _reviewsService.UpdateBooksUserReview (bookId, userId, updatedReview);
                return NoContent ();
            } catch (NotFoundException e) {
                return NotFound (e.Message);
            }
        }
    }
}