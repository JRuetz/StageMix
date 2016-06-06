using NMock2;
using NUnit.Framework;
using QuServer;
using QuUtils;
using QuUtils.MidiMessages;
using System;
using System.Text;

namespace QuTest
{
    [TestFixture]
    public class MessageEncoderDecoderTest
    {
        private Mockery _mocks;

        [SetUp]
        public void Setup()
        {
            _mocks = new Mockery();
        }

        [TearDown]
        public void Teardown()
        {
        }

        [Test]
        public void MessageGetSystemState()
        {
            Assert.AreEqual(MsgGetSystemState.Request.Length, 12);
            Assert.AreEqual(MsgGetSystemState.Request, new byte[] { 0xF0, 0x00, 0x00, 0x1A, 0x50, 0x11, 0x01, 0x00, 0x7F, 0x10, 0x01, 0xF7 });
        }

        [Test]
        public void MessageGetSystemState_Check4Message()
        {
            Assert.IsFalse(MsgGetSystemState.Check4Request(new byte[] { 0xF0, 0x00, 0x00, 0x1A, 0x50, 0x11, 0x01, 0x00, 0x7F, 0x10, 0x01 }, 11));
            Assert.IsFalse(MsgGetSystemState.Check4Request(new byte[] { 0xF0, 0x00, 0x00, 0x1A, 0x50, 0x11, 0x01, 0x00, 0x7F, 0x10, 0x00, 0xF7 }, 12));
            Assert.IsTrue(MsgGetSystemState.Check4Request(new byte[] { 0xF0, 0x00, 0x00, 0x1A, 0x50, 0x11, 0x01, 0x00, 0x7F, 0x10, 0x01, 0xF7 }, 12));
        }

        [Test]
        public void ReplyGetSystemState()
        {
            const byte MidiChannel = 0x99;
            const byte BoxId = 0x04;
            string version = "01.99";
            byte[] majorVersion = VmQuServer.StringToByteArray(version.Substring(0, 2));
            byte[] minorVersion = VmQuServer.StringToByteArray(version.Substring(3, 2));
            byte[] byVersion = new byte[2];
            Buffer.SetByte(byVersion, 0, majorVersion[0]);
            Buffer.SetByte(byVersion, 1, minorVersion[0]);

            var reply = MsgGetSystemState.GetReply(MidiChannel, BoxId, byVersion);
            Console.WriteLine(VmQuServer.ByteArrayToString(reply));

            Assert.AreEqual(reply.Length, 13);
            Assert.AreEqual(reply, new byte[] { 0xF0, 0x00, 0x00, 0x1A, 0x50, 0x11, 0x01, 0x00, 0x99, 0x04, majorVersion[0], minorVersion[0], 0xF7});
        }

        [Test]
        public void MessageActiveSense()
        {
            Assert.AreEqual(ActiveSense.Message, new byte[] { 0xFE });
        }

        [Test]
        public void MessageActiveSense_Check4Message()
        {
            Assert.IsFalse(ActiveSense.Check4Message(new byte[1] { 0xF0 }, 1));
            Assert.IsFalse(ActiveSense.Check4Message(new byte[2] { 0xFE, 0xF0 }, 2));
            Assert.IsTrue(ActiveSense.Check4Message(new byte[1] { 0xFE }, 1));
        }
    }
}
