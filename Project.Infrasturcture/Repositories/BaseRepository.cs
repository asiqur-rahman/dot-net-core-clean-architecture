using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Project.Infrasturcture.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Project.Common.Support;
using Project.Core.Interfaces.Repositories;
using X.PagedList;
using Microsoft.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using Project.Core.Exceptions;
using Project.Core.Entities.Helper;

namespace Project.Infrasturcture.Repositories
{

    //Unit of Work Pattern
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        protected DbSet<T> DbSet => _dbContext.Set<T>();

        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAll(string query)
        {
            return await _dbContext.Set<T>().FromSqlRaw(query).AsNoTracking().ToListAsync();
        }

        public async Task<PaginatedDataViewModel<T>> GetPaginatedData(string query, int pageNumber, int pageSize)
        {
            var data = _dbContext.Set<T>()
                .FromSqlRaw(query)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking();

            var result = await data.ToListAsync();
            var totalCount = await _dbContext.Set<T>().FromSqlRaw(query).CountAsync();

            return new PaginatedDataViewModel<T>(result, totalCount);
        }

        public async Task<T> GetById<Tid>(Tid id, string query)
        {
            var data = query != null ? await _dbContext.Set<T>().FromSqlRaw(query).FirstAsync() : await _dbContext.Set<T>().FindAsync(id);
            if (data == null)
                throw new NotFoundException("No data found");
            return data;
        }

        public async Task<bool> IsExists<Tvalue>(string key, Tvalue value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, key);
            var constant = Expression.Constant(value);
            var equality = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

            return await _dbContext.Set<T>().AnyAsync(lambda);
        }

        //Before update existence check
        public async Task<bool> IsExistsForUpdate<Tid>(Tid id, string key, string value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, key);
            var constant = Expression.Constant(value);
            var equality = Expression.Equal(property, constant);

            var idProperty = Expression.Property(parameter, "Id");
            var idEquality = Expression.NotEqual(idProperty, Expression.Constant(id));

            var combinedExpression = Expression.AndAlso(equality, idEquality);
            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);

            return await _dbContext.Set<T>().AnyAsync(lambda);
        }


        public async Task<T> Create(T model)
        {
            await _dbContext.Set<T>().AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task Update(T model)
        {
            _dbContext.Set<T>().Update(model);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(T model)
        {
            _dbContext.Set<T>().Remove(model);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveChangeAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<T>> ExecuteQuery(string query)
        {
            return await _dbContext.Set<T>().FromSqlRaw(query).ToListAsync();
        }

    }
}
