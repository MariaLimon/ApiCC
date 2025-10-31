using Microsoft.AspNetCore.Mvc;
using ApiCC.models;

namespace ApiCC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static List<User> users = new()
        {
            new User { Id=1, Name = "Prueba", LastName="Api", Email="pruebaapi@email.com", IsAdmin=false, IsEmailConfirmed=true,}
        };

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(users);
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = users.FirstOrDefault(e => e.Id == id);
            if (user == null)
                return NotFound(new{error = "Usuario no encontrado"});

            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> CreateUser([FromBody]User user)
        {
            user.Id = users.Max(e => e.Id) + 1;
            users.Add(user);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
    }
}