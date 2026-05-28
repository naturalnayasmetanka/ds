using DS.Domain.Exceptions;
using System.Text.Json;

namespace DS.Application.DepartmentsLocations.Exceptions;

public class DepartmentLocationBindAlreadyExistsException : Exception
{
    public DepartmentLocationBindAlreadyExistsException(IEnumerable<Error> errors)
      : base(JsonSerializer.Serialize(errors))
    {

    }

    public DepartmentLocationBindAlreadyExistsException(Error error)
        : base(JsonSerializer.Serialize(error))
    {

    }
}
