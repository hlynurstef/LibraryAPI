using System;
using System.Linq;
using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;

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
                            ISBN = b.ISBN
                        }).ToList();
            return books;
        }
    }
}
