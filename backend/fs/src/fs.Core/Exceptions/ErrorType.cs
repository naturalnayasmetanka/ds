using System;
using System.Collections.Generic;
using System.Text;

namespace FS.Core.Exceptions;

public enum ErrorType
{
    Validation,
    NotFound,
    Conflict,
    Failure
}
