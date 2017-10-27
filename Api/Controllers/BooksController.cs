﻿using System;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="book"></param>
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

        [HttpGet("books/{bookId}", Name = "GetBookById")]
        public IActionResult GetBookById(int bookId) {
            // TODO:  Sækja öll gögn um bók (m.a. lánasögu)
            // TODO: Try catch if book not found
            return Ok(_booksService.GetBookById(bookId));
        }

        [HttpDelete("books/{bookId}")]
        public IActionResult DeleteBookById(int bookId){
            var book = _booksService.DeleteBookById(bookId);
            return Ok(book);
        }

        [HttpPut("books/{bookId}")]
        public IActionResult EditBookById(int bookId, [FromBody]BookView book ){
            if(GetBookById(bookId) == null)
            {
                return NotFound();
            }
            if(book == null){
                return BadRequest();
            }
            if(!ModelState.IsValid){
                return StatusCode(412, "modelstate is not valid");
            }
            var newBook = _booksService.EditBookById(bookId, book);
            return CreatedAtRoute("GetBookById", new {bookId = newBook.Id}, newBook);
        }
        
        // TODO: GET    /users/{userId}/books          - Sækja skráningu um bækur sem notandi er með í láni
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
