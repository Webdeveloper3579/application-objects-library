using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AOL_Portal.Data;

namespace AOL_Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly AOLContext _context;

        public CityController(AOLContext context)
        {
            _context = context;
        }

        // GET: api/City
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
            try
            {
                var cities = await _context.Cities
                    .OrderBy(c => c.CityName)
                    .ToListAsync();

                return Ok(cities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving cities: {ex.Message}");
            }
        }

        // GET: api/City/5
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(int id)
        {
            try
            {
                var city = await _context.Cities
                    .FirstOrDefaultAsync(c => c.CityId == id);

                if (city == null)
                {
                    return NotFound();
                }

                return Ok(city);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving city: {ex.Message}");
            }
        }

        // POST: api/City
        [HttpPost]
        public async Task<ActionResult<City>> CreateCity(City city)
        {
            try
            {
                city.CreatedDate = DateTime.UtcNow;
                _context.Cities.Add(city);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCity), new { id = city.CityId }, city);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating city: {ex.Message}");
            }
        }

        // PUT: api/City/5
        [HttpPut("{id}")]
        public async Task<ActionResult<City>> UpdateCity(int id, City city)
        {
            try
            {
                if (id != city.CityId)
                {
                    return BadRequest("ID mismatch");
                }

                var existingCity = await _context.Cities
                    .FirstOrDefaultAsync(c => c.CityId == id);

                if (existingCity == null)
                {
                    return NotFound();
                }

                existingCity.CityName = city.CityName;
                existingCity.CityCode = city.CityCode;
                existingCity.CityCounty = city.CityCounty;
                existingCity.CityCountry = city.CityCountry;
                existingCity.ModifiedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(existingCity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating city: {ex.Message}");
            }
        }

        // DELETE: api/City/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCity(int id)
        {
            try
            {
                var city = await _context.Cities
                    .FirstOrDefaultAsync(c => c.CityId == id);

                if (city == null)
                {
                    return NotFound();
                }

                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting city: {ex.Message}");
            }
        }
    }
}

