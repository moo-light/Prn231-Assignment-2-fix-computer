using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.DTOS
{
    public class CarRentingDTO
    {
        public int CarID { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime PickupDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }

    }
}
