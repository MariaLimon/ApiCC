// Controllers/UsersController.cs
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
        private readonly ILogger<UsersController> _logger;

        public UsersController(ApplicationDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return Ok(new { 
                    success = true,
                    data = users,
                    total = users.Count 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios");
                return StatusCode(500, new { 
                    success = false, 
                    error = "Error al obtener usuarios", 
                    details = ex.Message 
                });
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { 
                        success = false, 
                        error = "Usuario no encontrado" 
                    });
                }
                
                return Ok(new { 
                    success = true, 
                    data = user 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario {id}", id);
                return StatusCode(500, new { 
                    success = false, 
                    error = "Error al obtener usuario", 
                    details = ex.Message 
                });
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { 
                        success = false, 
                        error = "Datos inválidos", 
                        details = ModelState 
                    });
                }

                // Validar email único
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == user.Email);
                
                if (existingUser != null)
                {
                    return BadRequest(new { 
                        success = false, 
                        error = "El email ya está registrado" 
                    });
                }

                // Generar ID
                var maxId = await _context.Users.MaxAsync(u => (int?)u.Id) ?? 0;
                user.Id = maxId + 1;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new { 
                    success = true, 
                    data = user 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear usuario");
                return StatusCode(500, new { 
                    success = false, 
                    error = "Error al crear usuario", 
                    details = ex.Message 
                });
            }
        }

        // POST: api/Users/batch
        [HttpPost("batch")]
        public async Task<ActionResult<List<User>>> CreateMultipleUsers([FromBody] List<User> users)
        {
            try
            {
                if (users == null || !users.Any())
                {
                    return BadRequest(new { 
                        success = false, 
                        error = "La lista de usuarios no puede estar vacía" 
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new { 
                        success = false, 
                        error = "Datos inválidos", 
                        details = ModelState 
                    });
                }

                var createdUsers = new List<User>();
                var maxId = await _context.Users.MaxAsync(u => (int?)u.Id) ?? 0;

                foreach (var user in users)
                {
                    // Validar email único
                    var existingUser = await _context.Users
                        .FirstOrDefaultAsync(u => u.Email == user.Email);
                    
                    if (existingUser != null)
                    {
                        return BadRequest(new { 
                            success = false, 
                            error = $"El email {user.Email} ya está registrado" 
                        });
                    }

                    user.Id = ++maxId;
                    _context.Users.Add(user);
                    createdUsers.Add(user);
                }

                await _context.SaveChangesAsync();

                return Ok(new { 
                    success = true, 
                    message = $"Se crearon {createdUsers.Count} usuarios exitosamente",
                    data = createdUsers 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear múltiples usuarios");
                return StatusCode(500, new { 
                    success = false, 
                    error = "Error al crear usuarios", 
                    details = ex.Message 
                });
            }
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] User user)
        {
            try
            {
                if (id != user.Id)
                {
                    return BadRequest(new { 
                        success = false, 
                        error = "El ID no coincide" 
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new { 
                        success = false, 
                        error = "Datos inválidos", 
                        details = ModelState 
                    });
                }

                var existingUser = await _context.Users.FindAsync(id);
                if (existingUser == null)
                {
                    return NotFound(new { 
                        success = false, 
                        error = "Usuario no encontrado" 
                    });
                }

                // Validar email único (excepto el actual)
                var emailUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == user.Email && u.Id != id);
                
                if (emailUser != null)
                {
                    return BadRequest(new { 
                        success = false, 
                        error = "El email ya está registrado por otro usuario" 
                    });
                }

                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { 
                    success = true, 
                    data = user 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar usuario {id}", id);
                return StatusCode(500, new { 
                    success = false, 
                    error = "Error al actualizar usuario", 
                    details = ex.Message 
                });
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { 
                        success = false, 
                        error = "Usuario no encontrado" 
                    });
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(new { 
                    success = true, 
                    message = "Usuario eliminado exitosamente" 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar usuario {id}", id);
                return StatusCode(500, new { 
                    success = false, 
                    error = "Error al eliminar usuario", 
                    details = ex.Message 
                });
            }
        }

        // GET: api/Users/stats
        [HttpGet("stats")]
        public async Task<ActionResult> GetStats()
        {
            try
            {
                var totalUsers = await _context.Users.CountAsync();
                var adminUsers = await _context.Users.CountAsync(u => u.IsAdmin);
                var confirmedUsers = await _context.Users.CountAsync(u => u.IsEmailConfirmed);

                return Ok(new { 
                    success = true,
                    data = new {
                        totalUsers = totalUsers,
                        totalAdmins = adminUsers,
                        confirmedEmails = confirmedUsers,
                        pendingEmails = totalUsers - confirmedUsers,
                        regularUsers = totalUsers - adminUsers
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas");
                return StatusCode(500, new { 
                    success = false, 
                    error = "Error al obtener estadísticas", 
                    details = ex.Message 
                });
            }
        }

        // GET: api/Users/test-db
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