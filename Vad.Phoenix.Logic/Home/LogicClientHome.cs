using Vad.Phoenix.Logic.Home;
using Vad.Phoenix.Titan.Logic.DataStream;
using Vad.Phoenix.Titan.Logic.Math;

namespace Vad.Phoenix.Logic.Home
{
    public class LogicClientHome
    {
        private LogicLong _homeId;
        private string _homeJSON;

        public LogicClientHome()
        {
            this._homeId = new LogicLong(0, 1);
            this._homeJSON = """{\"objects\":[]}""";
        }

        public void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteLong(this._homeId);
            encoder.WriteString(this._homeJSON);
        }

    }
}