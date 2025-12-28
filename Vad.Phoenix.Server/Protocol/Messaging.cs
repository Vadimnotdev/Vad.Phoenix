using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Net.Sockets;
using Vad.Phoenix.Titan.Encryption;
using Vad.Phoenix.Titan.Logic.Debug;
using Vad.Phoenix.Titan.Logic.Message;
using Vad.Phoenix.Logic.Message;
using Vad.Phoenix.Server.Network.Connection;

namespace Vad.Phoenix.Server.Protocol
{
    class Messaging
    {
        public const int HEADER_SIZE = 7;

        private readonly ClientConnection _connection;
        private readonly ConcurrentQueue<PiranhaMessage> _incomingQueue;
        private readonly ConcurrentQueue<PiranhaMessage> _outgoingQueue;

        private RC4Encrypter _receiveEncrypter;
        private RC4Encrypter _sendEncrypter;


        private readonly LogicMessageFactory _factory;

        private static readonly string _rc4Key = "fhsd6f86f67rt8fw78fw789we78r9789wer6re";

        public Messaging(ClientConnection connection)
        {
            _connection = connection;
            _incomingQueue = new ConcurrentQueue<PiranhaMessage>();
            _outgoingQueue = new ConcurrentQueue<PiranhaMessage>();

            _receiveEncrypter = new RC4Encrypter(_rc4Key, "nonce");
            _sendEncrypter = new RC4Encrypter(_rc4Key, "nonce");

            _factory = new LogicGameMessageFactory();
        }

        public PiranhaMessage? NextMessage()
        {
            if (_incomingQueue.TryDequeue(out PiranhaMessage? message))
                return message;

            return null;
        }

        public int OnReceive(byte[] buffer, int length)
        {
            if (length >= HEADER_SIZE)
            {
                ReadHeader(buffer, out int messageType, out int messageLength, out int messageVersion);

                if (length - HEADER_SIZE >= messageLength)
                {
                    byte[] encryptedBytes = new byte[messageLength];
                    byte[] encodingBytes = new byte[messageLength];

                    Buffer.BlockCopy(buffer, HEADER_SIZE, encryptedBytes, 0, messageLength);

                    _receiveEncrypter.Decrypt(encryptedBytes, encodingBytes, messageLength);

                    PiranhaMessage? message = _factory.CreateMessageByType(messageType);
                    if (message != null)
                    {
                        message.GetByteStream().SetByteArray(encodingBytes, messageLength);
                        message.SetMessageVersion(messageVersion);

                        try
                        {
                            message.Decode();

                            if (_incomingQueue.Count >= 50)
                            {
                                Debugger.Warning($"Incoming message queue full. Message of type {messageType} discarded.");
                            }
                            else
                            {
                                if (!message.IsServerToClientMessage())
                                    _incomingQueue.Enqueue(message);
                            }
                        }
                        catch (Exception exception)
                        {
                            Debugger.Error($"Messaging.OnReceive: error while decoding message type {messageType}, trace: {exception}");
                        }
                    }
                    else if (messageType != 14102)
                    {
                        Debugger.Warning($"Ignoring message of unknown type {messageType}");
                    }

                    return messageLength + HEADER_SIZE;
                }
            }

            return 0;
        }

        public async Task Send(PiranhaMessage message)
        {
            int messageType = message.GetMessageType();

            if (!_connection.IsConnected())
            {
                Debugger.Warning($"Messaging.Send message type {messageType} when not connected");
                return;
            }

            if (_outgoingQueue.Count >= 50)
            {
                Debugger.Warning($"Outgoing message queue full. Message of type {messageType} discarded.");
            }
            else
            {
                _outgoingQueue.Enqueue(message);
            }
        }

        public async Task OnWakeup()
        {
            while (_outgoingQueue.TryDequeue(out PiranhaMessage message))
            {
                if (message.GetEncodingLength() == 0)
                    message.Encode();

                int encodingLength = message.GetEncodingLength();
                int encryptedLength = encodingLength;

                byte[] encodingBytes = message.GetMessageBytes();
                byte[] encryptedBytes = new byte[encodingBytes.Length];

                _sendEncrypter.Encrypt(encodingBytes, encryptedBytes, encodingLength);

                byte[] stream = new byte[encryptedLength + HEADER_SIZE];
                WriteHeader(message, stream, encryptedLength);

                Buffer.BlockCopy(encryptedBytes, 0, stream, HEADER_SIZE, encryptedLength);
                await _connection.Send(stream);
            }
        }

        private static void WriteHeader(PiranhaMessage message, byte[] stream, int length)
        {
            int messageType = message.GetMessageType();
            int messageVersion = message.GetMessageVersion();

            stream[0] = (byte)(messageType >> 8);
            stream[1] = (byte)messageType;
            stream[2] = (byte)(length >> 16);
            stream[3] = (byte)(length >> 8);
            stream[4] = (byte)length;
            stream[5] = (byte)(messageVersion >> 8);
            stream[6] = (byte)messageVersion;
        }

        private static void ReadHeader(byte[] stream, out int messageType, out int length, out int messageVersion)
        {
            messageType = (stream[0] << 8) | stream[1];
            length = (stream[2] << 16) | (stream[3] << 8) | stream[4];
            messageVersion = (stream[5] << 8) | stream[6];
        }
    }
}
