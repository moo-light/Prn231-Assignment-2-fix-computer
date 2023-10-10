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
    public class ReviewsController : ODataController
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewsController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }


        // GET: api/Reviews
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            return Ok(await _reviewRepository.GetReview());
        }

        // GET: api/Reviews/5
        [HttpGet("{carId},{customerId}")]
        [EnableQuery]
        public async Task<ActionResult<Review>> GetReview(int carId, int customerId)
        {

            var review = await _reviewRepository.GetReview(carId,customerId);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{carId},{customerId}")]
        public async Task<IActionResult> PutReview(int carId, int customerId, Review review)
        {
      
            var curReview = await _reviewRepository.GetReview(carId, customerId);

            if (curReview == null)
            {
                return NotFound();
            }
            Console.WriteLine(carId);
            Console.WriteLine(customerId);
            Console.WriteLine(review.ReviewComment);
            try
            {
                await _reviewRepository.UpdateReview(review);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(carId, customerId))
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

        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            await _reviewRepository.AddReview(review);
            return CreatedAtAction("GetReview", new { carId = review.CarId,customerId= review.CustomerId }, review);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{carId},{customerId}")]
        public async Task<IActionResult> DeleteReview(int carId, int customerId)
        {
            var review = await _reviewRepository.GetReview(carId,customerId);
            if (review == null)
            {
                return NotFound();
            }

            await _reviewRepository.DeleteReview(review);
            return NoContent();
        }

        private bool ReviewExists(int carId,int customerId)
        {
            return _reviewRepository.GetReview(carId, customerId) != null;
        }
    }
}
