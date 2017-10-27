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
    public class SeedController : Controller
    {
        private readonly ISeedService _seedService;

        public SeedController(ISeedService seedService) {
            _seedService = seedService;
        }

        [HttpPost("seed")]
        public IActionResult SeedDatabase() {
            _seedService.SeedDatabase();

            return Ok();
        }
    }
}