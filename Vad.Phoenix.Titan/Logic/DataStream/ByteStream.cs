using System.Text;
using Vad.Phoenix.Titan.Logic.Math;

namespace Vad.Phoenix.Titan.Logic.DataStream
{
    public class ByteStream : ChecksumEncoder
    {
        public byte[] _buffer = new byte[1];
        private int _offset;
        private int _length;
        private int _bitOffset;
        public ByteStream() { }
        public ByteStream(byte[] buffer, int length)
        {
            _buffer = buffer;
            _length = length;
        }
        public ByteStream(int capacity)
        {
            _buffer = new byte[capacity];
        }

        public void SetByteArray(byte[] buffer, int length)
        {
            ResetOffset();
            _buffer = buffer;
            _length = length;
        }
        public void SetOffset(int offset)
        {
            _offset = offset;
            _bitOffset = 0;
        }
        public int GetLength()
        {
            if (_offset < _length) return _length;
            return _offset;
        }
        public int GetOffset()
        {
            return _offset;
        }
        public int GetVIntSizeInBytes(int value)
        {
            int size = 1;
            bool v1 = false;
            if (value > -1)
            {
                if (value >= 64)
                {
                    size = 2;
                    v1 = value < 0x2000;
                    if (value >= 0x2000)
                    {
                        size = 3;
                        v1 = value < 0x100000;
                    }
                    if (!v1)
                    {
                        size = 4;
                        if (value < 0x8000000) return 4;
                        return size;
                    }
                }
                return size;
            }
            if (value > -64)
            {
                return size;
            }
            size = 2;
            v1 = value <= -8192;
            if (v1)
            {
                size = 3;
                v1 = value <= -1048576;
            }
            if (!v1)
            {
                return size;
            }
            if (value > -134217728)
            {
                return 4;
            }
            return 5;
        }
        public byte[] GetByteArray()
        {
            return _buffer;
        }
        public void ResetOffset()
        {
            _offset = 0;
            _bitOffset = 0;
        }
        public void Clear(int capacity)
        {
            _buffer = new byte[capacity];
            _offset = 0;
        }
        public void EnsureCapacity(int capacity)
        {
            int bufferLength = _buffer.Length;

            if (_offset + capacity > bufferLength)
            {
                byte[] tmpBuffer = new byte[_buffer.Length + capacity + 100];
                System.Buffer.BlockCopy(_buffer, 0, tmpBuffer, 0, bufferLength);
                _buffer = tmpBuffer;
            }
        }

        public bool IsAtEnd()
        {
            return _offset >= _length;
        }


        public void WriteIntToByteArray(int value)
        {
            EnsureCapacity(4);
            _bitOffset = 0;

            _buffer[_offset++] = (byte)(value >> 24);
            _buffer[_offset++] = (byte)(value >> 16);
            _buffer[_offset++] = (byte)(value >> 8);
            _buffer[_offset++] = (byte)value;
        }

        public override void WriteInt(int value)
        {
            base.WriteInt(value);
            WriteIntToByteArray(value);
        }

        public override void WriteByte(byte value)
        {
            base.WriteByte(value);

            EnsureCapacity(1);
            _bitOffset = 0;

            _buffer[_offset++] = value;
        }

        public override void WriteString(string value)
        {
            base.WriteString(value);

            if (value == null)
            {
                WriteInt(-1);
            }
            else
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                int length = bytes.Length;

                if (length <= 900000)
                {
                    EnsureCapacity(length + 4);
                    WriteIntToByteArray(length);

                    System.Buffer.BlockCopy(bytes, 0, _buffer, _offset, length);

                    _offset += length;
                }
                else
                {
                    WriteIntToByteArray(-1);
                }
            }
        }


        public override void WriteStringReference(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            int length = bytes.Length;

            if (length <= 900000)
            {
                this.EnsureCapacity(length + 4);
                this.WriteIntToByteArray(length);

                System.Buffer.BlockCopy(bytes, 0, _buffer, _offset, length);

                _offset += length;
            }
            else
            {
                this.WriteIntToByteArray(-1);
            }
        }


        public override void WriteShort(short value)
        {
            base.WriteShort(value);

            EnsureCapacity(2);
            _bitOffset = 0;

            _buffer[_offset++] = (byte)(value >> 8);
            _buffer[_offset++] = (byte)value;
        }


        public override void WriteBoolean(bool value)
        {
            base.WriteBoolean(value);

            if (_bitOffset == 0)
            {
                EnsureCapacity(1);
                _buffer[_offset++] = 0;
            }
            if (value == true)
            {
                _buffer[_offset - 1] |= (byte)(1 << _bitOffset);
            }
            _bitOffset = (_bitOffset + 1) & 7;
        }


        public override void WriteVInt(int value)
        {
            base.WriteVInt(value);

            EnsureCapacity(5);

            _bitOffset = 0;

            switch (value)
            {
                case >= 0 and >= 64:
                    if (value >= 0x2000)
                    {
                        if (value >= 0x100000)
                        {
                            if (value >= 0x8000000)
                            {
                                _buffer[_offset++] = (byte)(value & 0x3F | 0x80);
                                _buffer[_offset++] = (byte)(value >> 6 & 0x7F | 0x80);
                                _buffer[_offset++] = (byte)(value >> 13 & 0x7F | 0x80);
                                _buffer[_offset++] = (byte)(value >> 20 & 0x7F | 0x80);
                                _buffer[_offset++] = (byte)(value >> 27 & 0xF);
                            }
                            else
                            {
                                _buffer[_offset++] = (byte)(value & 0x3F | 0x80);
                                _buffer[_offset++] = (byte)(value >> 6 & 0x7F | 0x80);
                                _buffer[_offset++] = (byte)(value >> 13 & 0x7F | 0x80);
                                _buffer[_offset++] = (byte)(value >> 20 & 0x7F);
                            }
                        }
                        else
                        {
                            _buffer[_offset++] = (byte)(value & 0x3F | 0x80);
                            _buffer[_offset++] = (byte)(value >> 6 & 0x7F | 0x80);
                            _buffer[_offset++] = (byte)(value >> 13 & 0x7F);
                        }
                    }
                    else
                    {
                        _buffer[_offset++] = (byte)(value & 0x3F | 0x80);
                        _buffer[_offset++] = (byte)(value >> 6 & 0x7F);
                    }
                    break;
                case >= 0:
                    _buffer[_offset++] = (byte)(value & 0x3F);
                    break;
                case <= -0x40 and <= -0x2000:
                    if (value <= -0x100000)
                    {
                        if (value <= -0x8000000)
                        {
                            _buffer[_offset++] = (byte)(value & 0x3F | 0xC0);
                            _buffer[_offset++] = (byte)(value >> 6 & 0x7F | 0x80);
                            _buffer[_offset++] = (byte)(value >> 13 & 0x7F | 0x80);
                            _buffer[_offset++] = (byte)(value >> 20 & 0x7F | 0x80);
                            _buffer[_offset++] = (byte)(value >> 27 & 0xF);
                        }
                        else
                        {
                            _buffer[_offset++] = (byte)(value & 0x3F | 0xC0);
                            _buffer[_offset++] = (byte)(value >> 6 & 0x7F | 0x80);
                            _buffer[_offset++] = (byte)(value >> 13 & 0x7F | 0x80);
                            _buffer[_offset++] = (byte)(value >> 20 & 0x7F);
                        }
                    }
                    else
                    {
                        _buffer[_offset++] = (byte)(value & 0x3F | 0xC0);
                        _buffer[_offset++] = (byte)(value >> 6 & 0x7F | 0x80);
                        _buffer[_offset++] = (byte)(value >> 13 & 0x7F);
                    }
                    break;
                case <= -0x40:
                    _buffer[_offset++] = (byte)(value & 0x3F | 0xC0);
                    _buffer[_offset++] = (byte)(value >> 6 & 0x7F);
                    break;
                default:
                    _buffer[_offset++] = (byte)(value & 0x3F | 0x40);
                    break;
            }
        }


        public override void WriteBytes(byte[] value, int length)
        {
            base.WriteBytes(value, length);

            if (value == null)
            {
                WriteIntToByteArray(-1);
            }
            else
            {
                EnsureCapacity(length + 4);
                WriteIntToByteArray(length);

                Buffer.BlockCopy(value, 0, _buffer, _offset, length);

                _offset += length;
            }
        }

        public void WriteBytesWithoutLength(byte[] value, int length)
        {
            base.WriteBytes(value, length);

            if (value != null)
            {
                EnsureCapacity(length);
                Buffer.BlockCopy(value, 0, _buffer, _offset, length);
                _offset += length;
            }
        }

        public override void WriteLongLong(long value)
        {
            base.WriteLongLong(value);

            int highInt = (int)(value >> 32);
            WriteIntToByteArray(highInt);
            int lowInt = (int)value;
            WriteIntToByteArray(lowInt);
        }


        public void WriteLong(long value)
        {
            WriteIntToByteArray((int)(value >> 32));
            WriteIntToByteArray((int)value);
        }

        public int ReadBytesLength()
        {
            _bitOffset = 0;
            return (_buffer[_offset++] << 24) |
                   (_buffer[_offset++] << 16) |
                   (_buffer[_offset++] << 8) |
                   _buffer[_offset++];
        }

        public int ReadInt()
        {
            _bitOffset = 0;

            return _buffer[_offset++] << 24 |
                   _buffer[_offset++] << 16 |
                   _buffer[_offset++] << 8 |
                   _buffer[_offset++];
        }

        public byte ReadByte()
        {
            _bitOffset = 0;

            return _buffer[_offset++];
        }

        public string ReadString(int maxCapacity = 900000)
        {
            int length = ReadBytesLength();

            if (length <= -1)
            {
                if (length != -1)
                {
                    Console.WriteLine("Negative String length encountered.");
                }
            }
            else
            {
                if (length <= maxCapacity)
                {
                    string value = Encoding.UTF8.GetString(_buffer, _offset, length);
                    _offset += length;
                    return value;
                }

                Console.WriteLine("Too long String encountered, max " + maxCapacity);
            }

            return null;
        }

        public string ReadStringReference(int maxCapacity)
        {
            int length = ReadBytesLength();

            if (length <= -1)
            {
                return string.Empty;
            }
            else
            {
                if (length <= maxCapacity)
                {
                    string value = Encoding.UTF8.GetString(_buffer, _offset, length);
                    _offset += length;
                    return value;
                }
            }

            return string.Empty;
        }

        public short ReadShort()
        {
            _bitOffset = 0;

            return (short)(_buffer[_offset++] << 8 |
                            _buffer[_offset++]);
        }

        public bool ReadBoolean()
        {
            if (_bitOffset == 0)
            {
                ++_offset;
            }

            bool value = (_buffer[_offset - 1] & 1 << _bitOffset) != 0;
            _bitOffset = _bitOffset + 1 & 7;
            return value;
        }


        public int ReadVInt()
        {
            _bitOffset = 0;
            int value = 0;
            byte byteValue = _buffer![_offset++];

            if ((byteValue & 0x40) != 0)
            {
                value |= byteValue & 0x3F;

                if ((byteValue & 0x80) != 0)
                {
                    value |= ((byteValue = ReadByte()) & 0x7F) << 6;

                    if ((byteValue & 0x80) != 0)
                    {
                        value |= ((byteValue = ReadByte()) & 0x7F) << 13;

                        if ((byteValue & 0x80) != 0)
                        {
                            value |= ((byteValue = ReadByte()) & 0x7F) << 20;

                            if ((byteValue & 0x80) != 0)
                            {
                                value |= ((byteValue = ReadByte()) & 0x7F) << 27;
                                return (int)(value | 0x80000000);
                            }

                            return (int)(value | 0xF8000000);
                        }

                        return (int)(value | 0xFFF00000);
                    }

                    return (int)(value | 0xFFFFE000);
                }

                return (int)(value | 0xFFFFFFC0);
            }

            value |= byteValue & 0x3F;

            if ((byteValue & 0x80) != 0)
            {
                value |= ((byteValue = ReadByte()) & 0x7F) << 6;

                if ((byteValue & 0x80) != 0)
                {
                    value |= ((byteValue = ReadByte()) & 0x7F) << 13;

                    if ((byteValue & 0x80) != 0)
                    {
                        value |= ((byteValue = ReadByte()) & 0x7F) << 20;

                        if ((byteValue & 0x80) != 0)
                        {
                            value |= ((byteValue = ReadByte()) & 0x7F) << 27;
                        }
                    }
                }
            }

            return value;
        }


        public byte[] ReadBytes(int length, int maxCapacity)
        {
            _bitOffset = 0;

            if (length <= -1)
            {
                if (length != -1)
                {
                    Console.WriteLine("Negative readBytes length encountered.");
                }

                return null;
            }

            if (length <= maxCapacity)
            {
                byte[] array = new byte[length];
                Buffer.BlockCopy(_buffer, _offset, array, 0, length);
                _offset += length;
                return array;
            }

            Console.WriteLine($"readBytes too long array, max {maxCapacity}");

            return null;
        }

        public LogicLong ReadLong()
        {
            LogicLong longValue = new();
            longValue.Decode(this);
            return longValue;
        }
        public LogicLong ReadLongLong()
        {
            return ReadLong();
        }


    }
}
