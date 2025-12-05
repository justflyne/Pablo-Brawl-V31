namespace Supercell.Laser.Logic.Message.Account.Auth
{
    public class AuthenticationMessage : GameMessage
    {
        public AuthenticationMessage() : base()
        {
            AccountId = 0;
        }

        public long AccountId;
        public string PassToken;
        public string Device;        
        public int DeviceLang;
        public string Sha;        
        public string ClientVersion;
        public string Android;
        public string AndroidId;
        public int Major;
        public int Minor;
        public int Build;
        public string IMEI;
        public int RndKey;
        public bool IsAndroid;
        public string PreferredDeviceLanguage;

        public override void Decode()
        {
            AccountId = Stream.ReadLong();
            PassToken = Stream.ReadString();
            Major = Stream.ReadInt();
            Minor = Stream.ReadInt();
            Build = Stream.ReadInt();
            Sha = Stream.ReadString();
            Device = Stream.ReadString();
            Stream.ReadVInt();
            Stream.ReadVInt();
            PreferredDeviceLanguage = Stream.ReadString();
            Android = Stream.ReadString();
            IsAndroid = Stream.ReadBoolean();
            IMEI = Stream.ReadString();
            AndroidId = Stream.ReadString();
            Stream.ReadBoolean();
            Stream.ReadString();
            RndKey = Stream.ReadInt();
            Stream.ReadVInt();
            ClientVersion = Stream.ReadString();
            
            /*fields["AccountID"] = self.readLong()
        fields["PassToken"] = self.readString()
        fields["ClientMajor"] = self.readInt()
        fields["ClientMinor"] = self.readInt()
        fields["ClientBuild"] = self.readInt()
        fields["ResourceSha"] = self.readString()
        fields["Device"] = self.readString()
        fields["PreferredLanguage"] = self.readDataReference()
        fields["PreferredDeviceLanguage"] = self.readString()
        fields["OSVersion"] = self.readString()
        fields["isAndroid"] = self.readBoolean()
        fields["IMEI"] = self.readString()
        fields["AndroidID"] = self.readString()
        fields["isAdvertisingEnabled"] = self.readBoolean()
        fields["AppleIFV"] = self.readString()
        fields["RndKey"] = self.readInt()
        fields["AppStore"] = self.readVInt()
        fields["ClientVersion"] = self.readString()*/
            
            Console.WriteLine(
    $"AccountId: {AccountId}\n" +
    $"PassToken: {PassToken}\n" +
    $"Major: {Major}\n" +
    $"Build: {Build}\n" +
    $"Minor: {Minor}\n" +
    $"Sha: {Sha}\n" +
    $"Device: {Device}\n" +
    $"AndroidVersion: {Android}\n" +
    $"IsAndroid: {IsAndroid}\n" +
    $"AndroidID: {AndroidId}\n" +
    $"IMEI: {IMEI}\n" +
    $"RndKey: {RndKey}\n" +
    $"PreferredDeviceLanguage: {PreferredDeviceLanguage}\n" +
    $"ClientVersion: {ClientVersion}"
);                        
        }

        public override int GetMessageType()
        {
            return 10101;
        }

        public override int GetServiceNodeType()
        {
            return 1;
        }
    }
}
