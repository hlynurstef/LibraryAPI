using System.Collections.Generic;
using System.Linq;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Repositories;
using LibraryAPI.Exceptions;
using System;

namespace LibraryAPI.Repositories
{
    public class RecommendationsRepository : IRecommendationsRepository
    {
        private readonly AppDataContext _db;

        public RecommendationsRepository(AppDataContext db) {
            _db = db;
        }

        public IEnumerable<BookDTO> GetRecommendationsForUser(int userId)
        {
            List<BookDTO> recommendations = new List<BookDTO>();
            List<string> books = new List<string>(); 
            List<string> authors = new List<string>();

            var userCheck = _db.Users.Where(u => u.Id == userId).SingleOrDefault();

            if (userCheck == null) {
                throw new NotFoundException("User with id " + userId + " does not exist");
            }

            // Get the user
            var user = (from u in _db.Users
            where u.Id == userId
            && u.Deleted == false
            select new UserDTO {
                Id = u.Id,
                Name = u.Name,
                Address = u.Address,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                LoanHistory = (from l in _db.Loans
                                join b in _db.Books on l.BookId equals b.Id
                                where l.UserId == userId
                                && b.Deleted == false
                                select new LoanDTO {
                                    Id = l.Id,
                                    BookTitle = b.Title,
                                    LoanDate = l.LoanDate,
                                    EndDate = l.EndDate
                                }).ToList()
            }).SingleOrDefault();
            
            // Dig into the users loan history
            if (user.LoanHistory != null) {
                // Go over all the books the user has read
                // add the book titles to a list and authors to a separate list
                foreach (var book in user.LoanHistory)
                {
                    books.Add(book.BookTitle);
                    var author = (from b in _db.Books
                                    where b.Title == book.BookTitle
                                    && b.Deleted == false
                                    select b.Author).SingleOrDefault();
                    authors.Add(author);
                }

                 // Go over the authors that the user has read books from
                 // and find other books from these authors, if any
                 foreach(var author in authors) 
                 {
                     var recommendedBooks = (from b in _db.Books
                                                where b.Author == author
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
                                                        }).ToList();

                    // Add the recommended books to recommendations
                    foreach(var recBook in recommendedBooks) 
                    {
                        recommendations.Add(recBook);
                    }
                 }
                
                // Go over the recommendations and make sure the user has not read these books already.
                for(int i = 0; i < recommendations.Count; i++) 
                    {
                        if (books.Contains(recommendations[i].Title)) {
                            recommendations.RemoveAt(i);
                    }
                }
            }

            // If we find no books to recommend the user,
            // we supply him with random books to read, without recommending any that he has already read
            if (recommendations.Count < 1) {
                recommendations = GetRandomRecommendations(books);
            }
            
            return recommendations;
        }

        private List<BookDTO> GetRandomRecommendations(List<string> titles)
        {
            List<BookDTO> recommendations = new List<BookDTO>();
            var books = (from b in _db.Books
                        where b.Deleted == false
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
                        }).ToList();

            Random rnd = new Random();
            List<int> bookIds = new List<int>();
            for(int i = 0; i < 5; i++) {
                bookIds.Add(rnd.Next(1, books.Count));
            }

            foreach(var bookId in bookIds) 
            {
                if(!titles.Contains(books[bookId].Title) && !recommendations.Contains(books[bookId])) {
                    recommendations.Add(books[bookId]);
                }
                else if (recommendations.Contains(books[bookId])) {
                    int newIndex = 0;
                    while (true) {
                        newIndex = rnd.Next(1, books.Count);
                        if(!recommendations.Contains(books[newIndex])) {
                            recommendations.Add(books[newIndex]);
                            break;
                        }
                    }
                }
            }

            return recommendations;
        }
    }
}