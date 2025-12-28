using System.IO;
using Vad.Phoenix.Logic.Helper;
using Vad.Phoenix.Titan.Data;
using Vad.Phoenix.Titan.Logic.Math;
using Vad.Phoenix.Titan.Logic.Message;
using Vad.Phoenix.Logic.Data;
namespace Vad.Phoenix.Logic.Message.Auth
{
    public class LoginMessage : PiranhaMessage
    {
        private LogicLong _accountId;
        private string _PassToken;
        private int _ClientMajorVersion;
        private int _ClientMinorVersion;
        private int _ClientBuild;
        private string _ResourceSha;
        private string _UDID;
        private string _OpenUDID;
        private string _MacAddress;
        private string _Device;


        public override void Decode()
        {
            base.Decode();
            _accountId = this.stream.ReadLong();
            _PassToken = this.stream.ReadString();
            _ClientMajorVersion = this.stream.ReadInt();
            _ClientMinorVersion = this.stream.ReadInt();
            _ClientBuild = this.stream.ReadInt();
            _ResourceSha = this.stream.ReadString();
            _UDID = this.stream.ReadString();
            _OpenUDID = this.stream.ReadString();
            _MacAddress = this.stream.ReadString();
            _Device = this.stream.ReadString();

        }

        public LogicLong GetAccountId()
        {
            return _accountId;
        }

        public string? GetPassToken()
        {
            return _PassToken;
        }

        public int GetClientMajorVersion()
        {
            return _ClientMajorVersion;
        }

        public int GetClientMinorVersion()
        {
            return _ClientMinorVersion;
        }

        public int GetBuild()
        {
            return _ClientBuild;
        }

        public string GetResourceSHA()
        {
            return _ResourceSha;
        }

        public string GetDevice()
        {
            return _Device;
        }

        public override int GetMessageType()
        {
            return 10101;
        }
    }
}