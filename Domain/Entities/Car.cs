using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Car : BaseEntity
    {
        [DisplayName("Car ID")]
        [Column(name: "CarID")]
        public override int Id { get => base.Id; set => base.Id = value; }
        [Required]
        public string CarName { get; set; } = null!;
        public int CarModelYear { get; set; }
        [Required]
        public string Color { get; set; } = null!;

        public int Capacity { get; set; }
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public DateTime ImportDate { get; set; } = DateTime.Now;
        [DisplayFormat(ApplyFormatInEditMode = false,DataFormatString = "{0:C2}")]
        public decimal RentPrice { get; set; }
        public bool? Status { get; set; } = true;
        public int ProducerID { get; set; }

        public virtual CarProducer Producer { get; set; } = default!;
        public virtual List<Review> Reviews { get; set; } = new List<Review>();
        public virtual List<CarRental> CarRentals { get; set; } = new List<CarRental>();
    }
}
