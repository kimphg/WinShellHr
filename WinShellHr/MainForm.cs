using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Net.Sockets;
namespace WinShellHr
{
    public partial class MainForm : Form
    {
        SerialPort ComPort = new SerialPort();
        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,ProtocolType.Udp);
        public MainForm()
        {
            InitializeComponent();
            // show full screeen
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            //init com port
            string[] ArrayComPortsNames = null;

            ArrayComPortsNames = SerialPort.GetPortNames();
            if(ArrayComPortsNames.Length>0)
            {
                ComPort.PortName = ArrayComPortsNames[0];
                ComPort.BaudRate = 38400;
                ComPort.DataBits = 8;
                ComPort.StopBits = (StopBits.One);
                ComPort.Parity = (Parity.None);
                ComPort.Open();
                ComPort.DataReceived += ComPort_DataReceived;
            }
        }

        void ComPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (true)
            {
                String str = ComPort.ReadLine();
            }
        }
    }
}
