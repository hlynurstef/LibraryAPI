using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Exceptions;

namespace LibraryAPI.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBooksService _booksService;

        public BooksController(IBooksService booksService) {
            _booksService = booksService;
        }
        // GET /books
        /// <summary>
        /// Gets all books.
        /// </summary>
        /// <returns>All books in the library.</returns>
        [HttpGet("books")]
        public IActionResult GetBooks()
        {
            var books = _booksService.GetBooks();
            return Ok(books);
        }

        // POST /books
        /// <summary>
        /// Create a new book
        /// </summary>
        /// <param name="book">The book view object from body</param>
        [HttpPost("books")]
        public IActionResult AddBook([FromBody]BookView book){
            if(book == null){
                return BadRequest();
            }
            if(!ModelState.IsValid){
                return StatusCode(412, "modelstate is not valid");
            }
            var newBook = _booksService.AddBook(book);
            return CreatedAtRoute("GetBookById", new {bookId = newBook.Id}, newBook);
        }

        // GET /books/{bookId}
        /// <summary>
        /// Gets a single book by id.
        /// </summary>
        /// <param name="bookId">The Id of the book to get</param>
        /// <returns>The requested book</returns>
        [HttpGet("books/{bookId}", Name = "GetBookById")]
        public IActionResult GetBookById(int bookId) {
            // TODO:  Sækja öll gögn um bók (m.a. lánasögu)
            try {
                var book = _booksService.GetBookById(bookId);
                return Ok(book);
            }
            catch (NotFoundException e) {
                return NotFound(e.Message);
            }
        }

        // DELETE /books/{bookId}
        /// <summary>
        /// Deletes a single book by id.
        /// </summary>
        /// <param name="bookId">The Id of the book to delete</param>
        /// <returns>StatusCode 204 if successful</returns>
        [HttpDelete("books/{bookId}")]
        public IActionResult DeleteBookById(int bookId){
            try {
                _booksService.DeleteBookById(bookId);
                return NoContent();
            }
            catch(NotFoundException e) {
                return NotFound(e.Message);
            }
        }

        // PUT /books/{bookId}
        /// <summary>
        /// Updates a book by Id
        /// </summary>
        /// <param name="bookId">The id of the book to update</param>
        /// <param name="book">The updated information from the request body</param>
        /// <returns>StatusCode 204 if successful</returns>
        [HttpPut("books/{bookId}")]
        public IActionResult UpdateBookById(int bookId, [FromBody]BookView book ){
            if(book == null){
                return BadRequest();
            }
            if(!ModelState.IsValid){
                return StatusCode(412, "modelstate is not valid");
            }

            try {
                _booksService.UpdateBookById(bookId, book);
                return NoContent();
            }
            catch(NotFoundException e) {
                return NotFound(e.Message);
            }
        }
        
        // GET /users/{userId}/books
        /// <summary>
        /// Gets list of books that a user has checked out.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>A list of books</returns>
        [HttpGet("users/{userId}/books", Name = "GetBooksByUserId")]
        public IActionResult GetBooksByUserId(int userId) {
            try {
                var books = _booksService.GetBooksByUserId(userId);
                return Ok(books);
            }
            catch (NotFoundException e) {
                return NotFound(e.Message);
            }
        }

        // TODO: POST   /users/{userId}/books/{bookId} - Skrá útlán á bók

        // TODO: DELETE /users/{userId}/books/{bookId} - Skila bók

        // TODO: PUT    /users/{userId}/books/{bookId} - Uppfæra útlánaskráningu
    }
}
