using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Models;

namespace LibraryAPI.Repositories
{
    public class SeedRepository : ISeedRepository
    {
        private readonly AppDataContext _db;

        public SeedRepository(AppDataContext db) {
            _db = db;
        }

        public void SeedDatabase()
        {
            ClearTables();
            SeedBooks();
            SeedUsersAndLoans();            
        }

        public void ClearTables()
        {
            var deletedBooks = (from b in _db.Books
                                select b).ToList();
            foreach(var book in deletedBooks) 
            {
                _db.Books.Remove(book);
            }

            try {
                _db.SaveChanges();
            }
            catch(System.Exception e){
                Console.WriteLine(e);
            }

            var deletedUsers = (from u in _db.Users
                                select u).ToList();
            foreach (var user in deletedUsers) 
            {
                _db.Users.Remove(user);
            }

            try {
                _db.SaveChanges();
            }
            catch(System.Exception e){
                Console.WriteLine(e);
            }

            var deletedLoans = (from l in _db.Loans
                                select l).ToList();
            foreach (var loan in deletedLoans) 
            {
                _db.Loans.Remove(loan);
            }

            try {
                _db.SaveChanges();
            }
            catch(System.Exception e){
                Console.WriteLine(e);
            }
        }

        public void SeedBooks()
        {
            try {
                using (StreamReader r = new StreamReader("../Data/Books.json"))
                {    
                    string json = r.ReadToEnd();
                    
                    List<BookSeed> books = JsonConvert.DeserializeObject<List<BookSeed>>(json);
                    foreach(BookSeed book in books)
                    {
                        Book newBook = new Book {
                            Id = book.bok_id,
                            Title = book.bok_titill,
                            Author = (book.fornafn_hofundar + book.eftirnafn_hofundar),
                            ReleaseDate = book.utgafudagur,
                            ISBN = book.ISBN,
                            Available = true
                        };
                        _db.Books.Add(newBook);
                    }

                    try {
                        _db.SaveChanges();
                    }
                    catch(System.Exception e){
                        Console.WriteLine(e);
                    }
                }
            }
            catch (Exception e) {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        public void SeedUsersAndLoans()
        {
            try {
                using (StreamReader r = new StreamReader("../Data/Users.json"))
                {    
                    string json = r.ReadToEnd();
                    int counter = 0;

                    List<UserSeed> users = JsonConvert.DeserializeObject<List<UserSeed>>(json);
                    foreach(UserSeed user in users)
                    {
                        User newUser = new User {
                            Id = user.vinur_id,
                            Name = (user.fornafn + user.eftirnafn),
                            Address = user.heimilisfang,
                            Email = user.netfang,
                            PhoneNumber = null,
                            Deleted = false
                        };
                        
                        if (user.lanasafn != null) {
                            foreach(LoanSeed loan in user.lanasafn) {
                                counter++;
                                Loan newLoan = new Loan {
                                    Id = counter,
                                    UserId = user.vinur_id,
                                    BookId = loan.bok_id,
                                    LoanDate = loan.lanadagsetning,
                                    HasBeenReturned = false
                                };

                                _db.Loans.Add(newLoan);
                            }
                        }

                        _db.Users.Add(newUser);
                    }
                     
                    try {
                        _db.SaveChanges();
                    }
                    catch(System.Exception e){
                        Console.WriteLine(e);
                    }
                }
            }
            catch (Exception e) {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
    }
}