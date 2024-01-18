using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests;

public class AssignRoleRequest
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string RoleName { get; set; }
}