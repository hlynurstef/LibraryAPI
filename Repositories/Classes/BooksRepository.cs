using System;
using System.Linq;
using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Exceptions;

namespace LibraryAPI.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        private readonly AppDataContext _db;

        public BooksRepository(AppDataContext db) {
            _db = db;
        }

        public IEnumerable<BookDTOLite> GetBooks()
        {
            var books = (from b in _db.Books
                        where b.Deleted == false
                        select new BookDTOLite {
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            ReleaseDate = b.ReleaseDate,
                            ISBN = b.ISBN,
                            Available = b.Available,
                            // TODO: Populate list of reviews
                            Reviews = null
                        }).ToList();
            return books;
        }

        public BookDTOLite AddBook(BookView book){
            var bookEntity = new Book{
                    Title = book.Title,
                    Author = book.Author,
                    ReleaseDate = book.ReleaseDate,
                    ISBN = book.ISBN,
                    Available = true,
                    Deleted = false
                    };
            _db.Books.Add(bookEntity);
            try{
                _db.SaveChanges();
            }
            catch(System.Exception e){
                Console.WriteLine(e);
            }
            return new BookDTOLite{
                Id = bookEntity.Id,
                Title = book.Title,
                Author = book.Author,
                ReleaseDate = book.ReleaseDate,
                ISBN = book.ISBN,
                Available = true,
                // TODO: Populate list of reviews
                Reviews = null
            };

        }
        public BookDTO GetBookById(int id){
            var book = (from b in _db.Books
                        where b.Id == id 
                        && b.Deleted == false
                        // TODO: should be BookDTO (has loan history as well)
                        select new BookDTO{
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            ReleaseDate = b.ReleaseDate,
                            ISBN = b.ISBN,
                            Available = b.Available,  
                            Reviews = (from r in _db.Reviews
                                       where r.BookID == b.Id
                                       select new ReviewDTO{
                                           BookId = r.BookID,
                                           UserId = r.UserId,
                                           BookTitle = b.Title,
                                           ReviewText = r.ReviewText,
                                           Stars = r.Stars
                                       }).ToList(),
                            LoanHistory = (from l in _db.Loans
                                           where l.BookId == b.Id
                                           select new LoanDTO{
                                               BookTitle = b.Title,
                                               Id = l.Id,
                                               LoanDate = l.LoanDate
                                           }).ToList()                                   
                        }).FirstOrDefault();
            if (book == null) {
                throw new NotFoundException("Book with id: " + id + " not found.");
            }
            return book;
        }
        public void DeleteBookById(int id){
            var book = (from b in _db.Books
                        where b.Id == id
                        && b.Deleted == false
                        select b).SingleOrDefault();

            if (book == null) {
                throw new NotFoundException("Book with id: " + id + " not found.");
            }

            var bookEntity = (from b in _db.Books
                       where b.Id == id
                       select b).FirstOrDefault();

            try{
                _db.Remove(bookEntity);
                _db.SaveChanges();
            }
            catch(System.Exception e){
                Console.WriteLine(e);
            }
        }

        public void UpdateBookById(int id, BookView book) {
            var bookEntity = _db.Books.SingleOrDefault(b => b.Id == id);

            if (bookEntity == null) {
                throw new NotFoundException("Book with id: " + id + " not found.");
            }
            
            bookEntity.Author = book.Author;
            bookEntity.ISBN = book.ISBN;
            bookEntity.Title = book.Title;
            bookEntity.ReleaseDate = book.ReleaseDate;

            try{
                _db.SaveChanges();
            }
            catch(System.Exception e) {
                Console.WriteLine(e);
            }
        }

        public IEnumerable<BookDTOLite> GetBooksByUserId(int userId)
        {
            var books = (from b in _db.Books
                        join l in _db.Loans on b.Id equals l.BookId
                         where l.UserId == userId
                         && l.HasBeenReturned == false
                         select new BookDTOLite {
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            ReleaseDate = b.ReleaseDate,
                            ISBN = b.ISBN,
                            Available = b.Available,
                            Reviews = (from r in _db.Reviews
                                       where r.BookID == b.Id
                                       select new ReviewDTO{
                                           BookId = r.BookID,
                                           UserId = r.UserId,
                                           BookTitle = b.Title,
                                           ReviewText = r.ReviewText,
                                           Stars = r.Stars
                                       }).ToList(),
                         }).ToList();

            return books;
        }
        public void LendBookToUser(int userId, int bookId){
            var bookEntity = _db.Books.SingleOrDefault(b => (b.Id == bookId && b.Deleted == false));
            var userEntity = _db.Users.SingleOrDefault(u => (u.Id == userId && u.Deleted == false));
            
            if(bookEntity == null){
                throw new NotFoundException("Book with id: " + bookId + " not found.");
            }
            if(bookEntity.Available == false){
                throw new NotFoundException("Book with id: " + bookId + " is already out.");
            }
            if(userEntity == null){
                throw new NotFoundException("Book with id: " + bookId + " not found.");
            }

            var loan = new Loan{
                BookId = bookEntity.Id,
                UserId = userEntity.Id,
                LoanDate = DateTime.Now,
                HasBeenReturned = false
            };
            bookEntity.Available = false;
            _db.Loans.Add(loan);
            _db.SaveChanges();      
        }
    }
}
