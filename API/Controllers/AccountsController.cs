using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // POST: api/Accounts/Register
        [HttpPost("Register")]
        public async Task<ActionResult<Account>> Register(Account account)
        {
            // Check if email already exists
            if (await _context.Accounts.AnyAsync(a => a.Email == account.Email))
            {
                return BadRequest(new { message = "Email đã tồn tại" });
            }

            // Hash password (simple hash, should use proper hashing in production)
            account.Password = HashPassword(account.Password);
            account.CreateDate = DateTime.Now;
            account.Role = account.Role ?? "user";

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            // Remove password from response
            account.Password = string.Empty;

            return CreatedAtAction("GetAccount", new { id = account.ID }, account);
        }

        // POST: api/Accounts/Login
        [HttpPost("Login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest request)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Email == request.Email);

            if (account == null || account.Password != HashPassword(request.Password))
            {
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng" });
            }

            // Get customer or employee info
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.AccountID == account.ID);
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.AccountID == account.ID);

            return Ok(new
            {
                account = new
                {
                    id = account.ID,
                    email = account.Email,
                    role = account.Role,
                    phoneNumber = account.PhoneNumber
                },
                customer = customer,
                employee = employee
            });
        }

        // PUT: api/Accounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, Account account)
        {
            if (id != account.ID)
            {
                return BadRequest();
            }

            // If password is being updated, hash it
            if (!string.IsNullOrEmpty(account.Password))
            {
                var existingAccount = await _context.Accounts.AsNoTracking()
                    .FirstOrDefaultAsync(a => a.ID == id);
                if (existingAccount?.Password != account.Password)
                {
                    account.Password = HashPassword(account.Password);
                }
            }

            account.UpdateDate = DateTime.Now;
            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.ID == id);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}


