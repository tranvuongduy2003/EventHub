﻿using AutoMapper;
using EventHub.Application.Commands.Payment.Checkout;
using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.Aggregates.PaymentAggregate;
using EventHub.Domain.Aggregates.PaymentAggregate.Entities;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class PaymentMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<CheckoutItemDto, CheckoutItemCommand>();

        config.CreateMap<PaymentItem, PaymentItemDto>();

        config.CreateMap<CheckoutItemCommand, PaymentItem>();

        config.CreateMap<CheckoutDto, CheckoutCommand>();

        config.CreateMap<Payment, PaymentDto>()
            .ReverseMap();

    }
}
