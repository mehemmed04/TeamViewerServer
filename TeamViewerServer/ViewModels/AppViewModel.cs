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
using TeamViewerServer.Views;

namespace TeamViewerServer.ViewModels
{
    public class AppViewModel : BaseViewModel
    {

        public RelayCommand StartServerCommand { get; set; }
        public RelayCommand OpenScreenShareCommand { get; set; }
        private ObservableCollection<Client> allClients;

        public ObservableCollection<Client> AllClients
        {
            get { return allClients; }
            set { allClients = value; OnPropertyChanged(); }
        }

        private Client selectedClient;

        public Client SelectedClient
        {
            get { return selectedClient; }
            set { selectedClient = value; OnPropertyChanged(); }
        }


        public AppViewModel()
        {

            OpenScreenShareCommand = new RelayCommand((o) =>
            {
                ScreenShareViewModel vm = new ScreenShareViewModel(SelectedClient);
                ScreenShareWindow sv = new ScreenShareWindow();
                sv.DataContext = vm;
                sv.Show();
            });

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
                    }
                });


                Task.Run(() =>
                {
                    while (true)
                    {
                        DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
                        var path = di.Parent.Parent.FullName;
                        path = path + $@"\Images";
                        DeleteFileHeper.DeleteLastImages(path, 500);
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
                                        ImagePath = "../../DefaultImages/defaultimage.png"
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
