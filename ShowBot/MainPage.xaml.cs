using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ShowBot.Models;
using System.Collections.ObjectModel;
using Microsoft.Bot.Connector.DirectLine.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ShowBot
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<Messages> ListOfMessage { get; set; } = new ObservableCollection<Messages>();

        public MainPage()
        {
            this.InitializeComponent();
            lv.DataContext = this;
            ListOfMessage.CollectionChanged += (s, args) => ScrollToBottom();
        }

        public async void send_Click(object sender, RoutedEventArgs e)
        {
            string nome = name.Text;
            if (name.Text.Length == 0)
            {
                nome = "Desconhecido";
            }
            Messages msg = new Messages { Username = nome, Message = text.Text };
            ListOfMessage.Add(msg);
            Message result = await App.messageToBot(msg.Username, msg.Message);
            if (result.Id == "Error")
            {
                ContentDialog cd = new ContentDialog();
                cd.Content = "Um erro foi encontrado. Por favor, tente novamente.";
                await cd.ShowAsync();
            }
            else
            {
                ListOfMessage.Add(new Messages { Username = result.FromProperty, Message = result.Text });
            }
        }

        private void ScrollToBottom()
        {
            var selectedIndex = lv.Items.Count - 1;
            if (selectedIndex < 0)
                return;

            lv.SelectedIndex = selectedIndex;
            lv.UpdateLayout();

            lv.ScrollIntoView(lv.SelectedItem);
        }
    }
}
