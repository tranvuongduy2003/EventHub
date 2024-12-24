using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.EventAggregate.Entities;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class EmailContentMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<EmailContent, EmailContentDto>()
            .ForMember(dest => dest.AttachmentUrls, options =>
                options.MapFrom(source => source.EmailAttachments.Select(attachment => attachment.AttachmentUrl)));
    }
}
