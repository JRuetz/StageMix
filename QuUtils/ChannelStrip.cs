using QuUtils.MidiMessages;
using System.Collections.ObjectModel;

namespace QuUtils
{
    /// <summary>
    /// 
    /// </summary>
    public class ChannelStrip
    {
        const int MidiNrpnParId1 = 0x63;
        const int MidiNrpnParId2 = 0x62;
        const int MidiNrpnParId3 = 0x06;
        const int MidiNrpnParId4 = 0x26;

        private const byte ParameterIdFader = 0x17;
        private const byte IndexValueFader = 0x07;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="midiChannel"></param>
        /// <param name="mixerChannel"></param>
        /// <param name="faderValue"></param>
        public ChannelStrip(int midiChannel, int mixerChannel, int faderValue)
        {
            MidiChannel = midiChannel;
            MixerChannel = mixerChannel;
            FaderValue = faderValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Collection<MsgNrpn> GetFader()
        {
            Collection<MsgNrpn> buf = new Collection<MsgNrpn>();
            buf.Add(new MsgNrpn((byte)MidiChannel, MidiNrpnParId1, (byte)MixerChannel));
            buf.Add(new MsgNrpn((byte)MidiChannel, MidiNrpnParId2, ParameterIdFader));
            buf.Add(new MsgNrpn((byte)MidiChannel, MidiNrpnParId3, (byte)FaderValue));
            buf.Add(new MsgNrpn((byte)MidiChannel, MidiNrpnParId4, IndexValueFader));
            return buf;
        }

        /// <summary>
        /// 
        /// </summary>
        public int MidiChannel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int MixerChannel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int FaderValue { get; set; }
    }
}
