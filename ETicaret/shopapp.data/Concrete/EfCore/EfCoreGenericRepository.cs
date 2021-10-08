using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreGenericRepository<TContext, TEntity> : IRepository<TEntity>
        where TEntity:class
        where TContext: DbContext,new()
    {
        public void Add(TEntity entity)
        {
            using(var context = new TContext())
            {
                context.Entry(entity).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            using(var context = new ShopContext())
            {
                context.Entry(entity).State = EntityState.Added;
                await context.SaveChangesAsync();
                return entity;
            }
        }

        public void Delete(TEntity entity)
        {
            using(var context = new TContext())
            {
                context.Entry(entity).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public async Task DeleteAsync(TEntity entity)
        {
            using(var context = new ShopContext())
            {
                context.Entry(entity).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<TEntity>> GetAll(Expression<Func<TEntity,bool>> filter=null)
        {
            using(var context = new TContext())
            {
                return filter==null?
                    await context.Set<TEntity>().ToListAsync():
                    await context.Set<TEntity>().Where(filter).ToListAsync();
            }
        }

        public async Task<TEntity> GetById(int id)
        {
            using(var context = new TContext())
            {
                return await context.Set<TEntity>().FindAsync(id);
            }
        }

        public virtual void Update(TEntity entity)
        {
            using(var context = new TContext())
            {
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}