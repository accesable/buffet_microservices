using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.PaymentModels;

namespace PaymentServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public async Task<IActionResult> GetPayments([FromQuery]PaymentStatus ? paymentStatus)
        {
            IQueryable<Payment> qr = _context.Payments;
            if(paymentStatus.HasValue)
            {
                qr = qr.Where(payment => payment.Status == paymentStatus);
            }
            return new JsonResult(await qr.ToListAsync());
        }
        public class CreatePaymentRequest
        {
            [Required]
            public double Ammount {get;set;}
            public PaymentMethod PaymentMethod {get;set;} = PaymentMethod.CASH;
            public PaymentStatus PaymentStatus {get;set;} = PaymentStatus.PENDING;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest createPaymentRequest)
        {
            if (createPaymentRequest == null)
            {
                return BadRequest("Invalid payment request.");
            }

            var insertedPayment = new Payment()
            {
                Ammount = createPaymentRequest.Ammount,
                PaymentMethod = createPaymentRequest.PaymentMethod,
                Status = createPaymentRequest.PaymentStatus,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            };

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Payments.Add(insertedPayment);
                await _context.SaveChangesAsync();

                // Commit the transaction
                await transaction.CommitAsync();

                return await this.GetPaymentById(insertedPayment.PaymentId);
            }
            catch (Exception ex)
            {
                // Rollback the transaction if any error occurs
                await transaction.RollbackAsync();

                // Log the exception (not shown here for brevity)
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }
    }
}