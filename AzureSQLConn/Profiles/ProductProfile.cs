using AutoMapper;
using AzureSQLConn.Entities;
using AzureSQLConn.Models;

namespace AzureSQLConn.Profiles
{
    public class ProductProfile: Profile
    {
        public ProductProfile() {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}
