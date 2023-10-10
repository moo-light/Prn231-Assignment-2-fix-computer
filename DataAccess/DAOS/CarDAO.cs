using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Linq.Expressions;
using Domain;

namespace DataAccess.DAOS;

public class CarDAO : BaseDAO<Car>
{
    public static Expression<Func<Car, object>>[] Includes = new Expression<Func<Car, object>>[]
    {
        x=>x.CarRentals,
        x=>x.Reviews,
        x=>x.Producer,
    };
    public static new async Task DeleteAsync(Car p)
    {
        using (var context = new AppDBContext())
        {
            Car car = context.Cars.Include(x=>x.CarRentals).AsNoTracking().First(x => x.Id == p.Id);
            if (car.CarRentals.Count > 0)
            {
                p.Status = false;
                context.Entry(p).State = EntityState.Modified;
            }
            else
            {
                context.Remove(p);
            }
            context.Entry(car).State = EntityState.Detached;
            await context.SaveChangesAsync();
        }
    }
}
