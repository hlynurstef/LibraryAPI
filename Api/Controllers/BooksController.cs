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
        [HttpGet]
        [Route("books")]
        public IActionResult GetBooks()
        {
            var books = _booksService.GetBooks();
            return Ok(books);
        }

        [HttpPost("books")]
        public IActionResult PostBook([FromBody]BookView book){
            //Console.WriteLine(book);
            if(book == null){
                return BadRequest();
            }
            if(!ModelState.IsValid){
                return StatusCode(412, "modelstate is not valid");
            }
            // FIXME: should return created at route?
            return Ok(_booksService.AddBook(book));
        }

        // TODO: GET /books/{book_id} - Sækja öll gögn um bók (m.a. lánasögu)

        // TODO: DELETE /books/{book_id} - Fjarlæga bók

        // TODO: PUT /books/{book_id} - Uppfæra bók

         
        // TODO: GET /users/{user_id}/books - Sækja skráningu um bækur sem notandi er með í láni

        // TODO: POST /users/{user_id}/books/{book_id} - Skrá útlán á bók

        // TODO: DELETE /users/{user_id}/books/{book_id} - Skila bók

        // TODO: PUT /users/{user_id}/books/{book_id} - Uppfæra útlánaskráningu


    }
}
