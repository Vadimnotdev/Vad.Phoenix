namespace Vad.Phoenix.Titan.Encryption
{
    public class RC4Encrypter
    {
        private byte[] _key;
        private byte _v1;
        private byte _v2;

        public RC4Encrypter(string baseKey, string nonce)
        {
            InitState(baseKey, nonce);
        }

        public void Destruct()
        {
            _key = new byte[256];
            _v2 = 0;
            _v1 = 0;
        }

        public void InitState(string baseKey, string nonce)
        {
            string key = baseKey + nonce;

            _key = new byte[256];
            _v2 = 0;
            _v1 = 0;

            for (int i = 0; i < 256; i++)
            {
                _key[i] = (byte)i;
            }

            for (int i = 0, j = 0; i < 256; i++)
            {
                j = (byte)(j + _key[i] + key[i % key.Length]);

                byte tmpSwap = _key[i];

                _key[i] = _key[j];
                _key[j] = tmpSwap;
            }

            for (int i = 0; i < key.Length; i++)
            {
                _v2 += 1;
                _v1 += _key[_v2];

                byte tempSwap = _key[_v1];

                _key[_v1] = _key[_v2];
                _key[_v2] = tempSwap;
            }
        }

        public int Encrypt(byte[] input, byte[] output, int length)
        {
            for (int i = 0; i < length; i++)
            {
                _v2 += 1;
                _v1 += _key[_v2];

                byte tempSwap = _key[_v1];

                _key[_v1] = _key[_v2];
                _key[_v2] = tempSwap;

                output[i] = (byte)(input[i] ^ _key[(byte)(_key[_v2] + _key[_v1])]);
            }

            return 0;
        }

        public int Decrypt(byte[] input, byte[] output, int length)
        {
            return Encrypt(input, output, length);
        }
    }
}
