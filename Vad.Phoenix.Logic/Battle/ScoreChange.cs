using Vad.Phoenix.Titan.Logic.Math;
using Vad.Phoenix.Titan.Logic.DataStream;
namespace Vad.Phoenix.Logic.Message.Battle
{
    public class ScoreChange
    {
        public LogicLong AvatarId;
        public List<LevelScore> LevelScores;

        public void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteLong(AvatarId);

            if (LevelScores == null)
            {
                encoder.WriteInt(-1);
                return;
            }

            encoder.WriteInt(LevelScores.Count);
            foreach (var score in LevelScores)
                score.Encode(encoder);
        }
    }
}