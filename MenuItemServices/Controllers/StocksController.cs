using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
    public class StocksController(ApplicationDbContext applicationDbContext) : ControllerBase
    {
        private readonly ApplicationDbContext _context = applicationDbContext ;
        [HttpGet]
        public async Task<IActionResult> GetStocks(){
            var stocks = await _context.Stocks.Include(i=>i.Ingredient_Stocks).ThenInclude(id=>id.Ingredient)
            .Select(i=> new {
                StockId = i.StockId,
                NumberOfIngredient = i.NumberOfIngredient,
                ArrivedDate = i.ArrivedDate.ToShortDateString(),
                IngredientList = i.Ingredient_Stocks
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
            })
            .ToListAsync();

            return new JsonResult(stocks);
        }
        [HttpPost]
        public async Task<IActionResult> PostStock([FromBody] AddingIngredientToStockRequest addingIngredientToStockRequests){
            if (addingIngredientToStockRequests == null || !addingIngredientToStockRequests.addingIngredientItemToStockRequests.Any())
            {
                return BadRequest("Invalid request data.");
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var insertedStock = new Stock
                {
                    ArrivedDate = addingIngredientToStockRequests.ArrivedDate ?? DateTime.UtcNow
                };

                foreach (var item in addingIngredientToStockRequests.addingIngredientItemToStockRequests)
                {
                    var ingredient = await _context.Ingredients.FindAsync(item.IngredientId);
                    if (ingredient == null)
                    {
                        return NotFound($"Ingredient ID {item.IngredientId} not found.");
                    }

                    insertedStock.Ingredient_Stocks.Add(new Ingredient_Stock
                    {
                        CurrentQuantity = item.CurrentQuantity,
                        Ingredient = ingredient,
                        ExpiredAt = ingredient.ExpiredTime != TimeSpan.Zero ? DateTime.UtcNow.Add(ingredient.ExpiredTime) : (DateTime?)null
                    });
                }
                insertedStock.NumberOfIngredient = insertedStock.Ingredient_Stocks.Count;

                await _context.Stocks.AddAsync(insertedStock);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok("Stocking Added");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
        [HttpPut("update-stock-ingredients/{stockId}")]
        public async Task<IActionResult> UpdateStockIngredients([Required] int stockId, [FromBody] UpdateStockIngredientQuantityRequest updateStockIngredientQuantityRequest)
        {
            if (stockId != updateStockIngredientQuantityRequest.StockId)
            {
                return BadRequest($"Path parameter {stockId} does not match request body {updateStockIngredientQuantityRequest.StockId}");
            }

            // Validate if stock exists
            var stock = await _context.Stocks
                                    .Include(s => s.Ingredient_Stocks)
                                    .FirstOrDefaultAsync(s => s.StockId == stockId);
            
            if (stock == null)
            {
                return NotFound($"Stock with ID {stockId} not found");
            }

            // Start a transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Loop through the updates
                foreach (var item in updateStockIngredientQuantityRequest.IngredientQuantities )
                {
                    // Find the matching Ingredient_Stock record
                    var ingredientStock = stock.Ingredient_Stocks
                                            .FirstOrDefault(i => i.IngredientId == item.IngredientId);

                    if (ingredientStock == null)
                    {
                        return NotFound($"Ingredient with ID {item.IngredientId} not found in stock with ID {stockId}");
                    }

                    // Update the CurrentQuantity
                    ingredientStock.CurrentQuantity = item.UpdateQuantity;
                }

                // Save changes
                await _context.SaveChangesAsync();

                // Commit the transaction
                await transaction.CommitAsync();

                return Ok("Stock ingredients updated successfully");
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                await transaction.RollbackAsync();
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}