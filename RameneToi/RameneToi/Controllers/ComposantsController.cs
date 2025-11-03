using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RameneToi.Data;
using RameneToi.Models;
using RameneToi.Services;

namespace RameneToi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComposantsController : ControllerBase
    {
        private readonly RameneToiWebAPIContext _context;

        public ComposantsController(RameneToiWebAPIContext context)
        {
            _context = context;
        }

        // GET: api/Composants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Composant>>> GetComposants()
        {
            return await _context.Composants.ToListAsync();
        }

        // GET: api/Composants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Composant>> GetComposant(int id)
        {
            var composant = await _context.Composants.FindAsync(id);

            if (composant == null)
            {
                return NotFound();
            }

            return composant;
        }

        // PUT: api/Composants/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComposant(int id, Composant composant)
        {

            //il faut être admin pour pouvoir modifier un composant
            var valid = new AuthorizationService().IsTokenValid(this.Request.Headers.Authorization.ToString(), "admin");

            if (!valid)
            {
                return Unauthorized("Vous n'êtes pas autorisé à modifié un composant.");
            }

            if (id != composant.Id)
            {
                return BadRequest();
            }

            _context.Entry(composant).State = EntityState.Modified;

            var existing = await _context.Composants.FindAsync(id);
            if(existing == null)
            {
                return NotFound();
            }
            //On met juste a jour ce qu'on veut si dessous, pas plus
            existing.Type = composant.Type;
            existing.Marque = composant.Marque;
            existing.Modele = composant.Modele;
            existing.Prix = composant.Prix;
            existing.Stock = composant.Stock;
            existing.Score = composant.Score;

            // exécut l'enregistrement de façon asynchrone qui ne bloque pas le thread
            try
            {
                //SaveChangesAsync = traduit le changement en commande SQL et l'envoie à la DB
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComposantExists(id))
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

        // POST: api/Composants
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Composant>> PostComposant(Composant composant)
        {

            //il faut être admin pour pouvoir ajouter un composant
            var valid = new AuthorizationService().IsTokenValid(this.Request.Headers.Authorization.ToString(), "admin");

            if (!valid)
            {
                return Unauthorized("Vous n'êtes pas autorisé à ajouter un composant.");
            }

            if (composant == null)
            {
                return BadRequest();
            }

            // Juste contrôler les propretes que l'on veut perstité en DB, on persiste pas l'ID
            var composants = new Composant
            {
                Type = composant.Type,
                Marque = composant.Marque,
                Modele = composant.Modele,
                Prix = composant.Prix,
                Stock = composant.Stock,
                Score = composant.Score
            };
            _context.Composants.Add(composants);
            await _context.SaveChangesAsync();

            //outPut GetComposant
            return CreatedAtAction(nameof(GetComposant), new { id = composants.Id }, composants);
        }

        // DELETE: api/Composants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComposant(int id)
        {

            //il faut être admin pour pouvoir supprimer un composant
            var valid = new AuthorizationService().IsTokenValid(this.Request.Headers.Authorization.ToString(), "admin");

            if (!valid)
            {
                return Unauthorized("Vous n'êtes pas autorisé à supprimer un composant.");
            }

            var composant = await _context.Composants.FindAsync(id);
            if (composant == null)
            {
                return NotFound();
            }

            _context.Composants.Remove(composant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComposantExists(int id)
        {
            return _context.Composants.Any(e => e.Id == id);
        }
    }
}
