using Crud_API.Dtos.Get;
using Crud_API.Dtos.Post;
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
    }
}
