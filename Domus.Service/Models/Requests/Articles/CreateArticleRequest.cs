using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.Articles;

public class CreateArticleRequest
{
	[Required]
	public Guid CategoryId { get; set; }
}
