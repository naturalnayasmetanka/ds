using DS.Application.Abstractions.Handlers;
using DS.Contracts.Departments.Get;
using DS.Contracts.Common;

namespace DS.Application.Departments.Handlers.Queries.GetList;

/// <summary>
/// Query для получения постраничного списка подразделений.
/// </summary>
public record GetDepartmentsListQuery(GetDepartmentsListRequest Request) : IQuery;
