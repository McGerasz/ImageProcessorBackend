using ImageProcessorBackend.Enums;

namespace ImageProcessorBackend.Service
{
    public interface IImageProcessor
    {
        Task<byte[]> ProcessImageAsync(string imageData, CancellationToken token);
        string GetEncodingMime(EncodingType encodingType);
    }
}
