using System;
using System.Collections.Generic;
using System.Drawing;
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

                var path = "../../Images/";
                Directory.CreateDirectory(path);
                DateTime date = DateTime.Now;
                int year = date.Year;
                int month = date.Month;
                int day = date.Day;
                int hour = date.Hour;
                int minute =date.Minute;
                int second =date.Second;
                int msecond = date.Millisecond;
                path = path + $"/image{year}{month}{day}{hour}{minute}{second}{msecond}.png";
                bitmap1.Save(path);
                var imagepath = path;
                return imagepath;
            }
            else
            {
                return String.Empty;
            }

        }
    }
}
