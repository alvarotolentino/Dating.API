using System.Threading.Tasks;
using Dating.API.Dtos;
using Dating.API.Models;
using Dating.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Dating.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {

        private readonly IAuthRepository _repository;

        public AuthController(IAuthRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            if (await _repository.UserExists(userForRegisterDto.Username))
            {
                return BadRequest("Username is already taken");
            }

            var userToCreate = new User
            {
                UserName = userForRegisterDto.Username
            };

            var createUser = await _repository.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }
    }
}