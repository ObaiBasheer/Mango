using AutoMapper;
using Mango.Services.CouponAPI.Dtos;
using Mango.Services.CouponAPI.Models;

namespace Mango.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Coupon, CouponDto>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
