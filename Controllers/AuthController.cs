using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dating.API.Dtos;
using Dating.API.Models;
using Dating.API.Repository;
using DatingApp.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Dating.API.Controllers {
    [Route ("api/[controller]")]
    public class AuthController : Controller {

        private readonly IAuthRepository _repository;
        private readonly IConfiguration _configuration;

        public AuthController (IAuthRepository repository, IConfiguration configuration) {
            _repository = repository;
            _configuration = configuration;
        }

        [HttpPost ("register")]
        public async Task<IActionResult> Register ([FromBody] UserForRegisterDto userForRegisterDto) {
            if (!string.IsNullOrWhiteSpace (userForRegisterDto.Username)) {
                userForRegisterDto.Username = userForRegisterDto.Username.ToLower ();

            }

            if (await _repository.UserExists (userForRegisterDto.Username)) {
                ModelState.AddModelError ("Username", "Username already exists");
            }

            if (!ModelState.IsValid)
                return BadRequest (ModelState);

            var userToCreate = new User {
                UserName = userForRegisterDto.Username
            };

            var createUser = await _repository.Register (userToCreate, userForRegisterDto.Password);

            return StatusCode (201);
        }

        [HttpPost ("login")]
        public async Task<IActionResult> Login ([FromBody] UserForLoginDto userForLoginDto) {
            throw new Exception ("Computer says no!");

            var userFromRepo = await _repository.Login (userForLoginDto.Username.ToLower (), userForLoginDto.Password);
            if (userFromRepo == null) {
                return Unauthorized ();
            }

            var tokenHandler = new JwtSecurityTokenHandler ();
            var key = Encoding.ASCII.GetBytes (_configuration.GetSection ("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity (new Claim[] {
                new Claim (ClaimTypes.NameIdentifier, userFromRepo.Id.ToString ()),
                new Claim (ClaimTypes.Name, userFromRepo.UserName)
                }),
                Expires = DateTime.Now.AddDays (1),
                SigningCredentials = new SigningCredentials (new SymmetricSecurityKey (key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken (tokenDescriptor);
            var tokenString = tokenHandler.WriteToken (token);
            return Ok (new { tokenString });
        }

    }
}