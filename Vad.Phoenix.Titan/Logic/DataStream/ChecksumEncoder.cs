namespace Vad.Phoenix.Titan.Logic.DataStream
{
    using System.Runtime.CompilerServices;
    using Vad.Phoenix.Titan.Logic.Math;
    public class ChecksumEncoder
    {
        private int _checksum;
        private int _snapshotChecksum;
        private bool _enabled;

        public ChecksumEncoder()
        {
            _checksum = 0;
            _snapshotChecksum = 0;
            _enabled = true;
        }

        public virtual int GetChecksum()
        {
            if (_enabled)
            {
                return _checksum;
            }
            else return 0;
        }

        public void ResetCheckSum()
        {
            _checksum = 0;
        }

        public bool IsCheckSumEnabled()
        {
            return _enabled;
        }

        public virtual bool IsCheckSumOnlyMode()
        {
            return true;
        }

        public bool Equals(ChecksumEncoder encoder)
        {
            if (encoder != null)
            {
                int checksum = encoder._checksum;
                int m_checksum = _checksum;

                if (!encoder._enabled)
                {
                    checksum = encoder._snapshotChecksum;
                }

                if (!this._enabled)
                {
                    m_checksum = _snapshotChecksum;
                }

                return checksum == m_checksum;
            }

            return false;
        }

        public virtual void Destruct()
        {
            _checksum = 0;
            _snapshotChecksum = 0;
            _enabled = true;

        }

        public virtual void WriteLongLong(long value)
        {
            int high = (int)(value >> 32);
            int low = (int)value;

            _checksum = high + __ROR4__(low + __ROR4__(_checksum, 31) + 67, 31) + 91;

        }

        public virtual void WriteLong(LogicLong value)
        {
            value.Encode(this);
        }

        public virtual void WriteString(string value)
        {
            if (value != null)
            {
                _checksum = value.Length + 28;
            }

            else
            {
                _checksum = 27;
            }

            _checksum += __ROR4__(_checksum, 31);
        }

        public virtual void WriteInt(int value)
        {
            _checksum = value + __ROR4__(_checksum, 31) + 9;
        }

        public virtual void WriteBoolean(bool value)
        {
            if (value)
            {
                _checksum = 13 + __ROR4__(_checksum, 31);
            }
            else
            {
                _checksum = 7 + __ROR4__(_checksum, 31);
            }
        }

        public virtual void WriteByte(byte value)
        {
            _checksum = value + __ROR4__(_checksum, 31) + 11;
        }


        public virtual void WriteStringReference(string value)
        {
            _checksum = value.Length + __ROR4__(_checksum, 31) + 38;
        }

        public virtual void WriteVInt(int value)
        {
            _checksum = value + __ROR4__(_checksum, 31) + 33;
        }

        public virtual void WriteBytes(byte[] value, int length)
        {
            if (value != null)
            {
                _checksum = length + 28;
            }
            else
            {
                _checksum = 27;
            }

            _checksum = (_checksum + (_checksum >> 31)) | (_checksum << (32 - 31));
        }

        public virtual void WriteShort(short value)
        {
            _checksum = value + __ROR4__(_checksum, 31) + 19;
        }

        public void WriteVLong(LogicLong value)
        {
            WriteVInt(value.GetHigherInt());
            WriteVInt(value.GetLowerInt());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int __ROR4__(int value, int count)
        {
            return (value >> count) | (value << (32 - count));
        }


    }


}
