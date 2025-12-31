using Vad.Phoenix.Logic.Command;
using Vad.Phoenix.Titan.Logic.DataStream;
using Vad.Phoenix.Titan.Logic.Message;
namespace Vad.Phoenix.Logic.Message.Home
{
    public class AvailableServerCommandMessage : PiranhaMessage
    {
        private LogicServerCommand _serverCommand;

        public override void Encode() {
            base.Encode();

            LogicCommandManager.EncodeCommand(stream, _serverCommand);
        }

        public override int GetMessageType() {
            return 24111;
        }

        public void SetServerCommand(LogicServerCommand serverCommand) {
            _serverCommand = serverCommand;
        }
    }
}