using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using MenuItemServices.Dtos.Ingredients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Ingredients;

namespace MenuItemServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientsController(ApplicationDbContext applicationDbContext) : ControllerBase
    {
        private readonly ApplicationDbContext _context = applicationDbContext;
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientResponse>>> GetIngredients(){
            var ingredients = await _context.Ingredients.Include(i=>i.Ingredient_Stocks)
                .Select(static ingredient => new 
                {
                    IngredientId = ingredient.IngredientId,
                    IngredientName = ingredient.IngredientName,
                    IngredientDescription = ingredient.IngredientDescription,
                    UnitOfMeasurement = ingredient.UnitOfMeasurement,
                    InStock = ingredient.Ingredient_Stocks
                                .Select(i=>new {
                                    Id = i.Id,
                                    Ingredient=new {
                                        Id = i.IngredientId,
                                        Name=i.Ingredient.IngredientName,
                                    },
                                    CurrentQuantity = i.CurrentQuantity,
                                    ExpiredAt = i.ExpiredAt,
                                    Status = i.ExpiredAt == null ? "Referenced by Label" :
                                            i.ExpiredAt <= DateTime.UtcNow ? "Expired" : "Not Expired"
                                })
                }).ToListAsync();

            return Ok(ingredients);
        }
        [HttpGet("/available_ingredients")]
        public async Task<ActionResult<IEnumerable<IngredientResponse>>> GetAvailableIngredients(){
            var availableIngredients = await _context.Ingredient_Stocks
            .Where(i => i.CurrentQuantity > 0)
            .OrderBy(i => i.ExpiredAt)
            .Select(i=>new {
                Id = i.Id,
                IngredientId = i.IngredientId,
                IngredientName = i.Ingredient.IngredientName,
                CurrentQuantity = i.CurrentQuantity,
                StockId = i.StockId,
                ExpriedDate = i.ExpiredAt,
                Status = i.ExpiredAt == null ? "Referenced by Label" :
                        i.ExpiredAt <= DateTime.UtcNow ? "Expired" : "Not Expired"
            }).ToListAsync();
            return Ok(availableIngredients);
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<IngredientResponse>>> PostIngredients([FromBody] IngredientRequest ingredientRequest){
            TimeSpan expiredTime = TimeSpan.FromHours(ingredientRequest.ExpiredTimeInHours).Add(TimeSpan.FromDays(ingredientRequest.ExpiredTimeInDays)) ;
            var ingredient = new Ingredient()
            {
                IngredientName = ingredientRequest.IngredientName,
                ExpiredTime = expiredTime,
                IngredientDescription = ingredientRequest.IngredientDescription,
                UnitOfMeasurement = ingredientRequest.UnitOfMeasurement
            };

            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            return Ok(ingredient);
        }
        [HttpPost("AddIngredientsToItem/{itemId}")]
        public async Task<IActionResult> AddIngredientToItem([Required]int itemId,[FromBody] AddingIngredientToItemRequest addingIngredientToItemRequest){
            if (itemId != addingIngredientToItemRequest.ItemId || addingIngredientToItemRequest.IngredientItemRequests.Count == 0)
            {
                return BadRequest("Invalid request.");
            }

            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
            {
                    return BadRequest("Item not found.");
            }

            var itemIngredients = new List<Item_Ingredient>();

            foreach (var ingredientRequest in addingIngredientToItemRequest.IngredientItemRequests)
            {
                var ingredient = await _context.Ingredients.FindAsync(ingredientRequest.IngredientId);
                if (ingredient == null)
                {
                    return BadRequest($"Ingredient with ID {ingredientRequest.IngredientId} not found.");
                }

                itemIngredients.Add(new Item_Ingredient
                {
                    IngredientId = ingredientRequest.IngredientId,
                    ItemId = itemId,
                    MaxQuantity = ingredientRequest.MaxQuantity,
                    MinQuantity = ingredientRequest.MinQuantity
                });
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Item_Ingredients.AddRangeAsync(itemIngredients);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding ingredients to the item.");
            }

            return Ok("Ingredients successfully added to the item.");
        }
        [HttpPost("get-ingredients-on-items-and-quantity")]
        public async Task<IActionResult> GetIngredientsOnItemsAndQuantity([FromBody] ICollection<ItemQuantityRequest> itemQuantityRequests)
        {
            // Step 1: Consolidate item quantities
            var itemQuantitiesMap = new Dictionary<int, int>();
            foreach (var item in itemQuantityRequests)
            {
                if (itemQuantitiesMap.ContainsKey(item.ItemId))
                {
                    itemQuantitiesMap[item.ItemId] += item.ItemQuantity;
                }
                else
                {
                    itemQuantitiesMap[item.ItemId] = item.ItemQuantity;
                }
            }

            // Step 2: Retrieve ingredients for each item and calculate quantities
            var ingredientQuantities = new Dictionary<int, ItemQuantityIngredientResponse>();

            foreach (var kvp in itemQuantitiesMap)
            {
                var itemId = kvp.Key;
                var itemQuantity = kvp.Value;

                var ingredients = await _context.Item_Ingredients
                                                .Where(ii => ii.ItemId == itemId)
                                                .Include(ii => ii.Ingredient)
                                                .ToListAsync();

                foreach (var itemIngredient in ingredients)
                {
                    var ingredientId = itemIngredient.IngredientId;
                    var ingredientName = itemIngredient.Ingredient.IngredientName;
                    var recommendedUsedQuantity = ((itemIngredient.MaxQuantity+itemIngredient.MinQuantity)/2) * itemQuantity; // Assuming there's a RecommendedUsedQuantity property
                    var maxQuantity = itemIngredient.MaxQuantity; // Assuming there's a MaxQuantity property
                    var minQuantity = itemIngredient.MinQuantity; // Assuming there's a MinQuantity property

                    if (ingredientQuantities.ContainsKey(ingredientId))
                    {
                        ingredientQuantities[ingredientId].RecommendedUsedQuantity += recommendedUsedQuantity;
                    }
                    else
                    {
                        ingredientQuantities[ingredientId] = new ItemQuantityIngredientResponse
                        {
                            IngredientId = ingredientId,
                            IngredientName = ingredientName,
                            RecommendedUsedQuantity = recommendedUsedQuantity,
                            MaxQuantity = maxQuantity,
                            MinQuantity = minQuantity
                        };
                    }
                }
            }

            // Step 3: Prepare consolidated list
            var consolidatedList = ingredientQuantities.Values.ToList();

            // Step 4: Return result as JSON
            return new JsonResult(consolidatedList);
        }
        [HttpGet("ingredient-reports")]
        public async Task<IActionResult> GetUsedIngredientReports(DateTime? start, DateTime? end)
        {
            // Get the current UTC date's start and end times
            DateTime todayStart = DateTime.UtcNow.Date;
            DateTime todayEnd = todayStart.AddDays(1).AddTicks(-1);

            // Build the base query
            var query = _context.Order_IngredientStocks
                .Include(oi => oi.Order)
                .Include(oi => oi.Ingredient_Stock)
                    .ThenInclude(i => i.Ingredient)
                .Select(oi => new 
                {
                    oi.Id,
                    oi.UsedQuantity,
                    OrderCreatedAt = oi.Order.CreatedAt,
                    IngredientStockId = oi.Ingredient_Stock.Id,
                    IngredientName = oi.Ingredient_Stock.Ingredient.IngredientName
                });

            // Apply filtering based on the start and end query parameters
            if (start.HasValue && end.HasValue)
            {
                query = query.Where(oi => oi.OrderCreatedAt >= start.Value.ToUniversalTime() && oi.OrderCreatedAt <= end.Value.ToLocalTime());
            }
            else if (start.HasValue)
            {
                query = query.Where(oi => oi.OrderCreatedAt >= start.Value.ToUniversalTime() && oi.OrderCreatedAt <= DateTime.UtcNow);
            }
            else if (end.HasValue)
            {
                return BadRequest("End date cannot be specified without a start date.");
            }
            else
            {
                query = query.Where(oi => oi.OrderCreatedAt >= todayStart && oi.OrderCreatedAt <= todayEnd);
            }

            var orderedQuery = query.OrderByDescending(oi => oi.UsedQuantity);

            var result = await orderedQuery.ToListAsync();

            return new JsonResult(result);
        }

    }
}