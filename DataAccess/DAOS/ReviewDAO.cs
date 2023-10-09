using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System;
using Domain;
using System.Linq.Expressions;

namespace DataAccess.DAOS;

public class ReviewDAO : BaseDAO<Review>
{
    public static Expression<Func<Review, object>>[] Includes = new Expression<Func<Review, object>>[]
  {
        x=>x.Car,
        x=>x.Customer
  };

    public static async Task<Review?> GetByIdAsync(int carId, int customerId, params Expression<Func<Review, object>>[] includes)
    {
        Review? entity;
        using (var context = new AppDBContext())
        {
            var query = includes.Aggregate(context.Set<Review>().AsQueryable(),
                                            (current, include) => current.Include(include));
            entity = await query.FirstOrDefaultAsync(x => x.CarId == carId && x.CustomerId == customerId);
        }
        return entity;
    }
}
