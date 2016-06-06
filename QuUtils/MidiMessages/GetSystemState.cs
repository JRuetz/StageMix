using System;

namespace QuUtils.MidiMessages
{
    public class MsgGetSystemState
    {
        private static byte[] SysexHeaderAllCall = new byte[] { 0xF0, 0x00, 0x00, 0x1A, 0x50, 0x11, 0x01, 0x00, 0x7F };
        private static byte[] SysexHeader = new byte[] { 0xF0, 0x00, 0x00, 0x1A, 0x50, 0x11, 0x01, 0x00 };

        public static byte[] Request
        {
            get
            {
                byte[] msg = new byte[12];
                Buffer.BlockCopy(SysexHeaderAllCall, 0, msg, 0, SysexHeaderAllCall.Length);
                Buffer.SetByte(msg, SysexHeaderAllCall.Length, 0x10);
                Buffer.SetByte(msg, SysexHeaderAllCall.Length + 1, 0x01);
                Buffer.SetByte(msg, SysexHeaderAllCall.Length + 2, 0xF7);
                return msg;
            }
        }

        public static bool Check4Request(byte[] data, int i)
        {
            if (i != 12)
                return false;
            for (int idx = 0; idx < SysexHeaderAllCall.Length; idx++)
            {
                if (SysexHeaderAllCall[idx] != data[idx])
                    return false; 
            }
            if (data[SysexHeaderAllCall.Length] != 0x10 ||
                data[SysexHeaderAllCall.Length + 1] != 0x01 ||
                data[SysexHeaderAllCall.Length + 2] != 0xF7)
                return false;
            return true;
        }

        public static byte[] GetReply(byte midiChannel, byte boxId, byte[] version)
        {
            byte[] msg = new byte[13];
            Buffer.BlockCopy(SysexHeader, 0, msg, 0, SysexHeader.Length);
            Buffer.SetByte(msg, SysexHeader.Length, midiChannel);
            Buffer.SetByte(msg, SysexHeader.Length + 1, boxId);
            Buffer.SetByte(msg, SysexHeader.Length + 2, version[0]);
            Buffer.SetByte(msg, SysexHeader.Length + 3, version[1]);
            Buffer.SetByte(msg, SysexHeader.Length + 4, 0xF7);
            return msg;
        }

        public static bool Check4Reply(byte[] data, int i)
        {
            if (i < 13 && data[13-1] != 0xF7)
                return false;
            for (int idx = 0; idx < SysexHeader.Length; idx++)
            {
                if (SysexHeader[idx] != data[idx])
                    return false;
            }
            return true;
        }
    }
}
