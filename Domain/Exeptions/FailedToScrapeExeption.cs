namespace Domain.Exeptions;

public class FailedToScrapeExeption : Exception
{
    public FailedToScrapeExeption(string message) : base(message)
    {

    }
}
