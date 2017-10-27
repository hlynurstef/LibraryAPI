using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
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
    }
}