namespace Vad.Phoenix.Logic.Message
{
    using Vad.Phoenix.Logic.Message;
    using Vad.Phoenix.Logic.Message.Auth;
    using Vad.Phoenix.Titan.Logic.Message;
    using Vad.Phoenix.Logic.Message.Battle;
    using Vad.Phoenix.Logic.Message.Home;

    public class LogicGameMessageFactory : LogicMessageFactory
    {
        public static LogicGameMessageFactory Instance;

        static LogicGameMessageFactory()
        {
            Instance = new LogicGameMessageFactory();
        }


        private readonly Dictionary<int, Type> _allMessages;
        public LogicGameMessageFactory() : base()
        {
            _allMessages = new Dictionary<int, Type> {
                { 10101, typeof(LoginMessage) },
                { 10108, typeof(KeepAliveMessage) },
                { 10001, typeof(RequestStartLevelMessage) },
                { 14102, typeof(EndClientTurnMessage) },
                { 14400, typeof(RequestScoreUpdateMessage) }
            };
        }

        public override PiranhaMessage? CreateMessageByType(int messageType)
        {
            if (_allMessages.ContainsKey(messageType))
            {
                return Activator.CreateInstance(_allMessages[messageType]) as PiranhaMessage;
            }
            else
            {
                return null;
            }
        }

        public void Destruct()
        {
            throw new NotImplementedException();
        }
    }
}