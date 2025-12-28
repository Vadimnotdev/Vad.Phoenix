using Vad.Phoenix.Titan.Logic.Math;
using Vad.Phoenix.Titan.Logic.Message;

namespace Vad.Phoenix.Logic.Message.Auth;

public class LoginOkMessage : PiranhaMessage
{
    public LogicLong _accountId;
    public string _passToken;
    public string _facebookId;
    public string _GamecenterId;
    public int _ServerMajorVersion;
    public int _ServerMinorVersion;
    public int _ServerBuild;
    public int _ContentVersion;
    public string _ServerEnvironment;
    public int _SessionCount;
    public int _PlayTimeSeconds;
    public int _DaysSinceStartedPlaying;
    public string _FacebookAppId;
    public string _ServerTime;
    public string _AccountCreatedDate;
    public int _StartupCooldownSeconds;


    public LoginOkMessage() : base(0)
    {
        this._accountId = new LogicLong(0, 1);
        this._passToken = "PassToken";
        this._facebookId = "FacebookId";
        this._GamecenterId = "GamecenterId";
        this._ServerMajorVersion = 0;
        this._ServerMinorVersion = 2;
        this._ServerBuild = 10;
        this._ContentVersion = 1;
        this._ServerEnvironment = "ServerEnvironment";
        this._SessionCount = 0;
        this._PlayTimeSeconds = 0;
        this._DaysSinceStartedPlaying = 0;
        this._FacebookAppId = "FacebookAppId";
        this._ServerTime = "ServerTime";
        this._AccountCreatedDate = "AccountCreatedDate";
        this._StartupCooldownSeconds = 0;
    }

    public override void Encode()
    {
        base.Encode();
        this.stream.WriteLong(this._accountId);
        this.stream.WriteString(this._passToken);
        this.stream.WriteString(this._facebookId);
        this.stream.WriteString(this._GamecenterId);
        this.stream.WriteInt(this._ServerMajorVersion);
        this.stream.WriteInt(this._ServerMinorVersion);
        this.stream.WriteInt(this._ServerBuild);
        this.stream.WriteInt(this._ContentVersion);
        this.stream.WriteString(this._ServerEnvironment);
        this.stream.WriteInt(this._SessionCount);
        this.stream.WriteInt(this._PlayTimeSeconds);
        this.stream.WriteInt(this._DaysSinceStartedPlaying);
        this.stream.WriteString(this._FacebookAppId);
        this.stream.WriteString(this._ServerTime);
        this.stream.WriteString(this._AccountCreatedDate);
        this.stream.WriteInt(this._StartupCooldownSeconds);

    }

    public override int GetMessageType()
    {
        return 20104;
    }

}