using Crud_API.Dtos.Get;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> UserGetByID(int id)
        {
            var users = await _userService.GetById(id);
            if (users == null)
            {
                return BadRequest("No users were found with that id");
            }

            return Ok(users);
        }
    }
}
