using Domus.Service.Models;
using Domus.Service.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Service.Interfaces;

public interface IFileService
{
    Task<ServiceActionResult> UploadFile(FileModels fileModels);
    Task<Stream> GetFile(string fileName);
}