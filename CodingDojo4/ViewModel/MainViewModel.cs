using CD4_Client.Communication;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CD4_Client.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {
        private Client client;
        private bool isConnected = false;
        public string ChatName { get; set; }
        public string Message { get; set; }
        public ObservableCollection<string> ReceivedMessages { get; set; }
        public RelayCommand ConnectBtnClickedCommand { get; set; }
        public RelayCommand SendBtnClickedCommand { get; set; }

        public MainViewModel()
        {
            Message = "";
            ReceivedMessages = new ObservableCollection<string>();

            ConnectBtnClickedCommand = new RelayCommand(
                () =>
                {
                    isConnected = true;
                    client = new Client(new Action<string> (MessageReceived), ClientDisconnected);
                },
                () =>
                {
                    return !isConnected;
                }
                );

            SendBtnClickedCommand = new RelayCommand(
                () =>
                {
                    client.Send(ChatName + ": " + Message);
                    
                    ReceivedMessages.Add("YOU: " + Message);
                },
                () =>
                {
                    return isConnected && Message.Length >= 1 ;
                }
                );
        }

        private void ClientDisconnected()
        {
            isConnected = false;
            //do this to force the update of the button visibility otherwise change is done after focus change (clicking into gui)
            CommandManager.InvalidateRequerySuggested();
        }

        private void MessageReceived(string message)
        {
            App.Current.Dispatcher.Invoke(
                () => { ReceivedMessages.Add(message); });
        }
    }
}