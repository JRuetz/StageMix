using QuUtils;
using QuUtils.MidiMessages;
using QuUtils.ViewModel;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace QuClient
{
    /// <summary>
    /// 
    /// </summary>
    public class VmQuClient : VmBase
    {
        private const string LocalHost = "127.0.0.1";
        private const int QuPort = 51325;

        private string _message;
        private string _midiChannel;
        private int _boxId;
        private string _osVersion;

        /// <summary>
        /// 
        /// </summary>
        public VmQuClient()
        {
            Commands.AddCommand("DoSendRequest", DoSendRequest);
        }


        public  void DoSendRequest(object oData)
        {
            try
            {
                byte[] data = (byte[])oData;

                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                TcpClient client = new TcpClient(LocalHost, QuPort);

                // Get a client stream for reading and writing.
                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                string message = Encoding.ASCII.GetString(data, 0, data.Length);
                Message = string.Format("Sent: {0}", message);

                WindowUtils.DoEvents();
                Thread.Sleep(5000);

                // Receive the TcpServer.response.
                // Buffer to store the response bytes.
                data = new byte[256];

                // Read the first batch of the TcpServer response bytes.
                int bytes = stream.Read(data, 0, data.Length);

                if (MsgGetSystemState.Check4Reply(data, bytes))
                {
                    byte[] dataGetSystemState = new byte[13];
                    Buffer.BlockCopy(data, 0, dataGetSystemState, 0, 13);
                    MidiChannel = BitConverter.ToString(dataGetSystemState, 8, 1);
                    BoxId = dataGetSystemState[9];
                    OSVersion = string.Format("{0}.{1}", BitConverter.ToString(dataGetSystemState, 10, 1), BitConverter.ToString(dataGetSystemState, 11, 1));
                    Message = string.Format("Received: {0}", ByteArrayToString(dataGetSystemState));

                    for (int i = 0; i < 4; i++)
                    {
                        byte[] dataMsgNrpn = new byte[4];
                        Buffer.BlockCopy(data, 13 +i*4, dataMsgNrpn, 0, 4);
                        Message = string.Format("Received: {0}", ByteArrayToString(dataMsgNrpn));
                    }
                }
                else
                {
                    string responseData = Encoding.ASCII.GetString(data, 0, bytes);
                    Message = string.Format("Received: {0}", responseData);
                }
                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Message = string.Format("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Message = string.Format("SocketException: {0}", e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", ",");
        }

        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string MidiChannel
        {
            get { return _midiChannel; }
            set
            {
                _midiChannel = value;
                OnPropertyChanged("MidiChannel");
            }
        }

        public int BoxId
        {
            set
            {
                _boxId = value;
                OnPropertyChanged("BoxType");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BoxType
        {
            get
            {
                switch (_boxId)
                {
                    case 1: return "Qu-16";
                    case 2: return "Qu-24";
                    case 3: return "Qu-22";
                    case 4: return "Qu-Pac";
                    default: return string.Empty;
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string OSVersion
        {
            get { return _osVersion; }
            set
            {
                _osVersion = value;
                OnPropertyChanged("OSVersion");
            }
        }
    }
}
