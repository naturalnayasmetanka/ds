using DS.Domain.Exceptions;
using System.Text.Json;

namespace DS.Application.DepartmentsLocations.Exceptions
{
    public class DepartmentLocationNotFoundException : Exception
    {
        public DepartmentLocationNotFoundException(IEnumerable<Error> errors)
     : base(JsonSerializer.Serialize(errors))
        {

        }

        public DepartmentLocationNotFoundException(Error error)
            : base(JsonSerializer.Serialize(error))
        {

        }
    }
}
