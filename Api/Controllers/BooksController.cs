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
    /// <summary>
    /// Controller class for /books
    /// </summary>
    public class BooksController : Controller
    {
        private readonly IBooksService _booksService;

        /// <summary>
        /// Constructor for the books controller
        /// </summary>
        /// <param name="booksService">Dependency injection for the booksservice</param>
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

            try {
                var newBook = _booksService.AddBook(book);
                return CreatedAtRoute("GetBookById", new {bookId = newBook.Id}, newBook);
            }
            catch(AlreadyExistsException e) {
                return StatusCode(409, e.Message);
            }
            
        }

        // GET /books/{bookId}
        /// <summary>
        /// Gets a single book by id.
        /// </summary>
        /// <param name="bookId">The Id of the book to get</param>
        /// <returns>The requested book</returns>
        [HttpGet("books/{bookId}", Name = "GetBookById")]
        public IActionResult GetBookById(int bookId) {
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
        // DELETE /users/{userId}/books/{bookId} - Skila bók
        /// <summary>
        /// A user returns a book using the userId and the bookId
        /// </summary>
        /// <param name="userId">The Id of the user that is going to return a book</param>
        /// <param name="bookId">The Id of the book to return</param>
        /// <returns>StatusCode 204 if successful</returns>
        [HttpDelete("users/{userId}/books/{bookId}")]
        public IActionResult ReturnBook(int userId, int bookId){
            try {
                _booksService.ReturnBook(userId, bookId);
                return NoContent();
            }
            catch(NotFoundException e) {
                return NotFound(e.Message);
            }
        }

        // POST  /users/{userId}/books/{bookId}
        /// <summary>
        /// Lends a book to a user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="bookId">The id of the book</param>
        /// <returns>The loan record</returns>
         [HttpPost("users/{userId}/books/{bookId}")]
         public IActionResult LendBookToUser(int userId, int bookId){
             try {
                 _booksService.LendBookToUser(userId, bookId);
                 return StatusCode(201, "Created");
            }
            catch (NotFoundException e){
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Updates a users loan for a given book
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="bookId">The id of the book </param>
        /// <param name="loan">The updated loan itself </param>
        /// <returns></returns>
        [HttpPut("/users/{userId}/books/{bookId}")]
        public IActionResult UpdateLoanRegistration(int userId, int bookId, [FromBody]LoanView loan){
            if(loan == null){
                return BadRequest();
            }
            try{
                _booksService.UpdateLoanRegistration(userId, bookId, loan);
                return NoContent();
            }
            catch(NotFoundException e){
                return NotFound(e.Message);   
            }
        }
    }
}
