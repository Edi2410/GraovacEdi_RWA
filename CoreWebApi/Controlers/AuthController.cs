using CoreWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CoreWebApi.Models;
using AutoMapper;
using DAL.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace CoreWebApi.Controlers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly RwaMoviesContext _context;
        private readonly IMapper _mapper;

        public AuthController(RwaMoviesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: AuthController
        [HttpPost]
        public ActionResult<Tokens> GetTokens()
        {
            // Get secret key bytes
            var tokenKey = Encoding.UTF8.GetBytes("12345678901234567890123456789012");

            // Create a token descriptor (represents a token, kind of a "template" for token)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            // Create token using that descriptor, serialize it and return it
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var serializedToken = tokenHandler.WriteToken(token);

            return new Tokens
            {
                Token = serializedToken
            };
        }


        [HttpPost("[action]")]
        public ActionResult<User> Register([FromBody] UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Username: Normalize and check if username exists
                var normalizedUsername = request.Username.ToLower().Trim();
                if (_context.Users.Any(x => x.Username.Equals(normalizedUsername)))
                    throw new InvalidOperationException("Username already exists");

                // Password: Salt and hash password
                byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
                string b64Salt = Convert.ToBase64String(salt);

                byte[] hash =
                    KeyDerivation.Pbkdf2(
                        password: request.Password,
                        salt: salt,
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 100000,
                        numBytesRequested: 256 / 8);
                string b64Hash = Convert.ToBase64String(hash);

                // SecurityToken: Random security token
                byte[] securityToken = RandomNumberGenerator.GetBytes(256 / 8);
                string b64SecToken = Convert.ToBase64String(securityToken);

                // Id: Next id
                int nextId = 1;
                if (_context.Users.Any())
                {
                    nextId = _context.Users.Max(x => x.Id) + 1;
                }

                // New user
                var newUser = new DAL.Models.User
                {
                    Id = nextId,
                    Username = request.Username,
                    Email = request.Email,
                    Phone = request.Phone,
                    IsConfirmed = false,
                    SecurityToken = b64SecToken,
                    PwdSalt = b64Salt,
                    PwdHash = b64Hash
                };
                _context.Users.Add(newUser);
                _context.SaveChangesAsync();
                return Ok(new UserRegisterResponse
                {
                    Id = newUser.Id,
                    SecurityToken = newUser.SecurityToken
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
