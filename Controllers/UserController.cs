using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Data;
using api.src.Dtos;
using api.src.Interfaces;
using api.src.Mappers;
using api.src.Models;
using api.src.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //Retorna un listado de todos los usuarios en la base de datos
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string sort, [FromQuery] string gender)
        {
            if (!string.IsNullOrEmpty(sort) && sort != "asc" && sort != "desc")
            {
                return BadRequest("Orden de nombre inválido");
            }

            if (!string.IsNullOrEmpty(gender) && gender != "Masculino" && gender != "Femenino" && gender != "Otro" && gender != "Prefiero no decirlo")
            {
                return BadRequest("Género inválido");
            }
                

            var users = await _userRepository.GetAllUsersAsync(sort, gender);
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostUserRequestDto userDto)
        {
            //Si el rut NO es valido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userRepository.GetUserByRutAsync(userDto.Rut);
            //Si el Rut YA existe
            if (existingUser != null)
            {
                return Conflict("El RUT ya existe.");
            }

            //Si el genero NO es uno de los disponibles
            if (userDto.Gender != "Masculino" && userDto.Gender != "Femenino" && userDto.Gender != "Otro" && userDto.Gender != "Prefiero no decirlo")
            {
                return BadRequest("El género proporcionado no es válido.");
            }

            var userModel = userDto.ToUserFromPostDto();
            await _userRepository.PostUser(userModel);
            return CreatedAtAction(nameof(GetAll), new { id = userModel.Id }, userDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutId([FromRoute] int id, [FromBody] PutUserRequestDto putUser)
        {
            var userExist = await _userRepository.GetUserByIdAsync(id);
            //Si no existe un usuario para actualizar
            if(userExist == null)
            {
                return NotFound("Usuario NO encontrado.");
            }
            var userSameRut = await _userRepository.GetUserByRutAsync(putUser.Rut);
            if(userSameRut != null && userSameRut.Id != id)
            {
                return Conflict("El Rut YA esta registrado por otro usuario");
            }

            if (putUser.Gender != "Masculino" && putUser.Gender != "Femenino" && putUser.Gender != "Otro" && putUser.Gender != "Prefiero no decirlo")
            {
                return BadRequest("El género escrito NO es válido.");
            }
            
            userExist.Rut = putUser.Rut;
            userExist.Name = putUser.Name;
            userExist.Email = putUser.Email;
            userExist.Gender = putUser.Gender;
            userExist.BirthDate = putUser.BirthDate;

            await _userRepository.PutUser(userExist);
            return Ok(userExist);
        }



        /*
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userRepository.FirstOrDefault(u => u.Id == id);
            // Si no hay usuarios en la base de datos
            if(user == null)
            {
                return NotFound("Usuario NO eistente en el sistema :(");
            }
            return Ok(user);
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
        */
        
    }
}