using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Function;
using EventHub.Domain.SeedWork.Query;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Function.GetFunctionsByRoleId;

public class GetFunctionsByRoleIdQueryHandler : IQueryHandler<GetFunctionsByRoleIdQuery, List<FunctionDto>>
{
    private readonly IMapper _mapper;
    private readonly RoleManager<Domain.Aggregates.UserAggregate.Entities.Role> _roleManager;

    public GetFunctionsByRoleIdQueryHandler(RoleManager<Domain.Aggregates.UserAggregate.Entities.Role> roleManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<List<FunctionDto>> Handle(GetFunctionsByRoleIdQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Aggregates.UserAggregate.Entities.Role role = await _roleManager.Roles
            .Where(x => x.Id == request.RoleId)
            .Include(x => x.Permissions)
                .ThenInclude(x => x.Function)
            .FirstOrDefaultAsync(cancellationToken);

        var functions = role!.Permissions.Select(x => x.Function).ToList();

        List<FunctionDto> functionDtos = _mapper.Map<List<FunctionDto>>(functions.Where(x => x.ParentId == null));

        foreach (FunctionDto function in functionDtos)
        {
            var children = functions
                .Where(x => x.ParentId == function.Id)
                .ToList();

            function.Children = _mapper.Map<List<FunctionDto>>(children);
        }

        return functionDtos;
    }
}
