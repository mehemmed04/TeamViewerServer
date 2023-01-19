using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamViewerServer.Helpers
{
    public class ImageHelper
    {
        public static string SaveAndGetImagePath(byte[] buffer)
        {
            ImageConverter ic = new ImageConverter();
            var data = ic.ConvertFrom(buffer);

            Image img = data as Image;
            if (img != null)
            {
                Bitmap bitmap1 = new Bitmap(img);

                DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());

                var path = di.Parent.Parent.FullName;

                path =path + $@"\Images";
                Directory.CreateDirectory(path);
                DateTime date = DateTime.Now;
                int year = date.Year;
                int month = date.Month;
                int day = date.Day;
                int hour = date.Hour;
                int minute =date.Minute;
                int second =date.Second;
                int msecond = date.Millisecond;
                path = path + $@"\image{year}{month}{day}{hour}{minute}{second}{msecond}.jpg";
                ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);
                bitmap1.Save(path, codec, encoderParams);

                return path;
            }
            else
            {
                return String.Empty;
            }

        }
    }
}
