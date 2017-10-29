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
        public IEnumerable<BookDTOLite> GetBooks() {
            var books = _repo.GetBooks();
            return books;
        }

        public BookDTOLite AddBook(BookView book)
        {
            return _repo.AddBook(book);
        }
        public BookDTO GetBookById(int id)
        {
            return _repo.GetBookById(id);
        }
        public void DeleteBookById(int id)
        {
            _repo.DeleteBookById(id);
        } 
        public void UpdateBookById(int id, BookView book)
        {
            _repo.UpdateBookById(id, book);
        }
        public IEnumerable<BookDTOLite> GetBooksByUserId(int userId)
        {
            return _repo.GetBooksByUserId(userId);
        }
<<<<<<< HEAD
        public void LendBookToUser(int userId, int bookId){
            _repo.LendBookToUser(userId, bookId);
=======

        public void ReturnBook(int userId, int bookId)
        {
            _repo.ReturnBook(userId, bookId);
>>>>>>> c8e733fe10ceba1f7ee0bd2a6040da69b3a9d4df
        }
    }
}