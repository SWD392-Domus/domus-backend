using Domus.Common.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Common;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Domus.Service.Interfaces;

public interface IFileService : IAutoRegisterable
{
    Task<ServiceActionResult> UploadFile(FileModels fileModels);
    Task<Stream> GetFile(string fileName);
    
    Task<ICollection<string>> GetUrlAfterUploadedFile(List<IFormFile> files);
}

