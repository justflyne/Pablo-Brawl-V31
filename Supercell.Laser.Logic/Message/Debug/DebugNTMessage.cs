namespace Supercell.Laser.Logic.Message.Debug
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Titan.DataStream;

    public class DebugNTMessage : GameMessage
    {                

        public override int GetMessageType()
        {
            return 10772;
        }
        
        public override int GetServiceNodeType()
		{
			return 1;
		}
    }
}
