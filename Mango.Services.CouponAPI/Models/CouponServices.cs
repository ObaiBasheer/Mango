using AutoMapper;
using Mango.Services.CouponAPI.Data;

namespace Mango.Services.CouponAPI.Models
{
    public class CouponServices(
        ILogger<CouponServices> logger,
        AppDbContext context,
        IMapper mapper
        )
    {
        public ILogger<CouponServices> Logger { get;  } = logger;
        public AppDbContext Context { get;  } = context;
        public IMapper mapper { get; } = mapper;    
    }
}
