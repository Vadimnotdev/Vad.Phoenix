namespace Vad.Phoenix.Titan.Logic.Message
{
    using Vad.Phoenix.Titan.Logic.DataStream;
    public class PiranhaMessage
    {
        private int _proxySessionId;

        public int messageVersion;
        public ByteStream stream = new(10);

        public PiranhaMessage(int messageVersion)
        {
            _proxySessionId = 0;

            this.messageVersion = messageVersion;
        }

        public PiranhaMessage() { }

        public virtual void Encode() { }

        public virtual void Decode() { }

        public virtual int GetMessageType()
        {
            return 0;
        }

        public bool IsClientToServerMessage()
        {
            return GetMessageVersion() >= 10000;
        }

        public bool IsServerToClientMessage()
        {
            return GetMessageVersion() >= 10000;
        }

        public byte[] GetMessageBytes()
        {
            return stream.GetByteArray();
        }

        public int GetEncodingLength()
        {
            return stream.GetLength();
        }

        public ByteStream GetByteStream()
        {
            return stream;
        }

        public int GetProxySessionId()
        {
            return _proxySessionId;
        }

        public void SetProxySessionId(int proxySessionId)
        {
            _proxySessionId = proxySessionId;
        }

        public int GetMessageVersion()
        {
            return messageVersion;
        }

        public void SetMessageVersion(int messageVersion)
        {
            this.messageVersion = messageVersion;
        }

        public virtual void Destruct()
        {
            _proxySessionId = 0;

            messageVersion = 0;
            stream = new ByteStream(10);
        }

        public void Clear()
        {
            stream = new ByteStream(10);
        }
    }
}
