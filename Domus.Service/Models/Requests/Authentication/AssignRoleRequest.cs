using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.Authentication;

public class AssignRoleRequest
{
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string RoleName { get; set; } = null!;
}
