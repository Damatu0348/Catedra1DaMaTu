using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Data;
using api.src.Mappers;
using api.src.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UserController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            //Retorna un listado de todos los usuarios en la base de datos
            var users = _context.Users.ToList()
            .Select(u => u.ToUserDto());
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            // Si no hay usuarios en la base de datos
            if(user == null)
            {
                return NotFound("Usuario NO eistente en el sistema :(");
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            // Validaciones de modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Comprobar si el RUT ya existe
            if (_context.Users.Any(u => u.Rut == user.Rut))
            {
                return Conflict("El RUT ya existe.");
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Post), new { id = user.Rut }, user);
        }

        [HttpPut("{id}")]
        public IActionResult PutId([FromRoute] int id, [FromBody] User user)
        {
            var userToUpdate = _context.Users.FirstOrDefault(u => u.Id == id);
            //Si no existe un usuario para actualizar
            if(userToUpdate == null)
            {
                return NotFound();
            }
            userToUpdate.Rut = user.Rut;
            userToUpdate.Name = user.Name;
            userToUpdate.Email = user.Email;
            userToUpdate.Gender = user.Gender;
            userToUpdate.BirthDate = user.BirthDate;

            _context.SaveChanges();
            return Ok(userToUpdate);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteId([FromRoute] int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if(user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok("Usuario eliminado exitosamente");
        }
    }
}