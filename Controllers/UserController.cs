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
            await _userRepository.PostUserAsync(userModel);
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

            await _userRepository.PutUserAsync(userExist);
            return Ok("Usuario editado exitosamente");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if(user == null)
            {
                return NotFound("Usuario NO encontrado");
            }
            await _userRepository.DeleteUserAsync(id);
            return Ok("Usuario eliminado exitosamente");
        }        
    }
}