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
                            Reviews = (from r in _db.Reviews
                                        where r.BookId == b.Id
                                        select new ReviewDTO {
                                        BookId = r.BookId,
                                        UserId = r.UserId,
                                        BookTitle = b.Title,
                                        ReviewText = r.ReviewText,
                                        Stars = r.Stars 
                            }).ToList()
                        }).ToList();
            return books;
        }

        public BookDTOLite AddBook(BookView book){
            var bookCheck = (from b in _db.Books
                            where b.ISBN == book.ISBN
                            && b.Deleted == false
                            select b).SingleOrDefault();
            if (bookCheck != null) {
                throw new AlreadyExistsException("The request could not be completed due to a conflict with the current state of the resource.");
            }
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
                Reviews = (from r in _db.Reviews
                            where r.BookId == bookEntity.Id
                            select new ReviewDTO{
                                BookId = r.BookId,
                                UserId = r.UserId,
                                BookTitle = bookEntity.Title,
                                ReviewText = r.ReviewText,
                                Stars = r.Stars 
                }).ToList()
            };
        }
        
        public BookDTO GetBookById(int id){
            var book = (from b in _db.Books
                        where b.Id == id 
                        && b.Deleted == false
                        select new BookDTO{
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            ReleaseDate = b.ReleaseDate,
                            ISBN = b.ISBN,
                            Available = b.Available,  
                            Reviews = (from r in _db.Reviews
                                       where r.BookId == b.Id
                                       select new ReviewDTO{
                                           BookId = r.BookId,
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
                                               LoanDate = l.LoanDate,
                                               EndDate = l.EndDate
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

            book.Deleted = true;
            
            try{
                _db.SaveChanges();
            }
            catch(System.Exception e){
                Console.WriteLine(e);
            }
        }

        public void UpdateBookById(int id, BookView book) {
            var bookEntity = _db.Books.SingleOrDefault(b => b.Id == id && b.Deleted == false);

            if (bookEntity == null) {
                throw new NotFoundException("Book with id " + id + " not found.");
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
                        where b.Deleted == false
                        && l.UserId == userId
                        && l.HasBeenReturned == false
                        select new BookDTOLite {
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            ReleaseDate = b.ReleaseDate,
                            ISBN = b.ISBN,
                            Available = b.Available,
                            Reviews = (from r in _db.Reviews
                                       where r.BookId == b.Id
                                       select new ReviewDTO{
                                           BookId = r.BookId,
                                           UserId = r.UserId,
                                           BookTitle = b.Title,
                                           ReviewText = r.ReviewText,
                                           Stars = r.Stars
                                       }).ToList(),
                         }).ToList();
            return books;
        }

        public void LendBookToUser(int userId, int bookId){
            // Check if book exists and is available
            var bookEntity = _db.Books.SingleOrDefault(b => (b.Id == bookId && b.Deleted == false));
            if(bookEntity == null){
                throw new NotFoundException("Book with id " + bookId + " not found.");
            }
            if(bookEntity.Available == false){
                throw new NotFoundException("Book with id " + bookId + " is already out.");
            }

            // Check if user exists
            var userEntity = _db.Users.SingleOrDefault(u => (u.Id == userId && u.Deleted == false));
            if(userEntity == null){
                throw new NotFoundException("User with id " + userId + " not found.");
            }

            var loan = new Loan{
                BookId = bookEntity.Id,
                UserId = userEntity.Id,
                LoanDate = DateTime.Now,
                EndDate = null,
                HasBeenReturned = false
            };
            bookEntity.Available = false;
            
            _db.Loans.Add(loan);

            try {
                _db.SaveChanges();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
        public void ReturnBook(int userId, int bookId)
        {
            // Check if book exists
            var bookEntity = _db.Books.SingleOrDefault(b => (b.Id == bookId && b.Deleted == false));
            if(bookEntity == null){
                throw new NotFoundException("Book with id " + bookId + " not found.");
            }

            // Check if user exists
            var userEntity = _db.Users.SingleOrDefault(u => (u.Id == userId && u.Deleted == false));
            if(userEntity == null){
                throw new NotFoundException("User with id " + userId + " not found.");
            }

            // Check if the user has a loan for the book
            Loan loan = (from l in _db.Loans
                        where l.UserId == userId
                        && l.BookId == bookId
                        && l.HasBeenReturned == false
                        join b in _db.Books on l.BookId equals b.Id
                        select l).FirstOrDefault(); 

            if (loan == null) 
            {
                throw new NotFoundException("No loan found for user " + userId + " matching the book " + bookId);
            }

            loan.HasBeenReturned = true;
            loan.EndDate = DateTime.Now;

            try {
                _db.SaveChanges();
            }
            catch(System.Exception e) {
                Console.WriteLine(e);
            }
        }
        public void UpdateLoanRegistration(int userId, int bookId, LoanView loan){
            var bookEntity = _db.Books.SingleOrDefault(b => (b.Id == bookId && b.Deleted == false));
            if(bookEntity == null) {
                throw new NotFoundException("Book with id " + bookId + " not found.");
            }
            
            var userEntity = _db.Users.SingleOrDefault(u => (u.Id == userId && u.Deleted == false));
            if(userEntity == null) {
                throw new NotFoundException("User with id " + userId + " not found.");
            }
            
            var loanEntity = _db.Loans.SingleOrDefault(l => (l.UserId == userId && l.BookId == bookId));
            if(loanEntity == null) {
                throw new NotFoundException("Loan for book id " + bookId + " and user id " + userId + " not found.");
            }
            
            loanEntity.BookId = loan.BookId;
            loanEntity.HasBeenReturned = loan.HasBeenReturned;
            loanEntity.LoanDate = loan.LoanDate;
            loanEntity.EndDate = loan.EndDate;
            loanEntity.UserId = loan.UserId;

            try {
                _db.SaveChanges();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}
