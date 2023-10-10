using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Review : BaseEntity
    {
        [NotMapped]
        [JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; }
        [Key, Column(Order = 1)]
        public int CustomerId { get; set; }
        [Key, Column(Order = 2)]
        public int CarId { get; set; }
        [Required]
        [Range(1, 10,ErrorMessage ="You haven't select rate")]
        public int ReviewStar { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string ReviewComment { get; set; } = null!;
        //=======================================//
     
        public Customer Customer { get; set; } = default!;
        public Car Car { get; set; } = default!;
    }
}