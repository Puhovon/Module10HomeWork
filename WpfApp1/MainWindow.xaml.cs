using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TelegramMessageClient client;
        
        public MainWindow()
        {
            InitializeComponent();

            client = new TelegramMessageClient(this);

            logList.ItemsSource = client.BotMessageLog;
        }

        //private void btnMsgSendClick(object sender, RoutedEventArgs e)
        //{
        //    client.SendMessage(txtMsgSend.Text, TargetSend.Text);
        //}

        //private void MenuItemSaveClick(object sender, RoutedEventArgs e)
        //{

        //}

        //private void MenuItemClearHistoryClick(object sender, RoutedEventArgs e)
        //{
        //    //client.ClearHistory();
        //}

        

        private void BtnMsgSendClick(object sender, RoutedEventArgs e)
        {
            client.SendMessage(txtMsgSend.Text, TargetSend.Text);
        }

        private void MenuItem_Click_Save(object sender, RoutedEventArgs e)
        {
            client.SaveHistory();
        }

        

        private void MenuItem_Click_Clear(object sender, RoutedEventArgs e)
        {
            client.Clear();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_About(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "TeleBot by Puhovon(Arkady)",
                this.Title,
                MessageBoxButton.OK,
                MessageBoxImage.Information
                );
        }
    }
}
