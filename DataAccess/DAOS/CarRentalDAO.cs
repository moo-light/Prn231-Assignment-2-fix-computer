using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System;
using Domain;
using System.Linq.Expressions;

namespace DataAccess.DAOS;

public class CarRentalDAO : BaseDAO<CarRental>
{

    public static Expression<Func<CarRental, object>>[] Includes = new Expression<Func<CarRental, object>>[]
    {
        x=>x.Car,
        x=>x.Customer,
    };
    public static new async Task DeleteAsync(CarRental p)
    {
        using (var context = new AppDBContext())
        {
            p.Status = false;
            context.Entry(p).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
    public static new async Task AddAsync(CarRental p)
    {
        using (var context = new AppDBContext())
        {
            var existRent = context.CarRentals.Where(x => p.CarId == x.CarId && (x.PickupDate <= p.PickupDate
                                          && p.PickupDate <= x.ReturnDate
                                          || x.PickupDate <= p.ReturnDate
                                          && p.ReturnDate <= x.ReturnDate));

            if (await existRent.AnyAsync()) throw new InvalidDataException("Rent Exist!");
        }
        await BaseDAO<CarRental>.AddAsync(p);
    }
    public static new async Task UpdateAsync(CarRental p)
    {
        using (var context = new AppDBContext())
        {
            var oldRent = await context.CarRentals.FindAsync(new { p.CarId, p.CustomerId, p.PickupDate });

            var existRent = await context.CarRentals
                 .Where(x => x.CarId != oldRent.CarId || x.CustomerId != oldRent.CustomerId || x.PickupDate != oldRent.PickupDate)
                 .AnyAsync(x => p.CarId == x.CarId
                                &&( x.PickupDate <= p.PickupDate
                                && p.PickupDate <= x.ReturnDate
                                || x.PickupDate <= p.ReturnDate
                                && p.ReturnDate <= x.ReturnDate));
            if (existRent) throw new InvalidDataException("Rent Exist!");
        }
        await BaseDAO<CarRental>.UpdateAsync(p);
    }
    public static async Task<CarRental> GetByIdAsync(int carId, int customerId, DateTime date, params Expression<Func<CarRental, object>>[] includes)
    {

        CarRental? entity;
        using (var context = new AppDBContext())
        {
            var query = includes.Aggregate(context.Set<CarRental>().AsQueryable()
                , (c, i) => c.Include(i));
            entity = await query.FirstOrDefaultAsync(x => x.CarId == carId && x.CustomerId == customerId && x.PickupDate.Date == date.Date);
        }
        return entity;
    }
}
