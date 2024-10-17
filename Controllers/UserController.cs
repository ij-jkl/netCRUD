using Crud_API.Commons;
using Crud_API.Dtos.Get;
using Crud_API.Dtos.Login;
using Crud_API.Dtos.Post;
using Crud_API.Dtos.Put;
using Crud_API.Entities;
using Crud_API.Services.IServices; 
using Microsoft.AspNetCore.Mvc;

namespace Crud_API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseObjectJsonDto>> UsersGetAll()
        {
            return await _userService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseObjectJsonDto>> UserGetByID(int id)
        {
            return await _userService.GetById(id);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseObjectJsonDto>> UserCreatePost(UserPostDto userPostDto)
        {
            return await _userService.CreateUser(userPostDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseObjectJsonDto>> UserUpdatePut(int id, [FromBody] UserPutDto userPutDto)
        {
            return await _userService.UpdateUser(id, userPutDto);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseObjectJsonDto>> UserDelete(int id)
        {
            return await _userService.DeleteUser(id);
        }

        [HttpPost("verify")]
        public async Task<ActionResult<ResponseObjectJsonDto>> VerifyUser([FromBody] LoginDto loginDto)
        {
            return await _userService.VerifyUser(loginDto);
        }

        [HttpGet("exists/{username}")]
        public async Task<ActionResult<ResponseObjectJsonDto>> CheckUsernameExists(string username)
        {
            return await _userService.UserExists(username);
        }
    }
}
