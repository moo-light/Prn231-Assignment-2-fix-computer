using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class CarProducer : BaseEntity
    {
        [DisplayName("Producer ID")]
        [Column(name: "ProducerID")]
        public override int Id { get => base.Id; set => base.Id = value; }
        [Required]
        public string ProducerName { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        public string Country { get; set; } = null!;

        public virtual List<Car> Cars { get; set; } = new List<Car>();
    }
}