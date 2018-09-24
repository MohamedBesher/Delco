using System;
using System.IO;
using System.Web.Hosting;
using Saned.Delco.Api.Properties;

namespace Saned.Delco.Api.Models
{
    public static class ImageSaver
    {

        /// <summary>
        /// used to convert base64 to image and save it .
        /// </summary>
        /// <param name="imageBase64"></param>
        /// <returns></returns>
        public static string SaveImage(string imageBase64)
        {
            // create random guid to represent image name 
            var randomImage = Guid.NewGuid().ToString() + ".png";
            string slogn = Settings.Default.UploadPath + randomImage;

            string filePath = (HostingEnvironment.MapPath($"~{slogn}"));

            SaveImageInFileSystem(imageBase64, filePath);

            return randomImage;
        }


        private static void SaveImageInFileSystem(string base64, string filePath)
        {

            if (base64 == null)
                return;
            Byte[] bytes = Convert.FromBase64String(base64);
            File.WriteAllBytes(filePath, bytes);



        }


    }
}