using Vad.Phoenix.Titan.Logic.DataStream;

namespace Vad.Phoenix.Logic.Command.LogicGame
{
    public class LogicDragHeroCommand : LogicCommand
    {
        private int _heroTileX;
        private bool _startDragging;

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            _heroTileX = stream.ReadInt();
            _startDragging = stream.ReadBoolean();
        }

        public override int GetCommandType()
        {
            return 511;
        }

    }
}
