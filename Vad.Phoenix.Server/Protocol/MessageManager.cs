using Vad.Phoenix.Server.Network.Connection;
using Vad.Phoenix.Titan.Logic.Message;
using Vad.Phoenix.Titan.Logic.Debug;
using Vad.Phoenix.Logic.Message.Auth;
using Vad.Phoenix.Logic.Message.Home;
using Vad.Phoenix.Logic.Message.Battle;
using System.Runtime.CompilerServices;
namespace Vad.Phoenix.Server.Protocol;

class MessageManager
{
    private ClientConnection _connection;
    private int requestedlevel;
    public MessageManager(ClientConnection connection)
    {
        this._connection = connection;
        
    }

    public async Task ReceiveMessage(PiranhaMessage message)
    {
        int messageType = message.GetMessageType();

        if (messageType != 10108)
            Debugger.Log($"MessageManager.ReceiveMessage: type={messageType}, name=" + message.GetType().Name);

        switch (messageType)
        {
            case 10101:
                await this.OnLoginMessageReceived((LoginMessage)message);
                break;
            case 10108:
                await this.OnKeepAliveMessageReceived();
                break;
            case 10001:
                await this.OnRequestStartLevelMessageReceived((RequestStartLevelMessage)message);
                break;
        }
    }

    private async Task OnLoginMessageReceived(LoginMessage loginMessage)
    {
        Debugger.Log($"New connection: accountId={loginMessage.GetAccountId()}, passToken={loginMessage.GetPassToken()}, client version={loginMessage.GetClientMajorVersion()}.{loginMessage.GetClientMinorVersion()}.{loginMessage.GetBuild()}, device={loginMessage.GetDevice()}");
        Debugger.Log($"client sha={loginMessage.GetResourceSHA()}");

        LoginOkMessage loginOkMessage = new LoginOkMessage();
        OwnHomeDataMessage ownHomeDataMessage = new OwnHomeDataMessage();

        await _connection.SendMessage(loginOkMessage);
        await _connection.SendMessage(ownHomeDataMessage);
    }

    private async Task OnKeepAliveMessageReceived()
    {
        KeepAliveServerMessage keepAliveServer = new KeepAliveServerMessage();

        await _connection.SendMessage(keepAliveServer);

    }

    private async Task OnRequestStartLevelMessageReceived(RequestStartLevelMessage requestStartLevelMessagesage)
    {
        int requestedlevel = requestStartLevelMessagesage.GetRequestedMessage();

        StartLevelMessage startLevelMessage = new StartLevelMessage();
        startLevelMessage.SetRequestedlevel(requestedlevel);

        await _connection.SendMessage(startLevelMessage);
    }

}