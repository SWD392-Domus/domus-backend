using Azure.Storage.Blobs;
using Domus.Common.Helpers;
using Domus.Common.Settings;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Common;
using Microsoft.Extensions.Configuration;

namespace Domus.Service.Implementations;

public class FileService : IFileService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _config ;
    private readonly AzureSettings _azureSettings;
    public FileService(IConfiguration config)
    {
        _config = config ;
        _azureSettings = _config.GetSection(nameof(AzureSettings)).Get<AzureSettings>() ?? throw new Exception("Invalid AzureSettings configuration. Check AzureBlobStorage value.");
        _blobServiceClient = new BlobServiceClient(_azureSettings.AzureBlobStorage);
    }
 
    public async Task<ServiceActionResult> UploadFile(FileModels fileModels)
    {
        var containerInstance = _blobServiceClient.GetBlobContainerClient(_azureSettings.BlobContainer);
        var blobInstance = containerInstance.GetBlobClient(fileModels.ImageFile.FileName.TrimSpaceString());
        await blobInstance.UploadAsync(fileModels.ImageFile.OpenReadStream());
        return new ServiceActionResult()
        {
            IsSuccess = true,
            Data = blobInstance.Uri.ToString()
        };
    }

    public async Task<Stream> GetFile(string fileName)
    {
        var containerInsatance = _blobServiceClient.GetBlobContainerClient(_azureSettings.BlobContainer);
        var blobInstance = containerInsatance.GetBlobClient(fileName);
        var downloadResult = await blobInstance.DownloadAsync();
        return downloadResult.Value.Content;
    }
}