using Application.Interfaces;
using Application.Utils;
using DTOS.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace FUCarRentingSystem_Api.Controllers.Auth
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        public readonly ICustomerRepository _customerRepository;
        public AuthController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        [HttpPost("sign-in")]

        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var customer = await _customerRepository.SignIn(loginDto);
            if (customer == null)
            {
                return NotFound("Sign In Failed!");
            }
            return Ok(customer);
        }
    }
}
