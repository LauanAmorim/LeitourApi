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
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LeitourApi.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly LeitourContext context;
        internal DbSet<T> DbSet;

        public GenericRepository(LeitourContext context)
        {
            this.context = context;
            DbSet = context.Set<T>();
        }

        public async Task<T> GetById(int id) => await DbSet.FindAsync(id);
        public async Task<List<T>> GetAll() => await DbSet.ToListAsync();

       // Expression<Func<T, bool>> vFilter = c => c.IsDeleted == false;
       // public async Task<List<T>> GetAllById(int id) => await DbSet.Where(e => e.Id == id).ToListAsync();

        public async Task<T> FindByCondition(Expression<Func<T, bool>> predicate) =>
            await DbSet.FirstOrDefaultAsync(predicate);
        
        public async Task<List<T>> FindByConditionList(Expression<Func<T, bool>> predicate) =>
            await DbSet.Where(predicate).ToListAsync();

        // Observação, talvez esses métodos abaixo precisem do SaveChangesAsync.
        public async Task Add(T entity){
            await DbSet.AddAsync(entity);
        //    await context.SaveChangesAsync();
        }
        public async Task Update(T entity){ 
            context.Entry(entity).State = EntityState.Modified;
        //    await context.SaveChangesAsync();
        }
        public async Task Delete(T entity){
            DbSet.Remove(entity);
        //    await context.SaveChangesAsync();
        }

        public string Debug(string value) => "Valor: " + value;
    }
}