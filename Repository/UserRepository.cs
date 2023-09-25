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
    public class UserRepository: GenericRepository<User>, IUserRepository
    {        
   
        public UserRepository(LeitourContext context) : base(context)
        {
        }

        // Implements General Methods
        public Task<List<User>> GetAllUsers() => context.Set<User>().GetAll().ToListAsync();
        
    }
}