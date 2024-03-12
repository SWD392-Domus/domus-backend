using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.Quotations;

public class UpdateQuotationStatusRequest
{
    [Required]
    public string Status { get; set; } = null!;
}