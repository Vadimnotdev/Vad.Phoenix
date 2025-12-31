using Vad.Phoenix.Titan.Logic.DataStream;

namespace Vad.Phoenix.Logic.Command.LogicGame
{
    public class LogicChangeGameStateCommand : LogicServerCommand
    {
        private bool _stateFlag; //0 PuzzleState 1 CityMapState

        public override void Encode(ChecksumEncoder encoder)
        {
            base.Encode(encoder);
            encoder.WriteBoolean(_stateFlag);
        }

        public override int GetCommandType()
        {
            return 8;
        }

        public void SetState(bool value)
        {
            _stateFlag = value;
        }
    }
}
