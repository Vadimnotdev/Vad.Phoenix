using Vad.Phoenix.Titan.Logic.DataStream;

namespace Vad.Phoenix.Logic.Command.LogicGame
{
    public class LogicUseBoostCommand : LogicCommand
{
    private int _boostId;
    private int _targetLane;
    private int _heroTileX;

    public override void Decode(ByteStream stream)
    {
        base.Decode(stream);
        _boostId    = stream.ReadInt();
        _targetLane = stream.ReadInt();
        _heroTileX  = stream.ReadInt();
    }

        public override int GetCommandType()
        {
            return 514;
        }
}

}