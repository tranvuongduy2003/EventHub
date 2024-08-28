using System.Data;
using Dapper;
using EventHub.Domain.Abstractions;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Permission;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Permission.GetFullPermissions;

public class GetFullPermissionsQueryHandler : IQueryHandler<GetFullPermissionsQuery, List<FullPermissionDto>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger<GetFullPermissionsQueryHandler> _logger;

    public GetFullPermissionsQueryHandler(ISqlConnectionFactory sqlConnectionFactory,
        ILogger<GetFullPermissionsQueryHandler> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _logger = logger;
    }

    public async Task<List<FullPermissionDto>> Handle(GetFullPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetFullPermissionsQueryHandler");

        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        var sql = @"SELECT f.Id,
	                       f.Name,
	                       f.ParentId,
	                       sum(case when sa.Id = 'CREATE' then 1 else 0 end) as HasCreate,
	                       sum(case when sa.Id = 'UPDATE' then 1 else 0 end) as HasUpdate,
	                       sum(case when sa.Id = 'DELETE' then 1 else 0 end) as HasDelete,
	                       sum(case when sa.Id = 'VIEW' then 1 else 0 end) as HasView,
	                       sum(case when sa.Id = 'APPROVE' then 1 else 0 end) as HasApprove
                        from Functions f join CommandInFunctions cif on f.Id = cif.FunctionId
		                    left join Commands sa on cif.CommandId = sa.Id
                        GROUP BY f.Id,f.Name, f.ParentId
                        order BY f.ParentId";
        
        var permissions = await connection.QueryAsync<FullPermissionDto>(sql, commandType: CommandType.Text);

        _logger.LogInformation("END: GetFullPermissionsQueryHandler");

        return permissions.ToList();
    }
}