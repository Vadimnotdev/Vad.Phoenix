using Vad.Phoenix.Titan.Logic.DataStream;

namespace Vad.Phoenix.Logic.Command.LogicGame
{
    public class LogicSwapHeroesCommand : LogicCommand
    {
        private int _fromTileX;
        private int _toTileX;

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            _fromTileX = stream.ReadInt();
            _toTileX   = stream.ReadInt();
        }

        public override int GetCommandType()
        {
            return 510;
        }

    }
}
