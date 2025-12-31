using Vad.Phoenix.Titan.Logic.DataStream;

namespace Vad.Phoenix.Logic.Command.LogicGame
{
    public class LogicLevelCompleteCommand : LogicCommand
    {
        private bool _isLevelCompleted;
        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            _isLevelCompleted = stream.ReadBoolean();
        }

        public override int GetCommandType()
        {
            return 504;
        }

    }
}
