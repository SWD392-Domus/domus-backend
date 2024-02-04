using Domus.Common.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Common;

namespace Domus.Service.Interfaces;

public interface IFileService : IAutoRegisterable
{
    Task<ServiceActionResult> UploadFile(FileModels fileModels);
    Task<Stream> GetFile(string fileName);
}
