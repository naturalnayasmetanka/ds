using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Application.Abstractions.Handlers;
using DS.Contracts.Departments.GetById;
using DS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DS.Application.Departments.Handlers.Queries.GetBy;

public class GetDepartmentHandler : IQueryHandler<GetDepartmentResponse?, GetDepartmentQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetDepartmentHandler> _logger;
    public GetDepartmentHandler(
        IReadDbContext readDbContext,
        ILogger<GetDepartmentHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<Result<GetDepartmentResponse?, Errors>> Handle(
        GetDepartmentQuery query,
        CancellationToken cancellationToken = default)
    {
        var department = await _readDbContext.DepartmentsRead.FirstOrDefaultAsync(x => x.Id == query.request.Id && x.IsActive);

        if (department is null)
            return Result.Failure<GetDepartmentResponse?, Errors>(Error.NotFound("department.not.found", "Подразделение не найдено", query.request.Id));

        var departmentResponse = new GetDepartmentResponse()
        {
            Id = department.Id,
            Name = department.Name.Value,
            Identifier = department.Identifier.Value,
            Path = department.Path.Value,
            ParentId = department.ParentId.Value,
            Depth = department.Depth,
            ChildrenCount = department.ChildrenCount,
            IsActive = department.IsActive,
            CreatedAt = department.CreatedAt,
            UpdatedAt = department.UpdatedAt,
        };

        return Result.Success<GetDepartmentResponse?, Errors>(departmentResponse);
    }
}
