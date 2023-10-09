using Domain.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Customer : BaseEntity
    {
        [DisplayName("Customer Id")]
        [Column(name: "CustomerID")]
        public override int Id { get => base.Id; set => base.Id = value; }
        [Required]
        public string CustomerName { get; set; } = null!;
        [EmailAddress]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; } = null!;
        [Required]
        [DataType(DataType.Date)]
        [DateRange("01/01/1901", "12/31/2024")]
        public DateTime Birthday { get; set; }
        [Required]
        [StringLength(12)]
        [MinLength(9)]
        public string IdentityCard { get; set; } = null!;
        [Required]
        [StringLength(12)]
        [MinLength(9)] 
        public string LicenceNumber { get; set; } = null!;
        [Required]
        [DataType(DataType.Date)]
        [DateRange("01/01/1901", "12/31/2024")]
        public DateTime LicenceDate { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [DataType(DataType.Password)]
        public string? Password { get; set; } = default;
        public virtual List<Review> Review { get; set; } = new List<Review>();
        public virtual List<CarRental> CarRentals { get; set; } = new List<CarRental>();
        
        //
        [NotMapped]
        public string Role { get; set; } = "User";
    }
}