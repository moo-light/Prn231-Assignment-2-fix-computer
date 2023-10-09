using Bogus;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

internal class DBInitializer
{
    internal static void InitializeData(AppDBContext context)
    {
        List<CarProducer> producers;
        List<Car> cars;
        List<Customer> customers;
        List<CarRental> carRentals;
        List<Review> reviews;
        DbSet<CarProducer> producersDBSet = context.Set<CarProducer>();
        DbSet<Customer> customersDBSet = context.Set<Customer>();
        DbSet<Car> carsDBSet = context.Set<Car>();
        DbSet<CarRental> carRentalsDBSet = context.Set<CarRental>();
        DbSet<Review> reviewsDBSet = context.Set<Review>();

        producers = producersDBSet.Any() ? producersDBSet.ToList() : GenerateProducers(producersDBSet);
        context.SaveChanges();
        customers = customersDBSet.Any() ? customersDBSet.ToList() : GenerateCustomers(customersDBSet);
        context.SaveChanges();
        cars = carsDBSet.Any() ? carsDBSet.ToList() : GenerateCarRentals(carsDBSet, producers);
        context.SaveChanges();
        carRentals = carRentalsDBSet.Any() ? carRentalsDBSet.ToList() : GenerateCarRentals(carRentalsDBSet, cars, customers);
        context.SaveChanges();
        reviews = reviewsDBSet.Any() ? reviewsDBSet.ToList() : GenerateReviews(reviewsDBSet, cars, customers);
        context.SaveChanges();
    }

    private static List<Review> GenerateReviews(DbSet<Review> reviews, List<Car> cars, List<Customer> customers)
    {
        var reviewFaker = new Faker<Review>()
           .RuleFor(r => r.ReviewStar, f => f.Random.Number(1, 10))  // Generate a random review star (between 1 and 5)
           .RuleFor(r => r.ReviewComment, f => f.Lorem.Sentence());
        var count = 10;
        var fakeReviews = new List<Review>();
        Faker faker = new Faker();
        while (fakeReviews.Count < count)
        {
            var customer = customers.ElementAt(faker.Random.Number(0, customers.Count - 1));
            var car = cars.ElementAt(faker.Random.Number(0, cars.Count - 1));

            // Check if this combination already exists in the generated car rentals
            var isDuplicate = fakeReviews.Any(cr => cr.Customer.Id == customer.Id && cr.Car.Id == car.Id);

            if (!isDuplicate)
            {
                // Create a new car rental with this unique combination
                var carRental = reviewFaker
                       .RuleFor(cr => cr.Customer, customer)
                       .RuleFor(cr => cr.Car, car)
                       .Generate();

                fakeReviews.Add(carRental);
            }
        }
        reviews.AddRange(fakeReviews);
        return fakeReviews;
    }

    private static List<CarRental> GenerateCarRentals(DbSet<CarRental> carRentals, List<Car> cars, List<Customer> customers)
    {
        var carRentalFaker = new Faker<CarRental>()
           .RuleFor(cr => cr.PickupDate, f => f.Date.Between(DateTime.Parse("01/01/1950"), DateTime.Parse("12/31/2023")))
           .RuleFor(cr => cr.ReturnDate, (f, cr) => f.Date.Between(cr.PickupDate, cr.PickupDate.AddDays(30)))  // Set ReturnDate within 30 days from PickupDate
           .RuleFor(cr => cr.RentPrice, f => f.Random.Decimal(50, 500))
           .RuleFor(cr => cr.Status, f => f.Random.Bool());
        var count = 10;
        var fakeCarRentals = new List<CarRental>();
        Faker faker = new Faker();
        while (fakeCarRentals.Count < count)
        {
            var customer = customers.ElementAt(faker.Random.Number(0, customers.Count - 1));
            var car = cars.ElementAt(faker.Random.Number(0, cars.Count - 1));

            // Check if this combination already exists in the generated car rentals
            var isDuplicate = fakeCarRentals.Any(cr => cr.Customer.Id == customer.Id && cr.Car.Id == car.Id);

            if (!isDuplicate)
            {
                // Create a new car rental with this unique combination
                var carRental = carRentalFaker
                    .RuleFor(cr => cr.Customer, customer)
                    .RuleFor(cr => cr.Car, car)
                    .Generate();
                fakeCarRentals.Add(carRental);
            }
        }
        carRentals.AddRange(fakeCarRentals);
        return fakeCarRentals;
    }

    private static List<Customer> GenerateCustomers(DbSet<Customer> customers)
    {
        var customerFaker = new Faker<Customer>()
            .RuleFor(c => c.CustomerName, f => f.Person.FullName)
            .RuleFor(c => c.Email, (f) => f.Internet.Email())
            .RuleFor(c => c.Mobile, f => f.Phone.PhoneNumber())
            .RuleFor(c => c.Birthday, f => f.Date.Past())
            .RuleFor(c => c.IdentityCard, f => f.Random.String2(9, 12, "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"))
            .RuleFor(c => c.LicenceNumber, f => f.Random.AlphaNumeric(8).ToUpper())
            .RuleFor(c => c.Password, f => "12345")
            .RuleFor(c => c.LicenceDate, f => f.Date.Past());

        // Generate a fake customer
        var fakeCustomers = new List<Customer>();
        while (fakeCustomers.Count < 10)
        {
            var customer = customerFaker.Generate();
            bool isDuplicate = fakeCustomers.Any(x => x.Email == customer.Email
                                                   || x.Mobile == customer.Mobile
                                                   || x.IdentityCard == customer.IdentityCard
                                                   || x.LicenceNumber == customer.LicenceNumber);
            if (!isDuplicate)
            {
                fakeCustomers.Add(customer);
            }
        }

        customers.AddRange(fakeCustomers);
        return fakeCustomers;
    }

    private static List<Car> GenerateCarRentals(DbSet<Car> cars, List<CarProducer> producers)
    {
        var carFaker = new Faker<Car>()
          .RuleFor(c => c.CarName, f => f.Vehicle.Model())
          .RuleFor(c => c.CarModelYear, f => f.Random.Number(2000, DateTime.Now.Year))
          .RuleFor(c => c.Color, f => f.Commerce.Color())
          .RuleFor(c => c.Capacity, f => f.Random.Number(2, 8))
          .RuleFor(c => c.Description, f => f.Vehicle.Type())
          .RuleFor(c => c.ImportDate, f => f.Date.Past())
          .RuleFor(c => c.RentPrice, f => f.Random.Decimal(20, 200))
          .RuleFor(c => c.Status, f => f.Random.Bool())
          .RuleFor(c => c.Producer, f => producers.ElementAt(f.Random.Number(0, producers.Count - 1)));
        var carsFaker = carFaker.Generate(10);
        cars.AddRange(carsFaker);
        return carsFaker;
    }

    private static List<CarProducer> GenerateProducers(DbSet<CarProducer> producers)
    {
        var producerFaker = new Faker<CarProducer>()
          .RuleFor(p => p.ProducerName, f => f.Company.CompanyName())
          .RuleFor(p => p.Address, f => f.Address.StreetAddress())
          .RuleFor(p => p.Country, f => f.Address.Country());

        // Generate 10 fake producers
        var fakeProducers = producerFaker.Generate(10);

        producers.AddRange(fakeProducers);
        return fakeProducers;
    }
}