using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.DAOS
{
    public abstract class BaseDAO<TEntity> where TEntity : BaseEntity
    {

        public static async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            IEnumerable<TEntity> list;
            using (var context = new AppDBContext())
            {
                list = await includes.Aggregate(context.Set<TEntity>().AsQueryable()
                    , (c, i) => c.Include(i)).ToListAsync();
            }
            return list;
        }

        public static async Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes)
        {
            TEntity? entity;
            using (var context = new AppDBContext())
            {
                var query = includes.Aggregate(context.Set<TEntity>().AsQueryable()
                    , (c, i) => c.Include(i));
                entity = await query.FirstOrDefaultAsync(x => x.Id == id);
            }
            return entity;
        }

        public static async Task AddAsync(TEntity p)
        {
            using (var context = new AppDBContext())
            {
                context.Set<TEntity>().Add(p);
                await context.SaveChangesAsync();
            }
        }

        public static async Task AddRangeAsync(List<TEntity> ls)
        {
            using (var context = new AppDBContext())
            {
                await context.Set<TEntity>().AddRangeAsync(ls);
                await context.SaveChangesAsync();
            }
        }

        public static async Task UpdateAsync(TEntity p)
        {
            using (var context = new AppDBContext())
            {
                context.Entry(p).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public static async Task DeleteAsync(TEntity p)
        {
            using (var context = new AppDBContext())
            {
                context.Remove(p);
                await context.SaveChangesAsync();
            }
        }
    }

}