using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AOL_Portal.Data;

namespace AOL_Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FieldController : ControllerBase
    {
        private readonly AOLContext _context;

        public FieldController(AOLContext context)
        {
            _context = context;
        }

        // GET: api/Field
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Field>>> GetFields()
        {
            return await _context.Fields.ToListAsync();
        }

        // GET: api/Field/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Field>> GetField(int id)
        {
            var field = await _context.Fields.FindAsync(id);

            if (field == null)
            {
                return NotFound();
            }

            return field;
        }

        // GET: api/Field/object/5
        [HttpGet("object/{objectId}")]
        public async Task<ActionResult<IEnumerable<Field>>> GetFieldsByObjectId(int objectId)
        {
            var fields = await _context.Fields.Where(f => f.ObjectId == objectId).ToListAsync();
            return fields;
        }

        // POST: api/Field
        [HttpPost]
        public async Task<ActionResult<Field>> CreateField(Field field)
        {
            field.CreatedDate = DateTime.UtcNow;
            _context.Fields.Add(field);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetField), new { id = field.Id }, field);
        }

        // POST: api/Field/multiple
        [HttpPost("multiple")]
        public async Task<ActionResult<IEnumerable<Field>>> CreateMultipleFields(IEnumerable<Field> fields)
        {
            var fieldList = fields.ToList();
            foreach (var field in fieldList)
            {
                field.CreatedDate = DateTime.UtcNow;
                _context.Fields.Add(field);
            }
            await _context.SaveChangesAsync();

            return Ok(fieldList);
        }

        // PUT: api/Field/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateField(int id, Field field)
        {
            if (id != field.Id)
            {
                return BadRequest();
            }

            field.ModifiedDate = DateTime.UtcNow;
            _context.Entry(field).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FieldExists(id))
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

        // DELETE: api/Field/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteField(int id)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null)
            {
                return NotFound();
            }

            _context.Fields.Remove(field);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FieldExists(int id)
        {
            return _context.Fields.Any(e => e.Id == id);
        }
    }
}
