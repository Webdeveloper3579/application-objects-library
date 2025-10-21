using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AOL_Portal.Data;

namespace AOL_Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FieldValueController : ControllerBase
    {
        private readonly AOLContext _context;

        public FieldValueController(AOLContext context)
        {
            _context = context;
        }

        // GET: api/FieldValue
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FieldValue>>> GetFieldValues()
        {
            return await _context.FieldValues.ToListAsync();
        }

        // GET: api/FieldValue/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FieldValue>> GetFieldValue(int id)
        {
            var fieldValue = await _context.FieldValues.FindAsync(id);

            if (fieldValue == null)
            {
                return NotFound();
            }

            return fieldValue;
        }

        // GET: api/FieldValue/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<FieldValue>>> GetFieldValuesByUserId(string userId)
        {
            var fieldValues = await _context.FieldValues.Where(fv => fv.UserId == userId).ToListAsync();
            return fieldValues;
        }

        // GET: api/FieldValue/object/{objectId}
        [HttpGet("object/{objectId}")]
        public async Task<ActionResult<IEnumerable<FieldValue>>> GetFieldValuesByObjectId(int objectId)
        {
            var fieldValues = await _context.FieldValues.Where(fv => fv.ObjectId == objectId).ToListAsync();
            return fieldValues;
        }

        // GET: api/FieldValue/field/{fieldId}
        [HttpGet("field/{fieldId}")]
        public async Task<ActionResult<IEnumerable<FieldValue>>> GetFieldValuesByFieldId(int fieldId)
        {
            var fieldValues = await _context.FieldValues.Where(fv => fv.FieldId == fieldId).ToListAsync();
            return fieldValues;
        }

        // GET: api/FieldValue/user/{userId}/object/{objectId}
        [HttpGet("user/{userId}/object/{objectId}")]
        public async Task<ActionResult<IEnumerable<FieldValue>>> GetFieldValuesByUserAndObject(string userId, int objectId)
        {
            var fieldValues = await _context.FieldValues
                .Where(fv => fv.UserId == userId && fv.ObjectId == objectId)
                .ToListAsync();
            return fieldValues;
        }

        // POST: api/FieldValue
        [HttpPost]
        public async Task<ActionResult<FieldValue>> CreateFieldValue(FieldValue fieldValue)
        {
            fieldValue.CreatedDate = DateTime.UtcNow;
            _context.FieldValues.Add(fieldValue);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFieldValue), new { id = fieldValue.Id }, fieldValue);
        }

        // POST: api/FieldValue/multiple
        [HttpPost("multiple")]
        public async Task<ActionResult<IEnumerable<FieldValue>>> CreateMultipleFieldValues(IEnumerable<FieldValue> fieldValues)
        {
            var fieldValueList = fieldValues.ToList();
            foreach (var fieldValue in fieldValueList)
            {
                if (fieldValue.Id == 0)
                {
                    // Create new field value
                    fieldValue.CreatedDate = DateTime.UtcNow;
                    _context.FieldValues.Add(fieldValue);
                }
                else
                {
                    // Update existing field value
                    fieldValue.ModifiedDate = DateTime.UtcNow;
                    _context.Entry(fieldValue).State = EntityState.Modified;
                }
            }
            await _context.SaveChangesAsync();

            return Ok(fieldValueList);
        }

        // PUT: api/FieldValue/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFieldValue(int id, FieldValue fieldValue)
        {
            if (id != fieldValue.Id)
            {
                return BadRequest();
            }

            fieldValue.ModifiedDate = DateTime.UtcNow;
            _context.Entry(fieldValue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FieldValueExists(id))
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

        // DELETE: api/FieldValue/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFieldValue(int id)
        {
            var fieldValue = await _context.FieldValues.FindAsync(id);
            if (fieldValue == null)
            {
                return NotFound();
            }

            _context.FieldValues.Remove(fieldValue);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/FieldValue/user/{userId}
        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteFieldValuesByUserId(string userId)
        {
            var fieldValues = await _context.FieldValues.Where(fv => fv.UserId == userId).ToListAsync();
            if (fieldValues == null || !fieldValues.Any())
            {
                return NotFound();
            }

            _context.FieldValues.RemoveRange(fieldValues);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/FieldValue/object/{objectId}
        [HttpDelete("object/{objectId}")]
        public async Task<IActionResult> DeleteFieldValuesByObjectId(int objectId)
        {
            var fieldValues = await _context.FieldValues.Where(fv => fv.ObjectId == objectId).ToListAsync();
            if (fieldValues == null || !fieldValues.Any())
            {
                return NotFound();
            }

            _context.FieldValues.RemoveRange(fieldValues);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/FieldValue/field/{fieldId}
        [HttpDelete("field/{fieldId}")]
        public async Task<IActionResult> DeleteFieldValuesByFieldId(int fieldId)
        {
            var fieldValues = await _context.FieldValues.Where(fv => fv.FieldId == fieldId).ToListAsync();
            if (fieldValues == null || !fieldValues.Any())
            {
                return NotFound();
            }

            _context.FieldValues.RemoveRange(fieldValues);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FieldValueExists(int id)
        {
            return _context.FieldValues.Any(e => e.Id == id);
        }
    }
}
