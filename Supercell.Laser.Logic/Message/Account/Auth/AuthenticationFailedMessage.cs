namespace Supercell.Laser.Logic.Message.Account.Auth
{
    public class AuthenticationFailedMessage : GameMessage
    {
        public int ErrorCode;
        public string FingerprintSha;
        public string Message;
        public string UpdateUrl;
        public string ContentUrl;
        public int MaintenanceTime;

        public override void Encode()
        {
            Stream.WriteInt(ErrorCode);
            Stream.WriteString(FingerprintSha);
            Stream.WriteString(null); // Redirect
            Stream.WriteString(ContentUrl); // content url
            Stream.WriteString(UpdateUrl); // update url
            Stream.WriteString(Message); // message of error
            Stream.WriteInt(MaintenanceTime); // timer of ending maintenance
            Stream.WriteBoolean(true); // show support page

            Stream.WriteInt(0);
            Stream.WriteInt(0);
            Stream.WriteInt(0);
            Stream.WriteInt(3);
            Stream.WriteInt(0);
            Stream.WriteInt(0);
            Stream.WriteInt(0);
            Stream.WriteInt(0);
            Stream.WriteInt(0);
        }        

        public override int GetMessageType()
        {
            return 20103;
        }

        public override int GetServiceNodeType()
        {
            return 1;
        }
    }
}
