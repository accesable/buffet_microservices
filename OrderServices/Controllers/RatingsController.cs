using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.OrderModels;

namespace OrderServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        [HttpGet]
        public async Task<IActionResult> GetRatings()
        {
            return new JsonResult(await _context.Ratings.ToListAsync());
        }
        public class AddRatingRequest
        {
            [Required]
            public int Star {get;set;}
            public string ?Comment {get;set;}

        }
        [HttpPost("{orderId}")]
        public async Task<IActionResult> PostRating(int orderId,[FromBody] AddRatingRequest addRatingRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newRating = new Rating
            {
                Star = addRatingRequest.Star,
                Comment = addRatingRequest.Comment,
            };
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Retrieve the order based on the orderId
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
                if (order == null)
                {
                    return NotFound($"Order ID: {orderId} is not found");
                }

                // Add the new rating
                _context.Ratings.Add(newRating);
                await _context.SaveChangesAsync();

                // Assign the new rating's ID to the order
                order.RatingId = newRating.RatingId;

                // Update the order in the context
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                // Commit the transaction
                await transaction.CommitAsync();

                return await GetRatingById(newRating.RatingId);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRatingById(int id)
        {
            var rating = await _context.Ratings.FindAsync(id);

            if (rating == null)
            {
                return NotFound();
            }

            return Ok(rating);
        }
    }
}