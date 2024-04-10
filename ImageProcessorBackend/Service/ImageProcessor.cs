
using ImageProcessorBackend.Enums;
using OpenCvSharp;
using System.Runtime.InteropServices;
using System.Threading;

namespace ImageProcessorBackend.Service
{
    public class ImageProcessor : IImageProcessor
    {
        //Importing unmanaged code from ImageProcessorLibrary.dll for image blurring.
        [DllImport(@"ImageProcessorLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr blur_byte_img([MarshalAs(UnmanagedType.LPArray)] byte[] byteArr, int length);

        public async Task<byte[]> ProcessImageAsync(string input, CancellationToken token)
        {
            // Convert input base64 string to byte array
            byte[] base64InByte = Convert.FromBase64String(input);
            return await Task.Run(() =>
            {
                //Call unmanaged method to process image and retrieve result pointer
                IntPtr resultPtr = blur_byte_img(base64InByte, base64InByte.Length);
                //Convert pointer into a Mat object
                Mat processedImage = new Mat(resultPtr);
                //Check if the token has been cancelled
                if (token.IsCancellationRequested)
                {
                    //Return an empty byte array to terminate the process
                    return new byte[0];
                }
                //Return processed image as byte array
                return processedImage.ToBytes();
            }, token);
        }
        public string GetEncodingMime(EncodingType encodingType)
        {
            //Get MIME type as string based on the EndcodingType enum
            return "image/" + Enum.GetName(typeof(EncodingType), encodingType)?.ToLower();
        }
    }
}
