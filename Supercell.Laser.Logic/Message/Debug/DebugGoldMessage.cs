namespace Supercell.Laser.Logic.Message.Debug
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Titan.DataStream;

    public class DebugGoldMessage : GameMessage
    {                

        public override int GetMessageType()
        {
            return 10768;
        }
        
        public override int GetServiceNodeType()
		{
			return 1;
		}
    }
}
