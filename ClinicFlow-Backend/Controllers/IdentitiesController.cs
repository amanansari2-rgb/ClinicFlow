using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicFlow_Backend.Data;
using ClinicFlow_Backend.Model;

namespace ClinicFlow_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentitiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IdentitiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Identities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Identity>>> Getidentity()
        {
            return await _context.identity.ToListAsync();
        }

        // GET: api/Identities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Identity>> GetIdentity(int id)
        {
            var identity = await _context.identity.FindAsync(id);

            if (identity == null)
            {
                return NotFound();
            }

            return identity;
        }

        // PUT: api/Identities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIdentity(int id, Identity identity)
        {
            if (id != identity.Id)
            {
                return BadRequest();
            }

            _context.Entry(identity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IdentityExists(id))
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

        // POST: api/Identities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Identity>> PostIdentity(Identity identity)
        {
            _context.identity.Add(identity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIdentity", new { id = identity.Id }, identity);
        }

        // DELETE: api/Identities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIdentity(int id)
        {
            var identity = await _context.identity.FindAsync(id);
            if (identity == null)
            {
                return NotFound();
            }

            _context.identity.Remove(identity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IdentityExists(int id)
        {
            return _context.identity.Any(e => e.Id == id);
        }
    }
}
