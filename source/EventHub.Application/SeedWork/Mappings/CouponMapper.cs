using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Coupon;
using EventHub.Domain.Aggregates.CouponAggregate;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class CouponMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config
            .CreateMap<Coupon, CouponDto>()
            .ForMember(dest => dest.Events, options =>
                options.MapFrom(source => source.EventCoupons.Select(x => x.Event)));
        
        config.CreateMap<Coupon, EventCouponDto>();
        
        config.CreateMap<Pagination<Coupon>, Pagination<CouponDto>>()
            .ForMember(dest => dest.Items, options =>
                options.MapFrom(source => source.Items));

        config.CreateMap<UpdateCouponDto, Coupon>();
    }
}
