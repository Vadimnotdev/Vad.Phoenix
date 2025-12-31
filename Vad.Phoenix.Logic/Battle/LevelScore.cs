using Vad.Phoenix.Titan.Logic.Math;
using Vad.Phoenix.Titan.Logic.DataStream;
namespace Vad.Phoenix.Logic.Message.Battle{
    public class LevelScore
    {
        public int Level;
        public int Score;

        public void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteInt(Level);
            encoder.WriteInt(Score);
        }
    }
}