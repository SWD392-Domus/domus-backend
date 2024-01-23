using Domus.Api.Controllers.Base;
using Domus.Service.Interfaces;
using Domus.Service.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers;
[Microsoft.AspNetCore.Components.Route("/api/[controller]")]
public class FileController : BaseApiController
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpGet("/get")]
    public async Task<IActionResult> GetFile(string fileName)
    {
        var imageFileStream = await _fileService.GetFile(fileName);
        string fileType = "jpeg";
        if (fileName.Contains("png"))
        {
            fileType = "png";
        }

        return File(imageFileStream, $"image/{fileType}");
    }

    [HttpPost("/upload")]
    public async Task<IActionResult> Upload([FromForm] FileModels file)
    {
        return await ExecuteServiceLogic(
            async() => await _fileService.UploadFile(file).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
}