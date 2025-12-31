using Vad.Phoenix.Titan.Logic.DataStream;

namespace Vad.Phoenix.Logic.Command.LogicGame
{
    public class LogicFailLevelCommand : LogicCommand
    {
        private bool _resetBoostEnergy;

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            _resetBoostEnergy = stream.ReadBoolean();
        }

        public override int GetCommandType()
        {
            return 502;
        }

    }
}
