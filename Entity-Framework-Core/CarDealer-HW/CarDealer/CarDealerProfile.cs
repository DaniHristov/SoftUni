using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CarDealer.DTO;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<InputSupplierModel, Supplier>();

            CreateMap<InputPartsModel, Part>();

            CreateMap<InputCarsModel, Car>();

            CreateMap<InputCustomerModel, Customer>();

            CreateMap<InputSalesModel, Sale>();
        }
    }
}
