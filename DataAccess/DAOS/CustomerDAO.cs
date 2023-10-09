using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Linq.Expressions;
using Domain;

namespace DataAccess.DAOS;

public class CustomerDAO : BaseDAO<Customer>
{
    public static Expression<Func<Customer, object>>[] Includes = new Expression<Func<Customer, object>>[]
    {
        x=>x.CarRentals,
        x=>x.Review
    };

    public static new async Task UpdateAsync(Customer cus)
    {
        using (var context = new AppDBContext())
        {
            if (cus.Password == null)
            {
                Customer? customer = await context.Customers.FindAsync(cus.Id);
                cus.Password = customer.Password;
                context.Entry(customer).State = EntityState.Detached;
            }
            context.Entry(cus).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
