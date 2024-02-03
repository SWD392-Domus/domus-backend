using static System.String;

namespace Domus.Service.Models.Email;

public class ServeralEmail
{
    public List<string>? To { get; set; }
    public string Subject { get; set; } = Empty;
    public string EmailBody { get; set; } = Empty;
}