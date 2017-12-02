using CD4_Server.Communication;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;

namespace CD4_Server.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private Server server;
        private bool isConnected = false;
        public ObservableCollection<string> Users { get; set; }
        public ObservableCollection<string> Messages { get; set; }
        public RelayCommand StartBtnClickedCommand { get; set; }
        public RelayCommand StopBtnClickedCommand { get; set; }
        public RelayCommand DropBtnClickedCommand { get; set; }
        public RelayCommand SaveToLogBtnClickedCommand { get; set; }
        public MainViewModel()
        {
            
            Messages = new ObservableCollection<string>();
            Users = new ObservableCollection<string>();

            StartBtnClickedCommand = new RelayCommand(
                () =>
                {
                    server = new Server();
                    server.StartAccepting();
                    isConnected = true;
                },
                () => { return !isConnected; });
        }
    }
}