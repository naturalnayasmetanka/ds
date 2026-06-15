using DS.Domain.Exceptions;
using System.Text.Json;

namespace DS.Application.Locations.Exceptions;

public class LocationNotFoundException : Exception
{
    public LocationNotFoundException(IEnumerable<Error> errors)
       : base(JsonSerializer.Serialize(errors))
    {

    }

    public LocationNotFoundException(Error error)
        : base(JsonSerializer.Serialize(error))
    {

    }
}
