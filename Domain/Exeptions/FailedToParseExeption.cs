namespace Domain.Exeptions;

public class FailedToParseExeption : Exception
{
    public FailedToParseExeption(string message) : base(message)
    {

    }
}
