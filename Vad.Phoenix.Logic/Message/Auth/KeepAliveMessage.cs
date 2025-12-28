using Vad.Phoenix.Titan.Logic.Message;
namespace Vad.Phoenix.Logic.Message.Auth
{
    public class KeepAliveMessage : PiranhaMessage
    {


        public override void Decode()
        {

        }
        public override int GetMessageType()
        {
            return 10108;
        }
    }
}