using Vad.Phoenix.Titan.Logic.Message;
using Vad.Phoenix.Titan.Logic.Debug;
namespace Vad.Phoenix.Logic.Message.Battle
{
    public class RequestScoreUpdateMessage : PiranhaMessage
    {
    public List<CurrentScore> CurrentScores;

    public override void Decode()
    {
        base.Decode();

        int count = stream.ReadInt();
        if (count < 0)
        {
            CurrentScores = null;
            return;
        }

        if (count >= 10000)
            Debugger.Warning("Too many scores");

        CurrentScores = new List<CurrentScore>(count);

        for (int i = 0; i < count; i++)
        {
            var score = new CurrentScore();
            score.Decode(stream);
            CurrentScores.Add(score);
        }
    }

        public override int GetMessageType()
        {
            return 14400;
        }
    }
}