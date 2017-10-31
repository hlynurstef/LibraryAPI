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
                .UseInMemoryDatabase(databaseName: "UsersDB")
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
            Assert.Fail("Should have thrown an AlreadyExistsException");            
        }

        [TestMethod]
        public void Users_GetUsers_GetTwoUsers() {
            // Arrange
            var repo = new UsersRepository(context);
            context.Users.Add(new User {
                Name = "Jón",
                Address = "Dúfnahólar 10",
                Email = "jon15@ru.is",
                PhoneNumber = "123-4567",
                Deleted = false
            });
            context.SaveChanges();

            // Act
            var users = repo.GetUsers();

            // Assert
            Assert.AreEqual(2, users.Count());
        }

        [TestMethod]
        public void Users_GetUsers_EmptyList() {
            // Arrange
            var repo = new UsersRepository(context);
            var usersToDelete = (from u in context.Users
                                select u).ToList();
            foreach(User user in usersToDelete) {
                user.Deleted = true;
            }
            context.SaveChanges();

            // Act
            var users = repo.GetUsers();

            // Assert
            Assert.AreEqual(0, users.Count());
        }

        [TestMethod]
        public void Users_GetUserById_ThatExists() {
            // Arrange
            var repo = new UsersRepository(context);
            int id = (context.Users.Where(u => u.Name == NAME_DABS).SingleOrDefault()).Id;

            // Act
            var user = repo.GetUserById(id);

            // Assert
            Assert.AreEqual(NAME_DABS, user.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void Users_GetUserById_ThatDoesntExist() {
            // Arrange
            var repo = new UsersRepository(context);
            int id = (context.Books.OrderByDescending(u => u.Id).FirstOrDefault()).Id;

            // Act
            var user = repo.GetUserById(id+1);

            // Assert
            Assert.Fail("Should have thrown NotFoundException");
        }

        [TestMethod]
        public void Users_DeleteUser_ThatExist() {
            // Arrange
            var repo = new UsersRepository(context);
            int id = (context.Users.Where(u => u.Name == NAME_DABS).SingleOrDefault()).Id;

            // Act
            repo.DeleteUser(id);

            // Assert
            Assert.AreEqual(true, (context.Users.Where(u => u.Name == NAME_DABS).SingleOrDefault()).Deleted);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void Users_DeleteUser_ThatDoesntExist() {
            // Arrange
            var repo = new UsersRepository(context);
            int id = (context.Books.OrderByDescending(u => u.Id).FirstOrDefault()).Id;

            // Act
            repo.DeleteUser(id+1);

            // Assert
            Assert.Fail("Should have thrown NotFoundException");
        }

        [TestMethod]
        public void Users_UpdateUser_ThatExists() {
            // Arrange
            var repo = new UsersRepository(context);
            int id = (context.Users.Where(u => u.Name == NAME_DABS).SingleOrDefault()).Id;
            var updatedUser = new UserView {
                Name = "dabs",
                Address = ADDRESS_DABS,
                Email = EMAIL_DABS,
                PhoneNumber = "55-12345"
            };

            // Act
            repo.UpdateUser(updatedUser, id);

            // Assert
            Assert.AreEqual("dabs", (context.Users.Where(u => u.Id == id).SingleOrDefault()).Name);
            Assert.AreEqual("55-12345", (context.Users.Where(u => u.Id == id).SingleOrDefault()).PhoneNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void Users_UpdateUser_ThatDoesntExists() {
            // Arrange
            var repo = new UsersRepository(context);
            int id = (context.Books.OrderByDescending(u => u.Id).FirstOrDefault()).Id;
            var updatedUser = new UserView {
                Name = "dabs",
                Address = ADDRESS_DABS,
                Email = EMAIL_DABS,
                PhoneNumber = "55-12345"
            };

            // Act
            repo.UpdateUser(updatedUser, id+1);

            // Assert
            Assert.Fail("Should have thrown a NotFoundException");
        }

        [TestMethod]
        public void Users_GetUsersOnLoan_WithValidResult()
        {
            // Arrange
            var repo = new UsersRepository(context);
            int userId = (context.Users.OrderByDescending(u => u.Id).FirstOrDefault()).Id;  



            var book1 = (new Book {
                Title = "Stuff",
                Author = "Things",
                ReleaseDate = new DateTime(1999, 10, 10),
                ISBN = "15252",
                Available = true,
                Deleted = false
            });

            context.Loans.Add(new Loan {
                UserId = userId,
                BookId = book1.Id,
                LoanDate = new DateTime(2017,10,29),
                EndDate = null,
                HasBeenReturned = false
            });

            context.Books.Add(book1);
            context.SaveChanges();

            // Act
            var users = repo.GetUsersOnLoan(new DateTime(2017,10,30));

            // Assert
            Assert.AreEqual(1, users.Count());
            Assert.AreEqual("Daníel B. Sigurgeirsson", (context.Users.Where(u => u.Id == userId).SingleOrDefault()).Name);
        }

        [TestMethod]
        public void Users_GetUsersOnLoan_WithEmptyList()
        {
            // Arrange
            var repo = new UsersRepository(context);
            int userId = (context.Users.OrderByDescending(u => u.Id).FirstOrDefault()).Id;  



            // Act
            var users = repo.GetUsersOnLoan(new DateTime(2017,10,30));

            // Assert
            Assert.AreEqual(0, users.Count());
            Assert.AreEqual("Daníel B. Sigurgeirsson", (context.Users.Where(u => u.Id == userId).SingleOrDefault()).Name);
        }

        [TestMethod]
        public void Users_GetUsersOnLoan_WithTwoResults()
        {
            // Arrange
            var repo = new UsersRepository(context);
            int userId = (context.Users.OrderByDescending(u => u.Id).FirstOrDefault()).Id;  


            var extraUser = new User {
                Name = "Jón",
                Address = "Dúfnahólar 10",
                Email = "jon15@ru.is",
                PhoneNumber = "123-4567",
                Deleted = false
            };
            context.Users.Add(extraUser);
            context.SaveChanges();

            var book1 = (new Book {
                Title = "Stuff",
                Author = "Things",
                ReleaseDate = new DateTime(1999, 10, 10),
                ISBN = "15252",
                Available = true,
                Deleted = false
            });

            context.Loans.Add(new Loan {
                UserId = userId,
                BookId = book1.Id,
                LoanDate = new DateTime(2017,10,29),
                EndDate = null,
                HasBeenReturned = false
            });

            context.Loans.Add(new Loan {
                UserId = extraUser.Id,
                BookId = book1.Id,
                LoanDate = new DateTime(2017,10,25),
                EndDate = null,
                HasBeenReturned = false
            });

            context.Books.Add(book1);
            context.SaveChanges();

            // Act
            var users = repo.GetUsersOnLoan(new DateTime(2017,10,30));

            // Assert
            Assert.AreEqual(2, users.Count());
            Assert.AreEqual("Daníel B. Sigurgeirsson", (context.Users.Where(u => u.Id == userId).SingleOrDefault()).Name);
            Assert.AreEqual("Jón", (context.Users.Where(u => u.Id == extraUser.Id).SingleOrDefault()).Name);
        }
    }
}