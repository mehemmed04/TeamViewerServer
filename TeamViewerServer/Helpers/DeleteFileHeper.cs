using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TeamViewerServer.Helpers
{
    public class DeleteFileHeper
    {
        public static void DeleteLastImages(string path,int seconds)
        {
            seconds = -1 * seconds;
            DirectoryInfo folderInfo = new DirectoryInfo(path);

            foreach (FileInfo file in folderInfo.GetFiles())
            {
                if (file.CreationTime.Date < DateTime.Now.AddMilliseconds(seconds))
                {
                    file.Delete();
                }
            }
        }
    }
}
