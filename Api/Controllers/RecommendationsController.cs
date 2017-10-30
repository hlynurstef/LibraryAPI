using LibraryAPI.Services;
using LibraryAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    /// <summary>
    /// A controller class for book recommendations
    /// </summary>
    public class RecommendationsController : Controller
    {
        /// <summary>
        /// The service private variable
        /// </summary>
        private readonly IRecommendationsService _recService;

        /// <summary>
        /// Constructor for RecommendationsController
        /// </summary>
        /// <param name="recService">Dependency injection for the service</param>
        public RecommendationsController(IRecommendationsService recService) {
            _recService = recService;
        }

        // GET /users/{userId}/recommendation - Fá lista yfir vænlegar og ólesnar bækur
        /// <summary>
        /// Returns a list of recommendations for the user.
        /// </summary>
        /// <remarks>
        /// This is done by finding books the user has read and finding books by the same authors that the user has not read
        /// If no such books are found, we recommend five random books that the user has not read
        /// </remarks>
        /// <param name="userId">The id of the user in question</param>
        /// <returns>A list of books that we recommend to the user</returns>
        [HttpGet("users/{userId}/recommendation")]
        public IActionResult GetRecommendationsForUser(int userId) {
            try {
                return Ok(_recService.GetRecommendationsForUser(userId));
            }
            catch(NotFoundException e) {
                return NotFound(e.Message);
            }
        }
    }
}