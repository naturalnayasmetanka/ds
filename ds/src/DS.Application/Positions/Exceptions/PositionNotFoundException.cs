namespace DS.Application.Positions.Exceptions;

public class PositionNotFoundException : Exception
{
    public PositionNotFoundException(string message = "Должность не найдена") : base(message)
    {
    }
}
