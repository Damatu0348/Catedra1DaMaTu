using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Dtos;
using api.src.Models;

namespace api.src.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this User userModel)
        {
            //Retorna SOLO lo que esta dentro del metodo
            return new UserDto
            {
                //Id = userModel.Id,
                Rut = userModel.Rut,
                Name = userModel.Name,
                Email = userModel.Email,
                Gender = userModel.Gender,
                BirthDate = userModel.BirthDate
            };
        }
    }
}