namespace Domus.Service.Models.Email;

public interface IBaseTemplateEmail
{
    string GenerateEmailBody();
    string LoadEmailTemplate();
}