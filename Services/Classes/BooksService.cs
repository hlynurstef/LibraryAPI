using System;
using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services {
    public class BooksService : IBooksService {
        private readonly IBooksRepository _repo;

        public BooksService (IBooksRepository repo) {
            _repo = repo;
        }
        public IEnumerable<BookDTOLite> GetBooks () {
            var books = _repo.GetBooks ();
            return books;
        }

        public BookDTOLite AddBook (BookView book) {
            return _repo.AddBook (book);
        }
        public BookDTO GetBookById (int id) {
            return _repo.GetBookById (id);
        }
        public void DeleteBookById (int id) {
            _repo.DeleteBookById (id);
        }
        public void UpdateBookById (int id, BookView book) {
            _repo.UpdateBookById (id, book);
        }
        public IEnumerable<BookDTOLite> GetBooksByUserId (int userId) {
            return _repo.GetBooksByUserId (userId);
        }
        public void LendBookToUser (int userId, int bookId) {
            _repo.LendBookToUser (userId, bookId);
        }
        public void ReturnBook (int userId, int bookId) {
            _repo.ReturnBook (userId, bookId);
        }

        public void UpdateLoanRegistration (int userId, int bookId, LoanView book) {
            _repo.UpdateLoanRegistration (userId, bookId, book);
        }

        public IEnumerable<BookDTOLite> GetBooksOnLoan (DateTime loanDate) {
            return _repo.GetBooksOnLoan (loanDate);
        }
    }
}