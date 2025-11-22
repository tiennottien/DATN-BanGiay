using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using System.Linq;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.Voucher)
                .Include(o => o.AddressDelivery)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductDetail)
                        .ThenInclude(pd => pd.Product)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductDetail)
                        .ThenInclude(pd => pd.Color)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductDetail)
                        .ThenInclude(pd => pd.Size)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductDetail)
                        .ThenInclude(pd => pd.Image)
                .ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.Voucher)
                .Include(o => o.AddressDelivery)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductDetail)
                        .ThenInclude(pd => pd.Product)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductDetail)
                        .ThenInclude(pd => pd.Color)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductDetail)
                        .ThenInclude(pd => pd.Size)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductDetail)
                        .ThenInclude(pd => pd.Image)
                .FirstOrDefaultAsync(o => o.ID == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // GET: api/Orders/Customer/5
        [HttpGet("Customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByCustomer(int customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerID == customerId)
                .Include(o => o.Voucher)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductDetail)
                        .ThenInclude(pd => pd.Product)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductDetail)
                        .ThenInclude(pd => pd.Image)
                .OrderByDescending(o => o.CreateDate)
                .ToListAsync();
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            // Clear navigation properties to avoid binding issues
            order.Customer = null;
            order.Employee = null;
            order.Voucher = null;
            order.AddressDelivery = null;
            
            // Clear navigation properties in OrderDetails
            if (order.OrderDetails != null)
            {
                foreach (var detail in order.OrderDetails)
                {
                    detail.ProductDetail = null;
                    detail.Order = null;
                }
            }

            order.CreateDate = DateTime.Now;
            order.Code = "ORD" + DateTime.Now.ToString("yyyyMMddHHmmss");

            // Calculate totals
            if (order.OrderDetails != null && order.OrderDetails.Any())
            {
                order.SubTotal = order.OrderDetails.Sum(od => (od.Price ?? 0) * (od.Quantity ?? 0));
                
                // Apply voucher discount if exists
                if (order.VoucherID.HasValue)
                {
                    var voucher = await _context.Vouchers.FindAsync(order.VoucherID.Value);
                    if (voucher != null)
                    {
                        decimal discountAmount = 0;
                        if (voucher.Type == "percentage")
                        {
                            discountAmount = (order.SubTotal ?? 0) * (voucher.Discount ?? 0) / 100;
                            if (voucher.MaxDiscount.HasValue && discountAmount > voucher.MaxDiscount.Value)
                            {
                                discountAmount = voucher.MaxDiscount.Value;
                            }
                        }
                        else if (voucher.Type == "fixed")
                        {
                            discountAmount = voucher.Discount ?? 0;
                        }
                        order.Discount = discountAmount;
                    }
                }

                order.Total = (order.SubTotal ?? 0) - (order.Discount ?? 0);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Update product quantities
            if (order.OrderDetails != null)
            {
                foreach (var detail in order.OrderDetails)
                {
                    if (detail.ProductDetailID.HasValue)
                    {
                        var productDetail = await _context.ProductDetails.FindAsync(detail.ProductDetailID.Value);
                        if (productDetail != null)
                        {
                            productDetail.Quantity = (productDetail.Quantity ?? 0) - (detail.Quantity ?? 0);
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.ID }, order);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.ID)
            {
                return BadRequest();
            }

            order.UpdateDate = DateTime.Now;
            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.ID == id);
        }
    }
}

