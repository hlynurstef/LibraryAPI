using System;
using System.Linq;
using LibraryAPI.Exceptions;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryAPI.Models.EntityModels;

namespace LibraryAPI.UnitTests.BooksRepositoryTests
{
    [TestClass]
    public class BooksRepositoryTests
    {
        #region UserInfo
        private const string NAME_DABS = "Daníel B. Sigurgeirsson";
        private const string ADDRESS_DABS = "Rauðrófustígur 14";
        private const string EMAIL_DABS = "dabs@ru.is";
        private const string PHONE_DABS = "1234567";
        private const bool DELETED_DABS = false;
        #endregion

        #region BookInfo
        private const string TITLE_LOTR = "The Lord of The Rings";
        private const string AUTHOR_LOTR = "J.R.R. Tolkien";
        private DateTime RELEASE_LOTR = new DateTime(1954,6,29);
        private const string ISBN_LOTR = "123456789";
        private const bool AVAILABLE_LOTR = true;
        private const bool DELETED_LOTR = false;
        #endregion

        #region LoanInfo
        private const int USERID_LOAN = 1;
        private const int BOOKID_LOAN = 1;
        private DateTime STARTDATE_LOAN = new DateTime(2017,10,29);
        private const bool RETURNED_LOAN = false;
        private DateTime? ENDDATE_LOAN = null;
        #endregion

        private AppDataContext context;

        [TestInitialize]
        public void InitializeTest() {
            var options = new DbContextOptionsBuilder<AppDataContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            context = new AppDataContext(options);

            context.Users.Add(new User {
                Name = NAME_DABS,
                Address = ADDRESS_DABS,
                Email = EMAIL_DABS,
                PhoneNumber = PHONE_DABS,
                Deleted = DELETED_DABS
            });

            context.Books.Add(new Book {
                Title = TITLE_LOTR,
                Author = AUTHOR_LOTR,
                ReleaseDate = RELEASE_LOTR,
                ISBN = ISBN_LOTR,
                Available = AVAILABLE_LOTR,
                Deleted = DELETED_LOTR
            });

            context.Loans.Add(new Loan {
                UserId = USERID_LOAN,
                BookId = BOOKID_LOAN,
                LoanDate = STARTDATE_LOAN,
                EndDate = ENDDATE_LOAN,
                HasBeenReturned = RETURNED_LOAN
            });

            context.SaveChanges();
        }

        [TestCleanup]
        public void CleanupTest() {
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void Books_AddBook_OneBook()
        {
            // Arrange
            var repo = new BooksRepository(context);
            var book = new BookView {
                Title = "A Game Of Thrones",
                Author = "George R.R. Martin",
                ReleaseDate = new DateTime(1996, 8, 1),
                ISBN = "535135468543246846541"
            };

            // Act
            repo.AddBook(book);

            // Assert
            Assert.AreEqual(2, context.Books.Count());
            Assert.AreEqual("A Game Of Thrones", context.Books.Where(x => x.Title == "A Game Of Thrones").SingleOrDefault().Title);
            Assert.AreEqual("George R.R. Martin", context.Books.Where(x => x.Title == "A Game Of Thrones").SingleOrDefault().Author);
            Assert.AreEqual(new DateTime(1996,8,1), context.Books.Where(x => x.Title == "A Game Of Thrones").SingleOrDefault().ReleaseDate);
            Assert.AreEqual("535135468543246846541", context.Books.Where(x => x.Title == "A Game Of Thrones").SingleOrDefault().ISBN);
        }

        [TestMethod]
        public void Books_GetBooks_OneBook()
        {
            // Arrange
            var repo = new BooksRepository(context);
            
            // Act
            var books = repo.GetBooks();

            // Assert
            Assert.AreEqual(1, books.Count());
            Assert.AreEqual("The Lord of The Rings", books.Where(x => x.Title == "The Lord of The Rings").SingleOrDefault().Title);
        }

        [TestMethod]
        public void Books_GetBooks_FourBooks()
        {
            // Arrange
            var repo = new BooksRepository(context);
            
            var book1 = new BookView {
                Title = "A Game Of Thrones",
                Author = "George R.R. Martin",
                ReleaseDate = new DateTime(1996, 8, 1),
                ISBN = "214124235"
            };

            var book2 = new BookView {
                Title = "The Hobbit",
                Author = "Tolkien",
                ReleaseDate = new DateTime(1996, 8,  1),
                ISBN = "541312312"
            };

            var book3 = new BookView {
                Title = "Programming with c++",
                Author = "Bjarne Stoustrup",
                ReleaseDate = new DateTime(1996, 8, 1),
                ISBN = "2423423123"
            };

            
            // Act
            repo.AddBook(book1);
            repo.AddBook(book2);
            repo.AddBook(book3);

            // Act
            var books = repo.GetBooks();

            // Assert
            Assert.AreEqual(4, books.Count());
            Assert.AreEqual("The Lord of The Rings", books.Where(x => x.Title == "The Lord of The Rings").SingleOrDefault().Title);
            Assert.AreEqual("A Game Of Thrones", books.Where(x => x.Title == "A Game Of Thrones").SingleOrDefault().Title);
            Assert.AreEqual("The Hobbit", books.Where(x => x.Title == "The Hobbit").SingleOrDefault().Title);
            Assert.AreEqual("Programming with c++", books.Where(x => x.Title == "Programming with c++").SingleOrDefault().Title);
        }

        [TestMethod]
        public void Books_DeleteBookById()
        {
            // Arrange
            var repo = new BooksRepository(context);

            // Act
            repo.DeleteBookById(1);

            // Assert
            Assert.AreEqual(0, context.Books.Count());
        }
    }
}
