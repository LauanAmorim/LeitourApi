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
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal LeitourContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(LeitourContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        // Implements General Methods

        public Task<List<TEntity>> GetAll() => dbSet.ToListAsync();

        public Task<List<TEntity>> GetByAll(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}