using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RameneToi.Data;
using RameneToi.Models;

namespace RameneToi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationPcController : ControllerBase
    {
        private readonly RameneToiWebAPIContext _context;

        public ConfigurationPcController(RameneToiWebAPIContext context)
        {
            _context = context;
        }

        // GET: api/ConfigurationPcs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConfigurationPcDto>>> GetAll()
        {
            var list = await _context.ConfigurationPcs
                .Include(cp => cp.Composants)
                .Include(cp => cp.Commande)
                .Select(cp => new ConfigurationPcDto
                {
                    Id = cp.Id,
                    NomConfiguration = cp.NomConfiguration,
                    UtilisateurId = cp.UtilisateurId,
                    ComposantIds = cp.Composants.Select(c => c.Id).ToList(),
                    CommandeId = cp.Commande != null ? cp.Commande.Id : (int?)null
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/ConfigurationPcs/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ConfigurationPcDto>> GetById(int id)
        {
            var cp = await _context.ConfigurationPcs
                .Include(c => c.Composants)
                .Include(c => c.Commande)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cp == null) return NotFound();

            var dto = new ConfigurationPcDto
            {
                Id = cp.Id,
                NomConfiguration = cp.NomConfiguration,
                UtilisateurId = cp.UtilisateurId,
                ComposantIds = cp.Composants.Select(c => c.Id).ToList(),
                CommandeId = cp.Commande != null ? cp.Commande.Id : (int?)null
            };

            return Ok(dto);
        }

        // POST: api/ConfigurationPcs
        [HttpPost]
        public async Task<ActionResult<ConfigurationPcDto>> Create(ConfigurationPcCreateDto dto)
        {
            // Validate utilisateur exists
            var userExists = await _context.Utilisateurs.AnyAsync(u => u.Id == dto.UtilisateurId);
            if (!userExists) return BadRequest($"Utilisateur {dto.UtilisateurId} introuvable.");

            // Validate composants and load them
            var composants = new List<Composant>();
            if (dto.ComposantIds?.Any() == true)
            {
                composants = await _context.Composants
                    .Where(c => dto.ComposantIds.Contains(c.Id))
                    .ToListAsync();

                var missing = dto.ComposantIds.Except(composants.Select(c => c.Id)).ToList();
                if (missing.Any()) return BadRequest($"Composant(s) introuvable(s) : {string.Join(", ", missing)}");
            }

            var configuration = new ConfigurationPc
            {
                NomConfiguration = dto.NomConfiguration,
                UtilisateurId = dto.UtilisateurId,
                Composants = composants
            };

            _context.ConfigurationPcs.Add(configuration);
            await _context.SaveChangesAsync();

            var resultDto = new ConfigurationPcDto
            {
                Id = configuration.Id,
                NomConfiguration = configuration.NomConfiguration,
                UtilisateurId = configuration.UtilisateurId,
                ComposantIds = configuration.Composants.Select(c => c.Id).ToList(),
                CommandeId = null
            };

            return CreatedAtAction(nameof(GetById), new { id = configuration.Id }, resultDto);
        }

        // PUT: api/ConfigurationPcs/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ConfigurationPcUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest("L'ID de l'URL et de l'objet diffèrent.");

            var configuration = await _context.ConfigurationPcs
                .Include(c => c.Composants)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (configuration == null) return NotFound();

            // Optionnel : interdire la modification si une commande est associée
            if (configuration.Commande != null)
            {
                // Si vous souhaitez autoriser certaines modifications même si une commande existe,
                // adaptez cette logique.
                return BadRequest("Impossible de modifier une configuration liée à une commande.");
            }

            // Update simple fields
            configuration.NomConfiguration = dto.NomConfiguration;
            configuration.UtilisateurId = dto.UtilisateurId;

            // Update composants (many-to-many)
            var newComposants = new List<Composant>();
            if (dto.ComposantIds?.Any() == true)
            {
                newComposants = await _context.Composants
                    .Where(c => dto.ComposantIds.Contains(c.Id))
                    .ToListAsync();

                var missing = dto.ComposantIds.Except(newComposants.Select(c => c.Id)).ToList();
                if (missing.Any()) return BadRequest($"Composant(s) introuvable(s) : {string.Join(", ", missing)}");
            }

            // Remplacer la collection. EF Core mettra à jour la table de jointure.
            configuration.Composants = newComposants;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ConfigurationExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/ConfigurationPcs/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var configuration = await _context.ConfigurationPcs
                .Include(c => c.Commande)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (configuration == null) return NotFound();

            if (configuration.Commande != null)
            {
                return BadRequest("Impossible de supprimer une configuration liée à une commande.");
            }

            _context.ConfigurationPcs.Remove(configuration);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConfigurationExists(int id) =>
            _context.ConfigurationPcs.Any(e => e.Id == id);
    }

    // DTOs utilisés par le controller
    public class ConfigurationPcDto
    {
        public int Id { get; set; }
        public string NomConfiguration { get; set; } = null!;
        public int UtilisateurId { get; set; }
        public List<int> ComposantIds { get; set; } = new();
        public int? CommandeId { get; set; }
    }

    public class ConfigurationPcCreateDto
    {
        public string NomConfiguration { get; set; } = null!;
        public int UtilisateurId { get; set; }
        public List<int>? ComposantIds { get; set; } = new();
    }

    public class ConfigurationPcUpdateDto
    {
        public int Id { get; set; }
        public string NomConfiguration { get; set; } = null!;
        public int UtilisateurId { get; set; }
        public List<int>? ComposantIds { get; set; } = new();
    }
}