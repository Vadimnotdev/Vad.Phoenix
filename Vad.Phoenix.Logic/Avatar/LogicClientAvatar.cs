using Vad.Phoenix.Titan.Logic.DataStream;
using Vad.Phoenix.Titan.Logic.Math;

namespace Vad.Phoenix.Logic.Avatar
{
    public class LogicClientAvatar
    {
        private LogicLong _id;
        private string _name;
        private string _facebookId;
        private int _lastPlayedLevel;
        private int _diamonds;
        private int _freeDiamonds;
        private bool _nameSetByUser;
        private int _cumulativePurchasedDiamonds;
        private int _resourceCount;
        private int _energy;

        private int _achievementsClaimed;
        private int _achievementProgress;
        private int _levelScore;
        private int _levelAreaScore;

        public LogicClientAvatar()
        {
            this._id = new LogicLong(0, 1);
            this._name = null;
            this._facebookId = null;
            this._lastPlayedLevel = 0;
            this._diamonds = 50;
            this._freeDiamonds = 50;
            this._nameSetByUser = false;
            this._cumulativePurchasedDiamonds = 0;
            this._resourceCount = 1;
            this._energy = 10;
            this._achievementsClaimed = 0;
            this._achievementProgress = 0;
            this._levelScore = 79;
            this._levelAreaScore = 0;
        }

        public void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteLong(this._id);
            encoder.WriteString(this._name);
            encoder.WriteString(this._facebookId);
            encoder.WriteInt(this._lastPlayedLevel);
            encoder.WriteInt(this._diamonds);
            encoder.WriteInt(this._freeDiamonds);
            encoder.WriteBoolean(this._nameSetByUser);
            encoder.WriteInt(this._cumulativePurchasedDiamonds);
            encoder.WriteInt(this._resourceCount);
            encoder.WriteInt(9000001);
            encoder.WriteInt(this._energy);
            encoder.WriteInt(this._energy);
            encoder.WriteInt(this._achievementsClaimed);
            encoder.WriteInt(this._achievementProgress);
            encoder.WriteInt(this._levelScore);
            for (int i = 1; i <= 79; i++)
            {
                encoder.WriteInt(14000000 + i);
                encoder.WriteInt(30000);
                encoder.WriteInt(0);
            }
            encoder.WriteInt(this._levelAreaScore);

        }
    }
}
