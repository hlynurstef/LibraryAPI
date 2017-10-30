using System;
using System.Linq;
using LibraryAPI.Exceptions;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryAPI.Models.EntityModels;

namespace LibraryAPI.UnitTests.UsersRepositoryTests
{
    [TestClass]
    public class UsersRepositoryTests
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
        public void Users_AddUser_OneUser()
        {
            // Arrange
            var repo = new UsersRepository(context);
            var user = new UserView {
                Name = "Hlynur",
                Address = "Laufrimi 85",
                Email = "hlynurs15@ru.is",
                PhoneNumber = "823-8028"
            };

            // Act
            repo.AddUser(user);

            // Assert
            Assert.AreEqual(2, context.Users.Count());
            Assert.AreEqual("Hlynur", context.Users.Where(x => x.Name == "Hlynur").SingleOrDefault().Name);
            Assert.AreEqual("Laufrimi 85", context.Users.Where(x => x.Address == "Laufrimi 85").SingleOrDefault().Address);
            Assert.AreEqual("hlynurs15@ru.is", context.Users.Where(x => x.Email == "hlynurs15@ru.is").SingleOrDefault().Email);
            Assert.AreEqual("823-8028", context.Users.Where(x => x.PhoneNumber == "823-8028").SingleOrDefault().PhoneNumber);
        }
        
        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void Users_AddUser_SameUserTwice()
        {
            // Arrange
            var repo = new UsersRepository(context);
            var user = new UserView {
                Name = "Hlynur",
                Address = "Laufrimi 85",
                Email = "hlynurs15@ru.is",
                PhoneNumber = "823-8028"
            };

            // Act
            repo.AddUser(user);
            repo.AddUser(user);

            // Assert
            Assert.AreEqual(2, context.Users.Count());            
        }
    }
}
