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
        //Usado al usar Gets en UserController
        public static UserDto ToUserDto(this User userModel)
        {
            //Retorna SOLO lo que esta dentro del metodo
            return new UserDto
            {
                Id = userModel.Id,
                Rut = userModel.Rut,
                Name = userModel.Name,
                Email = userModel.Email,
                Gender = userModel.Gender,
                BirthDate = userModel.BirthDate
            };
        }

        //Usado al usar Posts en UserController
        public static User ToUserFromPostDto(this PostUserRequestDto postUserRequestDto)
        {

            return new User
            {
                Rut = postUserRequestDto.Rut,
                Name = postUserRequestDto.Name,
                Email = postUserRequestDto.Email,
                Gender = postUserRequestDto.Gender,
                BirthDate = postUserRequestDto.BirthDate
            };
            
        }
    }
}