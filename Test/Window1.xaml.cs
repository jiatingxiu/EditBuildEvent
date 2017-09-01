using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Test
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            var vv = "as".GetHashCode();
            return;
            //TextBox
            //text.Typography.ContextualLigatures = false;
            string sss = ToErJin("192.255.255.0").Trim('0');
            var ip = IPAddress.Parse("10.5.1.61");
            var mask = IPAddress.Parse("248.0.0.0");
            var getway = IPAddress.Parse("10.6.254.254");
            var result = ip.IsInSameSubnet(getway, mask);
            ValidationIPSubMaskGetWay("10.5.1.61", "255.255.0.0", "10.5.254.254");
        }

        public bool ValidationIPSubMaskGetWay(string ip, string mask, string getway)
        {
            byte[] byteIP = IPAddress.Parse(ip).GetAddressBytes();
            byte[] byteMask = IPAddress.Parse(mask).GetAddressBytes();
            byte[] byteGetway = IPAddress.Parse(getway).GetAddressBytes();
            return false;
        }

        /// <summary>
        /// 转换为二进制
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToErJin(string ipAddress)
        {
            var returnIpStr = "";
            byte[] numArray = IPAddress.Parse(ipAddress).GetAddressBytes();
            if (numArray.Length == 4)
            {
                for (var i = 0; i < 4; i++)
                {
                    var curr_num = numArray[i];
                    var number_Bin = Convert.ToString(numArray[i], 2);
                    var iCount = 8 - number_Bin.Length;
                    for (var j = 0; j < iCount; j++)
                    {
                        number_Bin = "0" + number_Bin;
                    }
                    returnIpStr += number_Bin;
                }
            }
            return returnIpStr;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            //System.Windows.Forms.SendKeys.Send("{PGDN}");
            webBrowser1.Navigate("javascript:var s = function() { window.scrollBy(0,-100); }; s();");
        }
    }

    public static class SubnetMask
    {
        public static readonly IPAddress ClassA = IPAddress.Parse("255.0.0.0");
        public static readonly IPAddress ClassB = IPAddress.Parse("255.255.0.0");
        public static readonly IPAddress ClassC = IPAddress.Parse("255.255.255.0");

        public static IPAddress CreateByHostBitLength(int hostpartLength)
        {
            int hostPartLength = hostpartLength;
            int netPartLength = 32 - hostPartLength;

            if (netPartLength < 2)
                throw new ArgumentException("Number of hosts is to large for IPv4");

            Byte[] binaryMask = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                if (i * 8 + 8 <= netPartLength)
                    binaryMask[i] = (byte)255;
                else if (i * 8 > netPartLength)
                    binaryMask[i] = (byte)0;
                else
                {
                    int oneLength = netPartLength - i * 8;
                    string binaryDigit =
                        String.Empty.PadLeft(oneLength, '1').PadRight(8, '0');
                    binaryMask[i] = Convert.ToByte(binaryDigit, 2);
                }
            }
            return new IPAddress(binaryMask);
        }

        public static IPAddress CreateByNetBitLength(int netpartLength)
        {
            int hostPartLength = 32 - netpartLength;
            return CreateByHostBitLength(hostPartLength);
        }

        public static IPAddress CreateByHostNumber(int numberOfHosts)
        {
            int maxNumber = numberOfHosts + 1;

            string b = Convert.ToString(maxNumber, 2);

            return CreateByHostBitLength(b.Length);
        }
    }

    public static class IPAddressExtensions
    {
        public static IPAddress GetNetworkAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
            }
            return new IPAddress(broadcastAddress);
        }

        public static bool IsInSameSubnet(this IPAddress address2, IPAddress address, IPAddress subnetMask)
        {
            IPAddress network1 = address.GetNetworkAddress(subnetMask);
            IPAddress network2 = address2.GetNetworkAddress(subnetMask);

            return network1.Equals(network2);
        }
    }
}
