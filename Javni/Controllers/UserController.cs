using System.Security.Cryptography;
using System.Text;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Javni.Models;
using AutoMapper;

namespace Javni.Controllers
{
    public class UserController : Controller
    {
        private readonly RwaMoviesContext _context;
        private readonly IMapper _mapper;
        public UserController(RwaMoviesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Login()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUser userData)
        {
            ModelState.Clear();
            
            var DALUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userData.Username);

            if (DALUser != null)
            {
                string hashed = HashPwd(userData.Password, Convert.FromBase64String(DALUser.PwdSalt));

                if (hashed == DALUser.PwdHash)
                {
                    CookieOptions cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(1),
                        HttpOnly = true
                    };
                    Response.Cookies.Append("username", DALUser.Username, cookieOptions);

                    return Redirect("/Videos/index");
                }
                else
                {
                    ModelState.AddModelError("Password", "Wrong password");
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("username");
            return Redirect("/User/Login");
        }

        public IActionResult Profile()
        {
            string username = Request.Cookies["username"];
            var user = _context.Users.Include(v => v.CountryOfResidence).FirstOrDefault(u => u.Username == username);
            var BLUser = _mapper.Map<IEnumerable<User>>(user);
            return View(BLUser);
        }

        public IActionResult ChangePass()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePass(LoginUser user)
        {
            ModelState.Clear();
            string username = Request.Cookies["username"];
            var DALUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (DALUser != null)
            {
                string hashed = HashPwd(user.Password, Convert.FromBase64String(DALUser.PwdSalt));

                byte[] saltBytes = Salt();
                string saltString = Convert.ToBase64String(saltBytes);

                DALUser.PwdHash = HashPwd(user.Password, saltBytes);
                DALUser.PwdSalt = saltString;

                _context.Update(DALUser);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Videos");

            }

            return View(nameof(ChangePass));
        }

        public IActionResult Register()
        {
            ViewData["CountryOfResidenceId"] = new SelectList(_context.Countries, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUser user)
        {
            ModelState.Clear();

            byte[] saltBytes = Salt();
            string saltString = Convert.ToBase64String(saltBytes);

            string randomToken = GenerateRandomToken();

            User userForDb = new User
            {
                CreatedAt = DateTime.Now,
                DeletedAt = null,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PwdHash = HashPwd(user.Password, saltBytes),
                PwdSalt = saltString,
                Phone = user.Phone,
                IsConfirmed = false,
                SecurityToken = randomToken,
                CountryOfResidenceId = user.CountryOfResidenceId
            };

            if (ModelState.IsValid)
            {
                _context.Add(userForDb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }

            ViewData["CountryOfResidenceId"] = new SelectList(_context.Countries, "Id", "Id", user.CountryOfResidenceId);
            return View(user);
        }

        private string GenerateRandomToken()
        {
            int length = 6;
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            return new string(result);
        }

        private byte[] Salt()
        {
            int saltSize = 16;
            byte[] saltBytes = new byte[saltSize];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            return saltBytes;
        }

        private string HashPwd(string password, byte[] saltBytes)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];

                Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
                Buffer.BlockCopy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);

                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}