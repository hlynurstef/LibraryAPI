using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService) {
            _usersService = usersService;
        }
        // GET /users
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>All users in the library.</returns>
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _usersService.GetUsers();
            return Ok(users);
        }

        // TODO: POST   /users          - Bæta við notanda

        // TODO: GET    /users/{userId} - Sækja upplýsingar um notanda (m.a. lánasögu)

        // TODO: DELETE /users/{userId} - Fjarlæga notanda

        // TODO: PUT    /users/{userId} - Uppfæra notanda

    }
}
