namespace Vad.Phoenix.Titan.Logic.Message
{
    public abstract class LogicMessageFactory
    {
        public abstract PiranhaMessage? CreateMessageByType(int messageType);
    }
}
