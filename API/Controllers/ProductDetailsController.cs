using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDetail>>> GetProductDetails()
        {
            return await _context.ProductDetails
                .Include(pd => pd.Product)
                .Include(pd => pd.Color)
                .Include(pd => pd.Size)
                .Include(pd => pd.Image)
                .ToListAsync();
        }

        // GET: api/ProductDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetail>> GetProductDetail(int id)
        {
            var productDetail = await _context.ProductDetails
                .Include(pd => pd.Product)
                .Include(pd => pd.Color)
                .Include(pd => pd.Size)
                .Include(pd => pd.Image)
                .FirstOrDefaultAsync(pd => pd.ID == id);

            if (productDetail == null)
            {
                return NotFound();
            }

            return productDetail;
        }

        // GET: api/ProductDetails/Product/5
        [HttpGet("Product/{productId}")]
        public async Task<ActionResult<IEnumerable<ProductDetail>>> GetProductDetailsByProduct(int productId)
        {
            return await _context.ProductDetails
                .Where(pd => pd.ProductID == productId)
                .Include(pd => pd.Color)
                .Include(pd => pd.Size)
                .Include(pd => pd.Image)
                .ToListAsync();
        }

        // POST: api/ProductDetails
        [HttpPost]
        public async Task<ActionResult<ProductDetail>> PostProductDetail(ProductDetail productDetail)
        {
            // Clear navigation properties to avoid binding issues
            productDetail.Product = null;
            productDetail.Color = null;
            productDetail.Size = null;
            productDetail.Image = null;
            productDetail.CartDetails = null;
            productDetail.OrderDetails = null;
            
            productDetail.CreateDate = DateTime.Now;
            _context.ProductDetails.Add(productDetail);
            await _context.SaveChangesAsync();

            // Reload with navigation properties
            var createdDetail = await _context.ProductDetails
                .Include(pd => pd.Product)
                .Include(pd => pd.Color)
                .Include(pd => pd.Size)
                .Include(pd => pd.Image)
                .FirstOrDefaultAsync(pd => pd.ID == productDetail.ID);

            return CreatedAtAction("GetProductDetail", new { id = productDetail.ID }, createdDetail);
        }

        // PUT: api/ProductDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductDetail(int id, ProductDetail productDetail)
        {
            if (id != productDetail.ID)
            {
                return BadRequest();
            }

            _context.Entry(productDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductDetailExists(id))
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

        // DELETE: api/ProductDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductDetail(int id)
        {
            var productDetail = await _context.ProductDetails.FindAsync(id);
            if (productDetail == null)
            {
                return NotFound();
            }

            _context.ProductDetails.Remove(productDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductDetailExists(int id)
        {
            return _context.ProductDetails.Any(e => e.ID == id);
        }
    }
}

