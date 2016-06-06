using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace QuClient
{
    /// <summary>
    /// Interaction logic for PopUpMessageBox.xaml
    /// </summary>
    public partial class PopUpMessageBox 
    {
        public PopUpMessageBox()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as VmQuClient;
            if (viewModel == null)
                return;

            byte[] data = Encoding.ASCII.GetBytes(tbMessage.Text);
            var command = viewModel.Commands["DoSendRequest"];
            if (command.CanExecute(data))
                command.Execute(data);

            IsOpen = false;
        }

        private void Popup_Loaded(object sender, RoutedEventArgs e)
        {
            tbMessage.Clear();
            tbMessage.Focus();
        }
    }
}
