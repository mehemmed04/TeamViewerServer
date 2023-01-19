using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
                    await Task.Run(() =>
                    {
                        var stream = Client.TcpClient.GetStream();
                        bool cc = true;
                        byte[] data = new byte[500000];
                        while (cc)
                        {
                            var bb = Task.Run(() =>
                            {
                                try
                                {
                                    stream.Read(data,0,data.Length);
                                    var path = ImageHelper.SaveAndGetImagePath(data);
                                    ImagePath = path;
                                    cc = false;
                                }
                                catch (Exception)
                                {

                                }
                            }).Wait(1);
                        }
                    });
                }

            });

        }
    }
}
