using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Exceptions;

namespace LibraryAPI.Controllers
{
    /// <summary>
    /// A class that handles seeding the database with the given dataset
    /// </summary>
    public class SeedController : Controller
    {
        private readonly ISeedService _seedService;

        /// <summary>
        /// Constructor for the seed controller
        /// </summary>
        /// <param name="seedService">Dependency injection for the seedservice</param>
        public SeedController(ISeedService seedService) {
            _seedService = seedService;
        }

        /// <summary>
        /// Post function, send a post request on this route in order to seed the database.
        /// </summary>
        /// <returns>200 ok</returns>
        [HttpPost("seed")]
        public IActionResult SeedDatabase() {
            _seedService.SeedDatabase();

            return Ok();
        }
    }
}