using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domus.Service.Models.Requests.Contracts;

public class SignedContractRequest
{
    [Required]
    public string FullName { get; set; }
    [Required]
    public IFormFile Signature { get; set; }
}