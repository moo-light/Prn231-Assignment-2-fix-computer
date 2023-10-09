using System.ComponentModel.DataAnnotations;

namespace DTOS.DTOS
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; } = null!;

        public override bool Equals(object? obj)
        {
            return obj is LoginDTO loginDTO && Password == loginDTO.Password && Email== loginDTO.Email;
        }
    }
}