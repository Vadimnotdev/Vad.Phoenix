using Vad.Phoenix.Titan.Logic.DataStream;

namespace Vad.Phoenix.Logic.Command.LogicGame
{
    public class LogicDragCommand : LogicCommand
    {
        private int _fromX;
        private int _fromY;
        private int _toX;
        private int _toY;

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            _fromX = stream.ReadInt();
            _fromY = stream.ReadInt();
            _toX = stream.ReadInt();
            _toY = stream.ReadInt();
        }

        public override int GetCommandType()
        {
            return 500;
        }

    }
}
