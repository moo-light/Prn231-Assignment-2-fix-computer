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
using Microsoft.AspNetCore.OData.Formatter;

namespace FUCarRentingSystem_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ODataController
    {
        private readonly ICustomerRepository _carProducerRepository;

        public CustomersController(ICustomerRepository carProducerRepository)
        {
            _carProducerRepository = carProducerRepository;
        }


        // GET: api/Customers
        [HttpGet("")]
        [EnableQuery]
        public async Task<IActionResult> GetCustomers()
        {

            var customers = await _carProducerRepository.GetCustomer();
            return Ok(customers);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<ActionResult<Customer>> GetCustomer([FromRoute]int id)
        {

            var customer = await _carProducerRepository.GetCustomer(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer carProducer)
        {
            if (id != carProducer.Id)
            {
                return BadRequest();
            }

            var curCustomer = await _carProducerRepository.GetCustomer(id);

            if (curCustomer == null)
            {
                return NotFound("Customer not found");
            }

            try
            {
                await _carProducerRepository.UpdateCustomer(carProducer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound("Customer not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("")]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            await _carProducerRepository.AddCustomer(customer);
            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var carProducer = await _carProducerRepository.GetCustomer(id);
            if (carProducer == null)
            {
                return NotFound();
            }

            await _carProducerRepository.DeleteCustomer(carProducer);
            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _carProducerRepository.GetCustomer(id) != null;
        }

    }
}
