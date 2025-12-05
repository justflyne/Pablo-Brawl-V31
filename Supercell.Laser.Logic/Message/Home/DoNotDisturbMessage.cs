namespace Supercell.Laser.Logic.Message.Club
{
    public class DoNotDisturbMessage : GameMessage
    {
        public bool State;

        public override void Decode()
        {
            State = Stream.ReadBoolean();
        }

        public override int GetMessageType()
        {
            return 14777;
        }

        public override int GetServiceNodeType()
        {
            return 8; // я хз вроде 8 или 9
        }
    }
}
