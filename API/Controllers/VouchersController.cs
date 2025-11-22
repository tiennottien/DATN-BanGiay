using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VouchersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VouchersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Vouchers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetVouchers()
        {
            return await _context.Vouchers
                .Include(v => v.Category)
                .ToListAsync();
        }

        // GET: api/Vouchers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Voucher>> GetVoucher(int id)
        {
            var voucher = await _context.Vouchers
                .Include(v => v.Category)
                .FirstOrDefaultAsync(v => v.ID == id);

            if (voucher == null)
            {
                return NotFound();
            }

            return voucher;
        }

        // GET: api/Vouchers/Code/ABC123
        [HttpGet("Code/{code}")]
        public async Task<ActionResult<Voucher>> GetVoucherByCode(string code)
        {
            var voucher = await _context.Vouchers
                .Include(v => v.Category)
                .FirstOrDefaultAsync(v => v.Code == code);

            if (voucher == null)
            {
                return NotFound();
            }

            // Check if voucher is valid
            var now = DateTime.Now;
            if (voucher.Status != "active" || 
                (voucher.StartDate.HasValue && voucher.StartDate > now) ||
                (voucher.EndDate.HasValue && voucher.EndDate < now) ||
                (voucher.Quantity.HasValue && voucher.Quantity <= 0))
            {
                return BadRequest(new { message = "Mã giảm giá không hợp lệ hoặc đã hết hạn" });
            }

            return voucher;
        }

        // POST: api/Vouchers
        [HttpPost]
        public async Task<ActionResult<Voucher>> PostVoucher(Voucher voucher)
        {
            voucher.CreateDate = DateTime.Now;
            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVoucher", new { id = voucher.ID }, voucher);
        }

        // PUT: api/Vouchers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoucher(int id, Voucher voucher)
        {
            if (id != voucher.ID)
            {
                return BadRequest();
            }

            _context.Entry(voucher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoucherExists(id))
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

        // DELETE: api/Vouchers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoucher(int id)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                return NotFound();
            }

            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VoucherExists(int id)
        {
            return _context.Vouchers.Any(e => e.ID == id);
        }
    }
}


