using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MaMontreal.Models;
using MaMontreal.Models.NotMapped;
using MaMontreal.Repositories;

namespace MaMontreal.Services
{
    public class AzureStorageService
    {
        // TODO: Delete file logic so we don't use up too much space
        // Tutorial: https://blog.christian-schou.dk/how-to-use-azure-blob-storage-with-asp-net-core/
        private readonly string? _storageConnectionString = null!;
        private readonly string? _storageContainerName = null!;
        private readonly ILogger<AzureStorageService> _logger = null!;

        public AzureStorageService(IConfiguration configuration, ILogger<AzureStorageService> logger)
        {
            _storageConnectionString = configuration?.GetValue<string>("BlobConnectionString");
            _storageContainerName = configuration?.GetValue<string>("BlobNameMeetingImages");
            if (_storageConnectionString == null || _storageContainerName == null)
            {
                logger.LogError("BlobConnectionString or BlobNameMeetingImages is null");
                throw new ArgumentNullException("BlobConnectionString or BlobNameMeetingImages is null");
            }
            _logger = logger;
        }

        public async Task<BlobResponseDto> UploadMeetingImage(IFormFile file, Meeting meeting)
        {
            // Create new upload response object that we can return to the requesting method
            BlobResponseDto response = new();

            // Get a reference to a container named in appsettings.json and then create it
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            //await container.CreateAsync();
            try
            {
                // Get a reference to the blob just uploaded from the API in a container from configuration settings
                BlobClient client = container.GetBlobClient($"{meeting.Id}_{DateTime.Now.ToFileTime()}_{file.FileName}");


                // Open a stream for the file we want to upload
                await using (Stream? data = file.OpenReadStream())
                {
                    // Upload the file async
                    await client.UploadAsync(data);
                }

                // Everything is OK and file got uploaded
                response.Status = $"File {file.FileName} Uploaded Successfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = client.Name;

            }
            // If the file already exists, we catch the exception and do not upload it
            catch (RequestFailedException ex)
               when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                // TODO: Handle File Already Exists and change logger/exception messages
                _logger.LogError($"File with name {file.FileName} already exists in container. Set another name to store the file in the container: '{_storageContainerName}.'");
                response.Status = $"File with name {file.FileName} already exists. Please use another name to store your file.";
                response.Error = true;
                return response;
            }
            // If we get an unexpected error, we catch it here and return the error message
            catch (RequestFailedException ex)
            {
                // Log error to console and create a new response we can return to the requesting method
                _logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
                response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
                response.Error = true;
                return response;
            }

            // Return the BlobUploadResponse object
            return response;
        }
    }
}