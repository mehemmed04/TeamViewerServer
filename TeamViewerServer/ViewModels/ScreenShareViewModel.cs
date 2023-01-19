using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TeamViewerServer.Helpers;
using TeamViewerServer.Models;
using TeamViewerServer.NetworkHelper;

namespace TeamViewerServer.ViewModels
{
    public class ScreenShareViewModel : BaseViewModel
    {
        private string imagePath;

        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; OnPropertyChanged(); }
        }


        private Client client;

        public Client Client
        {
            get { return client; }
            set { client = value; OnPropertyChanged(); }
        }
        public ScreenShareViewModel(Client client)
        {
            Client = client;
            ImagePath = Client.ImagePath;

           

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Run(async () =>
                    {
                        var stream = Client.TcpClient.GetStream();
                        var br = new BinaryReader(stream);
                        var bytes = new byte[500000];
                        bool cc = true;
                        while (cc)
                        {
                            var bb = Task.Run(async () =>
                            {
                                try
                                {
                                    bytes = br.ReadBytes(500000);
                                    var path = ImageHelper.SaveAndGetImagePath(bytes);
                                    ImagePath = path;
                                    cc = false;
                                }
                                catch (Exception ex)
                                {

                                }
                            }).Wait(1);
                        }
                    });
                }
                MessageBox.Show("Session Finished");

            });

        }
    }
}
