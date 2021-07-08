using AutoMapper;
using Stripe;
using Pay.TopUps.Domain;

namespace Pay.TopUps
{
    public class ClassMapping : Profile
    {
        public ClassMapping()
        {
            CreateMap<CardDetails, CardCreateNestedOptions>();
            CreateMap<BillingDetails, CardCreateNestedOptions>();
        }
    }

    public static class AutoMapperExtensions
    {
        public static TDestination Map<TSource, TDestination>(
            this TDestination destination, TSource source, IMapper mapper)
        {
            return mapper.Map(source, destination);
        }
    }
}