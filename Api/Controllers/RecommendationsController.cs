using LibraryAPI.Services;
using LibraryAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    public class RecommendationsController : Controller
    {
        private readonly IRecommendationsService _recService;

        public RecommendationsController(IRecommendationsService recService) {
            _recService = recService;
        }

        // TODO: GET /users/{userId}/recommendation - Fá lista yfir vænlegar og ólesnar bækur
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