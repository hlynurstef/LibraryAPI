using System;
using System.Linq;
using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;

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
                    Available = true 
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
        public BookDTOLite GetBookById(int id){
            var book = (from b in _db.Books
                       where b.Id == id
                       select new BookDTOLite{
                           Id = b.Id,
                           Title = b.Title,
                           Author = b.Author,
                           ReleaseDate = b.ReleaseDate,
                           ISBN = b.ISBN,
                           Available = b.Available,
                           // TODO: Populate list of reviews
                            Reviews = null
                       }  
            
            ).FirstOrDefault();
            return book;
        }
        public BookDTOLite DeleteBookById(int id){
            var book = (from b in _db.Books
                    where b.Id == id
                    select new BookDTOLite{
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            ReleaseDate = b.ReleaseDate,
                            ISBN = b.ISBN,
                            Available = b.Available,
                            // TODO: Populate list of reviews
                            Reviews = null
                    }
            ).FirstOrDefault();

            var bookEntity = (from b in _db.Books
                       where b.Id == id
                       select b
            ).FirstOrDefault();
            _db.Remove(bookEntity);
            try{
                _db.SaveChanges();
            }
            catch(System.Exception e){
                Console.WriteLine(e);
            }
            return book;
        }

        public BookDTOLite EditBookById(int id, BookView book) {
            var bookEntity = _db.Books.SingleOrDefault(b => b.Id == id);
            
            bookEntity.Author = book.Author;
            bookEntity.ISBN = book.ISBN;
            bookEntity.Title = book.Title;
            bookEntity.ReleaseDate = book.ReleaseDate;

            _db.SaveChanges();

            var retBook = new BookDTOLite{
                            Id = id,
                            Title = bookEntity.Title,
                            Author = bookEntity.Author,
                            ReleaseDate = bookEntity.ReleaseDate,
                            ISBN = bookEntity.ISBN,
                            Available = bookEntity.Available,
                            // TODO: Populate list of reviews
                            Reviews = null
            };
            return retBook;
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
                            // TODO: Populate list of reviews
                            Reviews = null
                         }).ToList();

            return books;
        }
    }
}
