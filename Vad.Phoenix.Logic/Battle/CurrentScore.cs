using Vad.Phoenix.Titan.Logic.Math;
using Vad.Phoenix.Titan.Logic.DataStream;
namespace Vad.Phoenix.Logic.Message.Battle
{
    public class CurrentScore
    {
        public LogicLong AvatarId;
        public int Score;

        public void Decode(ByteStream stream)
        {
            Score = stream.ReadInt();
            AvatarId = stream.ReadLong();
        }
    }
}
