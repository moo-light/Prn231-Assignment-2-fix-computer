// See https://aka.ms/new-console-template for more information
using Domain;
using Domain.Entities;
using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Options;

var queryBuilder = new ODataQueryBuilder("https://localhost:5000/api", new ODataQueryBuilderOptions
{
    UseCorrectDateTimeFormat = false,
    
});
var today = DateTime.Today;
var today2 = DateTime.Today.AddDays(5);
var query = queryBuilder.For<CarRental>("Carrentals").ByList()
    .Filter((s, f, o)=> s.Status ==true &&f.Date(s.PickupDate) <= today
                        && f.Date(s.ReturnDate) >= today
                        || f.Date(s.PickupDate) <= today2
                        && f.Date(s.ReturnDate) >= today2)
    .ToUri();
Console.WriteLine(query);
