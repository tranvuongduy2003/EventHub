using AutoMapper;
using EventHub.Application.Commands.Payment.Checkout;
using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.Aggregates.PaymentAggregate;
using EventHub.Domain.Aggregates.PaymentAggregate.Entities;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class PaymentMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<CheckoutItemDto, CheckoutItemCommand>();

        config.CreateMap<PaymentItem, PaymentItemDto>();

        config.CreateMap<CheckoutItemCommand, PaymentItem>()
            .ForMember(dest => dest.UnitPrice, options => options.MapFrom(dest => dest.Price))
            .ForMember(dest => dest.TotalPrice, options => options.MapFrom(dest => dest.Price * dest.Quantity));

        config.CreateMap<CheckoutDto, CheckoutCommand>();

        config.CreateMap<Payment, ValidateSessionResponseDto>()
             .ForMember(dest => dest.OrganizerId, options => options.MapFrom(dest => dest.Event.AuthorId));

        config.CreateMap<Payment, PaymentDto>()
            .ReverseMap();

        config.CreateMap<Pagination<Payment>, Pagination<PaymentDto>>()
            .ForMember(dest => dest.Items, options =>
                options.MapFrom(source => source.Items));
    }
}
