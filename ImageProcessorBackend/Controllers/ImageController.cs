using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using ImageProcessorBackend.Enums;
using Microsoft.AspNetCore.Mvc;
using OpenCvSharp;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using ImageProcessorBackend.Contracts;
using System.Threading;
using ImageProcessorBackend.Service;

namespace ImageProcessorBackend.Controllers;

[ApiController]
public class ImageController : ControllerBase
{
    private readonly IImageProcessor _imageProcessor;

    public ImageController(IImageProcessor imageProcessor)
    {
        _imageProcessor = imageProcessor;
    }

    [HttpPost("image_processing")]
    public async Task<IActionResult> ImageProcessing([FromBody] ImageProcessingRequest request, CancellationToken token)
    {
        try
        {
            //Process the image asynchronously
            byte[] processedImageBytes = await _imageProcessor.ProcessImageAsync(request.Image, token);
            //Check if the token has been cancelled
            token.ThrowIfCancellationRequested();
            //Convert the image from a byte array into a memory stream
            MemoryStream memoryStream = new MemoryStream(processedImageBytes);
            //Get the MIME type for the output
            string mime = _imageProcessor.GetEncodingMime(request.Output_encoding);
            //Return the processed image in the desired format
            return new FileStreamResult(memoryStream, mime);
        }
        catch (OperationCanceledException)
        {
            //Handles cancellation exception
            return StatusCode(499, "Client closed request");
        }
    }
}