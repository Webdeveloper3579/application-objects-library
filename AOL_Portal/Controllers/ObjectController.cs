using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AOL_Portal.Data;
using DataObject = AOL_Portal.Data.Object;

namespace AOL_Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ObjectController : ControllerBase
    {
        private readonly AOLContext _context;

        public ObjectController(AOLContext context)
        {
            _context = context;
        }

        // GET: api/Object
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DataObject>>> GetObjects()
        {
            return await _context.Objects.ToListAsync();
        }

        // GET: api/Object/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DataObject>> GetObject(int id)
        {
            var obj = await _context.Objects.FindAsync(id);

            if (obj == null)
            {
                return NotFound();
            }

            return obj;
        }

        // POST: api/Object
        [HttpPost]
        public async Task<ActionResult<DataObject>> CreateObject(DataObject obj)
        {
            obj.CreatedDate = DateTime.UtcNow;
            _context.Objects.Add(obj);
            await _context.SaveChangesAsync();

            return Ok(obj);
        }

        // PUT: api/Object/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateObject(int id, DataObject obj)
        {
            if (id != obj.Id)
            {
                return BadRequest();
            }

            obj.ModifiedDate = DateTime.UtcNow;
            _context.Entry(obj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObjectExists(id))
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

        // DELETE: api/Object/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObject(int id)
        {
            var obj = await _context.Objects.FindAsync(id);
            if (obj == null)
            {
                return NotFound();
            }

            _context.Objects.Remove(obj);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ObjectExists(int id)
        {
            return _context.Objects.Any(e => e.Id == id);
        }
    }
}
