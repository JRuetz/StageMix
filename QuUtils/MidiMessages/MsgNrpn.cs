using System;

namespace QuUtils.MidiMessages
{
    /// <summary>
    /// 
    /// </summary>
    public class MsgNrpn
    {
        private byte[] _message = new byte[4];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="midiChannel"></param>
        /// <param name="parameterId"></param>
        /// <param name="parameterValue"></param>
        public MsgNrpn(byte midiChannel, byte parameterId, byte parameterValue)
        {
            int controlChangeHeader = 0xB0 | midiChannel;
            Buffer.SetByte(_message, 0, (byte)controlChangeHeader);
            Buffer.SetByte(_message, 1, parameterId);
            Buffer.SetByte(_message, 2, parameterValue);
            Buffer.SetByte(_message, 3, 0xF7);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool Check4Reply(byte[] data, int i)
        {
            if (i < 4 && data[4-1] != 0xF7)
                return false;
            if ((data[0] & 0xB0) == data[0])
                return false;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Message
        {
            get { return _message; }
        }
    }
}
