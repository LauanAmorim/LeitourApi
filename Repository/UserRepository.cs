using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using LeitourApi.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using LeitourApi.Interfaces;

namespace LeitourApi.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {        
        public UserRepository(LeitourContext context) : base(context)
        {
        }

        // Implements General Methods
        public Task<List<User>> GetAll() => DbSet.ToListAsync();//context.Set<User>().GetAll().ToListAsync();
        public async Task<User> GetById(int id) => await DbSet.FindAsync(id);//context.Set<User>().GetAll().ToListAsync();
        
    }
}