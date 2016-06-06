namespace QuUtils.MidiMessages
{
    public class ActiveSense
    {
        private const byte ActiveSensedSign = 0xFE;

        public static byte[] Message
        {
            get { return new byte[1] { ActiveSensedSign }; }
        }

        public static bool Check4Message(byte[] data, int i)
        {
            if (i > 1)
                return false;
            if (data[0] == ActiveSensedSign)
                return true;
            return false;
        }
    }
}
