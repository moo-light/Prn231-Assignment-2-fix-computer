using DTOS.DTOS;
using DTOS.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class CarRental : BaseEntity
    {
        public CarRental()
        {
        }
        public CarRental(CarRentalDTO carRental, int customerId)
        {
            this.CarId = carRental.CarId.Value;
            this.PickupDate = carRental.PickupDate.Value;
            this.ReturnDate = carRental.ReturnDate.Value;
            this.RentPrice = carRental.RentPrice;
            this.CustomerId = customerId;
        }

        [NotMapped, JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; }
        [Key, Column(Order = 1)]
        public int CustomerId { get; set; }
        [Key, Column(Order = 2)]
        public int CarId { get; set; }
        [Key, Column(Order = 3)]
        [TodayRequired]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}", ApplyFormatInEditMode =false)]
        public DateTime PickupDate { get; set; }
        [Required]
        [TodayRequired]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime ReturnDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false,DataFormatString = "{0:C2}")]
        public decimal RentPrice { get; set; }
        public bool? Status { get; set; } = true;
        //=======================================//

        public Customer Customer { get; set; } = default!;
        public Car Car { get; set; } = default!;

        public void UpdateRentPrice(decimal price)
        {
            RentPrice = price * ((ReturnDate - PickupDate).Days + 1);
        }
    }
}