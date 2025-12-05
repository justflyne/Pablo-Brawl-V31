namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Titan.DataStream;

    public class LogicTeamChatMuteStateChangedCommand : Command
    {
        public bool State;
        public int Unk1;

        public override void Encode(ByteStream stream)
        {
            stream.WriteBoolean(State);
            base.Encode(stream);
        }
        
        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);            
            Unk1 = stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            return 0;
        }

        public override int GetCommandType()
        {
            return 221;
        }
    }
}
