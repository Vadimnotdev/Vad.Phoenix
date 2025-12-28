using System.Net;
using System.Net.Sockets;
using Vad.Phoenix.Server.Network.Connection;
using Vad.Phoenix.Server.Network.Extensions;

namespace Vad.Phoenix.Server.Network.TCP;

internal class TCPGateway
{
    private Socket _socket;

    private Task _listenTask;
    private CancellationTokenSource _tokenSource;
    private ClientConnectionManager _manager;

    public TCPGateway()
    {
        this._socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        this._tokenSource = new();
        this._manager = new();

    }

    public void Start()
    {
        _socket.Bind(new IPEndPoint(IPAddress.Any, 9339));
        _socket.Listen(100);

        _listenTask = HandleAsync(_tokenSource.Token);
    }

    private async Task HandleAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            Socket? client = await _socket.AcceptSocketAsync(token);
            if (client == null) break;

            _manager.OnConnect(client);
        }
    }

    public async Task Stop()
    {
        if (this._listenTask != null)
        {
            await this._tokenSource.CancelAsync();
            await this._listenTask!;
        }

        this._socket.Close();
    }
}