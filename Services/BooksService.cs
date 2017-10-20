using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services
{
    public class BooksService : IBooksService
    {
        private readonly IBooksRepository _repo;

        public BooksService(IBooksRepository repo) {
            _repo = repo;
        }
        public IEnumerable<BookDTO> GetBooks() {
            var books = _repo.GetBooks();
            return books;
        }

        public BookDTO AddBook(BookView book)
        {
            return _repo.AddBook(book);
        }
        public BookDTO GetBookById(int id)
        {
            return _repo.GetBookById(id);
        } 
    }
}