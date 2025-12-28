using Vad.Phoenix.Titan.Logic.Message;

namespace Vad.Phoenix.Logic.Message.Battle{
    public class StartLevelMessage : PiranhaMessage
    {
        public int _requestedlevel;


        public override void Encode()
        {
            base.Encode();
            this.stream.WriteInt(this._requestedlevel);


        }

        public void SetRequestedlevel(int value)
        {
            _requestedlevel = value;
        }

        public override int GetMessageType()
        {
            return 20001;
        }

    }
}