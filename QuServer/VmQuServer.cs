using QuUtils;
using QuUtils.MidiMessages;
using QuUtils.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Media;

namespace QuServer
{
    /// <summary>
    /// 
    /// </summary>
    public class VmQuServer : VmBase
    {
        private const string LocalHost = "127.0.0.1";
        private const int QuPort = 51325;
        private const byte ActiveSensedSign = 0xFE;

        private const int DefMidiChannel = 9;
        private const int DefBoxId = 3;
        private const string DefVersion = "01.99";

        private string _status;
        private string _messageSent;
        private string _messageReceived;
        private SolidColorBrush _avtiveSensed;

        private int _midiChannel = DefMidiChannel;
        private int _boxId = DefBoxId;
        private string _version = DefVersion;

        private TcpListener server = null;
        private NetworkStream stream;

        /// <summary>
        /// 
        /// </summary>
        public VmQuServer()
        {
            Commands.AddCommand("DoConnect", x => ThreadPool.QueueUserWorkItem(y => DoConnect()));
            Commands.AddCommand("DoActiveSensed", x => ThreadPool.QueueUserWorkItem(y => DoActiveSensed()));
            Commands.AddCommand("DoReplyGetSystemState", x => DoReplyGetSystemState());
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoConnect()
        {
            try
            {
                byte[] bytes = new byte[256];
                string data = null;

                IPAddress localAddr = IPAddress.Parse(LocalHost);
                server = new TcpListener(localAddr, QuPort);
                server.Start();
                while (true)
                {
                    Status = "Waiting for a connection... ";

                    TcpClient client = server.AcceptTcpClient();
                    Status = "Connected!";

                    data = null;
                    stream = client.GetStream();

                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var command = Commands["DoActiveSensed"];
                        if (ActiveSense.Check4Message(bytes, i) && command.CanExecute(null))
                        {
                            MessageReceived = "Received: DoActiveSensed";
                            command.Execute(null);
                            break;
                        }
                        command = Commands["DoReplyGetSystemState"];
                        if (MsgGetSystemState.Check4Request(bytes, i) && command.CanExecute(null))
                        {
                            MessageReceived = "Received: GetSystemState";
                            command.Execute(null);
                            break;
                        }
                        else
                        {
                            data = Encoding.ASCII.GetString(bytes, 0, i);
                            MessageReceived = string.Format("Received: {0}", data);

                            data = data.ToUpper();

                            byte[] msg = Encoding.ASCII.GetBytes(data);

                            stream.Write(msg, 0, msg.Length);
                            MessageSent = string.Format("Sent: {0}", data);
                        }
                    }
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Status = string.Format("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoActiveSensed()
        {
            ActiveSensed = Brushes.Red;
            Thread.Sleep(200);
            ActiveSensed = Brushes.Gray;
        }

        public void DoReplyGetSystemState()
        {
            byte[] majorVersion = StringToByteArray(Version.Substring(0, 2));
            byte[] minorVersion = StringToByteArray(Version.Substring(3, 2));

            byte[] reply = MsgGetSystemState.GetReply((byte)MidiChannel, (byte)(BoxId +1), new byte[2] { majorVersion[0], minorVersion[0] });
            stream.Write(reply, 0, reply.Length);
            MessageSent = string.Format("Sent: {0}", ByteArrayToString(reply));

            const int MixerChannel = 1;
            const int FaderValue0Db = 0x6B;
            var channelStrip = new ChannelStrip(MidiChannel, MixerChannel, FaderValue0Db);
            var msg = channelStrip.GetFader()[0].Message;
            stream.Write(msg, 0, msg.Length);
            MessageSent = string.Format("Sent: {0}", ByteArrayToString(msg));

            foreach (MsgNrpn msgNrpn in channelStrip.GetFader())
            {
                stream.Write(msgNrpn.Message, 0, msgNrpn.Message.Length);
                MessageSent = string.Format("Sent: {0}", ByteArrayToString(msgNrpn.Message));
            }
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", ",");
        }

        /// <summary>
        /// 
        /// </summary>
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MessageSent
        {
            get { return _messageSent; }
            set
            {
                _messageSent = value;
                OnPropertyChanged("MessageSent");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MessageReceived
        {
            get { return _messageReceived; }
            set
            {
                _messageReceived = value;
                OnPropertyChanged("MessageReceived");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public SolidColorBrush ActiveSensed
        {
            get { return _avtiveSensed; }
            set
            {
                _avtiveSensed = value;
                OnPropertyChanged("ActiveSensed");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int MidiChannel
        {
            get { return _midiChannel; }
            set
            {
                _midiChannel = value;
                OnPropertyChanged("MidiChannel");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int BoxId
        {
            get { return _boxId; }
            set
            {
                _boxId = value;
                OnPropertyChanged("BoxId");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Version
        {
            get { return _version; }
            set
            {
                _version = value;
                OnPropertyChanged("Version");
            }
        }
    }
}
