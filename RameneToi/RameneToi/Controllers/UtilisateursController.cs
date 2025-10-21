using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RameneToi.Data;
using RameneToi.Models;

namespace RameneToi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly RameneToiWebAPIContext _context;

        public UtilisateursController(RameneToiWebAPIContext context)
        {
            _context = context;
        }

        // GET: api/Utilisateurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilisateurs>>> GetUtilisateurs()
        {
            return await _context.Utilisateurs.ToListAsync();
        }

        // GET: api/Utilisateurs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Utilisateurs>> GetUtilisateurs(int id)
        {
            var utilisateurs = await _context.Utilisateurs.FindAsync(id);

            if (utilisateurs == null)
            {
                return NotFound();
            }

            return utilisateurs;
        }

        // PUT: api/Utilisateurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilisateurs(int id, Utilisateurs utilisateurs)
        {
            if (id != utilisateurs.Id)
            {
                return BadRequest();
            }

            _context.Entry(utilisateurs).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilisateursExists(id))
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

        // POST: api/Utilisateurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Utilisateurs>> PostUtilisateurs(Utilisateurs utilisateurs)
        {
            _context.Utilisateurs.Add(utilisateurs);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUtilisateurs", new { id = utilisateurs.Id }, utilisateurs);
        }

        // DELETE: api/Utilisateurs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilisateurs(int id)
        {
            var utilisateurs = await _context.Utilisateurs.FindAsync(id);
            if (utilisateurs == null)
            {
                return NotFound();
            }

            _context.Utilisateurs.Remove(utilisateurs);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UtilisateursExists(int id)
        {
            return _context.Utilisateurs.Any(e => e.Id == id);
        }
    }
}
