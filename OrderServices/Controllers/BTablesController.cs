using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;

namespace OrderServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BTablesController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        [HttpGet]
        public async Task<IActionResult> GetTables(){
            var tables = await _context.BTables
                .Select(b => new 
                {
                    BTableId = b.BTableId,
                    Status = b.Status,
                    Capacity = b.Capacity
                }).ToListAsync();

            return new JsonResult(tables);
        }
    }
}