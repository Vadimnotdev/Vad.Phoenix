using Vad.Phoenix.Titan.Logic.DataStream;
using Vad.Phoenix.Titan.Logic.Debug;
namespace Vad.Phoenix.Logic.Command
{
    public class LogicCommand
    {
        private int executeTick;

    public void SetExecuteTick(int tick)
    {
        executeTick = tick;
    }
    public int GetExecuteTick()
    {
        return executeTick;
    }

    public virtual void Encode(ChecksumEncoder encoder)
    {
        encoder.WriteInt(this.executeTick);
    }

    public virtual void Decode(ByteStream stream)
    {
        this.executeTick = stream.ReadInt();
    }

    public virtual int GetCommandType()
    {
        return 0;
    }
    }
}