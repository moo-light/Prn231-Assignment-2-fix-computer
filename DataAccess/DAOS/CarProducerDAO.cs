using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Linq.Expressions;

namespace DataAccess.DAOS;

public class CarProducerDAO : BaseDAO<CarProducer>
{

    public static Expression<Func<CarProducer, object>>[] Includes = new Expression<Func<CarProducer, object>>[]
    {
        x=>x.Cars,
    };
}
