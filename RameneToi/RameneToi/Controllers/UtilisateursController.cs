using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RameneToi.Data;
using RameneToi.Models;
using RameneToi.Services;
using BCrypt.Net;

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
            //il faut être admin pour voir la liste des utilisateurs
            var valid = new AuthorizationService().IsTokenValid(this.Request.Headers.Authorization.ToString(), "admin");

            if(!valid)
            {
                return Unauthorized("Vous n'êtes pas autorisé à voir la liste des utilisateurs.");
            }
            return await _context.Utilisateurs
                .Include(u => u.Commandes)
                .ToListAsync();
        }

        // GET: api/Utilisateurs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Utilisateurs>> GetUtilisateurs(int id)
        {
            var utilisateurs = await _context.Utilisateurs
                .Include(u => u.Commandes)
                .FirstOrDefaultAsync(u => u.Id == id);

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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Hachage avec BCrypt 
            utilisateurs.MotDePasse = BCrypt.Net.BCrypt.HashPassword(utilisateurs.MotDePasse, workFactor: 12);

            _context.Utilisateurs.Add(utilisateurs);
            await _context.SaveChangesAsync();

            // Ne renvoie pas le mot de passe haché idéalement — pour l'instant retourne l'entité (réfléchis DTO)
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] string Email , [FromForm] string password)
        {
            var user = await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == Email);
            if (user == null) return BadRequest("Email ou mot de passe invalide.");

            bool verified = false;

            try
            {
                // Vérification BCrypt
                verified = BCrypt.Net.BCrypt.Verify(password, user.MotDePasse);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                // Fallback : l'ancien hash n'est pas bcrypt (ex: PasswordHasher). Tenter la verification avec PasswordHasher
                var ph = new PasswordHasher<Utilisateurs>();
                var res = ph.VerifyHashedPassword(user, user.MotDePasse, password);
                if (res == PasswordVerificationResult.Success || res == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    // Ré-hash avec BCrypt et sauvegarde (migration progressive des comptes)
                    user.MotDePasse = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
                    _context.Utilisateurs.Update(user);
                    await _context.SaveChangesAsync();
                    verified = true;
                }
            }

            if (!verified) return BadRequest("Email ou mot de passe invalide.");

            var token = new AuthorizationService().CreateToken(user);
            return Ok(new { token });
        }
    }
}
