using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderServices.Dtos;
using OrderServices.Dtos.Ingredients;
using OrderServices.Services;
using Shared.Data;
using Shared.Ingredients;
using Shared.OrderModels;

namespace OrderServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(ApplicationDbContext context,ExternalApiService externalApiService) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ExternalApiService _externalApiService = externalApiService;

        [HttpGet]
        public async Task<IActionResult> GetOrders(){
            var orders = await _context.Orders
            .Include(i=>i.OrderDetails).ThenInclude(i=>i.Item)
            .Include(e=>e.Employee)
            .Select(i=> new {
                OrderId = i.OrderId,
                TableId = i.BTableId,
                NumberOfCustomer = i.NumberOfCustomers,
                OrderStatus = i.Status,
                Employee = new {
                    Id = i.EmployeeId,
                    Name = i.Employee.FullName
                },
                OrderDetails = i.OrderDetails.Select(item=> new {
                    Id = item.OrderDetailId,
                    Item = new {
                        Id=item.ItemId,
                        ItemName=item.Item.ItemName
                    },
                    Quantity = item.Quantity,
                    DetailStatus = item.DetailStatus,
                    DetailNode = item.DetailNote,
                    CreatedAt = item.CreatedAt,
                    LastUpdateAt = item.LastUpdatedAt
                }).ToList(),
                CreatedAt = i.CreatedAt,
                LastUpdateAt = i.LastUpdatedAt,
                PaymentId = i.PaymentId,
                RatingId = i.RatingId
            })
            .ToListAsync();
            return new JsonResult(orders);
        }
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById([Required] int orderId)
        {
            var order = await _context.Orders
                .Include(i => i.OrderDetails).ThenInclude(i => i.Item)
                .Include(e => e.Employee)
                .Where(o => o.OrderId == orderId)
                .Select(i => new {
                    OrderId = i.OrderId,
                    TableId = i.BTableId,
                    NumberOfCustomer = i.NumberOfCustomers,
                    OrderStatus = i.Status,
                    Employee = new {
                        Id = i.EmployeeId,
                        Name = i.Employee.FullName
                    },
                    OrderDetails = i.OrderDetails.Select(item => new {
                        Id = item.OrderDetailId,
                        Item = new {
                            Id = item.ItemId,
                            ItemName = item.Item.ItemName
                        },
                        Quantity = item.Quantity,
                        DetailStatus = item.DetailStatus,
                        DetailNote = item.DetailNote,
                        CreatedAt = item.CreatedAt,
                        LastUpdateAt = item.LastUpdatedAt
                    }).ToList(),
                    CreatedAt = i.CreatedAt,
                    LastUpdateAt = i.LastUpdatedAt,
                    PaymentId = i.PaymentId,
                    RatingId = i.RatingId
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound($"Order ID: {orderId} is not found.");
            }

            return new JsonResult(order);
        }
        [HttpGet("order-details/{orderDetailId}")]
        public async Task<IActionResult> GetOrderDetailById([Required] int orderDetailId)
        {
            var orderDetail = await _context.OrderDetails
                .Include(od => od.Item)
                .Include(od => od.Order)
                .Where(od => od.OrderDetailId == orderDetailId)
                .Select(od => new {
                    Id = od.OrderDetailId,
                    OrderId = od.OrderId,
                    Item = new {
                        Id = od.ItemId,
                        ItemName = od.Item.ItemName
                    },
                    Quantity = od.Quantity,
                    DetailStatus = od.DetailStatus,
                    DetailNote = od.DetailNote,
                    CreatedAt = od.CreatedAt,
                    LastUpdateAt = od.LastUpdatedAt,
                    TableId = od.Order.BTableId,
                })
                .FirstOrDefaultAsync();

            if (orderDetail == null)
            {
                return NotFound($"Order Detail ID: {orderDetailId} is not found.");
            }

            return new JsonResult(orderDetail);
        }


        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] OpenOrderRequest openOrderRequest){
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var table = await _context.BTables.FindAsync(openOrderRequest.TableId);
                if (table == null)
                {
                    return NotFound($"Table ID : {openOrderRequest.TableId} is Not Found");
                }

                if (openOrderRequest.NumberOfCustomer > table.Capacity || openOrderRequest.NumberOfCustomer == 0)
                {
                    return BadRequest($"Invalid Number of guests for this table. Your request is for {openOrderRequest.NumberOfCustomer} customers, but the capacity is {table.Capacity}");
                }
                if(table.Status==BTableStatus.OCCUPIED)
                {
                    return BadRequest($"Table ID {table.BTableId} is current OCCUPIED");
                }
                var insertedOrder = new Order()
                {
                    BTableId = table.BTableId,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow,
                    NumberOfCustomers = openOrderRequest.NumberOfCustomer,
                    Status = OrderStatus.SERVING,
                    EmployeeId = openOrderRequest.EmployeeId ?? "No ID"
                };

                // Update table status
                table.Status = BTableStatus.OCCUPIED;

                // Add the order to the context
                _context.Orders.Add(insertedOrder);

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Commit the transaction
                await transaction.CommitAsync();

                // Redirect to GetOrderById action
                return await GetOrderById(insertedOrder.OrderId);
                // return CreatedAtAction(nameof(GetOrderById), new { orderId = insertedOrder.OrderId }, null);
            }
            catch (Exception ex)
            {
                // Rollback the transaction if an exception occurs
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPost("add-details/{orderId}")]
        public async Task<IActionResult> PostAddOrderDetails([Required]int orderId,[FromBody] AddOrderDetailRequest addOrderDetailRequest)
        {
            if(orderId!=addOrderDetailRequest.OrderId || addOrderDetailRequest.orderDetailRequests.Count==0)
            {
                return BadRequest("Invalid orderId or Order Details List is null or empty");
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                {
                    return NotFound($"Order ID: {orderId} is not found");
                }

                // Check if all items exist
                foreach (var item in addOrderDetailRequest.orderDetailRequests)
                {
                    bool menuItemExists = await _context.Items.AnyAsync(i => i.ItemId == item.ItemId);
                    if (!menuItemExists)
                    {
                        return NotFound($"Item ID: {item.ItemId} is not found");
                    }
                }

                // Create and add the order details
                foreach (var item in addOrderDetailRequest.orderDetailRequests)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = orderId,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        DetailNote = item.DetailNote ?? ""
                    };

                    _context.OrderDetails.Add(orderDetail);
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Commit the transaction
                await transaction.CommitAsync();

                // Redirect to GetOrderById action  
                return await GetOrderById(orderId);
                // return CreatedAtAction(nameof(GetOrderById), new { orderId = orderId }, null);
            }
            catch (Exception ex)
            {
                // Rollback the transaction if an exception occurs
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpGet("order-details")]
        public async Task<IActionResult> GetOrderDetails([FromQuery] DetailStatus? detailStatus)
        {
            try
            {
                // Query the database for order details
                IQueryable<OrderDetail> query = _context.OrderDetails
                                                .Include(dt=>dt.Order)
                                                .Include(dt => dt.Item);

                // Filter by detailStatus if provided
                if (detailStatus.HasValue)
                {
                    query = query.Where(detail => detail.DetailStatus == detailStatus);
                }

                // Execute the query and retrieve order details
                var orderDetails = await query
                                    .Select(dt => new {
                                        OrderDetailId = dt.OrderDetailId,
                                        Quantity = dt.Quantity,
                                        TableId = dt.Order.BTableId,
                                        Item = new {
                                            Id=dt.ItemId,
                                            ItemName = dt.Item.ItemName
                                        },
                                        Status = dt.DetailStatus,
                                        CreatedAt = dt.CreatedAt,
                                        LastUpdatedAt = dt.LastUpdatedAt
                                    })
                                    .ToListAsync();

                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPut("update-detail/{detailId}")]
        public async Task<IActionResult> PutOrderDetails([Required]int detailId,[FromBody] UpdateDetailStatusRequest updateDetailStatusRequest)
        {
            if (detailId != updateDetailStatusRequest.OrderDetailId)
            {
                return BadRequest("The detailId in the URL does not match the OrderDetailId in the request body.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Find the order detail
                var orderDetail = await _context.OrderDetails.FindAsync(detailId);
                if (orderDetail == null)
                {
                    return NotFound($"Order Detail ID: {detailId} is not found.");
                }

                // Update the status
                orderDetail.DetailStatus = updateDetailStatusRequest.Status;
                orderDetail.LastUpdatedAt = DateTime.UtcNow;

                // Save changes to the database
                _context.OrderDetails.Update(orderDetail);
                await _context.SaveChangesAsync();

                // Commit the transaction
                await transaction.CommitAsync();

                // Redirect to GetOrderById action
                return await GetOrderDetailById(detailId);
                // return CreatedAtAction(nameof(GetOrderDetailById), new { orderDetailId = detailId }, null);
            }
            catch (Exception ex)
            {
                // Rollback the transaction if an exception occurs
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPut("commit-payment/{orderId}")]
        public async Task<IActionResult> CommitPayment(int orderId,[FromBody] CreatePaymentRequest createPaymentRequest)
        {
            var order = await _context.Orders
                                .Include(od=>od.Table)
                                .Include(od=>od.OrderDetails)
                                .FirstOrDefaultAsync(od=>od.OrderId == orderId);
            if(order==null)
            {
                return NotFound($"Order ID : {orderId} is Not Founded");
            }
            if(order.Status==OrderStatus.FISNSHED)
            {
                return BadRequest($"Order ID : {orderId} is already commited to payed");
            }
            string apiUrl = $"https://localhost:7095/available_ingredients";
            var availableIngredientResponses = await _externalApiService.GetAvailableIngredientResponseAsync(apiUrl);
            var itemQuantityRequests = new List<ItemQuantityRequest>();
            foreach (var item in order.OrderDetails)
            {
                if(item.DetailStatus>DetailStatus.CONFIRMED && item.DetailStatus<DetailStatus.DECLINED)
                {
                    itemQuantityRequests.Add(new ItemQuantityRequest(){ItemId=item.ItemId,ItemQuantity=item.Quantity});
                }
            }
            string apiUrl1 = $"https://localhost:7095/api/Ingredients/get-ingredients-on-items-and-quantity";
            var usedIngredients = await _externalApiService.GetIngredientsOnItemsAndQuantityAsync(apiUrl1,itemQuantityRequests);
            var ingredientUsedInThisOrder = new List<Order_IngredientStock>();
            foreach (var i in usedIngredients)
            {
                foreach (var j in availableIngredientResponses)
                {
                    if(i.IngredientId==j.IngredientId && j.Status!="Expired")
                    {
                        ingredientUsedInThisOrder.Add(new Order_IngredientStock(){
                            IngredientStockId = j.Id,
                            OrderId = orderId,
                            UsedQuantity = i.RecommendedUsedQuantity
                        });
                        break;
                    }
                }
            }
            // Using transaction to ensure atomicity
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Order_IngredientStocks.AddRangeAsync(ingredientUsedInThisOrder);
                    order.Status = OrderStatus.FISNSHED;
                    order.Table.Status = BTableStatus.VACANT;
                    order.LastUpdatedAt = DateTime.UtcNow;
                    // Create Payment and Get it ID
                    int paymentId = await CreatePayment("https://localhost:7102/api/Payments",createPaymentRequest);
                    order.PaymentId = paymentId;
                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, "An error occurred while processing the request.");
                }
            }

            return await GetOrderById(orderId);
        }
        private async Task<int> CreatePayment(string apiUrl,CreatePaymentRequest request)
        {
            var response = await _externalApiService.PostCreatePaymentAsync(apiUrl,request);
            return response.PaymentId;
        }
    }
}