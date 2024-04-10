using ImageProcessorBackend.Enums;

namespace ImageProcessorBackend.Contracts
{
    public class ImageProcessingRequest
    {
        //The image that is recieved in a BASE64 string
        public string Image { get; set; }
        //The input that dictates the encoding format of the output
        public EncodingType Output_encoding { get; set; }
    }
}
