namespace CarDealer
{
    using AutoMapper;
    using Models;
    using DataTransferObjects;
    using CarDealer.DataTransferObjects.InputDTOs;
    using CarDealer.DataTransferObjects.OutputDTOs;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Input
            CreateMap<SupplierInputModel, Supplier>();

            CreateMap<PartInputModel, Part>();

            CreateMap<CustomerInputModel, Customer>();

            CreateMap<SaleInputModel, Sale>();

            //Output
            CreateMap<Car, CarOutputModel>();

            CreateMap<Car, CarMakeBMWOutputModel>();

            CreateMap<Supplier, SupplierOutputModel>()
              .ForMember(x => x.PartsCount, opt => opt.MapFrom(s => s.Parts.Count));
           
        }
    }
}
