using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AOL_Portal.Data;

namespace AOL_Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SelectValueController : ControllerBase
    {
        private readonly AOLContext _context;

        public SelectValueController(AOLContext context)
        {
            _context = context;
        }

        // GET: api/SelectValue
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SelectValue>>> GetSelectValues()
        {
            return await _context.SelectValues.ToListAsync();
        }

        // GET: api/SelectValue/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SelectValue>> GetSelectValue(int id)
        {
            var selectValue = await _context.SelectValues.FindAsync(id);

            if (selectValue == null)
            {
                return NotFound();
            }

            return selectValue;
        }

        // GET: api/SelectValue/field/5
        [HttpGet("field/{fieldId}")]
        public async Task<ActionResult<IEnumerable<SelectValue>>> GetSelectValuesByFieldId(int fieldId)
        {
            var selectValues = await _context.SelectValues.Where(sv => sv.FieldId == fieldId).ToListAsync();
            return selectValues;
        }

        // POST: api/SelectValue
        [HttpPost]
        public async Task<ActionResult<SelectValue>> CreateSelectValue(SelectValue selectValue)
        {
            selectValue.CreatedDate = DateTime.UtcNow;
            _context.SelectValues.Add(selectValue);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSelectValue), new { id = selectValue.Id }, selectValue);
        }

        // POST: api/SelectValue/multiple
        [HttpPost("multiple")]
        public async Task<ActionResult<IEnumerable<SelectValue>>> CreateMultipleSelectValues(IEnumerable<SelectValue> selectValues)
        {
            var selectValueList = selectValues.ToList();
            foreach (var selectValue in selectValueList)
            {
                selectValue.CreatedDate = DateTime.UtcNow;
                _context.SelectValues.Add(selectValue);
            }
            await _context.SaveChangesAsync();

            return Ok(selectValueList);
        }

        // PUT: api/SelectValue/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSelectValue(int id, SelectValue selectValue)
        {
            if (id != selectValue.Id)
            {
                return BadRequest();
            }

            selectValue.ModifiedDate = DateTime.UtcNow;
            _context.Entry(selectValue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SelectValueExists(id))
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

        // DELETE: api/SelectValue/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSelectValue(int id)
        {
            var selectValue = await _context.SelectValues.FindAsync(id);
            if (selectValue == null)
            {
                return NotFound();
            }

            _context.SelectValues.Remove(selectValue);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/SelectValue/field/5
        [HttpDelete("field/{fieldId}")]
        public async Task<IActionResult> DeleteSelectValuesByFieldId(int fieldId)
        {
            var selectValues = await _context.SelectValues.Where(sv => sv.FieldId == fieldId).ToListAsync();
            if (selectValues == null || !selectValues.Any())
            {
                return NotFound();
            }

            _context.SelectValues.RemoveRange(selectValues);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SelectValueExists(int id)
        {
            return _context.SelectValues.Any(e => e.Id == id);
        }
    }
}
