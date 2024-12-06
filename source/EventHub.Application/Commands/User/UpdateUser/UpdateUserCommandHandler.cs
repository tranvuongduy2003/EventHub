using EventHub.Application.Abstractions;
using EventHub.Application.DTOs.File;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.Shared.Constants;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Commands.User.UpdateUser;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    private readonly IFileService _fileService;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public UpdateUserCommandHandler(IFileService fileService,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager)
    {
        _fileService = fileService;
        _userManager = userManager;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            throw new NotFoundException("User does not exist!");
        }

        user.Email = request.Email;
        user.PhoneNumber = request.PhoneNumber;
        user.Dob = request.Dob;
        user.FullName = request.FullName;
        user.UserName = request.UserName;
        user.Gender = request.Gender;
        user.Bio = request.Bio;

        if (request.Avatar != null)
        {
            if (!string.IsNullOrEmpty(user.AvatarFileName))
            {
                await _fileService.DeleteAsync(user.AvatarFileName, FileContainer.USERS);
            }
            BlobResponseDto avatarImage = await _fileService.UploadAsync(request.Avatar, FileContainer.USERS);
            user.AvatarUrl = avatarImage.Blob.Uri;
            user.AvatarFileName = avatarImage.Blob.Name;
        }

        IdentityResult result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new BadRequestException(result);
        }
    }
}
