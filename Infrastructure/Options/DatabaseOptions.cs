namespace Infrastructure.Options;
public class DatabaseOptions
{
    public const string Key = "DatabaseSettings";

    public string Host { get; set; }
    public string Port { get; set; }
    public string Password { get; set; }
    public string Userid { get; set; }
    public string UsersDataBase { get; set; }

}
