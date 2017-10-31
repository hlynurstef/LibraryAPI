using System;
using System.Collections.Generic;
using System.Linq;
using LibraryAPI.Exceptions;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibraryAPI.UnitTests.ReviewsRepositoryTests {
    [TestClass]
    public class ReviewsRepositoryTests {
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
        private DateTime RELEASE_LOTR = new DateTime (1954, 6, 29);
        private const string ISBN_LOTR = "123456789";

        private const string REVIEW_TEXT_LOTR = "I really liked it";
        private const int STARS_LOTR = 5;
        private const bool AVAILABLE_LOTR = true;
        private const bool DELETED_LOTR = false;
        #endregion

        #region LoanInfo
        private const int USERID_LOAN = 1;
        private const int BOOKID_LOAN = 1;
        private DateTime STARTDATE_LOAN = new DateTime (2017, 10, 29);
        private const bool RETURNED_LOAN = false;
        private DateTime? ENDDATE_LOAN = null;
        #endregion

        private AppDataContext context;

        [TestInitialize]
        public void InitializeTest () {
            var options = new DbContextOptionsBuilder<AppDataContext> ()
                .UseInMemoryDatabase (databaseName: "ReviewsDB")
                .Options;

            context = new AppDataContext (options);

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

            context.Users.Add (user);
            context.Books.Add (book);
            context.SaveChanges ();

            context.Loans.Add (new Loan {
                UserId = user.Id,
                    BookId = book.Id,
                    LoanDate = STARTDATE_LOAN,
                    EndDate = ENDDATE_LOAN,
                    HasBeenReturned = RETURNED_LOAN
            });

            context.Reviews.Add (new Review {
                UserId = user.Id,
                    BookId = book.Id,
                    ReviewText = REVIEW_TEXT_LOTR,
                    Stars = STARS_LOTR
            });

            context.SaveChanges ();
        }

        [TestCleanup]
        public void CleanupTest () {
            context.Database.EnsureDeleted ();
        }

        [TestMethod]
        public void Reviews_GetAllBookReviews_OneReview () {
            // Arrange
            var repo = new ReviewsRepository (context);

            // Act
            var reviews = repo.GetAllBookReviews ();

            // Assert
            Assert.AreEqual (1, reviews.Count ());
            Assert.AreEqual (5, reviews.Where (b => b.ReviewText == "I really liked it").SingleOrDefault ().Stars);
        }

        [TestMethod]
        public void Reviews_GetReviewsForUser_UserExists () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int userId = (context.Users.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            // Act
            var reviews = repo.GetReviewsForUser (userId);

            // Assert
            Assert.AreEqual (1, reviews.Count ());
            Assert.AreEqual ("I really liked it", reviews.ElementAt (0).ReviewText);
            Assert.AreEqual (5, reviews.ElementAt (0).Stars);
        }

        [TestMethod]
        [ExpectedException (typeof (NotFoundException))]
        public void Reviews_GetReviewsForUser_UserDoesNotExist () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int userId = (context.Users.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            // Act
            var reviews = repo.GetReviewsForUser (userId + 1);

            // Assert
            Assert.Fail ("Should have thrown NotFoundException");
        }

        [TestMethod]
        public void Reviews_GetAllReviewsForBook_BookExists () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int bookId = (context.Books.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            // Act
            var reviews = repo.GetAllReviewsForBook (bookId);

            // Assert
            Assert.AreEqual (1, reviews.Count ());
            Assert.AreEqual ("I really liked it", reviews.ElementAt (0).ReviewText);
            Assert.AreEqual (5, reviews.ElementAt (0).Stars);
        }

        [TestMethod]
        [ExpectedException (typeof (NotFoundException))]
        public void Reviews_GetAllReviewsForBook_BookDoesNotExist () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int bookId = (context.Books.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            // Act
            var reviews = repo.GetAllReviewsForBook (bookId + 1);

            // Assert
            Assert.Fail ("Should have thrown NotFoundException");
        }

        [TestMethod]
        public void Reviews_AddReviewToBook_UserAndBookExist () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int bookId = (context.Books.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            var newUser = new User {
                Name = "Gux",
                Address = "Rvk",
                Email = "Gux@gmail.com",
                PhoneNumber = "699-6666",
                Deleted = false
            };

            context.Users.Add (newUser);
            context.SaveChanges ();

            int userId = newUser.Id;

            // Act
            repo.AddReviewToBook (newUser.Id, bookId, new ReviewView {
                ReviewText = "Such wow",
                    Stars = 4
            });

            // Assert
            Assert.AreEqual (2, context.Reviews.Count ());
            Assert.AreEqual ("Such wow", context.Reviews.Where (b => b.ReviewText == "Such wow").SingleOrDefault ().ReviewText);
            Assert.AreEqual (4, context.Reviews.Where (b => b.Stars == 4).SingleOrDefault ().Stars);
        }

        [TestMethod]
        [ExpectedException (typeof (NotFoundException))]
        public void Reviews_AddReviewToBook_UserDoesNotExist () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int bookId = (context.Books.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;
            int userId = (context.Users.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            // Act
            repo.AddReviewToBook (userId + 1, bookId, new ReviewView {
                ReviewText = "Such wow",
                    Stars = 4
            });

            // Assert
            Assert.Fail ("Should have thrown NotFoundException");
        }

        [TestMethod]
        [ExpectedException (typeof (NotFoundException))]
        public void Reviews_AddReviewToBook_BookDoesNotExist () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int bookId = (context.Books.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;
            int userId = (context.Users.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            // Act
            repo.AddReviewToBook (userId, bookId + 1, new ReviewView {
                ReviewText = "Such wow",
                    Stars = 4
            });

            // Assert
            Assert.Fail ("Should have thrown NotFoundException");
        }

        [TestMethod]
        public void Reviews_DeleteUsersBookReview_UserAndBookAndReviewExist () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int bookId = (context.Books.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;
            int userId = (context.Users.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            // Act
            repo.DeleteUsersBookReview (userId, bookId);

            // Assert
            Assert.AreEqual (0, context.Reviews.Count ());
        }

        [TestMethod]
        [ExpectedException (typeof (NotFoundException))]
        public void Reviews_DeleteUsersBookReview_UserDoesNotExist () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int bookId = (context.Books.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;
            int userId = (context.Users.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            // Act
            repo.DeleteUsersBookReview (userId + 1, bookId);

            // Assert
            Assert.Fail ("Should have thrown NotFoundException");
        }

        [TestMethod]
        [ExpectedException (typeof (NotFoundException))]
        public void Reviews_DeleteUsersBookReview_BookDoesNotExist () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int bookId = (context.Books.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;
            int userId = (context.Users.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            // Act
            repo.DeleteUsersBookReview (userId, bookId + 1);

            // Assert
            Assert.Fail ("Should have thrown NotFoundException");
        }

        [TestMethod]
        [ExpectedException (typeof (NotFoundException))]
        public void Reviews_DeleteUsersBookReview_ReviewDoesNotExist () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int bookId = (context.Books.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;
            int userId = (context.Users.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            var newUser = new User {
                Name = "Gux",
                Address = "Rvk",
                Email = "Gux@gmail.com",
                PhoneNumber = "699-6666",
                Deleted = false
            };

            context.Users.Add (newUser);
            context.SaveChanges ();

            // Act
            repo.DeleteUsersBookReview (newUser.Id, bookId);

            // Assert
            Assert.Fail ("Should have thrown NotFoundException");
        }

        [TestMethod]
        public void Reviews_UpdateUsersBookRewiew_UserAndBookAndReviewExist () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int bookId = (context.Books.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;
            int userId = (context.Users.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            // Act
            repo.UpdateBooksUserReview (bookId, userId, new ReviewView {
                ReviewText = "Such wow",
                    Stars = 4
            });

            // Assert
            Assert.AreEqual (1, context.Reviews.Count ());
            Assert.AreEqual ("Such wow", context.Reviews.Where (b => b.ReviewText == "Such wow").SingleOrDefault ().ReviewText);
            Assert.AreEqual (4, context.Reviews.Where (b => b.Stars == 4).SingleOrDefault ().Stars);
        }

        [TestMethod]
        [ExpectedException (typeof (NotFoundException))]
        public void Reviews_UpdateUsersBookRewiew_UserDoesNotExist () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int bookId = (context.Books.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;
            int userId = (context.Users.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            // Act
            repo.UpdateBooksUserReview (bookId, userId + 1, new ReviewView {
                ReviewText = "Such wow",
                    Stars = 4
            });

            // Assert
            Assert.Fail ("Should have thrown NotFoundException");
        }

        [TestMethod]
        [ExpectedException (typeof (NotFoundException))]
        public void Reviews_UpdateUsersBookRewiew_BookDoesNotExist () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int bookId = (context.Books.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;
            int userId = (context.Users.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            // Act
            repo.UpdateBooksUserReview (bookId + 1, userId, new ReviewView {
                ReviewText = "Such wow",
                    Stars = 4
            });

            // Assert
            Assert.Fail ("Should have thrown NotFoundException");
        }

        [TestMethod]
        [ExpectedException (typeof (NotFoundException))]
        public void Reviews_UpdateUsersBookRewiew_ReviewDoesNotExist () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int bookId = (context.Books.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;
            int userId = (context.Users.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            var newUser = new User {
                Name = "Gux",
                Address = "Rvk",
                Email = "Gux@gmail.com",
                PhoneNumber = "699-6666",
                Deleted = false
            };

            context.Users.Add (newUser);
            context.SaveChanges ();
            // Act
            repo.UpdateBooksUserReview (bookId, newUser.Id, new ReviewView {
                ReviewText = "Such wow",
                    Stars = 4
            });

            // Assert
            Assert.Fail ("Should have thrown NotFoundException");
        }

        [TestMethod]
        public void Reviews_GetBookReviewFromUser_BookAndUserExist () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int bookId = (context.Books.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;
            int userId = (context.Users.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            // Act
            var review = repo.GetBookReviewFromUser (userId, bookId);

            // Assert
            Assert.AreEqual (1, context.Reviews.Count ());
            Assert.AreEqual ("I really liked it", review.ReviewText);
            Assert.AreEqual (5, review.Stars);
            Assert.AreEqual (userId, review.UserId);
            Assert.AreEqual (bookId, review.BookId);
        }

        [TestMethod]
        [ExpectedException (typeof (NotFoundException))]
        public void Reviews_GetBookReviewFromUser_BookDoesNotExist () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int bookId = (context.Books.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;
            int userId = (context.Users.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            // Act
            var review = repo.GetBookReviewFromUser (userId, bookId + 1);

            // Assert
            Assert.Fail ("Should have thrown NotFoundException");
        }

        [TestMethod]
        [ExpectedException (typeof (NotFoundException))]
        public void Reviews_GetBookReviewFromUser_UserDoesNotExist () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int bookId = (context.Books.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;
            int userId = (context.Users.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            // Act
            var review = repo.GetBookReviewFromUser (userId + 1, bookId);

            // Assert
            Assert.Fail ("Should have thrown NotFoundException");
        }

        [TestMethod]
        [ExpectedException (typeof (NotFoundException))]
        public void Reviews_GetBookReviewFromUser_ReviewDoesNotExist () {
            // Arrange
            var repo = new ReviewsRepository (context);
            int bookId = (context.Books.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;
            int userId = (context.Users.OrderByDescending (b => b.Id).FirstOrDefault ()).Id;

            var newUser = new User {
                Name = "Gux",
                Address = "Rvk",
                Email = "Gux@gmail.com",
                PhoneNumber = "699-6666",
                Deleted = false
            };

            context.Users.Add (newUser);
            // Act
            var review = repo.GetBookReviewFromUser (newUser.Id, bookId);

            // Assert
            Assert.Fail ("Should have thrown NotFoundException");
        }
    }
}