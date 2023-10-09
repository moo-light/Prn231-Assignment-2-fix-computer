using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Entities;
using Application.Interfaces;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;

namespace FUCarRentingSystem_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ODataController
    {
        private readonly ICarRepository _carRepository;

        public CarsController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }


        // GET: api/Cars
        [HttpGet("")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            return Ok(await _carRepository.GetCar());
        }

        // GET: api/Cars/5
        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<ActionResult<Car>> GetCar(int id)
        {

            var car = await _carRepository.GetCar(id);

            if (car == null)
            {
                return NotFound();
            }

            return car;
        }

        // PUT: api/Cars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(int id, Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

            var curCar = await _carRepository.GetCar(id);

            if (curCar == null)
            {
                return NotFound();
            }

            try
            {
                await _carRepository.UpdateCar(car);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(Car car)
        {
            await _carRepository.AddCar(car);
            return CreatedAtAction("GetCar", new { id = car.Id }, car);
        }

        // DELETE: api/Cars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _carRepository.GetCar(id);
            if (car == null)
            {
                return NotFound();
            }

            await _carRepository.DeleteCar(car);
            return NoContent();
        }

        private bool CarExists(int id)
        {
            return _carRepository.GetCar(id) != null;
        }
    }
}
