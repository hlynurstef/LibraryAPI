using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Models.ViewModels;
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

        // POST /users
        /// <summary>
        /// Takes the User View model from the body of the request and adds
        /// a new user entry to the database.
        /// </summary>
        /// <param name="user">The UserView object from the request body</param>
        /// <returns>The newly created user</returns>
        [HttpPost("users")]
        public IActionResult AddUser([FromBody] UserView user) {
            if(user == null){
                return BadRequest();
            }
            if(!ModelState.IsValid){
                return StatusCode(412, "Modelstate is not valid");
            }

            var newUser = _usersService.AddUser(user);
            return CreatedAtRoute("GetUserById", new {userId = newUser.Id}, newUser);
        }

        // GET /users/{userId} - Sækja upplýsingar um notanda (m.a. lánasögu)
        /// <summary>
        /// Gets information about a specific user by id (including loan history)
        /// </summary>
        /// <param name="userId">The Id of the user to get</param>
        /// <returns>The requested User</returns>
        [HttpGet("users/{userId}", Name = "GetUserById")]
        public IActionResult GetUserById(int userId) {
            var user = _usersService.GetUserById(userId);
            return Ok(user);
        }

        

        // TODO: DELETE /users/{userId} - Fjarlæga notanda

        // TODO: PUT    /users/{userId} - Uppfæra notanda

    }
}
