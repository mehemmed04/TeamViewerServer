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
                    byte[] bytes = new byte[5000000];
                    await Task.Run(() =>
                    {
                        var stream = Client.TcpClient.GetStream();

                        int i;
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {

                            ImageHelper imageHelper = new ImageHelper();
                            var path = ImageHelper.SaveAndGetImagePath(bytes);
                            ImagePath = path;
                        }


                        //bool cc = true;
                        //while (cc)
                        //{
                        //    var bb = Task.Run(() =>
                        //    {
                        //        try
                        //        {
                        //            byte[] data = new byte[500000];
                        //            stream.Read(data, 0, data.Length);
                        //            var path = ImageHelper.SaveAndGetImagePath(data);
                        //            ImagePath = path;

                        //            cc = false;
                        //        }
                        //        catch (Exception)
                        //        {

                        //        }
                        //    }).Wait(1);
                        //}
                    });
                }

            });

        }
    }
}

