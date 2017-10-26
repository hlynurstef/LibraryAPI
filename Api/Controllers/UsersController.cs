using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Exceptions;
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
            try {
            var user = _usersService.GetUserById(userId);
            return Ok(user);
            }
            catch(NotFoundException e) {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Deletes a user from the database by id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("users/{userId}")]
        public IActionResult DeleteUser(int userId) {
            // TODO: Add cascade on delete?
            try {
                _usersService.DeleteUser(userId);
                return StatusCode(204);
            }
            catch(NotFoundException e) {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Updates a user from the database by id.
        /// </summary>
        /// <param name="user">The view model with the updated user information</param>
        /// <param name="userId">The user id corresponding to the user that should be updated</param>
        /// <returns>204 if succesful</returns>
        [HttpPut("users/{userId}")]
        public IActionResult DeleteUser([FromBody] UserView user, int userId) {
            if(user == null){
                return BadRequest();
            }
            if(!ModelState.IsValid){
                return StatusCode(412, "Modelstate is not valid");
            }

            try {
                // TODO: Should this return the newly updated user ? 
                _usersService.UpdateUser(user, userId);
                return StatusCode(204);
            }
            catch(NotFoundException e) {
                return NotFound(e.Message);
            }
        }
    }
}
