using Vad.Phoenix.Titan.Logic.DataStream;

namespace Vad.Phoenix.Logic.Command.LogicGame
{
    public class LogicContinueGameCommand : LogicCommand
    {

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
        }

        public override int GetCommandType()
        {
            return 501;
        }

    }
}
