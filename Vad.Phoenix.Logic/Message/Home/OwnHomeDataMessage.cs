using Vad.Phoenix.Logic.Avatar;
using Vad.Phoenix.Logic.Home;
using Vad.Phoenix.Titan.Logic.Message;

namespace Vad.Phoenix.Logic.Message.Home
{
    public class OwnHomeDataMessage : PiranhaMessage
    {
        public LogicClientHome _logicClientHome = new();
        public LogicClientAvatar _logicClientAvatar = new();
        private int _secondsSinceLastSave = 0;
        private int _randomSeed = 0;

        public OwnHomeDataMessage() : base(0)
        {
        }

        public override void Encode()
        {
            //base.Encode();
            this.stream.WriteInt(_secondsSinceLastSave);
            this.stream.WriteInt(_randomSeed);
            _logicClientHome.Encode(stream);
            _logicClientAvatar.Encode(stream);
        }

        public override int GetMessageType()
        {
            return 24101;
        }

    }
}