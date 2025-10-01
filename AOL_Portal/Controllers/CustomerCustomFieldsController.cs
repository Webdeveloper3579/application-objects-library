using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AOL_Portal.Data;

namespace AOL_Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerCustomFieldsController : ControllerBase
    {
        private readonly AOLContext _context;

        public CustomerCustomFieldsController(AOLContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AolCustomerCustomField>>> GetCustomerCustomFields()
        {
            var customFields = await _context.AolCustomerCustomFields
                .OrderBy(cf => cf.CustomerCustomFieldName)
                .ToListAsync();

            return Ok(customFields);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AolCustomerCustomField>> UpdateCustomerCustomField(int id, AolCustomerCustomField updatedField)
        {
            try
            {
                var existingField = await _context.AolCustomerCustomFields
                    .FirstOrDefaultAsync(cf => cf.CustomerFieldId == id);

                if (existingField == null)
                {
                    return NotFound();
                }

                // Update only the fields that can be modified
                existingField.CustomerCustomFieldLabel = updatedField.CustomerCustomFieldLabel;
                existingField.CustomerCustomFieldDescription = updatedField.CustomerCustomFieldDescription;
                existingField.CustomerCustomFieldStatus = updatedField.CustomerCustomFieldStatus;
                existingField.ModifiedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(existingField);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<AolCustomerCustomField>> CreateCustomerCustomField(AolCustomerCustomField newField)
        {
            try
            {
                // Set default values
                newField.CreatedDate = DateTime.UtcNow;
                newField.ModifiedDate = null;
                newField.CustomerCustomFieldStatus = "released";
                newField.CustomerCustomType = "Custom";

                _context.AolCustomerCustomFields.Add(newField);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCustomerCustomFields), new { }, newField);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
