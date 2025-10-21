using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AOL_Portal.Data;
using Microsoft.AspNetCore.Authorization;

namespace AOL_Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly AOLContext _context;

        public CustomerController(AOLContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates default customer records for a new user with standard custom fields
        /// </summary>
        [HttpPost("create-default-records/{userId}")]
        public async Task<ActionResult> CreateDefaultCustomerRecords(string userId)
        {
            try
            {
                // Get all standard custom fields
                var standardFields = await _context.AolCustomerCustomFields
                    .Where(cf => cf.CustomerCustomType == "Standard")
                    .ToListAsync();

                if (!standardFields.Any())
                {
                    return BadRequest("No standard custom fields found. Please ensure standard fields are seeded.");
                }

                var customerRecords = new List<AspNetCustomer>();

                foreach (var field in standardFields)
                {
                    var customerRecord = new AspNetCustomer
                    {
                        CustomerId = userId,
                        CustomFieldId = field.CustomerFieldId,
                        CustomFieldValue = GetDefaultValueForField(field.CustomerCustomFieldName),
                        CreatedDate = DateTime.UtcNow,
                        ModifiedDate = null
                    };

                    customerRecords.Add(customerRecord);
                }

                _context.AspNetCustomers.AddRange(customerRecords);
                await _context.SaveChangesAsync();

                return Ok(new { 
                    Message = "Default customer records created successfully",
                    RecordsCreated = customerRecords.Count 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating customer records: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets all customer records for a specific user
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetCustomerRecords(string userId)
        {
            try
            {
                var customerRecords = await _context.AspNetCustomers
                    .Where(c => c.CustomerId == userId)
                    .Include(c => c.CustomField)
                    .Select(c => new
                    {
                        c.Id,
                        c.CustomerId,
                        c.CustomFieldId,
                        c.CustomFieldValue,
                        c.CreatedDate,
                        c.ModifiedDate,
                        FieldName = c.CustomField.CustomerCustomFieldName,
                        FieldLabel = c.CustomField.CustomerCustomFieldLabel,
                        FieldType = c.CustomField.CustomerCustomFieldType,
                        FieldDescription = c.CustomField.CustomerCustomFieldDescription,
                        FieldStatus = c.CustomField.CustomerCustomFieldStatus,
                        CustomType = c.CustomField.CustomerCustomType
                    })
                    .ToListAsync();

                return Ok(customerRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving customer records: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new customer record for a specific user and field
        /// </summary>
        [HttpPost("create-record")]
        public async Task<ActionResult> CreateCustomerRecord([FromBody] CreateCustomerRecordRequest request)
        {
            try
            {
                var customerRecord = new AspNetCustomer
                {
                    CustomerId = request.CustomerId,
                    CustomFieldId = request.CustomFieldId,
                    CustomFieldValue = request.CustomFieldValue,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = null
                };

                _context.AspNetCustomers.Add(customerRecord);
                await _context.SaveChangesAsync();

                return Ok(new { 
                    Message = "Customer record created successfully",
                    RecordId = customerRecord.Id 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating customer record: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates a customer record
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomerRecord(int id, [FromBody] UpdateCustomerRecordRequest request)
        {
            try
            {
                var customerRecord = await _context.AspNetCustomers
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (customerRecord == null)
                {
                    return NotFound();
                }

                customerRecord.CustomFieldValue = request.CustomFieldValue;
                customerRecord.ModifiedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { Message = "Customer record updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating customer record: {ex.Message}");
            }
        }

        private string GetDefaultValueForField(string fieldName)
        {
            return fieldName.ToLower() switch
            {
                "customername" => "",
                "customertype" => "",
                "address" => "",
                _ => ""
            };
        }
    }

    public class UpdateCustomerRecordRequest
    {
        public string CustomFieldValue { get; set; } = string.Empty;
    }

    public class CreateCustomerRecordRequest
    {
        public string CustomerId { get; set; } = string.Empty;
        public int CustomFieldId { get; set; }
        public string CustomFieldValue { get; set; } = string.Empty;
    }
}
