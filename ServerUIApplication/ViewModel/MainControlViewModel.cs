using ClientServerComponents;
using ClientServerComponents.Models;
using ClientServerComponents.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows;

namespace ServerUIApplication.ViewModel
{
    public class MainControlViewModel : ViewModelBase
    {
        private delegate void Method();
        private readonly ServerComponent Server = new ServerComponent();
        private List<UserModel> _Users;
        public List<UserModel> Users { get => _Users; set { _Users = value; OnPropertyChanged(); } }
        public ObservableCollection<string> Logs { get; set; } = new ObservableCollection<string>();
        public RelayCommand Run { get; set; }
        public RelayCommand Stop { get; set; }
        public MainControlViewModel()
        {
            Server.Log += PrintLog;
            Run = new RelayCommand(Server.Start);
            Stop = new RelayCommand(Server.StopListening);
            Timer timer = new Timer(1000);
            timer.Elapsed += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, ElapsedEventArgs e)
        {
            ExecuteInMain(UpdateUsersList);
        }

        private void ExecuteInMain(Method method)
        {
            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                method?.Invoke();
            }));
        }
        private void UpdateUsersList()
        {
            Users = null;
            Users = UsersManager.UsersOnline;
        }
        private void PrintLog(string log) => ExecuteInMain(() => Logs.Add(log));
    }
}
