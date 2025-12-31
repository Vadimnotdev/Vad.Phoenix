using Vad.Phoenix.Titan.Logic.DataStream;

namespace Vad.Phoenix.Logic.Command.LogicGame
{
    public class LogicBuyResourcesCommand : LogicCommand
    {
        private bool hasCommand;

        public override void Decode(ByteStream stream)
        {
            hasCommand = stream.ReadBoolean();

            base.Decode(stream);
        }

        public override int GetCommandType()
        {
            return 700;
        }

    }
}
