using Vad.Phoenix.Titan.Logic.DataStream;
using Vad.Phoenix.Logic.Command;
namespace Vad.Phoenix.Logic.Command.Resources {
    public class LogicDiamondsAddedCommand : LogicServerCommand {
        private bool _isFree = true;

        private int _amount = 100;

        private string _transactionId = "transactionId";

        public LogicDiamondsAddedCommand() {
            _transactionId = "";
        }

        public override void Encode(ChecksumEncoder encoder) {
            encoder.WriteBoolean(_isFree);

            encoder.WriteInt(_amount);

            encoder.WriteString(_transactionId);

            base.Encode(encoder);
        }

        public override int GetCommandType() {
            return 6;
        }
    }
}