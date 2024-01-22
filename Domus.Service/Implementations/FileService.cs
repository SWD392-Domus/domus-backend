using Azure.Storage.Blobs;
using Domus.Common.Settings;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Common;
using Microsoft.Extensions.Configuration;

namespace Domus.Service.Implementations;

public class FileService : IFileService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _config;
    public FileService(IConfiguration config)
    {
        _config = config;
        var a = _config.GetSection(nameof(AzureSettings)).Get<AzureSettings>() ?? throw new Exception();
        _blobServiceClient = new BlobServiceClient(a.AzureBlobStorage);
    }

    public Task<ServiceActionResult> UplooadFile(FileModels fileModels)
    {
        throw new NotImplementedException();

    }

    public Task<ServiceActionResult> GetFile()
    {
        throw new NotImplementedException();
    }
}