using AutoMapper;
using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.DTOs.File;
using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.Shared.Constants;
using EventHub.Domain.Shared.Enums.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Extensions;

namespace EventHub.Application.Commands.User.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserDto>
{
    private readonly IFileService _fileService;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;
    private readonly IHangfireService _hangfireService;
    private readonly IEmailService _emailService;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public CreateUserCommandHandler(IMapper mapper, IHangfireService hangfireService, IEmailService emailService,
        IFileService fileService,
        ICacheService cacheService,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager)
    {
        _mapper = mapper;
        _hangfireService = hangfireService;
        _emailService = emailService;
        _fileService = fileService;
        _cacheService = cacheService;
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
            Status = EUserStatus.ACTIVE,
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

        string key = "user";
        await _cacheService.RemoveData(key);

        _hangfireService.Enqueue(() =>
            _emailService
                .SendRegistrationConfirmationEmailAsync(user.Email, user.FullName)
                .Wait());

        return _mapper.Map<UserDto>(user);
    }
}
