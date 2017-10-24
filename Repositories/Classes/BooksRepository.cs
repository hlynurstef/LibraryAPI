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

        public IEnumerable<BookDTO> GetBooks()
        {
            var books = (from b in _db.Books
                        select new BookDTO {
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            ReleaseDate = b.ReleaseDate,
                            ISBN = b.ISBN,
                            Available = b.Available
                        }).ToList();
            return books;
        }

        public BookDTO AddBook(BookView book){
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
            return new BookDTO{
                Id = bookEntity.Id,
                Title = book.Title,
                Author = book.Author,
                ReleaseDate = book.ReleaseDate,
                ISBN = book.ISBN,
                Available = true
            };

        }
        public BookDTO GetBookById(int id){
            var book = (from b in _db.Books
                       where b.Id == id
                       select new BookDTO{
                           Id = b.Id,
                           Title = b.Title,
                           Author = b.Author,
                           ReleaseDate = b.ReleaseDate,
                           ISBN = b.ISBN,
                           Available = b.Available
                       }  
            
            ).FirstOrDefault();
            return book;
        }
        public BookDTO DeleteBookById(int id){

            var book = (from b in _db.Books
                    where b.Id == id
                    select new BookDTO{
                           Id = b.Id,
                           Title = b.Title,
                           Author = b.Author,
                           ReleaseDate = b.ReleaseDate,
                           ISBN = b.ISBN,
                           Available = b.Available
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
        BookDTO IBooksRepository.EditBookById(int id, BookView book){
            var bookEntity = _db.Books.SingleOrDefault(b => b.Id == id);
            
            bookEntity.Author = book.Author;
            bookEntity.ISBN = book.ISBN;
            bookEntity.Title = book.Title;
            bookEntity.ReleaseDate = book.ReleaseDate;

            _db.SaveChanges();

            var retBook = new BookDTO{
                          Id = id,
                          Title = bookEntity.Title,
                          Author = bookEntity.Author,
                          ReleaseDate = bookEntity.ReleaseDate,
                          ISBN = bookEntity.ISBN,
                          Available = bookEntity.Available
            };
            return retBook;
        }
    }
}
