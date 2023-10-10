using Application.Interfaces;
using Application.Repositories;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarProducerRepository, CarProducerRepository>();
builder.Services.AddScoped<ICarRentalRepository, CarRentalRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
// Add services to the container.
builder.Services.AddRouting(opt =>
{
    opt.LowercaseUrls = true;

});
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<CarProducer>("CarProducers");
modelBuilder.EntitySet<Car>("Cars");
modelBuilder.EntitySet<Customer>("Customers");
modelBuilder.EntitySet<CarRental>("CarRentals");
modelBuilder.EntityType<Review>();

builder.Services.AddControllers(opt => opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.WriteIndented = false;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}).AddOData(options => options.EnableQueryFeatures(maxTopValue: null)
                                            .AddRouteComponents(routePrefix: "odata", model: modelBuilder.GetEdmModel())); ;


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Initialize data for DB
//SeedDatabase();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseODataBatching();
app.MapControllers();

app.Run();


static IEdmModel GetEDMModel()
{
    var builder = new ODataConventionModelBuilder();
    // ......
    builder.EntitySet<CarProducer>("CarProducers");
    builder.EntitySet<Car>("Cars");
    builder.EntitySet<Customer>("Customers");
    builder.EntitySet<CarRental>("CarRental");
    builder.EntityType<Review>();
    return builder.GetEdmModel();
}

void SeedDatabase()
{
    using var context = new AppDBContext();
    using (var transaction = context.Database.BeginTransaction())
    {
        try
        {
            context.Database.EnsureCreated(); // create database if not exist, add table if not has any
            DBInitializer.InitializeData(context);
            transaction.Commit();
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred when seeding the DB.");
            transaction.Rollback();
        }
    }
}