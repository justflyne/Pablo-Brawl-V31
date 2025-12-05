namespace Supercell.Laser.Logic.Message.Battle
{
    using Supercell.Laser.Titan.DataStream;

    public class PlayAgainMessage : GameMessage
    {
        /*public int Tick;
        public int HandledInputs;
        public int Viewers;
        public BitStream VisionBitStream;*/

        public override int GetMessageType()
        {
            return 14177;
        }

        public override int GetServiceNodeType()
        {
            return 4;
        }
    }
}