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

        //Metodo para obtener TODOS los usuarios, dando la posibilidad de filtrar por nombre y genero
        public async Task<IEnumerable<User>> GetAllUsersAsync(string sort, string gender)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(gender))
                query = query.Where(u => u.Gender == gender);

            if (!string.IsNullOrEmpty(sort))
                query = sort == "asc" ? query.OrderBy(u => u.Name) : query.OrderByDescending(u => u.Name);

            return await query.ToListAsync();
        }
    }
}