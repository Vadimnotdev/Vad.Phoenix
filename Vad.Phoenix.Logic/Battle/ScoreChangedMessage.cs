using Vad.Phoenix.Titan.Logic.Message;

namespace Vad.Phoenix.Logic.Message.Battle{
    public class ScoreChangedMessage : PiranhaMessage
    {
    public List<ScoreChange> ScoreChanges;

        public override void Encode()
        {
            base.Encode();

            if (ScoreChanges == null)
            {
                stream.WriteInt(-1);
                return;
            }

            stream.WriteInt(ScoreChanges.Count);

            foreach (var change in ScoreChanges)
                change.Encode(stream);
        }

        public override int GetMessageType()
        {
            return 20120;
        }

    }
}