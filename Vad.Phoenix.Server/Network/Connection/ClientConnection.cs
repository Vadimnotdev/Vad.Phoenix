using System.Net.Sockets;
using System.Security.Principal;
using System.Threading.Tasks;
using Vad.Phoenix.Server.Protocol;
using Vad.Phoenix.Titan.Logic;
using Vad.Phoenix.Titan.Logic.Math;
using Vad.Phoenix.Titan.Logic.Message;
using Vad.Phoenix.Titan.Logic.Debug;

namespace Vad.Phoenix.Server.Network.Connection;

public class ClientConnection
{
    public Socket Socket { get; }
    private Socket _socket;
    private Messaging _messaging;
    private MessageManager _messageManager;

    private byte[] _receiveBuffer;

    private LogicLong _currentAccountId;
    private Socket client;

    public ClientConnection(Socket socket)
    {
        this._socket = socket;
        this._messaging = new Messaging(this);
        this._messageManager = new MessageManager(this);
        this._receiveBuffer = GC.AllocateUninitializedArray<byte>(4096 * 2);
    }


    public bool IsConnected()
    {
        return _socket.Connected;
    }


    public async Task Receive()
    {
        int recvIdx = 0;
        Memory<byte> recvBufferMem = _receiveBuffer.AsMemory();

        while (true)
        {
            int r = await _socket.ReceiveAsync(recvBufferMem[recvIdx..], SocketFlags.None);
            if (r == 0)
                break;

            recvIdx += r;
            int consumedBytes = _messaging.OnReceive(_receiveBuffer, recvIdx);

            if (consumedBytes > 0)
            {
                Buffer.BlockCopy(_receiveBuffer, consumedBytes, _receiveBuffer, 0, recvIdx - consumedBytes);
                recvIdx -= consumedBytes;
            }
            else if (consumedBytes < 0)
            {
                break;
            }

            PiranhaMessage? message = this._messaging.NextMessage();
            if (message != null)
            {
                await this._messageManager.ReceiveMessage(message);
            }

            await this._messaging.OnWakeup();
        }
    }

    public async Task SendMessage(PiranhaMessage message)
    {
        await _messaging.Send(message);
    }

    public async Task Send(byte[] buffer)
    {
        await _socket.SendAsync(buffer, SocketFlags.None);
    }

}