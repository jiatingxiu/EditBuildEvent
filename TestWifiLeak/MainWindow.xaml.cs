using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestWifiLeak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WlanClient.WlanInterface[] _wlanIfaceList;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Convert.ToInt32(textbox.Text); i++)
            {
                WlanClient client = new WlanClient();
                {
                    _wlanIfaceList = client.Interfaces;
                    List<string> temp = new List<string>();
                    if (_wlanIfaceList != null && _wlanIfaceList.Count() != 0)
                    {
                        foreach (var item in _wlanIfaceList)
                        {
                            temp.Add(item.InterfaceName);
                        }
                    }
                    else
                    {

                    }
                }
            }


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Convert.ToInt32(textbox.Text); i++)
                using (WlanClient client = new WlanClient())
                {
                    _wlanIfaceList = client.Interfaces;
                    List<string> temp = new List<string>();
                    if (_wlanIfaceList != null && _wlanIfaceList.Count() != 0)
                    {
                        foreach (var item in _wlanIfaceList)
                        {
                            temp.Add(item.InterfaceName);
                        }

                    }
                    else
                    {

                    }
                }
        }
    }
}
