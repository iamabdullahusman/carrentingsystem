using carrentingsystem.Context;
using carrentingsystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace carrentingsystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly string _jwtSecretKey = "veryverysecret................................................................................";


        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<dynamic>> Login(Loginmodel loginmodel)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginmodel.Email && u.Password == loginmodel.Password);

            if (existingUser == null)
            {
                return NotFound("Invalid email or password");
            }
            //return Ok(existingUser);
            var token = GenerateJwtToken(existingUser);
            return Ok(new { Token = token, User = existingUser });// Return the authenticated user
        }
        [HttpPost("signup")]
        public async Task<ActionResult<User>> SignUp(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the email is already registered
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                return Conflict("Email is already registered.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
        private string GenerateJwtToken(User user)
        {

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecretKey);
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                // Add additional claims as needed
                 new Claim(ClaimTypes.Role, user.Role.ToString())
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1), // Token expiry time
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}


