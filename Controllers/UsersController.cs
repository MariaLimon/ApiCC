using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiCC.Models;
using ApiCC.Data;

namespace ApiCC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ ELIMINA LA LISTA ESTÁTICA
        // private static List<User> users = new() { ... };

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { error = "Usuario no encontrado" });

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Obtener el siguiente ID
            var maxId = await _context.Users.MaxAsync(u => (int?)u.Id) ?? 0;
            user.Id = maxId + 1;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { error = "Usuario no encontrado" });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        [HttpGet("test-db")]
        public async Task<ActionResult> TestDatabaseConnection()
        {
            try
            {
                var count = await _context.Users.CountAsync();
                return Ok(new { 
                    message = "Conexión exitosa a PostgreSQL", 
                    totalUsers = count 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = "Error de conexión a PostgreSQL", 
                    details = ex.Message 
                });
            }
        }
    }
}