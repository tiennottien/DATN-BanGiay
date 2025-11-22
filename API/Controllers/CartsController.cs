using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            return await _context.Carts
                .Include(c => c.Customer)
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.ProductDetail)
                        .ThenInclude(pd => pd.Product)
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.ProductDetail)
                        .ThenInclude(pd => pd.Color)
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.ProductDetail)
                        .ThenInclude(pd => pd.Size)
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.ProductDetail)
                        .ThenInclude(pd => pd.Image)
                .ToListAsync();
        }

        // GET: api/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
            var cart = await _context.Carts
                .Include(c => c.Customer)
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.ProductDetail)
                        .ThenInclude(pd => pd.Product)
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.ProductDetail)
                        .ThenInclude(pd => pd.Color)
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.ProductDetail)
                        .ThenInclude(pd => pd.Size)
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.ProductDetail)
                        .ThenInclude(pd => pd.Image)
                .FirstOrDefaultAsync(c => c.ID == id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        // GET: api/Carts/Customer/5
        [HttpGet("Customer/{customerId}")]
        public async Task<ActionResult<Cart>> GetCartByCustomer(int customerId)
        {
            var cart = await _context.Carts
                .Include(c => c.Customer)
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.ProductDetail)
                        .ThenInclude(pd => pd.Product)
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.ProductDetail)
                        .ThenInclude(pd => pd.Color)
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.ProductDetail)
                        .ThenInclude(pd => pd.Size)
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.ProductDetail)
                        .ThenInclude(pd => pd.Image)
                .FirstOrDefaultAsync(c => c.CustomerID == customerId && c.EndDate == null);

            if (cart == null)
            {
                // Create new cart if doesn't exist
                cart = new Cart
                {
                    CustomerID = customerId,
                    CreateDate = DateTime.Now
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        // POST: api/Carts
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            cart.CreateDate = DateTime.Now;
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCart", new { id = cart.ID }, cart);
        }

        // POST: api/Carts/AddItem
        [HttpPost("AddItem")]
        public async Task<ActionResult<CartDetail>> AddCartItem([FromBody] AddCartItemRequest request)
        {
            var cart = await _context.Carts
                .Include(c => c.CartDetails)
                .FirstOrDefaultAsync(c => c.ID == request.CartID);

            if (cart == null)
            {
                return NotFound(new { message = "Giỏ hàng không tồn tại" });
            }

            var existingItem = cart.CartDetails
                .FirstOrDefault(cd => cd.ProductDetailID == request.ProductDetailID);

            if (existingItem != null)
            {
                existingItem.Quantity = (existingItem.Quantity ?? 0) + (request.Quantity ?? 1);
            }
            else
            {
                var cartDetail = new CartDetail
                {
                    CartID = request.CartID,
                    ProductDetailID = request.ProductDetailID,
                    Quantity = request.Quantity ?? 1
                };
                _context.CartDetails.Add(cartDetail);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        // PUT: api/Carts/UpdateItem
        [HttpPut("UpdateItem")]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemRequest request)
        {
            var cartDetail = await _context.CartDetails.FindAsync(request.CartDetailID);
            if (cartDetail == null)
            {
                return NotFound();
            }

            if (request.Quantity <= 0)
            {
                _context.CartDetails.Remove(cartDetail);
            }
            else
            {
                cartDetail.Quantity = request.Quantity;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Carts/Item/5
        [HttpDelete("Item/{cartDetailId}")]
        public async Task<IActionResult> DeleteCartItem(int cartDetailId)
        {
            var cartDetail = await _context.CartDetails.FindAsync(cartDetailId);
            if (cartDetail == null)
            {
                return NotFound();
            }

            _context.CartDetails.Remove(cartDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class AddCartItemRequest
    {
        public int CartID { get; set; }
        public int ProductDetailID { get; set; }
        public int? Quantity { get; set; }
    }

    public class UpdateCartItemRequest
    {
        public int CartDetailID { get; set; }
        public int Quantity { get; set; }
    }
}


