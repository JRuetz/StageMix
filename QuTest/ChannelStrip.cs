using NUnit.Framework;
using QuUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuTest
{
    [TestFixture]
    public class ChannelStripTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void Teardown()
        {
        }

        [Test]
        public void GetFaderMessages()
        {
            const int MidiChannel = 9;
            const int MixerChannel = 1;
            const int FaderValue0Db = 0x6B;

            var channelStrip = new ChannelStrip(MidiChannel, MixerChannel, FaderValue0Db);
            Assert.AreEqual(new byte[] { 0xB9, 0x63, 0x01, 0xF7 }, channelStrip.GetFader()[0].Message);
            Assert.AreEqual(new byte[] { 0xB9, 0x62, 0x17, 0xF7 }, channelStrip.GetFader()[1].Message);
            Assert.AreEqual(new byte[] { 0xB9, 0x06, 0x6B, 0xF7 }, channelStrip.GetFader()[2].Message);
            Assert.AreEqual(new byte[] { 0xB9, 0x26, 0x07, 0xF7 }, channelStrip.GetFader()[3].Message);
        }
    }
}
