using Vad.Phoenix.Titan.Logic.Message;

namespace Vad.Phoenix.Logic.Message.Auth;

public class KeepAliveServerMessage : PiranhaMessage
{


    public override void Encode()
    {

    }

    public override int GetMessageType()
    {
        return 20112;
    }

}