namespace Supercell.Laser.Logic.Message.Club
{
    public class SendAllianceMailMessage : GameMessage
    {
        public int Id;
        public string Text;

        public override void Decode()
        {
            Id = Stream.ReadInt();
            Text = Stream.ReadString();
        }

        public override int GetMessageType()
        {
            return 14330;
        }

        public override int GetServiceNodeType()
        {
            return 11;
        }
    }
}
