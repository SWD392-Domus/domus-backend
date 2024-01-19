namespace Domus.Service.Models.Email;

public class ServeralEmail
{
    public List<string> To { get; set; }
    public string Subject { get; set; } = String.Empty;
    public string EmailBody { get; set; } = String.Empty;
}