using System.Text;
using System.Windows;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Media;

namespace QuServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VmQuServer _viewModel = new VmQuServer();

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;

            var command = _viewModel.Commands["DoConnect"];
            if (command.CanExecute(null))
                command.Execute(null);
        }
    }
}
