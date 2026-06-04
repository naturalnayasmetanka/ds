using DS.Application.Abstractions;
using DS.Contracts.Locations.Create;
using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Application.Locations.Handlers.Create;

public record CreateLocationCommand(CreateLocationRequest request) : ICommand;