using Vad.Phoenix.Titan.Logic.DataStream;

namespace Vad.Phoenix.Logic.Command.LogicGame
{
    public class LogicBuyBoostCommand : LogicCommand {
        private int _boostId;

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            _boostId = stream.ReadInt();
        }

        public override int GetCommandType()
        {
            return 516;
        }
}

}