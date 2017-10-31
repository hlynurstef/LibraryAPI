using System;
using System.Linq;
using LibraryAPI.Exceptions;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.DTOModels;
using System.Collections.Generic;

namespace LibraryAPI.UnitTests.RecommendationsRepositoryTests
{
    [TestClass]
    public class RecommendationsRepositoryTests
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

        private const string REVIEW_TEXT_LOTR = "I really liked it";
        private const int STARS_LOTR = 5;
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
                .UseInMemoryDatabase(databaseName: "RecommendationsDB")
                .Options;

            context = new AppDataContext(options);

            var user = new User {
                Name = NAME_DABS,
                Address = ADDRESS_DABS,
                Email = EMAIL_DABS,
                PhoneNumber = PHONE_DABS,
                Deleted = DELETED_DABS
            };
            var book = new Book {
                Title = TITLE_LOTR,
                Author = AUTHOR_LOTR,
                ReleaseDate = RELEASE_LOTR,
                ISBN = ISBN_LOTR,
                Available = AVAILABLE_LOTR,
                Deleted = DELETED_LOTR
            };

            context.Users.Add(user);
            context.Books.Add(book);
            context.SaveChanges();

            context.Loans.Add(new Loan {
                UserId = user.Id,
                BookId = book.Id,
                LoanDate = STARTDATE_LOAN,
                EndDate = ENDDATE_LOAN,
                HasBeenReturned = RETURNED_LOAN
            });

            context.Reviews.Add(new Review {
                UserId = user.Id,
                BookId = book.Id,
                ReviewText = REVIEW_TEXT_LOTR,
                Stars = STARS_LOTR
            });

            context.SaveChanges();
        }

        [TestCleanup]
        public void CleanupTest() {
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void Recommendations_GetRecommendationsForUser_WithOneMatch()
        {
            // Arrange
            var repo = new RecommendationsRepository(context);
            int userId = (context.Users.OrderByDescending(b => b.Id).FirstOrDefault()).Id;

            context.Books.Add( new Book {
                                Title = "Silmarillion",
                                Author = AUTHOR_LOTR,
                                ReleaseDate = new DateTime(1977, 9, 15),
                                ISBN = "0987654321",
                                Available = true,
                                Deleted = false
            });

            context.SaveChanges();

            // Act
            var recommendations = repo.GetRecommendationsForUser(userId);

            // Assert
            Assert.AreEqual(1, recommendations.Count());
            Assert.AreEqual("Silmarillion", recommendations.Where(b => b.Title == "Silmarillion").SingleOrDefault().Title);
            Assert.AreEqual("J.R.R. Tolkien", recommendations.Where(b => b.Title == "Silmarillion").SingleOrDefault().Author);
            Assert.AreEqual("0987654321", recommendations.Where(b => b.Title == "Silmarillion").SingleOrDefault().ISBN);
        }

        [TestMethod]
        public void Recommendations_GetRecommendationsForUser_WithNoMatchSoRandomResults()
        {
            // Arrange
            var repo = new RecommendationsRepository(context);
            int userId = (context.Users.OrderByDescending(b => b.Id).FirstOrDefault()).Id;

            for(int i = 1; i <= 5; i++)
            {
                context.Books.Add( new Book {
                                    Title = ("Book " + i),
                                    Author = ("Author " + i),
                                    ReleaseDate = new DateTime(1977, 9, 15),
                                    ISBN = i.ToString(),
                                    Available = true,
                                    Deleted = false
                });
            }

            context.SaveChanges();

            // Act
            var recommendations = repo.GetRecommendationsForUser(userId);

            // Assert
            Assert.AreEqual(5, recommendations.Count());
            Assert.AreEqual("Book 1", recommendations.Where(b => b.Title == "Book 1").SingleOrDefault().Title);
            Assert.AreEqual("Book 2", recommendations.Where(b => b.Title == "Book 2").SingleOrDefault().Title);
            Assert.AreEqual("Book 3", recommendations.Where(b => b.Title == "Book 3").SingleOrDefault().Title);
            Assert.AreEqual("Book 4", recommendations.Where(b => b.Title == "Book 4").SingleOrDefault().Title);
            Assert.AreEqual("Book 5", recommendations.Where(b => b.Title == "Book 5").SingleOrDefault().Title);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void Recommendations_GetRecommendationsForUser_InvalidUserId()
        {
            // Arrange
            var repo = new RecommendationsRepository(context);
            int userId = Int32.MaxValue;

            // Act
            var recommendations = repo.GetRecommendationsForUser(userId);

            // Assert
            Assert.Fail("Should have thrown NotFoundException");
        }
    }
}