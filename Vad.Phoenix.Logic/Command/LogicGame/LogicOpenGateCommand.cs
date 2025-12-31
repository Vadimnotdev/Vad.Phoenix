using Vad.Phoenix.Titan.Logic.DataStream;

namespace Vad.Phoenix.Logic.Command.LogicGame
{
    public class LogicOpenGateCommand: LogicCommand {
        private int gateId;

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            gateId = stream.ReadInt();
            //_gateData = LogicDataTables.GetGateData(gateId);
        }

        public override int GetCommandType()
        {
            return 518;
        }
}

}