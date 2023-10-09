using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace FUCarRentingSystem_RazorPage.Pages.User.Transaction
{
    public class CarRentalDTO
    {
        public int? CarId { get; set; } = default;
        [DataType(DataType.Date)]
        [TodayRequired]
        public DateTime? PickupDate { get; set; }
        [DataType(DataType.Date)]
        [TodayRequired]
        public DateTime? ReturnDate { get; set; }
        public decimal RentPrice { get; set; }

        public void UpdateRentPrice(decimal price)
        {
            if (ReturnDate.HasValue && PickupDate.HasValue)
                RentPrice = price * ((ReturnDate - PickupDate).Value.Days + 1);
            else
                RentPrice = 0;
        }

    }
}