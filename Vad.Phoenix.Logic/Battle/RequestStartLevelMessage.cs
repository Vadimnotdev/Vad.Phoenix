using Vad.Phoenix.Titan.Logic.Message;
namespace Vad.Phoenix.Logic.Message.Battle
{
    public class RequestStartLevelMessage : PiranhaMessage
    {
        private int _requestedlevel;



        public override void Decode()
        {
            base.Decode();
            _requestedlevel = this.stream.ReadInt();
   
        }


        public int GetRequestedMessage()
        {
            return _requestedlevel;
        }
        public override int GetMessageType()
        {
            return 10001;
        }
    }
}