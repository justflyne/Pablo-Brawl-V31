namespace Supercell.Laser.Logic.Message.Account
{
    public class AnalyticEventMessage : GameMessage
    {
        public override int GetMessageType()
        {
            return 10110;
        }

        public override int GetServiceNodeType()
        {
            return 1;
        }
    }
}
