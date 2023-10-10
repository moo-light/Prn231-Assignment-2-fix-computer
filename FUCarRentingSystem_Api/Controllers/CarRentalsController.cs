using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace FUCarRentingSystem_Api.Controllers
{
    [Route("api/[controller]")]
    public class CarRentalsController : ODataController
    {
        private readonly ICarRentalRepository _carRentalsRepository;
        private readonly ICarRepository _carsRepository;

        public CarRentalsController(ICarRentalRepository carRentalsRepository, ICarRepository carsRepository)
        {
            _carRentalsRepository = carRentalsRepository;
            _carsRepository = carsRepository;
        }



        // GET: api/CarRentals
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<CarRental>>> GetCarRentals()
        {
            var carRentals = await _carRentalsRepository.GetCarRental();
            return Ok(carRentals);
        }

        [HttpGet("{carId},{customerId},{date}")]
        [EnableQuery]
        public async Task<ActionResult<CarRental>> GetCarRental(int carId, int customerId, string date)
        {
            var carRental = await _carRentalsRepository.GetCarRental(carId, customerId, DateTime.Parse(date));

            if (carRental == null)
            {
                return NotFound();
            }

            return carRental;
        }

        // PUT: api/CarRentals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [EnableQuery]
        [HttpPut("{carId},{customerId},{date}")]
        public async Task<IActionResult> PutCarRental(int carId, int customerId, string date, [FromBody] CarRental carRental)
        {
            var curCarRental = await _carRentalsRepository.GetCarRental(carId, customerId, DateTime.Parse(date));
            if (curCarRental == null)
            {
                return NotFound("CarRental Not exist");
            }
            var actionResult = await Validate(carRental);
            if (actionResult != null) return actionResult;
            try
            {
                await _carRentalsRepository.UpdateCarRental(carId, customerId, DateTime.Parse(date),carRental);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarRentalExists(carId, customerId, DateTime.Parse(date)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (InvalidDataException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(carRental);
        }

        // POST: api/CarRentals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [EnableQuery]
        [HttpPost()]

        public async Task<IActionResult> Post([FromBody] CarRental carRental)
        {
            var actionResult = await Validate(carRental);
            if (actionResult != null) return actionResult;
            try
            {
                await _carRentalsRepository.AddCarRental(carRental);
            }
            catch (DbUpdateException)
            {
                if (CarRentalExists(carRental.CarId, carRental.CustomerId, carRental.PickupDate))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            catch (InvalidDataException e)
            {
                return new BadRequestObjectResult(e.Message);
            }

            return new ObjectResult(carRental) { StatusCode = 201 };
            //return Created(carRental);
        }


        // DELETE: api/CarRentals/5
        [EnableQuery]
        [HttpDelete("{carId},{customerId},{date}")]
        public async Task<IActionResult> DeleteCarRental(int carId, int customerId, string date)
        {
            var carRental = await _carRentalsRepository.GetCarRental(carId, customerId, DateTime.Parse(date));
            if (carRental == null)
            {
                return NotFound();
            }

            await _carRentalsRepository.DeleteCarRental(carRental);

            return NoContent();
        }

        private bool CarRentalExists(int carId, int customerId, DateTime date)
        {
            return (_carRentalsRepository.GetCarRental(carId, customerId, date) != null);
        }
        private async Task<IActionResult?> Validate(CarRental carRental)
        {
            if (carRental.CarId == 0) return new NotFoundObjectResult("Car is not Selected!");
            if (carRental.CustomerId == 0) return new NotFoundObjectResult("Customer not found!");
            var car = await _carsRepository.GetCar(carRental.CarId);
            if (car == null) return new NotFoundObjectResult("Car Not Found");
            if (carRental.PickupDate > carRental.ReturnDate) return new BadRequestObjectResult("Return date cannot be larger than Pickup date");
            //  Success validation
            carRental.UpdateRentPrice(car.RentPrice);
            return null;
        }
       
    }
}
