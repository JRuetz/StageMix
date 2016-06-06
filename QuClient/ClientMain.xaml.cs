using System;
using System.Text;
using System.Windows;
using System.Net.Sockets;
using QuUtils;
using QuUtils.MidiMessages;

namespace QuClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VmQuClient _viewModel = new VmQuClient();

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var command = _viewModel.Commands["DoSendRequest"];
            if (command.CanExecute(MsgGetSystemState.Request))
                command.Execute(MsgGetSystemState.Request);
            if (command.CanExecute(ActiveSense.Message))
                command.Execute(ActiveSense.Message);
        }

        private void mnuItemGetSystemState_Click(object sender, RoutedEventArgs e)
        {
            var command = _viewModel.Commands["DoSendRequest"];
            if (command.CanExecute(MsgGetSystemState.Request))
                command.Execute(MsgGetSystemState.Request);
        }

        private void mnuItemActiveSense_Click(object sender, RoutedEventArgs e)
        {
            var command = _viewModel.Commands["DoSendRequest"];
            if (command.CanExecute(ActiveSense.Message))
                command.Execute(ActiveSense.Message);
        }

        private void mnuItemSendRequest_Click(object sender, RoutedEventArgs e)
        {
            dlgMessageBox.IsOpen = true;
        }
    }
}
