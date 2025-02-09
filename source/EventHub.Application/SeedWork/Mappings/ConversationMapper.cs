using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Conversation;
using EventHub.Application.SeedWork.DTOs.Message;
using EventHub.Domain.Aggregates.ConversationAggregate;
using EventHub.Domain.Aggregates.ConversationAggregate.Entities;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class ConversationMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Message, MessageDto>()
            .ReverseMap();

        config.CreateMap<Message, ConversationLastMessageDto>()
            .ReverseMap();

        config.CreateMap<Pagination<Message>, Pagination<MessageDto>>()
            .ForMember(dest => dest.Items, options =>
                options.MapFrom(source => source.Items));

        config.CreateMap<Conversation, ConversationDto>()
            .ReverseMap();

        config.CreateMap<Pagination<Conversation>, Pagination<ConversationDto>>()
            .ForMember(dest => dest.Items, options =>
                options.MapFrom(source => source.Items));
    }
}
