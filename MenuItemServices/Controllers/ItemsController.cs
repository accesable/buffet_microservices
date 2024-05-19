using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MenuItemServices.Dtos;
using MenuItemServices.Dtos.Ingredients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.MenuItem;

namespace MenuItemServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        // GET: api/Items
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetItems(bool hasIngredients = false)
        {
            var items = await _context.Items
                .Include(i => i.Images)
                .Include(i => i.Category)
                .Include(i=>i.Item_Ingredients).ThenInclude(i=>i.Ingredient)
                .Select(item => new 
                {
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    ItemDescription = item.ItemDescription,
                    OriginalPrice = item.OriginalPrice,
                    CategoryId = item.CategoryId,
                    Category = new Category
                    {
                        CategoryId = item.Category.CategoryId,
                        CategoryName = item.Category.CategoryName
                    },
                    Images = item.Images.Select(img => new Image
                    {
                        ImageId = img.ImageId,
                        ImageUrl = img.ImageUrl
                    }).ToList(),
                    IsCharged = item.IsCharged,
                    IsLocked = item.IsLocked,
                    IngredientList = hasIngredients ? item.Item_Ingredients.Select(i=> new {
                        IngredientId = i.IngredientId,
                        IngredientName = i.Ingredient.IngredientName,
                        MaxQuantity = i.MaxQuantity,
                        MinQuantity = i.MinQuantity
                    }).ToList(): null,
                }).ToListAsync();
            return new JsonResult(items);
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await _context.Items
                    .Include(i => i.Images)
                    .Include(i => i.Category)
                    .Include(i => i.Item_Ingredients).ThenInclude(i => i.Ingredient)
                    .Where(i => i.ItemId == id) // Filter by id
                    .Select(item => new 
                    {
                        ItemId = item.ItemId,
                        ItemName = item.ItemName,
                        ItemDescription = item.ItemDescription,
                        OriginalPrice = item.OriginalPrice,
                        CategoryId = item.CategoryId,
                        Category = new 
                        {
                            CategoryId = item.Category.CategoryId,
                            CategoryName = item.Category.CategoryName
                        },
                        Images = item.Images.Select(img => new 
                        {
                            ImageId = img.ImageId,
                            ImageUrl = img.ImageUrl
                        }).ToList(),
                        IsCharged = item.IsCharged,
                        IsLocked = item.IsLocked,
                        IngredientList = item.Item_Ingredients.Select(i => new 
                        {
                            IngredientId = i.IngredientId,
                            IngredientName = i.Ingredient.IngredientName,
                            MaxQuantity = i.MaxQuantity,
                            MinQuantity = i.MinQuantity
                        }).ToList()
                    }).FirstOrDefaultAsync();

            if (item == null)
            {
                return NotFound();
            }
            return new JsonResult(item);
        }
        [HttpPut("status/{id}")]
        public async Task<ActionResult<Item>> PutItemStatus(int id, [Required] string status)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }
            if (status == "lock")
            {
                item.IsLocked = true;
            }
            else if (status == "unlock")
            {
                item.IsLocked = false;
            }
            await _context.SaveChangesAsync();

            return Ok(new { item.IsLocked });
        }

        // PUT: api/Items/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, Item item)
        {
            if (id != item.ItemId)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
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

        // POST: api/Items
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, DisableRequestSizeLimit]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Item>> PostItem([FromForm] ItemRequest itemRequest, [FromForm] List<IFormFile> files)
        {
            Item insertedItem = new()
            {
                ItemName = itemRequest.ItemName,
                ItemDescription = itemRequest.ItemDescription,
                OriginalPrice = itemRequest.OriginalPrice,
                CategoryId = itemRequest.CategoryId,
                IsCharged = (bool)itemRequest.IsCharged,
                Images = new List<Image>()
            };
            _context.Items.Add(insertedItem);
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    // Generate a unique file name to prevent file name conflicts
                    var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);

                    // Define the path to save the file; adjust the path as per your requirement
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageStaticFiles/Items", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Assuming you save the file in wwwroot/images and want to access it via a relative URL
                    var imageUrl = $"/Items/{fileName}";

                    // Create a new Image object and add it to the Images collection of the insertedItem
                    insertedItem.Images.Add(new Image { ImageUrl = imageUrl });
                }
            }
            await _context.SaveChangesAsync();

            // Construct the response object
            var response = new
            {
                insertedItem.ItemId,
                insertedItem.ItemName,
                insertedItem.ItemDescription,
                insertedItem.OriginalPrice,
                insertedItem.CategoryId,
                Images = insertedItem.Images.Select(img => new { img.ImageId, img.ImageUrl }).ToList()
            };

            // Return a 200 OK response with the item and image information
            return Ok(response);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }
        [HttpPost("get-items-by-ids")]
        public async Task<IActionResult> GetItemsByIds([FromBody] List<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return BadRequest("The list of item IDs cannot be null or empty.");
            }

            var items = await _context.Items
                            .Include(i => i.Images)
                            .Include(i => i.Category)
                            .Include(i => i.Item_Ingredients).ThenInclude(i => i.Ingredient)
                            .Where(i => ids.Contains(i.ItemId))
                            .Select(item => new 
                            {
                                ItemId = item.ItemId,
                                ItemName = item.ItemName,
                                ItemDescription = item.ItemDescription,
                                OriginalPrice = item.OriginalPrice,
                                CategoryId = item.CategoryId,
                                Category = new 
                                {
                                    CategoryId = item.Category.CategoryId,
                                    CategoryName = item.Category.CategoryName
                                },
                                Images = item.Images.Select(img => new 
                                {
                                    ImageId = img.ImageId,
                                    ImageUrl = img.ImageUrl
                                }).ToList(),
                                IsCharged = item.IsCharged,
                                IsLocked = item.IsLocked,
                                IngredientList = item.Item_Ingredients.Select(i => new 
                                {
                                    IngredientId = i.IngredientId,
                                    IngredientName = i.Ingredient.IngredientName,
                                    MaxQuantity = i.MaxQuantity,
                                    MinQuantity = i.MinQuantity
                                }).ToList()
                            })
                            .ToListAsync();

            if (items == null || items.Count == 0)
            {
                return NotFound();
            }

            return Ok(items);
        }
    }
}