using System;
using System.Linq;
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
        public void AddUserToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDataContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            // Act
            // Run the test against one instance of the context
            using (var context = new AppDataContext(options))
            {
                var service = new UsersRepository(context);
                service.AddUser(new UserView {
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
            }
        }
        [TestMethod]
        public void GetBookByIdFromDatabase(){
            
        }
    }
}
