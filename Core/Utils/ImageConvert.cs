
namespace Core.Service
{
    using System;
    using System.IO;

    public class ImageConvert
    {

        public string GetFileToBase64(string path)
        {
            //string pathRed = urlApi;
            //string path = Directory.GetCurrentDirectory() + string.Format("{0} {1}", {pathRed}, $"/{fileName}");
            //string path = string.Concat(pathRed, "/", nombreExpediente, $"/{fileName}");
            Byte[] bytes = File.ReadAllBytes(path);
            string strArchivoBase64 = Convert.ToBase64String(bytes);
            return strArchivoBase64;
        }

    }
}
