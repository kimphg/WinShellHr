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
using System.Net;
using System.Diagnostics;


namespace WinShellHr
{
    public partial class MainForm : Form
    {
        SerialPort ComPort = new SerialPort();
        UdpClient udpSocket;
        Timer watchDogTimer;
        bool hrRunning;
        Process process ;
        bool receive_data;
        public MainForm()
        {
            //timer init
            watchDogTimer = new Timer();
            watchDogTimer.Tick += watchDogTimer_Tick;

            watchDogTimer.Enabled = true;
            watchDogTimer.Start();
            hrRunning = false;
            InitializeComponent();
            //socket init
            udpSocket = new UdpClient(8001);
            receive_data = true;
            try
            {
                udpSocket.BeginReceive(new AsyncCallback(udpDataRev), null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            // process init
            process = new Process();
            process.StartInfo.FileName = "C:\\Program Files\\HR2D\\Jupiter_2.2.exe";
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
                ComPort.StopBits = StopBits.One;
                ComPort.Parity = Parity.None;
                ComPort.Open();
                ComPort.DataReceived += ComPort_DataReceived;

            }
        }

        private void udpDataRev(IAsyncResult ar)
        {
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 8900);
            byte[] received = udpSocket.EndReceive(ar, ref groupEP);
            receive_data = true;
        }

        void watchDogTimer_Tick(object sender, EventArgs e)
        {
            if (hrRunning == false)
            {
                process.Start();
                watchDogTimer.Interval = 5000;
                hrRunning = true;
            }
            else 
            {

                if (receive_data == false)
                {
                    
                    process.Kill();
                    watchDogTimer.Interval = 300;
                    hrRunning = false;
                    
                    
                }
                else
                {
                    receive_data = false;
                }
            }
            
        }

        void ComPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (true)
            {
                if (ComPort.IsOpen == false) break;
                String str = ComPort.ReadLine();
                if (str.Length == 0) break;
                byte[] data = Encoding.ASCII.GetBytes(str);
                udpSocket.Send(data, data.Length, "127.0.0.1", 8900);
            }
        }
    }
}
