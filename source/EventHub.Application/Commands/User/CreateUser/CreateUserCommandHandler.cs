using AutoMapper;
using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.DTOs.File;
using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.Shared.Constants;
using EventHub.Domain.Shared.Enums.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Extensions;

namespace EventHub.Application.Commands.User.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserDto>
{
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public CreateUserCommandHandler(IMapper mapper, IFileService fileService,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager)
    {
        _mapper = mapper;
        _fileService = fileService;
        _userManager = userManager;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new Domain.Aggregates.UserAggregate.User()
        {
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Dob = request.Dob,
            FullName = request.FullName,
            UserName = request.UserName,
            Gender = request.Gender,
            Bio = request.Bio,
            Status = EUserStatus.ACTIVE
        };

        if (request.Avatar != null)
        {
            BlobResponseDto avatarImage = await _fileService.UploadAsync(request.Avatar, FileContainer.USERS);
            user.AvatarUrl = avatarImage.Blob.Uri;
            user.AvatarFileName = avatarImage.Blob.Name;
        }

        IdentityResult result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new BadRequestException(result);
        }

        await _userManager.AddToRolesAsync(user,
            new List<string> { EUserRole.CUSTOMER.GetDisplayName(), EUserRole.ORGANIZER.GetDisplayName() });

        return _mapper.Map<UserDto>(user);
    }
}
