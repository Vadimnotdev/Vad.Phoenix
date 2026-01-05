using Vad.Phoenix.Server.Network.Connection;
using Vad.Phoenix.Titan.Logic.Message;
using Vad.Phoenix.Titan.Logic.Debug;
using Vad.Phoenix.Logic.Message.Auth;
using Vad.Phoenix.Logic.Message.Home;
using Vad.Phoenix.Logic.Message.Battle;
using System.Runtime.CompilerServices;
using Vad.Phoenix.Server.Database;
using Vad.Phoenix.Titan.Logic.Math;
using Vad.Phoenix.Logic.Avatar;
using Vad.Phoenix.Logic.Home;
using Vad.Phoenix.Logic.Command;
using Vad.Phoenix.Logic.Command.LogicGame;
namespace Vad.Phoenix.Server.Protocol;

class MessageManager
{
    private ClientConnection _connection;
    private int requestedlevel;

    private int _currentLevel;

    DatabaseManger dbManager = new DatabaseManger();

    private LogicLong _currentAccountId;
    public MessageManager(ClientConnection connection)
    {
        this._connection = connection;
        
    }

    public async Task ReceiveMessage(PiranhaMessage message)
    {
        int messageType = message.GetMessageType();

        if (messageType != 10108 && messageType != 14102)
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
            case 14102:
            {
                var endTurn = (EndClientTurnMessage)message;

                foreach (var command in endTurn.Commands)
                {
                    await HandleLogicCommandAsync(command);
                }

                break;
            }

        }
    }

    private async Task OnLoginMessageReceived(LoginMessage loginMessage)
    {
        LogicLong accountId = loginMessage.GetAccountId();
        Debugger.Log($"New connection: accountId={loginMessage.GetAccountId()}, passToken={loginMessage.GetPassToken()}, client version={loginMessage.GetClientMajorVersion()}.{loginMessage.GetClientMinorVersion()}.{loginMessage.GetBuild()}, device={loginMessage.GetDevice()}");
        Debugger.Log($"client sha={loginMessage.GetResourceSHA()}");

        LoginOkMessage loginOkMessage = new();
        OwnHomeDataMessage ownHomeDataMessage = null;

        if (accountId.GetHigherInt() == 0 && accountId.GetLowerInt() == 0)
        {
            var (lastHighId, lastLowId) = await dbManager.GetLastAccountIdAsync();
            int newHighId = lastHighId;
            int newLowId = lastLowId + 1;
            LogicLong accountIdnew = new LogicLong(newHighId, newLowId);
            await dbManager.CreateDefaultAccountAsync(newHighId, newLowId);
            var account = await dbManager.GetAccountAsync(newHighId, newLowId);

            loginOkMessage._accountId = new LogicLong(account.highId, account.lowId);
            loginOkMessage._passToken = account.passToken;

            ownHomeDataMessage = SetAccountData(account);
            _currentAccountId = accountIdnew;
        }
        else
        {
            int highId = accountId.GetHigherInt();
            int lowId = accountId.GetLowerInt();
            var account = await dbManager.GetAccountAsync(highId, lowId);

            loginOkMessage._accountId = new LogicLong(account.highId, account.lowId);
            loginOkMessage._passToken = account.passToken;

            ownHomeDataMessage = SetAccountData(account);
            _currentAccountId = accountId;
            await dbManager.RenewEnergyByTimeAsync(account);
        }

        await _connection.SendMessage(loginOkMessage);
        await _connection.SendMessage(ownHomeDataMessage);
    }

    private async Task OnKeepAliveMessageReceived()
    {
        KeepAliveServerMessage keepAliveServer = new KeepAliveServerMessage();

        await _connection.SendMessage(keepAliveServer);

    }

    private async Task OnRequestStartLevelMessageReceived(
        RequestStartLevelMessage requestStartLevelMessage)
    {
        _currentLevel = requestStartLevelMessage.GetRequestedMessage();

        int highId = _currentAccountId.GetHigherInt();
        int lowId = _currentAccountId.GetLowerInt();

        StartLevelMessage startLevelMessage = new StartLevelMessage();
        startLevelMessage.SetRequestedlevel(_currentLevel);

        await _connection.SendMessage(startLevelMessage);
        await dbManager.UseEnergyAsync(highId, lowId);
        await dbManager.UpdateLastPlayedLevelAsync(highId, lowId, _currentLevel +1);
    }



    private OwnHomeDataMessage SetAccountData(Accounts account)
    {
        var ownHomeDataMessage = new OwnHomeDataMessage();
        ownHomeDataMessage._logicClientHome = new LogicClientHome
        {
            _homeId = new LogicLong(account.highId, account.lowId),
        };

        ownHomeDataMessage._logicClientAvatar = new LogicClientAvatar
        {
            _id = new LogicLong(account.highId, account.lowId),
            _lastPlayedLevel = account.lastPlayedLevel,
            _diamonds = account.diamonds,
            _energy = account.energy,
            _levelScore = account.levelScore

        };

        return ownHomeDataMessage;
    }

    public async Task HandleLogicCommandAsync(LogicCommand command)
    {
        if (command == null || _currentAccountId == null)
            return;

        int highId = _currentAccountId.GetHigherInt();
        int lowId = _currentAccountId.GetLowerInt();

        switch (command)
        {
            case LogicContinueGameCommand:
                await dbManager.UseDiamondsAsync(highId, lowId);
                break;

            case LogicRetryLevelCommand:
                await dbManager.UseEnergyAsync(highId, lowId);
                break;

            case LogicBuyResourcesCommand:
                await dbManager.UseDiamondsAsync(highId, lowId);
                await dbManager.RenewEnergyAsync(highId, lowId);
                break;

            case LogicLevelCompleteCommand:
            {
                if (_currentLevel == 0)
                    return;

                await dbManager.UpdateLevelScoreIfHigherAsync(
                    highId,
                    lowId,
                    _currentLevel +1
                );
                break;
}



        }
    }

}