using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.DataProcessor.ExportDto
{
    public class JsonCustomerDto
    {
        public string MovieName { get; set; }

        public double Rating { get; set; }

        public decimal TotalIncome { get; set; }

        public CustomerDto[] Customers { get; set; }
    }
}
