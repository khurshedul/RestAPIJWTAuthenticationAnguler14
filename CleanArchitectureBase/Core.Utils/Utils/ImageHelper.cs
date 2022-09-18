using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;

namespace Core.Utils.Utils
{
    public static class ImageHelper
    {
        public static bool SaveImageFromBase64(string base64String, string path)
        {
            try
            {
                //base64String = base64String.Replace("\r\n  ", "");
                //int base64StringLength = base64String.Length;

                //string imagePart = viewModel.APPLICANT_PHOTO.Replace('-', '+');
                //imagePart = imagePart.Replace('_', '/');

                base64String = base64String.Replace("data:image/png;base64,", string.Empty);

                byte[] imageBytes = Convert.FromBase64String(base64String);
                //MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                //ms.Write(imageBytes, 0, imageBytes.Length);
                //Image image = Image.FromStream(ms, true);
                //image = ScaleImage(image, 1600, 1200);
                //image.Save(path);

                using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    Image image = Image.FromStream(ms, true);
                    //image = ScaleImage(image, 1600, 1200);
                    image.Save(path);
                }

                //using (var imageFile = new FileStream(path, FileMode.Create))
                //{
                //    imageFile.Write(imageBytes, 0, imageBytes.Length);
                //    imageFile.Flush();
                //}

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static Image GetImage(this string base64String)
        {
            try
            {
                base64String = base64String.Replace("data:image/png;base64,", string.Empty);
                byte[] imageBytes = Convert.FromBase64String(base64String);

                using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    Image image = Image.FromStream(ms, true);
                    return image;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool SaveImageFromBytes(byte[] imageBytes, string path)
        {
            try
            {
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                Image image = Image.FromStream(ms, true);
                image = ScaleImage(image, 200, 400);
                image.Save(path);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        public static string ConvertImageURLToBase64(string url)
        {
            StringBuilder stringBuilder = new StringBuilder();

            byte[] _byte = GetImageFromUrl(url);

            stringBuilder.Append(Convert.ToBase64String(_byte, 0, _byte.Length));

            return stringBuilder.ToString();
        }

        private static byte[] GetImageFromUrl(string url)
        {
            Stream stream = null;
            byte[] buffer;

            try
            {
                WebProxy myProxy = new WebProxy();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();

                using (BinaryReader binaryReader = new BinaryReader(stream))
                {
                    int length = (int)response.ContentLength;
                    buffer = binaryReader.ReadBytes(length);
                    binaryReader.Close();
                }

                stream.Close();
                response.Close();
            }
            catch (Exception exp)
            {
                buffer = null;
            }

            return buffer;
        }



        public static string ImageToBase64String(Image image)
        {
            Bitmap bitmap = new Bitmap(image);

            MemoryStream ms = new MemoryStream();

            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            byte[] byteArray = new byte[ms.Length];

            ms.Position = 0;
            ms.Read(byteArray, 0, (int)ms.Length);
            ms.Close();

            string stringBase64 = Convert.ToBase64String(byteArray);

            return stringBase64;
        }
    }
}
