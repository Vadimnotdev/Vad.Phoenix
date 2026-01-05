using Vad.Phoenix.Titan.Logic.DataStream;
using Vad.Phoenix.Titan.Logic.Math;

namespace Vad.Phoenix.Logic.Avatar
{
    public class LogicClientAvatar
    {
        public LogicLong _id;
        private string _name;
        private string _facebookId;
        public int _lastPlayedLevel;
        public int _diamonds;
        private int _freeDiamonds;
        private bool _nameSetByUser;
        private int _cumulativePurchasedDiamonds;
        private int _resourceCount;
        public int _energy;

        private int _achievementsClaimed;
        private int _achievementProgress;
        public int _levelScore;
        private int _levelAreaScore;

        public LogicClientAvatar()
        {
            this._name = null;
            this._facebookId = null;
            this._freeDiamonds = 0;
            this._nameSetByUser = false;
            this._cumulativePurchasedDiamonds = 0;
            this._resourceCount = 1;
            this._achievementsClaimed = 0;
            this._achievementProgress = 0;
            this._levelAreaScore = 0;
        }

        public void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteLong(_id);
            encoder.WriteString(this._name);
            encoder.WriteString(this._facebookId);
            encoder.WriteInt(_lastPlayedLevel);
            encoder.WriteInt(_diamonds);
            encoder.WriteInt(this._freeDiamonds);
            encoder.WriteBoolean(this._nameSetByUser);
            encoder.WriteInt(this._cumulativePurchasedDiamonds);
            encoder.WriteInt(this._resourceCount);
            encoder.WriteInt(9000001);
            encoder.WriteInt(_energy);
            encoder.WriteInt(0);
            encoder.WriteInt(this._achievementsClaimed);
            encoder.WriteInt(this._achievementProgress);
            if (_levelScore == 1 && _energy == 10)
            {
                encoder.WriteInt(_levelScore);
                encoder.WriteInt(14000001);
                encoder.WriteInt(0);
                encoder.WriteInt(0);
            }
            else
            {
                encoder.WriteInt(_levelScore +1);
                for (int i = 1; i <= _levelScore +1; i++)
                {
                    encoder.WriteInt(14000000 + i);
                    if (i == _levelScore +1)
                        encoder.WriteInt(0);
                    else
                        encoder.WriteInt(50000);

                    encoder.WriteInt(0);
                }

            }

            encoder.WriteInt(this._levelAreaScore);

        }
    }
}
