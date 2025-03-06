using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAPI.Data;
using UserAPI.Models;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserDbContext _context;

        public UsersController(UserDbContext context)
        {
            _context = context;
        }

        // REGISTER: api/Users/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(User user)
        {
            // Cek apakah username sudah digunakan
            if (_context.Users.Any(u => u.Username == user.Username))
            {
                return BadRequest(new { message = "Username sudah digunakan" });
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new { user.Id, user.Username });
        }

        // LOGIN: api/Users/login
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest loginRequest)
        {
            // Cek apakah user dengan username tersebut ada di database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.Username);
            if (user == null)
            {
                return Unauthorized(new { message = "Username atau password salah" });
            }

            return Ok(new { message = "Login berhasil", userId = user.Id });
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    // Model untuk request login
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
