using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Application.Locations.Validations.CustomRules;

public static class TimeZoneRules
{
    public static bool BeValidTimeZoneId(string? timeZone)
    {
        if (string.IsNullOrWhiteSpace(timeZone))
            return false;

        try
        {
            TimeZoneInfo.FindSystemTimeZoneById(timeZone.Trim());
            return true;
        }
        catch
        {
            return false;
        }
    }
}
