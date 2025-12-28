namespace Vad.Phoenix.Titan.Logic.Math
{
    using Vad.Phoenix.Titan.Logic.DataStream;
    public class LogicLong
    {
        private int _high;
        private int _low;

        public LogicLong()
        {
            
            _high = 0;
            _low = 0;
        }

        public LogicLong(int high, int low)
        {
            
            _high = high;
            _low = low;
        }

        public int GetLowerInt()
        {
            return _low;
        }

        public int GetHigherInt()
        {
            return _high;
        }

        public void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteInt(_high);
            encoder.WriteInt(_low);
        }

        public void Decode(ByteStream stream)
        {
            _high = stream.ReadInt();
            _low = stream.ReadInt();
        }

        public override string ToString()
        {
            return $"LogicLong({_high}-{_low})";
        }

        public static implicit operator LogicLong(long Long)
        {
            return new LogicLong((int)(Long >> 32), (int)Long);
        }

        public static implicit operator long(LogicLong Long)
        {
            return ((long)Long._high << 32) | (uint)Long._low;
        }

    }
}