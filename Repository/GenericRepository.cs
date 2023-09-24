using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using LeitourApi.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using LeitourApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LeitourApi.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal readonly LeitourContext context;
        internal DbSet<TEntity> DbSet;

        public GenericRepository(LeitourContext context)
        {
            this.context = context;
            DbSet = context.Set<TEntity>();
        }

        // Implements General Methods

        public Task<List<TEntity>> GetAll() => DbSet.ToListAsync();

        public string Debug(string value) => "Valor: " + value;

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