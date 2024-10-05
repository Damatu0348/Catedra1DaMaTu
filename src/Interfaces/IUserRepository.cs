using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Dtos;
using api.src.Models;

namespace api.src.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();

        Task<List<User>> GetAllUsersAsync(string sort, string gender);
        Task<User?> GetUserByRutAsync(string rut);
        Task<User?> GetUserByIdAsync(int id);
        Task<User> PostUser(User user);
        Task PutUser(User user);
    }
}