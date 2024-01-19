namespace Domus.Service.Models.Email;

public class Email
{
    public string To { get; set; } =  String.Empty;
    public string Subject { get; set; } = String.Empty;
    public string EmailBody { get; set; } = String.Empty;
}