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

namespace LibraryAPI.UnitTests.ReviewsRepositoryTests
{
    [TestClass]
    public class ReviewsRepositoryTests
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
                .UseInMemoryDatabase(databaseName: "ReviewsFB")
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

            var review = 

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
        public void Reviews_GetAllBookReviews_OneReview()
        {
            // Arrange
            var repo = new ReviewsRepository(context);
            
            // Act
            var reviews = repo.GetAllBookReviews();

            // Assert
            Assert.AreEqual(1, reviews.Count());
            Assert.AreEqual(5, reviews.Where(b => b.ReviewText == "I really liked it").SingleOrDefault().Stars);
        }

        [TestMethod]
        public void Reviews_GetReviewsForUser_UserExists()
        {
            // Arrange
            var repo = new ReviewsRepository(context);
            int userId = (context.Users.OrderByDescending(b => b.Id).FirstOrDefault()).Id;

            // Act
            var reviews = repo.GetReviewsForUser(userId);

            // Assert
            Assert.AreEqual(1, reviews.Count());
            Assert.AreEqual("I really liked it", reviews.ElementAt(0).ReviewText);
            Assert.AreEqual(5, reviews.ElementAt(0).Stars);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void Reviews_GetReviewsForUser_UserDoesNotExist()
        {
            // Arrange
            var repo = new ReviewsRepository(context);
            int userId = (context.Users.OrderByDescending(b => b.Id).FirstOrDefault()).Id;

            // Act
            var reviews = repo.GetReviewsForUser(userId + 1);

            // Assert
            Assert.Fail("Should have thrown NotFoundException");
        }
    }
}
