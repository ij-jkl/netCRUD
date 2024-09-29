using Crud_API.Dtos.Get;
using Crud_API.Dtos.Post;
using Crud_API.Dtos.Put;
using Crud_API.Entities;
using Crud_API.Services.IServices; 
using Microsoft.AspNetCore.Mvc;

namespace Crud_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<List<UserGetDto>>> UsersGetAll()
        {
            var users = await _userService.GetAll();  

            if (users == null || users.Count == 0)
            {
                return NotFound("No users found");
            }

            return Ok(users);
        }

        // GET: api/user/ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UserEntity>> UserGetByID(int id)
        {
            var users = await _userService.GetById(id);
            if (users == null)
            {
                return BadRequest("No users were found with that id");
            }

            return Ok(users);
        }

        // POST: api/user
        [HttpPost]
        public async Task<ActionResult<UserPostDto>> UserCreatePost(UserPostDto userPostDto)
        {
            if (userPostDto == null)
            {
                return BadRequest("User data is null");
            }

            var createdUser = await _userService.CreateUser(userPostDto);

            if (createdUser == null)
            {
                return BadRequest("User could not be created");
            }

            return CreatedAtAction(nameof(UserGetByID), new { id = createdUser.Id }, createdUser);
        }

        // PUT: api/user/ID
        [HttpPut("{id}")]
        public async Task<ActionResult<UserPutDto>> UserUpdatePut(int id, [FromBody] UserPostDto userPostDto)
        {
            if (userPostDto == null)
            {
                return BadRequest("User data is null");
            }

            // The id of the Url has to match the id we are sending in the put method
            if (id != userPostDto.Id)
            {
                return BadRequest("User ID mismatch");
            }

            try
            {
                var updatedUser = await _userService.UpdateUser(userPostDto);

                if (updatedUser == null)
                {
                    return NotFound("User not found for update");
                }

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                // Handles exceptions during updating
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
