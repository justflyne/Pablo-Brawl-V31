namespace Supercell.Laser.Logic.Message.Team
{
    public class TeamClearInviteMessage : GameMessage
    {
        public int Slot;

        public override void Decode()
        {
            Slot = Stream.ReadVInt();
        }

        public override int GetMessageType()
        {
            return 14367;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
