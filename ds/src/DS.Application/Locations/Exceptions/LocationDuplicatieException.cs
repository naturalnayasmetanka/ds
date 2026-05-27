using DS.Domain.Exceptions;
using System.Text.Json;

namespace DS.Application.Locations.Exceptions;

public class LocationDuplicatieException : Exception
{
    public LocationDuplicatieException(IEnumerable<Error> errors)
      : base(JsonSerializer.Serialize(errors))
    {

    }

    public LocationDuplicatieException(Error error)
        : base(JsonSerializer.Serialize(error))
    {

    }
}
