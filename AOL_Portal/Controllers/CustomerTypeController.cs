using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AOL_Portal.Data;

namespace AOL_Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerTypeController : ControllerBase
    {
        private readonly AOLContext _context;

        public CustomerTypeController(AOLContext context)
        {
            _context = context;
        }

        // GET: api/CustomerType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerType>>> GetCustomerTypes()
        {
            try
            {
                var customerTypes = await _context.CustomerTypes
                    .OrderBy(ct => ct.CustomerTypeName)
                    .ToListAsync();

                return Ok(customerTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving customer types: {ex.Message}");
            }
        }

        // GET: api/CustomerType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerType>> GetCustomerType(int id)
        {
            try
            {
                var customerType = await _context.CustomerTypes
                    .FirstOrDefaultAsync(ct => ct.CustomerTypeId == id);

                if (customerType == null)
                {
                    return NotFound();
                }

                return Ok(customerType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving customer type: {ex.Message}");
            }
        }

        // POST: api/CustomerType
        [HttpPost]
        public async Task<ActionResult<CustomerType>> CreateCustomerType(CustomerType customerType)
        {
            try
            {
                _context.CustomerTypes.Add(customerType);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCustomerType), new { id = customerType.CustomerTypeId }, customerType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating customer type: {ex.Message}");
            }
        }

        // PUT: api/CustomerType/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerType>> UpdateCustomerType(int id, CustomerType customerType)
        {
            try
            {
                if (id != customerType.CustomerTypeId)
                {
                    return BadRequest("ID mismatch");
                }

                var existingCustomerType = await _context.CustomerTypes
                    .FirstOrDefaultAsync(ct => ct.CustomerTypeId == id);

                if (existingCustomerType == null)
                {
                    return NotFound();
                }

                existingCustomerType.CustomerTypeName = customerType.CustomerTypeName;

                await _context.SaveChangesAsync();

                return Ok(existingCustomerType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating customer type: {ex.Message}");
            }
        }

        // DELETE: api/CustomerType/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomerType(int id)
        {
            try
            {
                var customerType = await _context.CustomerTypes
                    .FirstOrDefaultAsync(ct => ct.CustomerTypeId == id);

                if (customerType == null)
                {
                    return NotFound();
                }

                _context.CustomerTypes.Remove(customerType);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting customer type: {ex.Message}");
            }
        }
    }
}

