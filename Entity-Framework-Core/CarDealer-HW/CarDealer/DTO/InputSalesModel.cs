using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    public class InputSalesModel
    {
        public decimal Discount { get; set; }

        public int CarId { get; set; }

        public int CustomerId { get; set; }
    }
}
