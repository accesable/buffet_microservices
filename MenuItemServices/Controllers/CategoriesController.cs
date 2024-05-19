using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenuItemServices.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.MenuItem;

namespace MenuItemServices.Controllers
{
[Route("api/[controller]")]
    [ApiController]
/*    [Authorize(Roles ="Administrator")]*/
    public class CategoriesController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        // GET: api/Categories
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _context.Categories
                .Select(static category => new 
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    ItemList = category.Items,
                })
                .ToListAsync();

            return Ok(categories);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _context.Categories
                             .Include(c => c.Items) // Eagerly load Items
                             .SingleOrDefaultAsync(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            CategoryDto categoryDto = new()
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Items = category.Items.Select(item => new ItemDto
                {
                    ItemId = item.ItemId,
                    ItemName = item.ItemName
                }).ToList()
            };

            return categoryDto;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        public class CategoryEditModel
        {
            public int CategoryId { get; set; }
            public string ? CategoryName { get; set; }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id,[FromBody] CategoryEditModel categoryEditModel)
        {
            if (id != categoryEditModel.CategoryId)
            {
                return BadRequest();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(_context => _context.CategoryId == id);
            category.CategoryName = categoryEditModel.CategoryName;
            category.LastUpdatedAt = DateTime.UtcNow;
            if (category == null)
            {
                return NotFound("Category Not Founded");
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok($"Category {category.CategoryId} Name's Updated to {category.CategoryName}");
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> PostCategory(CategoryRequest categoryRequest)
        {
            var insertCategory = new Category()
            {
                CategoryName = categoryRequest.CategoryName
            };
            _context.Categories.Add(insertCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = insertCategory.CategoryId }, categoryRequest);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}