using Vad.Phoenix.Titan.Logic.Message;
namespace Vad.Phoenix.Server.Network.Connection
{
    public interface IConnectionListener
    {
        public delegate ValueTask SendCallback(Memory<byte> buffer);
        public delegate Task ReceiveCallback(PiranhaMessage message);

        SendCallback OnSend { set; }
        ReceiveCallback RecvCallback { set; }

        ValueTask<int> OnReceive(Memory<byte> buffer, int size);

        Task Send(PiranhaMessage message);
    }
}
