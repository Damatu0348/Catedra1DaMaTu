using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Data;
using api.src.Interfaces;
using api.src.Models;
using Microsoft.EntityFrameworkCore;

namespace api.src.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        
        public UserRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        //Metodo para obtener TODOS los usuarios, dando la posibilidad de filtrar por nombre y genero
        public async Task<List<User>> GetAllUsersAsync(string sort, string gender)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(gender))
                query = query.Where(u => u.Gender == gender);

            if (!string.IsNullOrEmpty(sort))
                query = sort == "asc" ? query.OrderBy(u => u.Name) : query.OrderByDescending(u => u.Name);

            return await query.ToListAsync();
        }

        public async Task<User> GetUserByRutAsync(string rut)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Rut == rut);
        }

        public async Task<User> PostUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}