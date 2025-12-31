using Vad.Phoenix.Titan.Logic.DataStream;
using Vad.Phoenix.Titan.Logic.Debug;
using Vad.Phoenix.Titan.Logic.Message;
using Vad.Phoenix.Logic.Command;

namespace Vad.Phoenix.Logic.Message.Home
{
    public class EndClientTurnMessage : PiranhaMessage
    {
        private List<LogicCommand> Commands;
        private int commandsCount;
        private int checksum;
        private int subTick;

        public EndClientTurnMessage() : base(0)
        {
        }

        public override void Decode()
        {
            base.Decode();

            subTick = stream.ReadInt();
            checksum = stream.ReadInt();
            commandsCount = stream.ReadInt();

            if (commandsCount > 512)
            {
                Debugger.Error(
                    $"EndClientTurn.Decode() command count is too high! ({commandsCount})"
                );
                return;
            }

            Commands = new List<LogicCommand>(commandsCount);

            for (int i = 0; i < commandsCount; i++)
            {
                LogicCommand command = LogicCommandManager.DecodeCommand(stream);

                if (command != null)
                {
                    Commands.Add(command);
                }
            }
        }

        public override int GetMessageType()
        {
            return 14102;
        }
    }
}
