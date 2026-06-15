using DS.Application.Abstractions.Handlers;
using DS.Contracts.Departments.GetById;

namespace DS.Application.Departments.Handlers.Queries.GetBy
{
    public record GetDepartmentQuery(GetDepartmentRequest request) : IQuery;
}
