using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TeamViewerServer.Commands;
using TeamViewerServer.Helpers;
using TeamViewerServer.Models;
using TeamViewerServer.NetworkHelper;

namespace TeamViewerServer.ViewModels
{
    public class AppViewModel : BaseViewModel
    {

        public RelayCommand StartServerCommand { get; set; }
        private ObservableCollection<Client> allClients;

        public ObservableCollection<Client> AllClients
        {
            get { return allClients; }
            set { allClients = value; OnPropertyChanged(); }
        }

        public AppViewModel()
        {
            StartServerCommand = new RelayCommand((o) =>
            {
                Task.Run(() =>
                {
                    Network.Connect();
                });


                Task.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            var client = Network.listener.AcceptTcpClient();
                            Network.Clients.Add(client);
                        }
                        catch (Exception)
                        {

                        }

                        Task.Run(() =>
                        {
                            var reader = Task.Run(() =>
                            {

                                foreach (var item in Network.Clients)
                                {
                                    Task.Run(() =>
                                    {
                                        var stream = item.GetStream();
                                        Network.br = new BinaryReader(stream);
                                        var bytes = new byte[500000];
                                        while (true)
                                        {
                                            try
                                            {

                                                bytes = Network.br.ReadBytes(50000);
                                                // var client = AllClients.FirstOrDefault((c) => { return c.TcpClient == item; });
                                                // var path = ImageHelper.SaveAndGetImagePath(bytes);
                                                //// client.ImagePath = path;
                                            }
                                            catch (Exception ex)
                                            {

                                                // MessageBox.Show($"{item.Client.RemoteEndPoint}  disconnected");
                                                Network.Clients.Remove(item);
                                            }
                                        }
                                    }).Wait(50);
                                }
                            });

                        });
                    }
                });


                Task.Run(async () =>
                {
                    while (true)
                    {
                        await Task.Delay(1000);
                        await Task.Run(() =>
                        {
                            App.Current.Dispatcher.Invoke(() =>
                            {

                                AllClients = new ObservableCollection<Client>();
                                foreach (var c in Network.Clients)
                                {
                                    AllClients.Add(new Client
                                    {
                                        TcpClient = c,
                                        Title = "Monitor " + c.Client.RemoteEndPoint.ToString(),
                                        ImagePath = "../../Images/defaultimage.png"
                                    });
                                }
                            });
                        });
                    }
                });

            });
        }

    }
}
