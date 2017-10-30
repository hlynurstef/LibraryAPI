using System;
using System.Linq;
using LibraryAPI.Exceptions;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibraryAPI.UnitTests
{
    [TestClass]
    public class UsersRepositoryTests
    {
        private const string NAME_DABS  = "Daníel B. Sigurgeirsson";
        private const string ADDRESS_DABS  = "Rauðrófustígur 14";
        private const string EMAIL_DABS  = "dabs@ru.is";
        private const string PHONE_DABS = "1234567";

        [TestMethod]
        public void AddUser_OneUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDataContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            // Act
            // Run the test against one instance of the context
            using (var context = new AppDataContext(options))
            {
                var repo = new UsersRepository(context);
                repo.AddUser(new UserView {
                    Name = NAME_DABS,
                    Address = ADDRESS_DABS,
                    Email = EMAIL_DABS,
                    PhoneNumber = PHONE_DABS
                });
            }

            // Assert
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new AppDataContext(options))
            {
                Assert.AreEqual(1, context.Users.Count());
                Assert.AreEqual(NAME_DABS, context.Users.Single().Name);
                Assert.AreEqual(ADDRESS_DABS, context.Users.Single().Address);
                Assert.AreEqual(EMAIL_DABS, context.Users.Single().Email);
                Assert.AreEqual(PHONE_DABS, context.Users.Single().PhoneNumber);
            }
        }
        
        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void AddUser_SameUserTwice()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDataContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            // Run the test against one instance of the context
            using (var context = new AppDataContext(options))
            {
                var repo = new UsersRepository(context);
                var user = new UserView {
                    Name = NAME_DABS,
                    Address = ADDRESS_DABS,
                    Email = EMAIL_DABS,
                    PhoneNumber = PHONE_DABS
                };

                repo.AddUser(user);
                
                try {
                    // Act
                    
                    repo.AddUser(user);
                    Assert.Fail();
                }
                catch(AlreadyExistsException e) {
                    // Assert
                    Assert.AreEqual(e.Message, "The request could not be completed due to a conflict with the current state of the resource.");
                }
            }
        }
    }
}
