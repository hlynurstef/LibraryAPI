using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("books")]
        public IActionResult AddBook([FromBody]BookView book){
            //Console.WriteLine(book);
            if(book == null){
                return BadRequest();
            }
            if(!ModelState.IsValid){
                return StatusCode(412, "modelstate is not valid");
            }
            var newBook = _booksService.AddBook(book);
            return CreatedAtRoute("GetBookById", new {bookId = newBook.Id}, newBook);
        }


        // TODO: GET    /books/{bookId} - Sækja öll gögn um bók (m.a. lánasögu)
        [HttpGet("books/{bookId}", Name = "GetBookById")]
        public IActionResult GetBookById(int bookId){
            return Ok(_booksService.GetBookById(bookId));
        }


        // TODO: DELETE /books/{bookId} - Fjarlæga bók

        // TODO: PUT    /books/{bookId} - Uppfæra bók

         
        // TODO: GET    /users/{userId}/books          - Sækja skráningu um bækur sem notandi er með í láni

        // TODO: POST   /users/{userId}/books/{bookId} - Skrá útlán á bók

        // TODO: DELETE /users/{userId}/books/{bookId} - Skila bók

        // TODO: PUT    /users/{userId}/books/{bookId} - Uppfæra útlánaskráningu


    }
}
