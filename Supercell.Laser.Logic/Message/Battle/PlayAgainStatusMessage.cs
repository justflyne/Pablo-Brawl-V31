namespace Supercell.Laser.Logic.Message.Battle
{
    using Supercell.Laser.Titan.DataStream;

    public class PlayAgainStatusMessage : GameMessage
    {
        /*public int Tick;
        public int HandledInputs;
        public int Viewers;
        public BitStream VisionBitStream;*/

        public PlayAgainStatusMessage() : base()
        {
            ;
        }

        public override void Encode()
        {
            Stream.WriteVInt(2); // 0 = Waiting, 1 = crash ?, 2 = Matchmaking
            Stream.WriteVInt(0); // unknown
            Stream.WriteVInt(0); // unknown
            Stream.WriteVInt(1); // players in again
        }

        public override int GetMessageType()
        {
            return 24777;
        }

        public override int GetServiceNodeType()
        {
            return 4;
        }
    }
}