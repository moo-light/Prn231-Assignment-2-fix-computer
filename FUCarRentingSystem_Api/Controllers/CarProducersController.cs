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
    public class CarProducersController : ODataController
    {
        private readonly ICarProducerRepository _carProducerRepository;

        public CarProducersController(ICarProducerRepository carProducerRepository)
        {
            _carProducerRepository = carProducerRepository;
        }


        // GET: api/CarProducers
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<CarProducer>>> GetCarProducers()
        {
            return Ok(await _carProducerRepository.GetCarProducer());
        }

        // GET: api/CarProducers/5
        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<ActionResult<CarProducer>> GetCarProducer(int id)
        {

            var carProducer = await _carProducerRepository.GetCarProducer(id);

            if (carProducer == null)
            {
                return NotFound();
            }

            return carProducer;
        }

        // PUT: api/CarProducers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarProducer(int id, CarProducer carProducer)
        {
            if (id != carProducer.Id)
            {
                return BadRequest();
            }

            var curCarProducer = await _carProducerRepository.GetCarProducer(id);

            if (curCarProducer == null)
            {
                return NotFound();
            }

            try
            {
                await _carProducerRepository.UpdateCarProducer(carProducer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarProducerExists(id))
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

        // POST: api/CarProducers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CarProducer>> PostCarProducer(CarProducer carProducer)
        {
            await _carProducerRepository.AddCarProducer(carProducer);
            return CreatedAtAction("GetCarProducer", new { id = carProducer.Id }, carProducer);
        }

        // DELETE: api/CarProducers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarProducer(int id)
        {
            var carProducer = await _carProducerRepository.GetCarProducer(id);
            if (carProducer == null)
            {
                return NotFound();
            }

            await _carProducerRepository.DeleteCarProducer(carProducer);
            return NoContent();
        }

        private bool CarProducerExists(int id)
        {
            return _carProducerRepository.GetCarProducer(id) != null;
        }
    }
}
