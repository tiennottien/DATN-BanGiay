using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductDetails)
                    .ThenInclude(pd => pd.Color)
                .Include(p => p.ProductDetails)
                    .ThenInclude(pd => pd.Size)
                .Include(p => p.ProductDetails)
                    .ThenInclude(pd => pd.Image)
                .ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductDetails)
                    .ThenInclude(pd => pd.Color)
                .Include(p => p.ProductDetails)
                    .ThenInclude(pd => pd.Size)
                .Include(p => p.ProductDetails)
                    .ThenInclude(pd => pd.Image)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // GET: api/Products/Category/5
        [HttpGet("Category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryID == categoryId)
                .Include(p => p.Category)
                .Include(p => p.ProductDetails)
                    .ThenInclude(pd => pd.Color)
                .Include(p => p.ProductDetails)
                    .ThenInclude(pd => pd.Size)
                .Include(p => p.ProductDetails)
                    .ThenInclude(pd => pd.Image)
                .ToListAsync();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            // Clear navigation properties to avoid binding issues
            product.Category = null;
            product.ProductDetails = null;
            
            product.CreateDate = DateTime.Now;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Reload with navigation properties
            var createdProduct = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ID == product.ID);

            return CreatedAtAction("GetProduct", new { id = product.ID }, createdProduct);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ID)
            {
                return BadRequest();
            }

            product.UpdateDate = DateTime.Now;
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ID == id);
        }
    }
}

