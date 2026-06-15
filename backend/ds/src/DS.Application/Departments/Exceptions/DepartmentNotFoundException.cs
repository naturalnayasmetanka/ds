using DS.Domain.Exceptions;
using System.Text.Json;

namespace DS.Application.Departments.Exceptions;

public class DepartmentNotFoundException : Exception
{
    public DepartmentNotFoundException(IEnumerable<Error> errors)
        : base(JsonSerializer.Serialize(errors))
    {

    }

    public DepartmentNotFoundException(Error error)
        : base(JsonSerializer.Serialize(error))
    {

    }
}
