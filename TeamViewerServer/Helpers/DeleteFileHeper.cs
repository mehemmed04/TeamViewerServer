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
        public static void DeleteLastImages(string directoryPath, int seconds)
        {

            seconds = -1 * seconds;
            string[] files = Directory.GetFiles(directoryPath);

            foreach (string file in files)
            {
                DateTime creationTime = File.GetCreationTime(file);

                if (creationTime < DateTime.Now.AddSeconds(seconds))
                {
                    // Delete the file
                    File.Delete(file);
                }
            }


        }
    }
}
