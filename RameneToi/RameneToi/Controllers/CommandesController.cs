using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public class CommandesController : ControllerBase
    {
        private readonly RameneToiWebAPIContext _context;

        public CommandesController(RameneToiWebAPIContext context)
        {
            _context = context;
        }

        // GET: api/Commandes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Commande>>> GetCommande()
        {
            //il faut être admin pour voir la liste des utilisateurs
            var valid = new AuthorizationService().IsTokenValid(this.Request.Headers.Authorization.ToString(), "admin");

            if (!valid)
            {
                return Unauthorized("Vous n'êtes pas autorisé à voir la liste des utilisateurs.");
            }

            return await _context.Commandes.ToListAsync();
        }

        // GET: api/Commandes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Commande>> GetCommande(int id)
        {
            var commande = await _context.Commandes.FindAsync(id);

            if (commande == null)
            {
                return NotFound();
            }

            return commande;
        }

        // PUT: api/Commandes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommande(int id, Commande commande)
        {
            if (id != commande.Id)
            {
                return BadRequest();
            }

            _context.Entry(commande).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommandeExists(id))
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

        // POST: api/Commandes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Commande>> PostCommande(Commande commande)
        {

            var LoadConfig = await _context.ConfigurationPcs
                .Include(c => c.Composants)
                .FirstOrDefaultAsync(c => c.Id == commande.ConfigurationPcId);

            if(LoadConfig == null) { return BadRequest($"Configuration PC {commande.ConfigurationPcId} introuvable"); }

            //commande.PrixConfiguration non nullable de base donc obligé de mettre ?? 0m = 0 type decimal
            //Si LoadConfig.Composants est null alors renvoie null
            //si Composants existe mais vide sum renvoie 0 alors
            decimal totalConfigHT = LoadConfig.Composants?.Sum(cp => Convert.ToDecimal(cp.Prix)) ?? 0m;

            decimal totalConfigTva = (totalConfigHT * 0.21m) + totalConfigHT;


            commande.PrixConfiguration = totalConfigTva;
            
            _context.Commandes.Add(commande);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommande", new { id = commande.Id }, commande);
        }

        // DELETE: api/Commandes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommande(int id)
        {
            var commande = await _context.Commandes.FindAsync(id);
            if (commande == null)
            {
                return NotFound();
            }

            _context.Commandes.Remove(commande);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommandeExists(int id)
        {
            return _context.Commandes.Any(e => e.Id == id);
        }
    }
}
